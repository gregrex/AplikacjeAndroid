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

    [Theory]
    [InlineData("pl")]
    [InlineData("en")]
    [InlineData("uk")]
    public void SetLanguage_AcceptsSupportedLanguages(string language)
    {
        var service = new LanguageService();

        service.SetLanguage(language);

        Assert.Equal(language, service.CurrentLanguage);
    }

    [Theory]
    [InlineData("de")]
    [InlineData("fr")]
    [InlineData("")]
    public void SetLanguage_FallsBackToPolish_ForUnsupportedLanguage(string language)
    {
        var service = new LanguageService();

        service.SetLanguage(language);

        Assert.Equal("pl", service.CurrentLanguage);
    }
}
