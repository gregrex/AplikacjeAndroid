using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class PokojMakeoverDataTests
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
    public void Scenario_Minimal_ReturnsDeclutterPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "room_type", Value = "bedroom" },
            new() { QuestionId = "style", Value = "minimal" },
            new() { QuestionId = "budget", Value = "low" },
            new() { QuestionId = "problem", Value = "clutter" },
            new() { QuestionId = "time", Value = "weekend" }
        };

        var match = engine.Match("minimal", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("minimal_declutter", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("minimal_declutter_premium", result!.Id);
    }

    [Fact]
    public void Scenario_Gaming_ReturnsGamingZonePlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "room_type", Value = "teen" },
            new() { QuestionId = "style", Value = "gaming" },
            new() { QuestionId = "budget", Value = "medium" },
            new() { QuestionId = "problem", Value = "clutter" },
            new() { QuestionId = "time", Value = "weekend" }
        };

        var match = engine.Match("gaming", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("gaming_zone", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("gaming_zone_premium", result!.Id);
    }

    [Fact]
    public void Scenario_StudyWork_ReturnsDeskPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "room_type", Value = "study" },
            new() { QuestionId = "style", Value = "study_work" },
            new() { QuestionId = "budget", Value = "low" },
            new() { QuestionId = "problem", Value = "bad_desk" },
            new() { QuestionId = "time", Value = "one_hour" }
        };

        var match = engine.Match("study_work", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("study_work_desk", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("study_work_desk_premium", result!.Id);
    }

    private static PokojMakeoverDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new PokojMakeoverDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "pokoj-makeover", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("pokoj-makeover data directory not found.");
    }

    private sealed class PokojMakeoverDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
