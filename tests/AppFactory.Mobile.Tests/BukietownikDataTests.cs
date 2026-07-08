using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class BukietownikDataTests
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
    public void Scenario_Birthday_ReturnsBirthdayBouquet()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "occasion", Value = "birthday" },
            new() { QuestionId = "colors", Value = "colorful" },
            new() { QuestionId = "flowers", Value = "mixed" },
            new() { QuestionId = "style", Value = "simple" },
            new() { QuestionId = "size", Value = "medium" }
        };

        var match = engine.Match("birthday", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("birthday_colorful", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("birthday_colorful_premium", result!.Id);
    }

    [Fact]
    public void Scenario_HomeDecor_ReturnsGreenHomeBouquet()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "occasion", Value = "home_decor" },
            new() { QuestionId = "colors", Value = "white_green" },
            new() { QuestionId = "flowers", Value = "greenery" },
            new() { QuestionId = "style", Value = "natural" },
            new() { QuestionId = "size", Value = "small" }
        };

        var match = engine.Match("home_decor", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("home_green", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("home_green_premium", result!.Id);
    }

    [Fact]
    public void Scenario_JustBecause_ReturnsNaturalBouquet()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "occasion", Value = "just_because" },
            new() { QuestionId = "colors", Value = "pastel" },
            new() { QuestionId = "flowers", Value = "field" },
            new() { QuestionId = "style", Value = "natural" },
            new() { QuestionId = "size", Value = "small" }
        };

        var match = engine.Match("just_because", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("just_because_natural", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("just_because_natural_premium", result!.Id);
    }

    private static BukietownikDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new BukietownikDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "bukietownik", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("bukietownik data directory not found.");
    }

    private sealed class BukietownikDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
