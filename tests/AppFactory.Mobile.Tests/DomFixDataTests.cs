using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class DomFixDataTests
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
    public void Scenario_HighRisk_StopsBeforeRepairInstruction()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "problem", Value = "clogged_drain" },
            new() { QuestionId = "place", Value = "bathroom" },
            new() { QuestionId = "tools", Value = "basic" },
            new() { QuestionId = "risk", Value = "high" },
            new() { QuestionId = "water", Value = "leak" },
            new() { QuestionId = "material", Value = "mixed" }
        };

        var match = engine.Match("drain", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("high_risk_stop", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("high_risk_stop_premium", result!.Id);
    }

    [Fact]
    public void Scenario_SqueakyDoor_ReturnsDoorInstruction()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "problem", Value = "squeaky_door" },
            new() { QuestionId = "place", Value = "room" },
            new() { QuestionId = "tools", Value = "basic" },
            new() { QuestionId = "risk", Value = "low" },
            new() { QuestionId = "water", Value = "none" },
            new() { QuestionId = "material", Value = "metal" }
        };

        var match = engine.Match("door", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("squeaky_door", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("squeaky_door_premium", result!.Id);
    }

    [Fact]
    public void Scenario_CloggedDrain_ReturnsDrainInstruction()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "problem", Value = "clogged_drain" },
            new() { QuestionId = "place", Value = "bathroom" },
            new() { QuestionId = "tools", Value = "cleaning" },
            new() { QuestionId = "risk", Value = "low" },
            new() { QuestionId = "water", Value = "small" },
            new() { QuestionId = "material", Value = "mixed" }
        };

        var match = engine.Match("drain", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("clogged_drain", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("clogged_drain_premium", result!.Id);
    }

    private static DomFixDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new DomFixDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "domfix", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("domfix data directory not found.");
    }

    private sealed class DomFixDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
