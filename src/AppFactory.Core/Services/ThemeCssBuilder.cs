using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class ThemeCssBuilder
{
    public string BuildCssVariables(ThemeDefinition theme)
    {
        return string.Join(Environment.NewLine, new[]
        {
            ":root {",
            $"  --primary: {theme.PrimaryColor};",
            $"  --secondary: {theme.SecondaryColor};",
            $"  --accent: {theme.AccentColor};",
            $"  --bg: {theme.BackgroundColor};",
            $"  --card: {theme.SurfaceColor};",
            $"  --text: {theme.TextColor};",
            $"  --muted: {theme.MutedTextColor};",
            $"  --danger: {theme.DangerColor};",
            "}"
        });
    }
}
