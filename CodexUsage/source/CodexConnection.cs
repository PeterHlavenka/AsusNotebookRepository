using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CodexUsage;

public sealed record Quota(double UsedPercent, long? ResetsAt);
public sealed record UsageSnapshot(Quota? FiveHour, Quota? Weekly, DateTimeOffset ObservedAt)
{
    public static UsageSnapshot Parse(JsonElement result)
    {
        JsonElement limits;
        if (result.TryGetProperty("rateLimitsByLimitId", out var buckets) &&
            buckets.ValueKind == JsonValueKind.Object && buckets.TryGetProperty("codex", out var codex))
            limits = codex;
        else if (result.TryGetProperty("rateLimits", out var legacy) && legacy.ValueKind == JsonValueKind.Object)
            limits = legacy;
        else throw new InvalidDataException("Limity Codexu nejsou dostupné");

        Quota? ParseWindow(string name, int expectedMinutes)
        {
            if (!limits.TryGetProperty(name, out var window) || window.ValueKind == JsonValueKind.Null) return null;
            if (!window.TryGetProperty("windowDurationMins", out var minutes) ||
                !minutes.TryGetInt32(out var duration) || duration != expectedMinutes) return null;
            if (!window.TryGetProperty("usedPercent", out var percent) ||
                !percent.TryGetDouble(out var used) || !double.IsFinite(used) || used < 0 || used > 100)
                throw new InvalidDataException("Neplatné procento využití");
            long? reset = null;
            if (window.TryGetProperty("resetsAt", out var resets) && resets.ValueKind != JsonValueKind.Null)
            {
                if (!resets.TryGetInt64(out var seconds) || seconds <= 0)
                    throw new InvalidDataException("Neplatný čas resetu");
                try { _ = DateTimeOffset.FromUnixTimeSeconds(seconds); } catch (ArgumentOutOfRangeException) { throw new InvalidDataException("Neplatný čas resetu"); }
                reset = seconds;
            }
            return new Quota(used, reset);
        }

        var primary = ParseWindow("primary", 300);
        var secondary = ParseWindow("secondary", 10080);
        if (primary is null && secondary is null) throw new InvalidDataException("Pětihodinový ani týdenní limit není dostupný");
        return new(primary, secondary, DateTimeOffset.UtcNow);
    }
}

public sealed class AccountUnavailableException(string message) : Exception(message);

public sealed class CodexConnection : IDisposable
{
    private readonly SemaphoreSlim gate = new(1, 1);
    private readonly ConcurrentDictionary<int, TaskCompletionSource<JsonElement>> pending = new();
    private readonly CancellationTokenSource lifetime = new();
    private Process? process;
    private readonly string? profileHome;
    private readonly string? expectedEmail;
    private TaskCompletionSource<bool>? loginCompletion;
    public CodexConnection(string? profileHome = null, string? expectedEmail = null)
    { this.profileHome = profileHome; this.expectedEmail = expectedEmail; }
    private int nextId;
    private bool disposed;
    public string? Email { get; private set; }

    public async Task<UsageSnapshot> ReadAsync()
    {
        await gate.WaitAsync(lifetime.Token);
        try
        {
            if (process is null || process.HasExited) await ConnectAsync();
            var account = await RequestAsync("account/read", new { refreshToken = false });
            if (!AccountMatches(account, expectedEmail))
                throw new AccountUnavailableException("Přihlas správný účet");
            Email = account.GetProperty("account").GetProperty("email").GetString();
            return UsageSnapshot.Parse(await RequestAsync("account/rateLimits/read", null));
        }
        catch { Disconnect(); throw; }
        finally { gate.Release(); }
    }

    public async Task LoginAsync(Action<Uri> openBrowser, CancellationToken cancellation)
    {
        if (profileHome is null) throw new InvalidOperationException("Přihlášení vyžaduje samostatný profil");
        await gate.WaitAsync(lifetime.Token);
        string? loginId = null;
        try
        {
            if (process is null || process.HasExited) await ConnectAsync();
            loginCompletion = new(TaskCreationOptions.RunContinuationsAsynchronously);
            var login = await RequestAsync("account/login/start", new { type = "chatgpt", useHostedLoginSuccessPage = true });
            loginId = login.GetProperty("loginId").GetString();
            var url = new Uri(login.GetProperty("authUrl").GetString()!);
            if (url.Scheme != "https" || !(url.Host.Equals("chatgpt.com", StringComparison.OrdinalIgnoreCase) ||
                url.Host.Equals("auth.openai.com", StringComparison.OrdinalIgnoreCase)))
                throw new InvalidDataException("Neplatná přihlašovací adresa");
            openBrowser(url);
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellation, lifetime.Token);
            timeout.CancelAfter(TimeSpan.FromMinutes(5));
            if (!await loginCompletion.Task.WaitAsync(timeout.Token))
                throw new AccountUnavailableException("Přihlášení se nepodařilo");
            var info = await RequestAsync("account/read", new { refreshToken = false });
            if (!AccountMatches(info, expectedEmail))
                throw new AccountUnavailableException("Byl přihlášen jiný účet");
            Email = info.GetProperty("account").GetProperty("email").GetString();
        }
        finally
        {
            loginCompletion = null;
            if (loginId is not null)
            {
                try { await RequestAsync("account/login/cancel", new { loginId }); } catch { }
            }
            gate.Release();
        }
    }

    public static bool AccountMatches(JsonElement response, string? expectedEmail) =>
        response.TryGetProperty("account", out var account) && account.ValueKind == JsonValueKind.Object &&
        account.TryGetProperty("type", out var type) && type.ValueKind == JsonValueKind.String && type.GetString() == "chatgpt" &&
        account.TryGetProperty("email", out var email) && email.ValueKind == JsonValueKind.String &&
        !string.IsNullOrWhiteSpace(email.GetString()) &&
        (expectedEmail is null || string.Equals(email.GetString(), expectedEmail, StringComparison.OrdinalIgnoreCase));

    private async Task ConnectAsync()
    {
        var executable = FindCodex();
        if (profileHome is not null) Directory.CreateDirectory(profileHome);
        process = new Process
        {
            StartInfo = new ProcessStartInfo(executable, "app-server")
            {
                UseShellExecute = false, CreateNoWindow = true,
                RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true,
                WorkingDirectory = AppContext.BaseDirectory,
            },
        };
        if (profileHome is not null)
        {
            process.StartInfo.Environment["CODEX_HOME"] = profileHome;
            process.StartInfo.ArgumentList.Add("app-server");
            process.StartInfo.ArgumentList.Add("-c");
            process.StartInfo.ArgumentList.Add("cli_auth_credentials_store=\"keyring\"");
            process.StartInfo.Arguments = "";
        }
        process.Start();
        var owned = process;
        _ = DrainErrorsAsync(owned);
        _ = ReadMessagesAsync(owned);
        await RequestAsync("initialize", new { clientInfo = new { name = "codex_usage_widget", title = "Codex Usage Widget", version = "1.0.0" } });
        await owned.StandardInput.WriteLineAsync(JsonSerializer.Serialize(new { method = "initialized", @params = new { } }));
        await owned.StandardInput.FlushAsync(lifetime.Token);
    }

    private async Task DrainErrorsAsync(Process owned)
    {
        try { while (await owned.StandardError.ReadLineAsync(lifetime.Token) is not null) { } }
        catch { }
    }

    private async Task ReadMessagesAsync(Process owned)
    {
        try
        {
            while (await owned.StandardOutput.ReadLineAsync(lifetime.Token) is { } line)
            {
                using var document = JsonDocument.Parse(line);
                var message = document.RootElement;
                if (message.TryGetProperty("method", out var method) && method.GetString() == "account/login/completed" &&
                    message.TryGetProperty("params", out var parameters) && parameters.TryGetProperty("success", out var success))
                    loginCompletion?.TrySetResult(success.ValueKind == JsonValueKind.True);
                if (!message.TryGetProperty("id", out var id) || !id.TryGetInt32(out var number) ||
                    !pending.TryRemove(number, out var completion)) continue;
                if (message.TryGetProperty("error", out _))
                    completion.TrySetException(new InvalidOperationException("Codex nemohl přečíst limity. Ověř přihlášení v Codexu"));
                else if (message.TryGetProperty("result", out var result)) completion.TrySetResult(result.Clone());
                else completion.TrySetException(new InvalidDataException("Neúplná odpověď Codexu"));
            }
        }
        catch { }
        finally
        {
            foreach (var entry in ReferenceEquals(process, owned) ? pending.ToArray() : Array.Empty<KeyValuePair<int, TaskCompletionSource<JsonElement>>>())
                if (pending.TryRemove(entry.Key, out var completion))
                    completion.TrySetException(new IOException("Spojení s Codexem bylo přerušeno"));
        }
    }

    private async Task<JsonElement> RequestAsync(string method, object? parameters)
    {
        int id = Interlocked.Increment(ref nextId);
        var completion = new TaskCompletionSource<JsonElement>(TaskCreationOptions.RunContinuationsAsynchronously);
        pending[id] = completion;
        try
        {
            var request = new Dictionary<string, object?> { ["method"] = method, ["id"] = id };
            if (parameters is not null) request["params"] = parameters;
            await process!.StandardInput.WriteLineAsync(JsonSerializer.Serialize(request));
            await process.StandardInput.FlushAsync(lifetime.Token);
            return await completion.Task.WaitAsync(TimeSpan.FromSeconds(20), lifetime.Token);
        }
        finally { pending.TryRemove(id, out _); }
    }

    public static string FindCodex()
    {
        var local = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OpenAI", "Codex", "bin");
        if (Directory.Exists(local))
        {
            var candidates = Directory.GetDirectories(local)
                .Select(directory => Path.Combine(directory, "codex.exe"))
                .Where(File.Exists).OrderByDescending(File.GetLastWriteTimeUtc).ToArray();
            if (candidates.Length > 0) return candidates[0];
        }
        foreach (var directory in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(Path.PathSeparator))
        {
            try { var candidate = Path.Combine(directory.Trim('"'), "codex.exe"); if (File.Exists(candidate)) return candidate; }
            catch (ArgumentException) { }
        }
        throw new FileNotFoundException("Codex nebyl nalezen. Nainstaluj desktopovou aplikaci Codex");
    }

    private void Disconnect()
    {
        var owned = process; process = null;
        if (owned is null) return;
        try { if (!owned.HasExited) owned.Kill(entireProcessTree: true); } catch { }
        owned.Dispose();
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        lifetime.Cancel();
        Disconnect();
        lifetime.Dispose();
    }
}
