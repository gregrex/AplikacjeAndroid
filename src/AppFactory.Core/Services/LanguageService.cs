namespace AppFactory.Mobile.Services;

public sealed class LanguageService
{
    private static readonly HashSet<string> SupportedLanguages = new(StringComparer.OrdinalIgnoreCase)
    {
        "pl",
        "en",
        "uk"
    };

    public string CurrentLanguage { get; private set; } = "pl";

    public IReadOnlyList<string> GetSupportedLanguages()
    {
        return SupportedLanguages.OrderBy(x => x).ToList();
    }

    public void SetLanguage(string language)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            CurrentLanguage = "pl";
            return;
        }

        CurrentLanguage = SupportedLanguages.Contains(language) ? language.ToLowerInvariant() : "pl";
    }
}
