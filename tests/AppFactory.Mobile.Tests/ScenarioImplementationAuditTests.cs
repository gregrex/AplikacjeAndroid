using System.Text.Json;
using System.Text.RegularExpressions;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class ScenarioImplementationAuditTests
{
    [Fact]
    public void EveryProductionScenario_HasImplementedActionsAndBusinessLogic()
    {
        var root = GetRepoRoot();
        var errors = new List<string>();
        var projects = new ProjectCatalogService().GetProjects();

        foreach (var project in projects)
        {
            var scenarioPath = Path.Combine(root, "projects", project.Id, "tests", "production-scenarios.md");
            var content = File.ReadAllText(scenarioPath);
            var scenarios = SplitScenarios(content);

            foreach (var scenario in scenarios)
            {
                var capabilities = InferCapabilities(scenario.Body);
                capabilities.Add("catalog");
                capabilities.Add("theme");
                capabilities.Add("quiz");
                capabilities.Add("rule-engine");
                capabilities.Add("result-data");

                foreach (var capability in capabilities)
                {
                    if (!IsCapabilityImplemented(root, project.Id, capability, out var evidence))
                    {
                        errors.Add($"{project.Id}/{scenario.Id}: missing capability '{capability}' ({evidence})");
                    }
                }
            }
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void EveryScenarioProject_HasFiveReachableBusinessFlows()
    {
        var root = GetRepoRoot();
        var errors = new List<string>();

        foreach (var project in new ProjectCatalogService().GetProjects())
        {
            var scenarioPath = Path.Combine(root, "projects", project.Id, "tests", "production-scenarios.md");
            var scenarios = SplitScenarios(File.ReadAllText(scenarioPath));
            var rulesPath = Path.Combine(root, "projects", project.Id, "data", "rules.json");
            var questionsPath = Path.Combine(root, "projects", project.Id, "data", "questions.json");
            var categoriesPath = Path.Combine(root, "projects", project.Id, "data", "categories.json");

            if (scenarios.Count != 5)
            {
                errors.Add($"{project.Id}: expected five scenarios, found {scenarios.Count}");
            }

            if (!HasJsonItems(rulesPath))
            {
                errors.Add($"{project.Id}: no business rules");
            }

            if (!HasJsonItems(questionsPath))
            {
                errors.Add($"{project.Id}: no quiz questions");
            }

            if (!HasJsonItems(categoriesPath))
            {
                errors.Add($"{project.Id}: no categories");
            }
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void PersistentActions_UseDeviceStorageAndSupportReopening()
    {
        var root = GetRepoRoot();
        var mobile = Path.Combine(root, "src", "AppFactory.Mobile");
        var errors = new List<string>();

        RequireTokens(errors, Path.Combine(mobile, "Services", "HistoryService.cs"), "Database.AddHistoryAsync", "Database.GetHistoryAsync", "ClearAsync");
        RequireTokens(errors, Path.Combine(mobile, "Services", "FavoritesService.cs"), "Database.AddFavoriteAsync", "Database.GetFavoritesAsync", "RemoveAsync", "ClearAsync");
        RequireTokens(errors, Path.Combine(mobile, "Pages", "History.razor"), "Otwórz wynik", "Wyczyść historię", "ProjectContext.SelectProject");
        RequireTokens(errors, Path.Combine(mobile, "Pages", "Favorites.razor"), "Otwórz zapisany wynik", "Usuń z ulubionych", "ProjectContext.SelectProject");
        RequireTokens(errors, Path.Combine(mobile, "Pages", "Result.razor"), "FreeResultId = FreeResultId", "PremiumResultId = PremiumResultId");

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    private static HashSet<string> InferCapabilities(string body)
    {
        var text = body.ToLowerInvariant();
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        AddWhen(result, text, "premium", "premium", "odblokuj", "pełną wersję", "pełny wynik", "pełną instrukcję");
        AddWhen(result, text, "favorites", "ulubion");
        AddWhen(result, text, "history", "histori");
        AddWhen(result, text, "localization", "język", "pl, en", "en i uk", "pl/en/uk");
        AddWhen(result, text, "fallback", "fallback", "domyśln", "bez reguły", "nietypową kombinację", "nieznany");
        AddWhen(result, text, "alternatives", "alternatyw");
        AddWhen(result, text, "clipboard", "schowek", "kopiuj", "skopi");
        AddWhen(result, text, "image-ai", "local-vision-v1", "analiza zdjęcia", "analizę zdjęcia", "analiza obrazu", "obraz onnx");
        AddWhen(result, text, "audio-ai", "local-audio-v1", "analiza dźwięku", "analizę dźwięku", "nagranie", "audio onnx");
        AddWhen(result, text, "project-tools", "licznik rzędów", "notatki robótki", "notatkę o włóczce");
        AddWhen(result, text, "persistence", "ponownie uruchom", "zamknij i", "restart", "pozostaje dostępna");
        AddWhen(result, text, "safety", "bezpieczeń", "ryzy", "fachow", "pleś", "spaleniz", "prąd", "gaz", "wyciek", "nie mieszaj");

        return result;
    }

    private static void AddWhen(HashSet<string> result, string text, string capability, params string[] markers)
    {
        if (markers.Any(marker => text.Contains(marker, StringComparison.OrdinalIgnoreCase)))
        {
            result.Add(capability);
        }
    }

    private static bool IsCapabilityImplemented(string root, string projectId, string capability, out string evidence)
    {
        evidence = capability;
        var mobile = Path.Combine(root, "src", "AppFactory.Mobile");
        var project = Path.Combine(root, "projects", projectId);

        return capability switch
        {
            "catalog" => FileContains(Path.Combine(mobile, "Services", "ProjectCatalogService.cs"), $"\"{projectId}\"", out evidence),
            "theme" => File.Exists(Path.Combine(project, "theme.json")) && File.Exists(Path.Combine(mobile, "wwwroot", "projects", projectId, "theme.json")),
            "quiz" => FileContainsAll(Path.Combine(mobile, "Pages", "Quiz.razor"), out evidence, "SelectAnswer", "ShowResult", "RuleEngine.Match"),
            "rule-engine" => FileContainsAll(Path.Combine(root, "src", "AppFactory.Core", "Services", "RuleEngineService.cs"), out evidence, "ConditionsMatch", "FreeResultId", "PremiumResultId"),
            "result-data" => HasResultData(project),
            "premium" => FileContainsAll(Path.Combine(mobile, "Pages", "Result.razor"), out evidence, "Wszystkie kroki są dostępne", "ShowPremiumBlocks=\"true\"", "PremiumResultId"),
            "favorites" => FileContainsAll(Path.Combine(mobile, "Pages", "Result.razor"), out evidence, "Favorites.AddAsync", "FavoriteEntry")
                           && FileContainsAll(Path.Combine(mobile, "Pages", "Favorites.razor"), out evidence, "Otwórz zapisany wynik", "Usuń z ulubionych"),
            "history" => FileContainsAll(Path.Combine(mobile, "Pages", "Result.razor"), out evidence, "History.AddAsync", "HistoryEntry")
                         && FileContainsAll(Path.Combine(mobile, "Pages", "History.razor"), out evidence, "Otwórz wynik", "Wyczyść historię"),
            "localization" => HasLanguageFiles(project)
                              && FileContainsAll(Path.Combine(root, "src", "AppFactory.Core", "Services", "LanguageService.cs"), out evidence, "GetResultLanguageWithFallback", "CurrentLanguage"),
            "fallback" => HasGlobalFallback(Path.Combine(project, "data", "rules.json")),
            "alternatives" => FileContainsAll(Path.Combine(root, "src", "AppFactory.Core", "Services", "RuleEngineService.cs"), out evidence, "AlternativeRuleIds", "AlternativePremiumResultIds")
                              && FileContains(Path.Combine(mobile, "Pages", "Result.razor"), "SelectAlternative", out evidence),
            "clipboard" => new ProjectUiProfileService().GetProfile(projectId).ShowCopyAction
                           && FileContainsAll(Path.Combine(mobile, "Components", "ProjectResultView.razor"), out evidence, "CopyResult", "ClipboardExport.CopyResultAsync"),
            "image-ai" => new ImageAnalysisPolicyService().GetPolicy(projectId).IsEnabled
                          && FileContainsAll(Path.Combine(mobile, "Components", "LocalAiPanel.razor"), out evidence, "AnalyzeImageAsync", "PickImageAsync", "ImageAnalysis.AnalyzeAsync")
                          && FileContains(Path.Combine(mobile, "Services", "OnDeviceImageAnalysisProvider.cs"), "local", out evidence),
            "audio-ai" => new AudioAnalysisPolicyService().GetPolicy(projectId).IsEnabled
                          && FileContainsAll(Path.Combine(mobile, "Components", "LocalAiPanel.razor"), out evidence, "AnalyzeAudioAsync", "PickAudioAsync", "AudioAnalysis.AnalyzeAsync")
                          && FileContains(Path.Combine(mobile, "Services", "OnDeviceAudioAnalysisProvider.cs"), "local", out evidence),
            "project-tools" => new ProjectUiProfileService().GetProfile(projectId).ToolKind == "crochet-counter"
                               && FileContainsAll(Path.Combine(mobile, "Pages", "ProjectTools.razor"), out evidence, "Increase", "SaveNotes", "ToolState")
                               && FileContains(Path.Combine(mobile, "Services", "ProjectToolStateService.cs"), "Preferences.Default", out evidence),
            "persistence" => HasPersistenceEvidence(mobile, projectId, out evidence),
            "safety" => new ProjectUiProfileService().GetProfile(projectId).ShowSafetyBlock
                        || HasWarnings(Path.Combine(project, "data", "results.pl.json")),
            _ => false
        };
    }

    private static bool HasPersistenceEvidence(string mobileRoot, string projectId, out string evidence)
    {
        evidence = "local persistence";
        if (projectId == "szydelko-pomocnik")
        {
            return FileContains(Path.Combine(mobileRoot, "Services", "ProjectToolStateService.cs"), "Preferences.Default", out evidence);
        }

        return FileContainsAll(Path.Combine(mobileRoot, "Services", "HistoryService.cs"), out evidence, "Database.AddHistoryAsync", "Database.GetHistoryAsync")
               && FileContainsAll(Path.Combine(mobileRoot, "Services", "FavoritesService.cs"), out evidence, "Database.AddFavoriteAsync", "Database.GetFavoritesAsync");
    }

    private static bool HasResultData(string projectRoot) =>
        File.Exists(Path.Combine(projectRoot, "data", "rules.json"))
        && File.Exists(Path.Combine(projectRoot, "data", "results.pl.json"))
        && File.Exists(Path.Combine(projectRoot, "data", "results.en.json"))
        && File.Exists(Path.Combine(projectRoot, "data", "results.uk.json"));

    private static bool HasLanguageFiles(string projectRoot) =>
        File.Exists(Path.Combine(projectRoot, "data", "results.pl.json"))
        && File.Exists(Path.Combine(projectRoot, "data", "results.en.json"))
        && File.Exists(Path.Combine(projectRoot, "data", "results.uk.json"));

    private static bool HasJsonItems(string path)
    {
        using var document = JsonDocument.Parse(File.ReadAllText(path));
        return document.RootElement.ValueKind == JsonValueKind.Array && document.RootElement.GetArrayLength() > 0;
    }

    private static bool HasGlobalFallback(string rulesPath)
    {
        using var document = JsonDocument.Parse(File.ReadAllText(rulesPath));
        return document.RootElement.EnumerateArray().Any(rule =>
            rule.TryGetProperty("categoryId", out var category)
            && category.GetString() == "*"
            && rule.TryGetProperty("when", out var when)
            && when.ValueKind == JsonValueKind.Object
            && !when.EnumerateObject().Any());
    }

    private static bool HasWarnings(string resultsPath)
    {
        using var document = JsonDocument.Parse(File.ReadAllText(resultsPath));
        return document.RootElement.EnumerateArray().Any(result =>
            result.TryGetProperty("warnings", out var warnings)
            && warnings.ValueKind == JsonValueKind.Array
            && warnings.GetArrayLength() > 0);
    }

    private static bool FileContains(string path, string marker, out string evidence)
    {
        evidence = $"{Path.GetFileName(path)}::{marker}";
        return File.Exists(path) && File.ReadAllText(path).Contains(marker, StringComparison.OrdinalIgnoreCase);
    }

    private static bool FileContainsAll(string path, out string evidence, params string[] markers)
    {
        evidence = $"{Path.GetFileName(path)}::{string.Join("+", markers)}";
        if (!File.Exists(path))
        {
            return false;
        }

        var content = File.ReadAllText(path);
        return markers.All(marker => content.Contains(marker, StringComparison.OrdinalIgnoreCase));
    }

    private static void RequireTokens(List<string> errors, string path, params string[] tokens)
    {
        if (!File.Exists(path))
        {
            errors.Add($"missing file: {path}");
            return;
        }

        var content = File.ReadAllText(path);
        foreach (var token in tokens)
        {
            if (!content.Contains(token, StringComparison.OrdinalIgnoreCase))
            {
                errors.Add($"{Path.GetFileName(path)}: missing '{token}'");
            }
        }
    }

    private static IReadOnlyList<ScenarioBlock> SplitScenarios(string content)
    {
        var matches = Regex.Matches(content, @"^## (SCN-\d{2})\s+—\s+.+$", RegexOptions.Multiline);
        var scenarios = new List<ScenarioBlock>();

        for (var index = 0; index < matches.Count; index++)
        {
            var start = matches[index].Index;
            var end = index + 1 < matches.Count ? matches[index + 1].Index : content.Length;
            scenarios.Add(new ScenarioBlock(matches[index].Groups[1].Value, content[start..end]));
        }

        return scenarios;
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

    private sealed record ScenarioBlock(string Id, string Body);
}
