using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class PiesTrenerDataTests
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
    public void Scenario_PuppyEasy_ReturnsPuppyPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "age", Value = "puppy" },
            new() { QuestionId = "energy", Value = "medium" },
            new() { QuestionId = "level", Value = "easy" }
        };

        var match = engine.Match("sit", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("puppy_easy", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("puppy_easy_premium", result!.Id);
    }

    [Fact]
    public void Scenario_HighEnergy_ReturnsHighEnergyPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "age", Value = "adult" },
            new() { QuestionId = "energy", Value = "high" },
            new() { QuestionId = "level", Value = "normal" }
        };

        var match = engine.Match("come", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("high_energy", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("high_energy_premium", result!.Id);
    }

    private static PiesDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new PiesDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "pies-trener-7dni", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("pies-trener-7dni data directory not found.");
    }

    private sealed class PiesDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
