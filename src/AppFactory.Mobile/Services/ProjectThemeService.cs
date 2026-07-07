using System.Text.Json;
using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class ProjectThemeService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ThemeDefinition CurrentTheme { get; private set; } = new()
    {
        ThemeId = "default",
        BrandName = "AppFactory",
        TargetAudience = "general",
        Tone = "minimal-practical",
        VisualMood = "clean, simple, mobile-first"
    };

    public async Task<ThemeDefinition> LoadThemeAsync(string projectId)
    {
        try
        {
            await using var stream = await FileSystem.OpenAppPackageFileAsync($"projects/{projectId}/theme.json");
            var theme = await JsonSerializer.DeserializeAsync<ThemeDefinition>(stream, JsonOptions);
            CurrentTheme = theme ?? CurrentTheme;
        }
        catch
        {
            CurrentTheme = new ThemeDefinition
            {
                ThemeId = "fallback",
                BrandName = projectId,
                TargetAudience = "general",
                Tone = "minimal-practical",
                VisualMood = "fallback minimal UI"
            };
        }

        return CurrentTheme;
    }
}
