using System.Text.Json;
using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Tests;

public sealed class PlamaRatownikDataIntegrityTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public void DataPack_HasRequiredFiles_AndConsistentReferences()
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

        var categoryIds = categories.Select(x => x.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var questionIds = questions.Select(x => x.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var resultIds = results.Select(x => x.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var rule in rules)
        {
            Assert.False(string.IsNullOrWhiteSpace(rule.Id));
            Assert.True(rule.CategoryId == "*" || categoryIds.Contains(rule.CategoryId), $"Rule {rule.Id} references missing category {rule.CategoryId}");
            Assert.True(resultIds.Contains(rule.FreeResultId), $"Rule {rule.Id} references missing free result {rule.FreeResultId}");
            Assert.True(resultIds.Contains(rule.PremiumResultId), $"Rule {rule.Id} references missing premium result {rule.PremiumResultId}");

            foreach (var condition in rule.When.Keys)
            {
                Assert.True(questionIds.Contains(condition), $"Rule {rule.Id} references missing question {condition}");
            }
        }
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
