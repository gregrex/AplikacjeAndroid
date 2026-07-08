using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class PakowaniePaczekDataTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public void DataPack_PassesSharedValidator()
    {
        var data = LoadDataPack();
        var validator = new DataPackValidationService();

        var errors = validator.Validate(data.Categories, data.Questions, data.Rules, data.Results);

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void Scenario_Books_ReturnsCornerChecklist()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "item_type", Value = "books" },
            new() { QuestionId = "fragility", Value = "medium" },
            new() { QuestionId = "size", Value = "medium" },
            new() { QuestionId = "transport", Value = "courier" },
            new() { QuestionId = "value", Value = "medium" }
        };

        var match = engine.Match("books", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("books_corners", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("books_corners_premium", result!.Id);
    }

    [Fact]
    public void Scenario_Clothes_ReturnsSoftPackingChecklist()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "item_type", Value = "clothes" },
            new() { QuestionId = "fragility", Value = "low" },
            new() { QuestionId = "size", Value = "medium" },
            new() { QuestionId = "transport", Value = "locker" },
            new() { QuestionId = "value", Value = "low" }
        };

        var match = engine.Match("clothes", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("clothes_soft", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("clothes_soft_premium", result!.Id);
    }

    private static PakowaniePaczekDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new PakowaniePaczekDataPack
        {
            Categories = ReadJson<List<CategoryDefinition>>(Path.Combine(dataDir, "categories.json")),
            Questions = ReadJson<List<QuestionDefinition>>(Path.Combine(dataDir, "questions.json")),
            Rules = ReadJson<List<RuleDefinition>>(Path.Combine(dataDir, "rules.json")),
            Results = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.pl.json"))
        };
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
            var candidate = Path.Combine(dir.FullName, "projects", "pakowanie-paczek", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }
            dir = dir.Parent;
        }
        throw new DirectoryNotFoundException("pakowanie-paczek data directory not found.");
    }

    private sealed class PakowaniePaczekDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
