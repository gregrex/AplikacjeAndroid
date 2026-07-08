using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class KolekDobieraczDataTests
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
    public void Scenario_ConcreteLight_ReturnsConcreteResult()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "wall", Value = "concrete" },
            new() { QuestionId = "weight", Value = "light" }
        };

        var match = engine.Match("picture", answers, data.Rules);
        var free = resultService.FindResult(data.Results, match.FreeResultId);
        var premium = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("concrete_light", match.RuleId);
        Assert.NotNull(free);
        Assert.NotNull(premium);
        Assert.Equal("concrete_light_free", free!.Id);
        Assert.Equal("concrete_light_premium", premium!.Id);
    }

    [Fact]
    public void Scenario_HeavyWeight_ReturnsRiskResult()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "wall", Value = "concrete" },
            new() { QuestionId = "weight", Value = "heavy" }
        };

        var match = engine.Match("shelf", answers, data.Rules);
        var free = resultService.FindResult(data.Results, match.FreeResultId);
        var premium = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("heavy_or_unknown", match.RuleId);
        Assert.NotNull(free);
        Assert.NotNull(premium);
        Assert.Equal("risk_free", free!.Id);
        Assert.Equal("risk_premium", premium!.Id);
    }

    private static KolekDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new KolekDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "kolek-dobieracz", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("kolek-dobieracz data directory not found.");
    }

    private sealed class KolekDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
