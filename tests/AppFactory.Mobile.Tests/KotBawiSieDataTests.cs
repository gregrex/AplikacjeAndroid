using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class KotBawiSieDataTests
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
    public void Scenario_KittenHighEnergy_ReturnsKittenPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "age", Value = "kitten" },
            new() { QuestionId = "energy", Value = "high" },
            new() { QuestionId = "time", Value = "10" },
            new() { QuestionId = "toy", Value = "fishing_rod" }
        };

        var match = engine.Match("kitten", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("kitten_high", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("kitten_high_premium", result!.Id);
    }

    [Fact]
    public void Scenario_CardboardDiy_ReturnsCardboardPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "age", Value = "adult" },
            new() { QuestionId = "energy", Value = "medium" },
            new() { QuestionId = "time", Value = "20" },
            new() { QuestionId = "toy", Value = "cardboard" }
        };

        var match = engine.Match("diy", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("cardboard_diy", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("cardboard_diy_premium", result!.Id);
    }

    [Fact]
    public void Scenario_Senior_ReturnsSeniorPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "age", Value = "senior" },
            new() { QuestionId = "energy", Value = "low" },
            new() { QuestionId = "time", Value = "5" },
            new() { QuestionId = "toy", Value = "no_toys" }
        };

        var match = engine.Match("senior", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("senior_calm", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("senior_calm_premium", result!.Id);
    }

    private static CatPlayDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new CatPlayDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "kot-bawi-sie", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("kot-bawi-sie data directory not found.");
    }

    private sealed class CatPlayDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
