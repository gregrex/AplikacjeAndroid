using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class PlamaRatownikDataIntegrityTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public void DataPack_HasRequiredFiles_AndPassesSharedValidator()
    {
        var root = FindRepositoryRoot();
        var dataDir = Path.Combine(root, "src", "AppFactory.Mobile", "wwwroot", "projects", "plama-ratownik");

        Assert.True(Directory.Exists(dataDir), $"Missing data directory: {dataDir}");

        var categories = ReadJson<List<CategoryDefinition>>(Path.Combine(dataDir, "categories.json"));
        var questions = ReadJson<List<QuestionDefinition>>(Path.Combine(dataDir, "questions.json"));
        var rules = ReadJson<List<RuleDefinition>>(Path.Combine(dataDir, "rules.json"));
        var results = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.pl.json"));

        Assert.NotEmpty(categories);
        Assert.NotEmpty(questions);
        Assert.NotEmpty(rules);
        Assert.NotEmpty(results);

        var validator = new DataPackValidationService();
        var errors = validator.Validate(categories, questions, rules, results);

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    private static T ReadJson<T>(string path)
    {
        Assert.True(File.Exists(path), $"Missing file: {path}");
        var json = File.ReadAllText(path);
        var value = JsonSerializer.Deserialize<T>(json, JsonOptions);
        Assert.NotNull(value);
        return value!;
    }

    private static string FindRepositoryRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, "src")) && Directory.Exists(Path.Combine(dir.FullName, "tests")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("Repository root not found.");
    }
}
