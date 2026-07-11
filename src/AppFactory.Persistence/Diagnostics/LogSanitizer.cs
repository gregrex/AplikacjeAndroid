using System.Text.RegularExpressions;

namespace AppFactory.Persistence.Diagnostics;

public static partial class LogSanitizer
{
    public static string Sanitize(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var sanitized = value.Replace("\0", string.Empty, StringComparison.Ordinal);
        sanitized = EmailRegex().Replace(sanitized, "[REDACTED_EMAIL]");
        sanitized = SecretAssignmentRegex().Replace(sanitized, "$1=[REDACTED]");
        sanitized = SensitiveQueryRegex().Replace(sanitized, "$1[REDACTED]");
        sanitized = BearerRegex().Replace(sanitized, "Bearer [REDACTED]");

        if (sanitized.Length > maxLength)
        {
            sanitized = $"{sanitized[..maxLength]}…[TRUNCATED]";
        }

        return sanitized;
    }

    [GeneratedRegex(@"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"(?i)\b(password|passwd|token|access[_ -]?token|refresh[_ -]?token|api[_ -]?key|secret|authorization)\b\s*[:=]\s*[^\s,;]+", RegexOptions.CultureInvariant)]
    private static partial Regex SecretAssignmentRegex();

    [GeneratedRegex(@"(?i)([?&](?:token|access_token|refresh_token|key|api_key|secret|password|signature|sig)=)[^&\s]+", RegexOptions.CultureInvariant)]
    private static partial Regex SensitiveQueryRegex();

    [GeneratedRegex(@"(?i)Bearer\s+[A-Za-z0-9._~+\-/]+=*", RegexOptions.CultureInvariant)]
    private static partial Regex BearerRegex();
}
