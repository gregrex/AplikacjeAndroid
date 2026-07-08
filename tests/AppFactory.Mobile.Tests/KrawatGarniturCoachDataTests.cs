using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class KrawatGarniturCoachDataTests
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
    public void Scenario_Interview_ReturnsSafeChecklist()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "occasion", Value = "interview" },
            new() { QuestionId = "suit_color", Value = "navy" },
            new() { QuestionId = "shirt_color", Value = "white" },
            new() { QuestionId = "neckwear", Value = "tie" },
            new() { QuestionId = "shoes", Value = "black" }
        };

        var match = engine.Match("interview", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("interview_safe", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("interview_safe_premium", result!.Id);
    }

    [Fact]
    public void Scenario_Business_ReturnsClassicChecklist()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "occasion", Value = "business" },
            new() { QuestionId = "suit_color", Value = "charcoal" },
            new() { QuestionId = "shirt_color", Value = "light_blue" },
            new() { QuestionId = "neckwear", Value = "tie" },
            new() { QuestionId = "shoes", Value = "black" }
        };

        var match = engine.Match("business", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("business_classic", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("business_classic_premium", result!.Id);
    }

    [Fact]
    public void Scenario_Evening_ReturnsEveningChecklist()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "occasion", Value = "evening" },
            new() { QuestionId = "suit_color", Value = "black" },
            new() { QuestionId = "shirt_color", Value = "white" },
            new() { QuestionId = "neckwear", Value = "bowtie" },
            new() { QuestionId = "shoes", Value = "black" }
        };

        var match = engine.Match("evening", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("evening_elegant", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("evening_elegant_premium", result!.Id);
    }

    private static KrawatGarniturCoachDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new KrawatGarniturCoachDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "krawat-garnitur-coach", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }
            dir = dir.Parent;
        }
        throw new DirectoryNotFoundException("krawat-garnitur-coach data directory not found.");
    }

    private sealed class KrawatGarniturCoachDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
