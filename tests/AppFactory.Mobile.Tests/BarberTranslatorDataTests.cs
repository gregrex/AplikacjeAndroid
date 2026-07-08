using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class BarberTranslatorDataTests
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
    public void Scenario_ShortMidFade_ReturnsShortFadeBrief()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "top_length", Value = "short" },
            new() { QuestionId = "sides", Value = "clipper_short" },
            new() { QuestionId = "transition", Value = "mid_fade" },
            new() { QuestionId = "fringe", Value = "short" },
            new() { QuestionId = "texture", Value = "straight" },
            new() { QuestionId = "style", Value = "modern" }
        };

        var match = engine.Match("fade", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("short_fade", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("short_fade_premium", result!.Id);
    }

    [Fact]
    public void Scenario_FormalStyle_ReturnsClassicFormalBrief()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "top_length", Value = "medium" },
            new() { QuestionId = "sides", Value = "classic_short" },
            new() { QuestionId = "transition", Value = "no_fade" },
            new() { QuestionId = "fringe", Value = "keep" },
            new() { QuestionId = "texture", Value = "straight" },
            new() { QuestionId = "style", Value = "formal" }
        };

        var match = engine.Match("formal", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("classic_formal", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("classic_formal_premium", result!.Id);
    }

    [Fact]
    public void Scenario_CurlyTexture_ReturnsCurlyShapeBrief()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "top_length", Value = "longer" },
            new() { QuestionId = "sides", Value = "keep_length" },
            new() { QuestionId = "transition", Value = "soft_taper" },
            new() { QuestionId = "fringe", Value = "keep" },
            new() { QuestionId = "texture", Value = "curly" },
            new() { QuestionId = "style", Value = "natural" }
        };

        var match = engine.Match("curly", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("curly_shape", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("curly_shape_premium", result!.Id);
    }

    private static BarberDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new BarberDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "barber-translator", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("barber-translator data directory not found.");
    }

    private sealed class BarberDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
