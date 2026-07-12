using System.Text.Json;
using System.Text.RegularExpressions;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class UiUxProductionTests
{
    private static readonly HashSet<string> AllowedResultViews = new(StringComparer.OrdinalIgnoreCase)
    {
        "instruction-checklist",
        "technical-safety-checklist",
        "seven-day-plan",
        "story-page",
        "sales-copy-card",
        "pet-activity-plan",
        "style-checklist",
        "creative-lesson",
        "arrangement-plan",
        "packing-checklist",
        "craft-helper",
        "diagnostic-checklist"
    };

    [Fact]
    public void EveryProject_HasCompleteDedicatedUiProfile()
    {
        var service = new ProjectUiProfileService();
        var errors = new List<string>();
        var badges = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var project in new ProjectCatalogService().GetProjects())
        {
            var profile = service.GetProfile(project.Id);

            Require(errors, project.Id, "ProjectId", profile.ProjectId);
            Require(errors, project.Id, "Icon", profile.Icon);
            Require(errors, project.Id, "Badge", profile.Badge);
            Require(errors, project.Id, "HeroTitle", profile.HeroTitle);
            Require(errors, project.Id, "HeroDescription", profile.HeroDescription);
            Require(errors, project.Id, "CategoryHeading", profile.CategoryHeading);
            Require(errors, project.Id, "QuizHeading", profile.QuizHeading);
            Require(errors, project.Id, "ResultHeading", profile.ResultHeading);
            Require(errors, project.Id, "PrimaryActionLabel", profile.PrimaryActionLabel);
            Require(errors, project.Id, "SecondaryActionLabel", profile.SecondaryActionLabel);

            if (!string.Equals(profile.ProjectId, project.Id, StringComparison.OrdinalIgnoreCase))
            {
                errors.Add($"{project.Id}: profile returned fallback ProjectId '{profile.ProjectId}'");
            }

            if (!AllowedResultViews.Contains(profile.ResultViewType))
            {
                errors.Add($"{project.Id}: unsupported ResultViewType '{profile.ResultViewType}'");
            }

            if (!badges.Add(profile.Badge))
            {
                errors.Add($"{project.Id}: badge is not project-specific: '{profile.Badge}'");
            }
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void EveryProject_HasValidSourceAndRuntimeTheme()
    {
        var root = GetRepoRoot();
        var errors = new List<string>();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        foreach (var project in new ProjectCatalogService().GetProjects())
        {
            var sourcePath = Path.Combine(root, "projects", project.Id, "theme.json");
            var runtimePath = Path.Combine(root, "src", "AppFactory.Mobile", "wwwroot", "projects", project.Id, "theme.json");

            ValidateTheme(errors, project.Id, "source", sourcePath, options);
            ValidateTheme(errors, project.Id, "runtime", runtimePath, options);
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void SharedUi_ContainsProductionNavigationAndAccessibleActions()
    {
        var root = GetRepoRoot();
        var mobile = Path.Combine(root, "src", "AppFactory.Mobile");
        var errors = new List<string>();

        RequireFileTokens(errors, Path.Combine(mobile, "Layout", "MainLayout.razor"), "bottom-nav", "/history", "/favorites", "/settings");
        RequireFileTokens(errors, Path.Combine(mobile, "Pages", "Home.razor"), "project-grid", "Szukaj aplikacji", "Otwórz aplikację");
        RequireFileTokens(errors, Path.Combine(mobile, "Pages", "Categories.razor"), "LocalAiPanel", "category-grid", "/quiz/");
        RequireFileTokens(errors, Path.Combine(mobile, "Pages", "Quiz.razor"), "answer-button", "quiz-progress", "Pokaż wynik");
        RequireFileTokens(errors, Path.Combine(mobile, "Pages", "Result.razor"), "Dlaczego taki wynik?", "Wszystkie kroki są dostępne", "AddFavorite");
        RequireFileTokens(errors, Path.Combine(mobile, "Components", "LocalAiPanel.razor"), "Features.LocalAiEnabled", "PickImageAsync", "PickAudioAsync", "AnalyzeAsync");
        RequireFileTokens(errors, Path.Combine(mobile, "Pages", "ProjectTools.razor"), "Licznik rzędów", "SaveNotes", "ToolState");
        RequireFileTokens(errors, Path.Combine(mobile, "Pages", "Settings.razor"), "/privacy", "/support", "Diagnostyka");
        RequireFileTokens(errors, Path.Combine(mobile, "Pages", "Privacy.razor"), "Polityka prywatności", "gbosko@gbcom.pl");
        RequireFileTokens(errors, Path.Combine(mobile, "Pages", "Support.razor"), "Wsparcie AppFactory", "gbosko@gbcom.pl");

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void CopyAndToolCapabilities_AreEnabledForExpectedProjects()
    {
        var service = new ProjectUiProfileService();

        Assert.True(service.GetProfile("vinted-olx-opis").ShowCopyAction);
        Assert.True(service.GetProfile("barber-translator").ShowCopyAction);
        Assert.Equal("crochet-counter", service.GetProfile("szydelko-pomocnik").ToolKind);
        Assert.True(string.IsNullOrWhiteSpace(service.GetProfile("router-wifi-diagnosta").ToolKind));
    }

    private static void ValidateTheme(List<string> errors, string projectId, string location, string path, JsonSerializerOptions options)
    {
        if (!File.Exists(path))
        {
            errors.Add($"{projectId}: missing {location} theme");
            return;
        }

        var theme = JsonSerializer.Deserialize<ThemeDefinition>(File.ReadAllText(path), options);
        if (theme is null)
        {
            errors.Add($"{projectId}: cannot deserialize {location} theme");
            return;
        }

        Require(errors, projectId, $"{location}.ThemeId", theme.ThemeId);
        Require(errors, projectId, $"{location}.BrandName", theme.BrandName);
        Require(errors, projectId, $"{location}.TargetAudience", theme.TargetAudience);
        Require(errors, projectId, $"{location}.Tone", theme.Tone);
        Require(errors, projectId, $"{location}.VisualMood", theme.VisualMood);

        ValidateColor(errors, projectId, $"{location}.PrimaryColor", theme.PrimaryColor);
        ValidateColor(errors, projectId, $"{location}.SecondaryColor", theme.SecondaryColor);
        ValidateColor(errors, projectId, $"{location}.BackgroundColor", theme.BackgroundColor);
        ValidateColor(errors, projectId, $"{location}.SurfaceColor", theme.SurfaceColor);
        ValidateColor(errors, projectId, $"{location}.TextColor", theme.TextColor);
    }

    private static void ValidateColor(List<string> errors, string projectId, string field, string value)
    {
        if (!Regex.IsMatch(value ?? string.Empty, "^#[0-9A-Fa-f]{6}$"))
        {
            errors.Add($"{projectId}: invalid {field} '{value}'");
        }
    }

    private static void Require(List<string> errors, string projectId, string field, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add($"{projectId}: empty {field}");
        }
    }

    private static void RequireFileTokens(List<string> errors, string path, params string[] tokens)
    {
        if (!File.Exists(path))
        {
            errors.Add($"missing UI file: {path}");
            return;
        }

        var content = File.ReadAllText(path);
        foreach (var token in tokens)
        {
            if (!content.Contains(token, StringComparison.OrdinalIgnoreCase))
            {
                errors.Add($"{Path.GetFileName(path)}: missing UI action/token '{token}'");
            }
        }
    }

    private static string GetRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, "projects")) && Directory.Exists(Path.Combine(dir.FullName, "src")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("Repository root not found.");
    }
}
