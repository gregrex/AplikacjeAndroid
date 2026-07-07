using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class ThemeCssBuilderTests
{
    [Fact]
    public void BuildCssVariables_ContainsExpectedThemeTokens()
    {
        var builder = new ThemeCssBuilder();
        var theme = new ThemeDefinition
        {
            PrimaryColor = "#111111",
            SecondaryColor = "#222222",
            AccentColor = "#333333",
            BackgroundColor = "#444444",
            SurfaceColor = "#555555",
            TextColor = "#666666",
            MutedTextColor = "#777777",
            DangerColor = "#888888"
        };

        var css = builder.BuildCssVariables(theme);

        Assert.Contains("--primary: #111111;", css);
        Assert.Contains("--secondary: #222222;", css);
        Assert.Contains("--accent: #333333;", css);
        Assert.Contains("--bg: #444444;", css);
        Assert.Contains("--card: #555555;", css);
        Assert.Contains("--text: #666666;", css);
        Assert.Contains("--muted: #777777;", css);
        Assert.Contains("--danger: #888888;", css);
    }
}
