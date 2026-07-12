using Microsoft.Maui.Storage;

namespace AppFactory.Mobile.Services;

public sealed class LanguageService
{
    private const string PreferenceKey = "appfactory:language:v1";

    private static readonly IReadOnlyList<LanguageDefinition> Languages = new List<LanguageDefinition>
    {
        new("bg", "Български", true),
        new("hr", "Hrvatski", true),
        new("cs", "Čeština", true),
        new("da", "Dansk", true),
        new("nl", "Nederlands", true),
        new("en", "English", true),
        new("et", "Eesti", true),
        new("fi", "Suomi", true),
        new("fr", "Français", true),
        new("de", "Deutsch", true),
        new("el", "Ελληνικά", true),
        new("hu", "Magyar", true),
        new("ga", "Gaeilge", true),
        new("it", "Italiano", true),
        new("lv", "Latviešu", true),
        new("lt", "Lietuvių", true),
        new("mt", "Malti", true),
        new("pl", "Polski", true),
        new("pt", "Português", true),
        new("ro", "Română", true),
        new("sk", "Slovenčina", true),
        new("sl", "Slovenščina", true),
        new("es", "Español", true),
        new("sv", "Svenska", true),
        new("uk", "Українська", false)
    };

    private static readonly HashSet<string> SupportedLanguageCodes = Languages
        .Select(x => x.Code)
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    public LanguageService()
    {
        CurrentLanguage = Normalize(Preferences.Default.Get(PreferenceKey, "pl"));
    }

    public string CurrentLanguage { get; private set; }

    public IReadOnlyList<LanguageDefinition> GetSupportedLanguages() => Languages;

    public IReadOnlyList<string> GetSupportedLanguageCodes() => Languages.Select(x => x.Code).ToArray();

    public void SetLanguage(string language)
    {
        CurrentLanguage = Normalize(language);
        Preferences.Default.Set(PreferenceKey, CurrentLanguage);
    }

    public string GetResultLanguageWithFallback(IReadOnlyCollection<string> availableResultLanguages)
    {
        if (availableResultLanguages.Contains(CurrentLanguage, StringComparer.OrdinalIgnoreCase))
        {
            return CurrentLanguage;
        }

        if (availableResultLanguages.Contains("en", StringComparer.OrdinalIgnoreCase))
        {
            return "en";
        }

        return "pl";
    }

    private static string Normalize(string? language)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            return "pl";
        }

        var normalized = language.Trim().ToLowerInvariant();
        return SupportedLanguageCodes.Contains(normalized) ? normalized : "pl";
    }
}

public sealed record LanguageDefinition(string Code, string NativeName, bool IsEuropeanUnionLanguage);
