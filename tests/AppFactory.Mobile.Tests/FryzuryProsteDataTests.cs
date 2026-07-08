using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class FryzuryProsteDataTests
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
    public void Scenario_QuickFive_ReturnsQuickPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "hair_type", Value = "straight" },
            new() { QuestionId = "time", Value = "five" },
            new() { QuestionId = "occasion", Value = "work" },
            new() { QuestionId = "accessories", Value = "elastic" },
            new() { QuestionId = "finish", Value = "natural" }
        };

        var match = engine.Match("quick", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("quick_five", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("quick_five_premium", result!.Id);
    }

    [Fact]
    public void Scenario_Sport_ReturnsSportHoldPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "hair_type", Value = "long" },
            new() { QuestionId = "time", Value = "ten" },
            new() { QuestionId = "occasion", Value = "sport" },
            new() { QuestionId = "accessories", Value = "elastic" },
            new() { QuestionId = "finish", Value = "hold" }
        };

        var match = engine.Match("sport", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("sport_hold", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("sport_hold_premium", result!.Id);
    }

    [Fact]
    public void Scenario_CurlyHair_ReturnsCurlySoftPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "hair_type", Value = "curly" },
            new() { QuestionId = "time", Value = "ten" },
            new() { QuestionId = "occasion", Value = "school" },
            new() { QuestionId = "accessories", Value = "clip" },
            new() { QuestionId = "finish", Value = "natural" }
        };

        var match = engine.Match("everyday", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("everyday_low", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("everyday_low_premium", result!.Id);
    }

    private static FryzuryProsteDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new FryzuryProsteDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "fryzury-proste", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("fryzury-proste data directory not found.");
    }

    private sealed class FryzuryProsteDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
