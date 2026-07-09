using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class AllProjectsQualityTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly string[] RequiredDataFiles =
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
    public void CatalogProjects_HaveCompleteSourceAndRuntimePacks()
    {
        var root = GetRepoRoot();
        var catalogProjects = new ProjectCatalogService().GetProjects();
        Assert.NotEmpty(catalogProjects);

        var errors = new List<string>();

        foreach (var project in catalogProjects)
        {
            var sourceDir = Path.Combine(root, "projects", project.Id);
            var dataDir = Path.Combine(sourceDir, "data");
            var runtimeDir = Path.Combine(root, "src", "AppFactory.Mobile", "wwwroot", "projects", project.Id);

            if (!Directory.Exists(sourceDir))
            {
                errors.Add($"{project.Id}: missing source project directory: {sourceDir}");
                continue;
            }

            if (!Directory.Exists(dataDir))
            {
                errors.Add($"{project.Id}: missing source data directory: {dataDir}");
                continue;
            }

            if (!Directory.Exists(runtimeDir))
            {
                errors.Add($"{project.Id}: missing runtime directory: {runtimeDir}");
                continue;
            }

            foreach (var file in RequiredDataFiles)
            {
                var sourceFile = Path.Combine(dataDir, file);
                var runtimeFile = Path.Combine(runtimeDir, file);

                if (!File.Exists(sourceFile))
                {
                    errors.Add($"{project.Id}: missing source data file {file}");
                }

                if (!File.Exists(runtimeFile))
                {
                    errors.Add($"{project.Id}: missing runtime data file {file}");
                }
            }

            if (!File.Exists(Path.Combine(sourceDir, "theme.json")))
            {
                errors.Add($"{project.Id}: missing source theme.json");
            }

            if (!File.Exists(Path.Combine(runtimeDir, "theme.json")))
            {
                errors.Add($"{project.Id}: missing runtime theme.json");
            }

            if (!File.Exists(Path.Combine(sourceDir, "marketing", "store-listing.pl.md")))
            {
                errors.Add($"{project.Id}: missing Polish store listing");
            }

            if (!File.Exists(Path.Combine(sourceDir, "tests", "manual-tests.md")))
            {
                errors.Add($"{project.Id}: missing manual tests");
            }
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void CatalogProjects_PassSharedDataValidator()
    {
        var root = GetRepoRoot();
        var catalogProjects = new ProjectCatalogService().GetProjects();
        var validator = new DataPackValidationService();
        var errors = new List<string>();

        foreach (var project in catalogProjects)
        {
            var dataDir = Path.Combine(root, "projects", project.Id, "data");
            if (!Directory.Exists(dataDir))
            {
                errors.Add($"{project.Id}: missing data directory");
                continue;
            }

            var categories = ReadJson<List<CategoryDefinition>>(Path.Combine(dataDir, "categories.json"));
            var questions = ReadJson<List<QuestionDefinition>>(Path.Combine(dataDir, "questions.json"));
            var rules = ReadJson<List<RuleDefinition>>(Path.Combine(dataDir, "rules.json"));
            var results = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.pl.json"));

            errors.AddRange(validator.Validate(categories, questions, rules, results).Select(error => $"{project.Id}: {error}"));
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void CatalogProjects_HaveLanguageParity()
    {
        var root = GetRepoRoot();
        var catalogProjects = new ProjectCatalogService().GetProjects();
        var errors = new List<string>();

        foreach (var project in catalogProjects)
        {
            var dataDir = Path.Combine(root, "projects", project.Id, "data");
            if (!Directory.Exists(dataDir))
            {
                errors.Add($"{project.Id}: missing data directory");
                continue;
            }

            var pl = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.pl.json"));
            var en = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.en.json"));
            var uk = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.uk.json"));

            var plIds = pl.Select(x => x.Id).OrderBy(x => x).ToArray();
            var enIds = en.Select(x => x.Id).OrderBy(x => x).ToArray();
            var ukIds = uk.Select(x => x.Id).OrderBy(x => x).ToArray();

            if (!plIds.SequenceEqual(enIds))
            {
                errors.Add($"{project.Id}: EN result ids differ from PL");
            }

            if (!plIds.SequenceEqual(ukIds))
            {
                errors.Add($"{project.Id}: UK result ids differ from PL");
            }

            foreach (var (language, results) in new[] { ("PL", pl), ("EN", en), ("UK", uk) })
            {
                foreach (var result in results)
                {
                    if (string.IsNullOrWhiteSpace(result.Id))
                    {
                        errors.Add($"{project.Id} {language}: result without id");
                    }

                    if (string.IsNullOrWhiteSpace(result.Title))
                    {
                        errors.Add($"{project.Id} {language}: result {result.Id} without title");
                    }

                    if (string.IsNullOrWhiteSpace(result.Summary))
                    {
                        errors.Add($"{project.Id} {language}: result {result.Id} without summary");
                    }

                    if (result.Steps.Count == 0)
                    {
                        errors.Add($"{project.Id} {language}: result {result.Id} without steps");
                    }
                }
            }
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void RuntimePacks_MatchSourceStructure()
    {
        var root = GetRepoRoot();
        var catalogProjects = new ProjectCatalogService().GetProjects();
        var errors = new List<string>();

        foreach (var project in catalogProjects)
        {
            var sourceDir = Path.Combine(root, "projects", project.Id);
            var dataDir = Path.Combine(sourceDir, "data");
            var runtimeDir = Path.Combine(root, "src", "AppFactory.Mobile", "wwwroot", "projects", project.Id);

            if (!Directory.Exists(dataDir) || !Directory.Exists(runtimeDir))
            {
                continue;
            }

            CompareAppId(project.Id, Path.Combine(dataDir, "app.json"), Path.Combine(runtimeDir, "app.json"), errors);
            CompareThemeId(project.Id, Path.Combine(sourceDir, "theme.json"), Path.Combine(runtimeDir, "theme.json"), errors);
            CompareIds<CategoryDefinition>(project.Id, "categories", Path.Combine(dataDir, "categories.json"), Path.Combine(runtimeDir, "categories.json"), x => x.Id, errors);
            CompareIds<QuestionDefinition>(project.Id, "questions", Path.Combine(dataDir, "questions.json"), Path.Combine(runtimeDir, "questions.json"), x => x.Id, errors);
            CompareIds<RuleDefinition>(project.Id, "rules", Path.Combine(dataDir, "rules.json"), Path.Combine(runtimeDir, "rules.json"), x => x.Id, errors);
            CompareIds<ResultDefinition>(project.Id, "results.pl", Path.Combine(dataDir, "results.pl.json"), Path.Combine(runtimeDir, "results.pl.json"), x => x.Id, errors);
            CompareIds<ResultDefinition>(project.Id, "results.en", Path.Combine(dataDir, "results.en.json"), Path.Combine(runtimeDir, "results.en.json"), x => x.Id, errors);
            CompareIds<ResultDefinition>(project.Id, "results.uk", Path.Combine(dataDir, "results.uk.json"), Path.Combine(runtimeDir, "results.uk.json"), x => x.Id, errors);
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    private static void CompareIds<T>(string projectId, string label, string sourcePath, string runtimePath, Func<T, string?> selector, List<string> errors)
    {
        if (!File.Exists(sourcePath) || !File.Exists(runtimePath))
        {
            return;
        }

        var sourceIds = ReadJson<List<T>>(sourcePath).Select(selector).Where(x => !string.IsNullOrWhiteSpace(x)).OrderBy(x => x).ToArray();
        var runtimeIds = ReadJson<List<T>>(runtimePath).Select(selector).Where(x => !string.IsNullOrWhiteSpace(x)).OrderBy(x => x).ToArray();

        if (!sourceIds.SequenceEqual(runtimeIds))
        {
            errors.Add($"{projectId}: runtime {label} ids differ from source");
        }
    }

    private static void CompareAppId(string projectId, string sourcePath, string runtimePath, List<string> errors)
    {
        if (!File.Exists(sourcePath) || !File.Exists(runtimePath))
        {
            return;
        }

        var source = ReadJson<AppConfig>(sourcePath);
        var runtime = ReadJson<AppConfig>(runtimePath);

        if (!string.Equals(source.AppId, runtime.AppId, StringComparison.Ordinal))
        {
            errors.Add($"{projectId}: runtime appId differs from source");
        }
    }

    private static void CompareThemeId(string projectId, string sourcePath, string runtimePath, List<string> errors)
    {
        if (!File.Exists(sourcePath) || !File.Exists(runtimePath))
        {
            return;
        }

        using var sourceJson = JsonDocument.Parse(File.ReadAllText(sourcePath));
        using var runtimeJson = JsonDocument.Parse(File.ReadAllText(runtimePath));

        var sourceThemeId = sourceJson.RootElement.TryGetProperty("themeId", out var sourceThemeIdProperty) ? sourceThemeIdProperty.GetString() : null;
        var runtimeThemeId = runtimeJson.RootElement.TryGetProperty("themeId", out var runtimeThemeIdProperty) ? runtimeThemeIdProperty.GetString() : null;

        if (!string.Equals(sourceThemeId, runtimeThemeId, StringComparison.Ordinal))
        {
            errors.Add($"{projectId}: runtime themeId differs from source");
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
