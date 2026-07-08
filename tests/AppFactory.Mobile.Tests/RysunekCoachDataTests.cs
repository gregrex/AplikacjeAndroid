using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class RysunekCoachDataTests
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
    public void Scenario_Animal_ReturnsAnimalLesson()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "topic", Value = "animal" },
            new() { QuestionId = "difficulty", Value = "easy" },
            new() { QuestionId = "time", Value = "short" },
            new() { QuestionId = "tool", Value = "pencil" },
            new() { QuestionId = "style", Value = "simple" }
        };

        var match = engine.Match("animals", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("animal_easy", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("animal_easy_premium", result!.Id);
    }

    [Fact]
    public void Scenario_Robot_ReturnsGeometricRobotLesson()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "topic", Value = "robot" },
            new() { QuestionId = "difficulty", Value = "medium" },
            new() { QuestionId = "time", Value = "normal" },
            new() { QuestionId = "tool", Value = "pencil" },
            new() { QuestionId = "style", Value = "geometric" }
        };

        var match = engine.Match("robots", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("robot_geometric", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("robot_geometric_premium", result!.Id);
    }

    [Fact]
    public void Scenario_Monster_ReturnsOriginalMonsterLesson()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "topic", Value = "monster" },
            new() { QuestionId = "difficulty", Value = "easy" },
            new() { QuestionId = "time", Value = "practice" },
            new() { QuestionId = "tool", Value = "crayon" },
            new() { QuestionId = "style", Value = "cute" }
        };

        var match = engine.Match("monsters", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("monster_original", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("monster_original_premium", result!.Id);
    }

    private static RysunekCoachDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new RysunekCoachDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "rysunek-coach", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("rysunek-coach data directory not found.");
    }

    private sealed class RysunekCoachDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
