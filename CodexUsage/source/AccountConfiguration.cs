using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CodexUsage;

public sealed class AccountDefinition
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Email { get; set; }
    public string? Icon { get; set; }

    public string AvatarText
    {
        get {
            if (!string.IsNullOrWhiteSpace(Icon)) return Icon;
            var words = Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return string.Concat(words.Take(2).Select(word => StringInfo.GetNextTextElement(word))).ToUpperInvariant();
        }
    }
}

public static class AccountConfiguration
{
    public static string UserDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CodexUsage");
    public static string FileName => Path.Combine(UserDirectory, "accounts.json");
    private static readonly JsonSerializerOptions JsonOptions = new() {
        PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true
    };

    public static AccountDefinition[] Default() => [new() { Id = "default", Name = "Codex", Icon = "C" }];

    public static AccountDefinition[] Load(string path, string legacyPath)
    {
        if (File.Exists(path)) return Parse(File.ReadAllText(path));
        var accounts = File.Exists(legacyPath) ? Parse(File.ReadAllText(legacyPath)) : Default();
        Save(path, accounts);
        return accounts;
    }

    public static AccountDefinition[] Parse(string json)
    {
        try {
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object) throw new InvalidDataException("Nastavení musí obsahovat objekt accounts");
            if (root.TryGetProperty("accounts", out var entries)) {
                if (entries.ValueKind != JsonValueKind.Array) throw new InvalidDataException("accounts musí být seznam účtů");
                var accounts = entries.Deserialize<AccountDefinition[]>(JsonOptions);
                if (accounts is null || accounts.Any(a => a is null)) throw new InvalidDataException("Neplatný seznam účtů");
                return Validate(accounts);
            }
            // Preserve profile IDs when migrating the previous email dictionary.
            var legacy = new List<AccountDefinition>();
            foreach (var property in root.EnumerateObject()) {
                if (property.Value.ValueKind != JsonValueKind.String) throw new InvalidDataException("Neplatný formát nastavení");
                if (!string.IsNullOrWhiteSpace(property.Value.GetString()))
                    legacy.Add(new() { Id = property.Name, Name = property.Name, Email = property.Value.GetString() });
            }
            return legacy.Count > 0 ? Validate(legacy) : Default();
        }
        catch (JsonException e) { throw new InvalidDataException("accounts.json není platný JSON", e); }
    }

    public static AccountDefinition[] Validate(IEnumerable<AccountDefinition> definitions)
    {
        var accounts = definitions.Select(a => new AccountDefinition {
            Id = a.Id?.Trim() ?? "", Name = a.Name?.Trim() ?? "",
            Email = string.IsNullOrWhiteSpace(a.Email) ? null : a.Email.Trim(),
            Icon = string.IsNullOrWhiteSpace(a.Icon) ? null : a.Icon.Trim()
        }).ToArray();
        if (accounts.Length == 0) throw new InvalidDataException("Ponech alespoň jeden účet");
        if (accounts.Length > 8) throw new InvalidDataException("Miniokno podporuje nejvýše osm účtů");
        var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var account in accounts) {
            if (!Regex.IsMatch(account.Id, @"^[a-zA-Z0-9][a-zA-Z0-9_-]{0,63}$") || !ids.Add(account.Id))
                throw new InvalidDataException("Účty musí mít jedinečné ID tvořené písmeny, číslicemi, pomlčkami a podtržítky");
            if (account.Name.Length is 0 or > 64 || account.Name.Any(char.IsControl))
                throw new InvalidDataException("Vyplň název účtu (nejvýše 64 znaků)");
            if (account.Email is not null && (!MailAddress.TryCreate(account.Email, out var parsed) ||
                !string.Equals(parsed.Address, account.Email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidDataException("Vyplň platný e-mail, nebo ho ponech prázdný");
            if (account.Icon is not null && (new StringInfo(account.Icon).LengthInTextElements > 2 || account.Icon.Any(char.IsControl)))
                throw new InvalidDataException("Ikona může obsahovat nejvýše dva znaky nebo emoji");
        }
        return accounts;
    }

    public static void Save(string path, IEnumerable<AccountDefinition> accounts)
    {
        var normalized = Validate(accounts);
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
        File.WriteAllText(path + ".tmp", JsonSerializer.Serialize(new { accounts = normalized.Select(a =>
            new { a.Id, a.Name, a.Email, a.Icon }).ToArray() }, JsonOptions), new UTF8Encoding(false));
        File.Move(path + ".tmp", path, true);
    }
}
