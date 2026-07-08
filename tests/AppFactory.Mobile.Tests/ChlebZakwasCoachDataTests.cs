using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class ChlebZakwasCoachDataTests
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
    public void Scenario_MoldSmell_ReturnsDiscardPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "problem", Value = "odd_smell" },
            new() { QuestionId = "stage", Value = "starter" },
            new() { QuestionId = "temperature", Value = "normal" },
            new() { QuestionId = "time", Value = "long" },
            new() { QuestionId = "smell", Value = "mold" }
        };

        var match = engine.Match("odd_smell", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("discard_mold", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("discard_mold_premium", result!.Id);
    }

    [Fact]
    public void Scenario_StarterNotRising_ReturnsRescuePlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "problem", Value = "starter_not_rising" },
            new() { QuestionId = "stage", Value = "starter" },
            new() { QuestionId = "temperature", Value = "cold" },
            new() { QuestionId = "time", Value = "ok" },
            new() { QuestionId = "smell", Value = "acidic" }
        };

        var match = engine.Match("starter_not_rising", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("starter_not_rising", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("starter_not_rising_premium", result!.Id);
    }

    [Fact]
    public void Scenario_PaleCrust_ReturnsCrustPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "problem", Value = "pale_crust" },
            new() { QuestionId = "stage", Value = "baking" },
            new() { QuestionId = "temperature", Value = "normal" },
            new() { QuestionId = "time", Value = "short" },
            new() { QuestionId = "smell", Value = "normal" }
        };

        var match = engine.Match("pale_crust", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("pale_crust", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("pale_crust_premium", result!.Id);
    }

    private static ChlebZakwasCoachDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new ChlebZakwasCoachDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "chleb-zakwas-coach", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("chleb-zakwas-coach data directory not found.");
    }

    private sealed class ChlebZakwasCoachDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
