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
        Check(CodexConnection.AccountMatches(account.RootElement, null));
        Check(!CodexConnection.AccountMatches(account.RootElement, ""));
        using var noAccount = JsonDocument.Parse("""{"account":null}""");
        Check(!CodexConnection.AccountMatches(noAccount.RootElement, "work@example.com"));
        Check(Widget.FormatReset(new DateTimeOffset(2026, 10, 8, 17, 5, 0, TimeSpan.FromHours(2)), 0) == "17:05");
        Check(Widget.FormatReset(new DateTimeOffset(2026, 10, 8, 0, 5, 0, TimeSpan.FromHours(2)), 0) == "00:05");
        Check(!CodexConnection.AccountMatches(noAccount.RootElement, null));
        using var emptyEmail = JsonDocument.Parse("""{"account":{"type":"chatgpt","email":""}}""");
        Check(!CodexConnection.AccountMatches(emptyEmail.RootElement, null));
        using var apiKey = JsonDocument.Parse("""{"account":{"type":"apiKey","email":"user@example.com"}}""");
        Check(!CodexConnection.AccountMatches(apiKey.RootElement, null));
        Check(AccountConfiguration.Default().Length == 1 && AccountConfiguration.Default()[0].Email is null);
        var single = AccountConfiguration.Parse("""{"accounts":[{"id":"one","name":"My account","icon":"A"}]}""");
        Check(single.Length == 1 && single[0].Email is null && single[0].AvatarText == "A");
        var multiple = AccountConfiguration.Parse("""{"accounts":[{"id":"first","name":"First","email":"first@example.com"},{"id":"second","name":"Second","email":"second@example.com"},{"id":"third","name":"Third"}]}""");
        Check(multiple.Length == 3 && multiple[1].Email == "second@example.com");
        var migrated = AccountConfiguration.Parse("""{"existing":"user@example.com","unused":""}""");
        Check(migrated.Length == 1 && migrated[0].Id == "existing");
        Check(AccountConfiguration.Parse("{}").Length == 1);
        void RejectConfiguration(string json) {
            try { AccountConfiguration.Parse(json); }
            catch (InvalidDataException) { count++; return; }
            throw new InvalidOperationException("Invalid account configuration was accepted");
        }
        RejectConfiguration("""{"accounts":[]}""");
        RejectConfiguration("""{"accounts":[{"id":"../other","name":"Unsafe"}]}""");
        RejectConfiguration("""{"accounts":[{"id":"same","name":"First"},{"id":"SAME","name":"Second"}]}""");
        RejectConfiguration("""{"accounts":[{"id":"one","name":"Bad email","email":"invalid"}]}""");
        RejectConfiguration("""{"accounts":[{"id":"one","name":"Bad icon","icon":"LONG"}]}""");
        RejectConfiguration("""{"accounts":[null]}""");
        RejectConfiguration("""{"accounts":"wrong"}""");
        RejectConfiguration("{");
        Check(new AccountDefinition { Name = "Alex Smith" }.AvatarText == "AS");
        Check(new AccountDefinition { Name = "Codex", Icon = "💼" }.AvatarText == "💼");
        Check(Widget.WidthForAccounts(1) == 121 && Widget.WidthForAccounts(2) == 240 && Widget.WidthForAccounts(3) == 359);
        return count;
    }
}
