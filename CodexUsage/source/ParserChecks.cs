using System;
using System.IO;
using System.Text.Json;

namespace CodexUsage;

public static class ParserChecks
{
    public static int Run()
    {
        int count = 0;
        UsageSnapshot Parse(string json) { using var document = JsonDocument.Parse(json); return UsageSnapshot.Parse(document.RootElement); }
        void Check(bool passed) { if (!passed) throw new InvalidOperationException("Quota parser check failed"); count++; }
        void Reject(string json) { try { Parse(json); } catch (InvalidDataException) { count++; return; } throw new InvalidOperationException("Invalid quota was accepted"); }
        const string window = """{"usedPercent":25,"windowDurationMins":300,"resetsAt":1791498130}""";
        var current = Parse("""{"rateLimits":{"primary":{"usedPercent":99,"windowDurationMins":300}},"rateLimitsByLimitId":{"codex":{"primary":""" + window + """}}}""");
        Check(current.FiveHour?.UsedPercent == 25 && current.Weekly is null);
        Check(Parse("""{"rateLimits":{"primary":""" + window + """}}""").FiveHour?.ResetsAt == 1791498130);
        Reject("""{"rateLimits":{"primary":null,"secondary":null}}""");
        Reject("""{"rateLimits":{"primary":{"usedPercent":101,"windowDurationMins":300}}}""");
        Reject("""{"rateLimits":{"primary":{"usedPercent":25,"windowDurationMins":15}}}""");
        Reject("""{"rateLimits":{"primary":{"usedPercent":25,"windowDurationMins":300,"resetsAt":-1}}}""");
        using var account = JsonDocument.Parse("""{"account":{"type":"chatgpt","email":"work@example.com"}}""");
        Check(CodexConnection.AccountMatches(account.RootElement, "WORK@example.com"));
        Check(!CodexConnection.AccountMatches(account.RootElement, "personal@example.com"));
        Check(!CodexConnection.AccountMatches(account.RootElement, null));
        using var noAccount = JsonDocument.Parse("""{"account":null}""");
        Check(!CodexConnection.AccountMatches(noAccount.RootElement, "work@example.com"));
        Check(Widget.FormatReset(new DateTimeOffset(2026, 10, 8, 17, 5, 0, TimeSpan.FromHours(2)), 0) == "17:05");
        Check(Widget.FormatReset(new DateTimeOffset(2026, 10, 8, 0, 5, 0, TimeSpan.FromHours(2)), 0) == "00:05");
        return count;
    }
}
