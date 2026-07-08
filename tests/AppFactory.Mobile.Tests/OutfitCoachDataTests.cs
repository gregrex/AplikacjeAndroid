using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class OutfitCoachDataTests
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
    public void Scenario_Interview_ReturnsInterviewSmartPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "occasion", Value = "interview" },
            new() { QuestionId = "weather", Value = "mild" },
            new() { QuestionId = "style", Value = "smart" },
            new() { QuestionId = "top", Value = "shirt" },
            new() { QuestionId = "bottom", Value = "chinos" },
            new() { QuestionId = "shoes", Value = "loafers" }
        };

        var match = engine.Match("interview", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("interview_smart", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("interview_smart_premium", result!.Id);
    }

    [Fact]
    public void Scenario_MeetingSmart_ReturnsMeetingPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "occasion", Value = "meeting" },
            new() { QuestionId = "weather", Value = "mild" },
            new() { QuestionId = "style", Value = "smart" },
            new() { QuestionId = "top", Value = "shirt" },
            new() { QuestionId = "bottom", Value = "chinos" },
            new() { QuestionId = "shoes", Value = "loafers" }
        };

        var match = engine.Match("meeting", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("meeting_smart", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("meeting_smart_premium", result!.Id);
    }

    [Fact]
    public void Scenario_Walk_ReturnsWeatherWalkPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "occasion", Value = "walk" },
            new() { QuestionId = "weather", Value = "rain" },
            new() { QuestionId = "style", Value = "casual" },
            new() { QuestionId = "top", Value = "sweater" },
            new() { QuestionId = "bottom", Value = "jeans" },
            new() { QuestionId = "shoes", Value = "boots" }
        };

        var match = engine.Match("walk", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("walk_weather", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("walk_weather_premium", result!.Id);
    }

    private static OutfitDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new OutfitDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "outfit-coach", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("outfit-coach data directory not found.");
    }

    private sealed class OutfitDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
