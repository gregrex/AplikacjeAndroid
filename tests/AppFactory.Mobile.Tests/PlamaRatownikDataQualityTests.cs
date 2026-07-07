using System.Text.Json;
using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Tests;

public sealed class PlamaRatownikDataQualityTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public void DataPack_HasUniqueIds()
    {
        var dataDir = GetDataDir();

        var categories = ReadJson<List<CategoryDefinition>>(Path.Combine(dataDir, "categories.json"));
        var questions = ReadJson<List<QuestionDefinition>>(Path.Combine(dataDir, "questions.json"));
        var rules = ReadJson<List<RuleDefinition>>(Path.Combine(dataDir, "rules.json"));
        var results = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.pl.json"));

        AssertUnique(categories.Select(x => x.Id), "category id");
        AssertUnique(questions.Select(x => x.Id), "question id");
        AssertUnique(rules.Select(x => x.Id), "rule id");
        AssertUnique(results.Select(x => x.Id), "result id");
    }

    [Fact]
    public void Results_HaveRequiredContent()
    {
        var dataDir = GetDataDir();
        var results = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.pl.json"));

        foreach (var result in results)
        {
            Assert.False(string.IsNullOrWhiteSpace(result.Id), "Result id cannot be empty.");
            Assert.False(string.IsNullOrWhiteSpace(result.Title), $"Result {result.Id} title cannot be empty.");
            Assert.False(string.IsNullOrWhiteSpace(result.Summary), $"Result {result.Id} summary cannot be empty.");
            Assert.NotEmpty(result.Steps);
            Assert.All(result.Steps, step => Assert.False(string.IsNullOrWhiteSpace(step), $"Result {result.Id} has empty step."));
        }
    }

    [Fact]
    public void Questions_HaveOptionsForSingleChoice()
    {
        var dataDir = GetDataDir();
        var questions = ReadJson<List<QuestionDefinition>>(Path.Combine(dataDir, "questions.json"));

        foreach (var question in questions.Where(x => x.Type == "single"))
        {
            Assert.NotEmpty(question.Options);
            Assert.All(question.Options, option =>
            {
                Assert.False(string.IsNullOrWhiteSpace(option.Value), $"Question {question.Id} has empty option value.");
                Assert.False(string.IsNullOrWhiteSpace(option.LabelKey), $"Question {question.Id} has empty option label key.");
            });
        }
    }

    private static void AssertUnique(IEnumerable<string> values, string name)
    {
        var list = values.ToList();
        var duplicates = list
            .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .ToList();

        Assert.True(duplicates.Count == 0, $"Duplicate {name}: {string.Join(", ", duplicates)}");
    }

    private static T ReadJson<T>(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json, JsonOptions) ?? throw new InvalidOperationException($"Cannot read {path}");
    }

    private static string GetDataDir()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "src", "AppFactory.Mobile", "wwwroot", "projects", "plama-ratownik");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("plama-ratownik data directory not found.");
    }
}
