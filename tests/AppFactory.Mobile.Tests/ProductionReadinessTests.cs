using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class ProductionReadinessTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly string[] RequiredSourceDataFiles =
    {
        "app.json",
        "categories.json",
        "questions.json",
        "rules.json",
        "results.pl.json",
        "results.en.json",
        "results.uk.json"
    };

    private static readonly string[] RequiredRuntimeDataFiles =
    {
        "app.json",
        "categories.json",
        "questions.json",
        "rules.json",
        "results.pl.json",
        "results.en.json",
        "results.uk.json"
    };

    [Fact]
    public void ProductionChecklistInfrastructure_Exists()
    {
        var root = GetRepoRoot();
        var requiredFiles = new[]
        {
            "docs/quality/PRODUCTION_CHECKLIST.md",
            "docs/quality/PRODUCTION_EXECUTION_REPORT.md",
            "docs/quality/QUALITY_STATUS.md",
            "docs/quality/RULE_ENGINE_V2.md",
            "docs/quality/BUILD_PROFILES.md",
            "docs/quality/IMAGE_ANALYSIS_V1.md",
            "docs/quality/LOCAL_AI_ON_DEVICE.md",
            "docs/quality/PROJECT_SCENARIO_COVERAGE.md",
            "docs/quality/SCENARIO_EXECUTION_TRACKER.md",
            "docs/quality/UI_UX_AUDIT.md",
            "docs/quality/SCENARIO_IMPLEMENTATION_AUDIT.md",
            "src/AppFactory.Mobile/Components/LocalAiPanel.razor",
            "src/AppFactory.Mobile/Pages/ProjectTools.razor",
            "src/AppFactory.Mobile/Services/LocalMediaInputService.cs",
            "src/AppFactory.Mobile/Services/ProjectToolStateService.cs",
            "tools/quality/run-quality-checks.ps1",
            "tools/quality/sync-runtime-packs.ps1",
            "tools/quality/write-project-quality-report.ps1",
            "tools/quality/write-build-profiles.ps1",
            ".github/workflows/quality-checks.yml"
        };

        var missing = requiredFiles
            .Select(path => Path.Combine(root, path.Replace('/', Path.DirectorySeparatorChar)))
            .Where(path => !File.Exists(path))
            .ToArray();

        Assert.True(missing.Length == 0, "Missing production checklist infrastructure:" + Environment.NewLine + string.Join(Environment.NewLine, missing));
    }

    [Fact]
    public void LocalAiInfrastructure_UsesOnnxRuntime()
    {
        var root = GetRepoRoot();
        var coreProject = File.ReadAllText(Path.Combine(root, "src", "AppFactory.Core", "AppFactory.Core.csproj"));
        var mobileProject = File.ReadAllText(Path.Combine(root, "src", "AppFactory.Mobile", "AppFactory.Mobile.csproj"));

        Assert.Contains("Microsoft.ML.OnnxRuntime", coreProject);
        Assert.Contains("Microsoft.ML.OnnxRuntime", mobileProject);
        Assert.Equal(new[] { 1, 1, 1, 256 }, LocalAiInputTensorFactory.DefaultInputShape);
        Assert.Equal("input", LocalAiInputTensorFactory.DefaultInputName);
    }

    [Fact]
    public void EveryCatalogProject_MeetsProductionChecklistDataRequirements()
    {
        var root = GetRepoRoot();
        var projects = new ProjectCatalogService().GetProjects();
        var errors = new List<string>();

        foreach (var project in projects)
        {
            var sourceDir = Path.Combine(root, "projects", project.Id);
            var sourceDataDir = Path.Combine(sourceDir, "data");
            var runtimeDir = Path.Combine(root, "src", "AppFactory.Mobile", "wwwroot", "projects", project.Id);

            foreach (var file in RequiredSourceDataFiles)
            {
                RequireFile(errors, project.Id, Path.Combine(sourceDataDir, file), $"source {file}");
            }

            foreach (var file in RequiredRuntimeDataFiles)
            {
                RequireFile(errors, project.Id, Path.Combine(runtimeDir, file), $"runtime {file}");
            }

            RequireFile(errors, project.Id, Path.Combine(sourceDir, "theme.json"), "source theme.json");
            RequireFile(errors, project.Id, Path.Combine(runtimeDir, "theme.json"), "runtime theme.json");
            RequireFile(errors, project.Id, Path.Combine(sourceDir, "marketing", "store-listing.pl.md"), "Polish store listing");
            RequireFile(errors, project.Id, Path.Combine(sourceDir, "tests", "manual-tests.md"), "manual tests");
            RequireFile(errors, project.Id, Path.Combine(sourceDir, "tests", "production-scenarios.md"), "five production scenarios");
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void EveryCatalogProject_HasRuleReasonsAndLanguageParity()
    {
        var root = GetRepoRoot();
        var projects = new ProjectCatalogService().GetProjects();
        var errors = new List<string>();

        foreach (var project in projects)
        {
            var sourceDataDir = Path.Combine(root, "projects", project.Id, "data");
            var runtimeDir = Path.Combine(root, "src", "AppFactory.Mobile", "wwwroot", "projects", project.Id);

            var sourceRules = ReadJson<List<RuleDefinition>>(Path.Combine(sourceDataDir, "rules.json"));
            var runtimeRules = ReadJson<List<RuleDefinition>>(Path.Combine(runtimeDir, "rules.json"));

            foreach (var rule in sourceRules)
            {
                if (string.IsNullOrWhiteSpace(rule.Reason))
                {
                    errors.Add($"{project.Id}: source rule {rule.Id} has empty reason");
                }
            }

            foreach (var rule in runtimeRules)
            {
                if (string.IsNullOrWhiteSpace(rule.Reason))
                {
                    errors.Add($"{project.Id}: runtime rule {rule.Id} has empty reason");
                }
            }

            var sourceRuleIds = sourceRules.Select(x => x.Id).OrderBy(x => x).ToArray();
            var runtimeRuleIds = runtimeRules.Select(x => x.Id).OrderBy(x => x).ToArray();
            if (!sourceRuleIds.SequenceEqual(runtimeRuleIds))
            {
                errors.Add($"{project.Id}: source/runtime rule ids differ");
            }

            AssertResultLanguageParity(errors, project.Id, sourceDataDir);
            AssertResultLanguageParity(errors, project.Id, runtimeDir);
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void BuildProfiles_CoverCatalogAndEveryProject()
    {
        var projects = new ProjectCatalogService().GetProjects();
        var profiles = new BuildProfileService().GetAllProfiles(projects);
        var profileProjectIds = profiles.Select(x => x.ProjectId).ToHashSet(StringComparer.OrdinalIgnoreCase);

        Assert.Contains(profiles, x => x.IsCatalogBuild && x.ApplicationId == "pl.gbcom.appfactory");

        foreach (var project in projects)
        {
            Assert.Contains(project.Id, profileProjectIds);
            var profile = profiles.Single(x => string.Equals(x.ProjectId, project.Id, StringComparison.OrdinalIgnoreCase));
            Assert.False(profile.IsCatalogBuild);
            Assert.Equal(project.Name, profile.ApplicationTitle);
            Assert.StartsWith("pl.gbcom.appfactory.", profile.ApplicationId, StringComparison.Ordinal);
            Assert.DoesNotContain("-", profile.ApplicationId);
        }
    }

    [Fact]
    public void ImageAnalysisPolicy_CoversExpectedProjectsOnly()
    {
        var enabled = new ImageAnalysisPolicyService().GetEnabledProjectIds();

        Assert.Equal(8, enabled.Count);
        Assert.Contains("plama-ratownik", enabled);
        Assert.Contains("pokoj-makeover", enabled);
        Assert.Contains("rysunek-coach", enabled);
        Assert.Contains("outfit-coach", enabled);
        Assert.Contains("fryzury-proste", enabled);
        Assert.Contains("barber-translator", enabled);
        Assert.Contains("zmywarka-diagnosta", enabled);
        Assert.Contains("silikon-fuga-fix", enabled);
    }

    [Fact]
    public void AudioAnalysisPolicy_CoversExpectedProjectsOnly()
    {
        var enabled = new AudioAnalysisPolicyService().GetEnabledProjectIds();

        Assert.Equal(3, enabled.Count);
        Assert.Contains("zmywarka-diagnosta", enabled);
        Assert.Contains("pies-trener-7dni", enabled);
        Assert.Contains("kot-bawi-sie", enabled);
    }

    [Fact]
    public void LocalAiModelCatalog_HasVisionAndAudioProfiles()
    {
        var catalog = new LocalAiModelCatalogService();

        Assert.NotNull(catalog.FindByModality("image"));
        Assert.NotNull(catalog.FindByModality("audio"));
        Assert.Contains(catalog.GetAll(), x => x.ModelId == "local-vision-v1");
        Assert.Contains(catalog.GetAll(), x => x.ModelId == "local-audio-v1");
    }

    [Fact]
    public void QualityStatus_HasNoBlockingRiskMarkers()
    {
        var root = GetRepoRoot();
        var statusPath = Path.Combine(root, "docs", "quality", "QUALITY_STATUS.md");
        var status = File.ReadAllText(statusPath);

        Assert.DoesNotContain("blokujące ryzyko", status, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("wymaga osobnego wyrównania", status, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("brakuje", status, StringComparison.OrdinalIgnoreCase);
    }

    private static void AssertResultLanguageParity(List<string> errors, string projectId, string dataDir)
    {
        var pl = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.pl.json"));
        var en = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.en.json"));
        var uk = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.uk.json"));

        var plIds = pl.Select(x => x.Id).OrderBy(x => x).ToArray();
        var enIds = en.Select(x => x.Id).OrderBy(x => x).ToArray();
        var ukIds = uk.Select(x => x.Id).OrderBy(x => x).ToArray();

        if (!plIds.SequenceEqual(enIds))
        {
            errors.Add($"{projectId}: EN result ids differ from PL in {dataDir}");
        }

        if (!plIds.SequenceEqual(ukIds))
        {
            errors.Add($"{projectId}: UK result ids differ from PL in {dataDir}");
        }
    }

    private static void RequireFile(List<string> errors, string projectId, string path, string label)
    {
        if (!File.Exists(path))
        {
            errors.Add($"{projectId}: missing {label}: {path}");
        }
    }

    private static T ReadJson<T>(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json, JsonOptions) ?? throw new InvalidOperationException($"Cannot read {path}");
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
