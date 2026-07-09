using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public static class MatchInfoParser
{
    public static MatchInfo Parse(string uri)
    {
        if (string.IsNullOrWhiteSpace(uri))
        {
            return new MatchInfo();
        }

        if (Uri.TryCreate(uri, UriKind.Absolute, out var parsedUri))
        {
            return ParseQuery(parsedUri.Query);
        }

        var queryStart = uri.IndexOf('?');
        return ParseQuery(queryStart >= 0 ? uri[queryStart..] : uri);
    }

    public static MatchInfo ParseQuery(string query)
    {
        var values = ParseQueryValues(query);

        return new MatchInfo
        {
            RuleId = GetValue(values, "ruleId"),
            Score = int.TryParse(GetValue(values, "score"), out var score) ? score : 0,
            Reason = GetValue(values, "reason"),
            MatchedConditions = SplitList(GetValue(values, "matched")),
            AlternativePremiumResultIds = SplitList(GetValue(values, "alternatives"))
        };
    }

    private static Dictionary<string, string> ParseQueryValues(string query)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(query))
        {
            return result;
        }

        var normalizedQuery = query.StartsWith("?", StringComparison.Ordinal) ? query[1..] : query;
        foreach (var part in normalizedQuery.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var separatorIndex = part.IndexOf('=');
            if (separatorIndex < 0)
            {
                result[Uri.UnescapeDataString(part)] = string.Empty;
                continue;
            }

            var key = Uri.UnescapeDataString(part[..separatorIndex]);
            var value = Uri.UnescapeDataString(part[(separatorIndex + 1)..]);
            result[key] = value;
        }

        return result;
    }

    private static string GetValue(Dictionary<string, string> values, string key)
    {
        return values.TryGetValue(key, out var value) ? value : string.Empty;
    }

    private static List<string> SplitList(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? new List<string>()
            : value.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
    }
}
