using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class LanguageServiceTests
{
    [Fact]
    public void DefaultLanguage_IsPolish()
    {
        var service = new LanguageService();

        Assert.Equal("pl", service.CurrentLanguage);
    }

    [Fact]
    public void GetSupportedLanguages_ReturnsAllEuOfficialLanguagesPlusUkrainian()
    {
        var service = new LanguageService();
        var codes = service.GetSupportedLanguageCodes();

        var expected = new[]
        {
            "bg", "hr", "cs", "da", "nl", "en", "et", "fi", "fr", "de", "el", "hu", "ga", "it", "lv", "lt", "mt", "pl", "pt", "ro", "sk", "sl", "es", "sv", "uk"
        };

        Assert.Equal(expected.OrderBy(x => x), codes.OrderBy(x => x));
    }

    [Theory]
    [InlineData("pl")]
    [InlineData("en")]
    [InlineData("uk")]
    [InlineData("de")]
    [InlineData("fr")]
    [InlineData("es")]
    [InlineData("it")]
    [InlineData("nl")]
    [InlineData("sv")]
    public void SetLanguage_AcceptsSupportedLanguages(string language)
    {
        var service = new LanguageService();

        service.SetLanguage(language);

        Assert.Equal(language, service.CurrentLanguage);
    }

    [Theory]
    [InlineData("no")]
    [InlineData("ru")]
    [InlineData("")]
    public void SetLanguage_FallsBackToPolish_ForUnsupportedLanguage(string language)
    {
        var service = new LanguageService();

        service.SetLanguage(language);

        Assert.Equal("pl", service.CurrentLanguage);
    }

    [Fact]
    public void GetResultLanguageWithFallback_ReturnsEnglish_WhenSelectedLanguageHasNoResults()
    {
        var service = new LanguageService();
        service.SetLanguage("de");

        var resultLanguage = service.GetResultLanguageWithFallback(new[] { "pl", "en", "uk" });

        Assert.Equal("en", resultLanguage);
    }

    [Fact]
    public void GetResultLanguageWithFallback_ReturnsSelectedLanguage_WhenAvailable()
    {
        var service = new LanguageService();
        service.SetLanguage("uk");

        var resultLanguage = service.GetResultLanguageWithFallback(new[] { "pl", "en", "uk" });

        Assert.Equal("uk", resultLanguage);
    }
}
