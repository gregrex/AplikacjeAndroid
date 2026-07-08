using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class SzydelkoPomocnikDataTests
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
    public void Scenario_RowCounter_ReturnsCounterSystem()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "project_type", Value = "scarf" },
            new() { QuestionId = "difficulty", Value = "beginner" },
            new() { QuestionId = "tool", Value = "medium_hook" },
            new() { QuestionId = "yarn", Value = "acrylic" },
            new() { QuestionId = "need", Value = "row_counter" }
        };

        var match = engine.Match("scarf", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("row_counter", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("row_counter_premium", result!.Id);
    }

    [Fact]
    public void Scenario_Abbreviations_ReturnsDictionaryCard()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "project_type", Value = "abbreviations" },
            new() { QuestionId = "difficulty", Value = "beginner" },
            new() { QuestionId = "tool", Value = "unknown" },
            new() { QuestionId = "yarn", Value = "unknown" },
            new() { QuestionId = "need", Value = "abbrev" }
        };

        var match = engine.Match("abbreviations", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("abbrev_dictionary", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("abbrev_dictionary_premium", result!.Id);
    }

    [Fact]
    public void Scenario_Scarf_ReturnsBeginnerPattern()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "project_type", Value = "scarf" },
            new() { QuestionId = "difficulty", Value = "beginner" },
            new() { QuestionId = "tool", Value = "medium_hook" },
            new() { QuestionId = "yarn", Value = "acrylic" },
            new() { QuestionId = "need", Value = "basic_pattern" }
        };

        var match = engine.Match("scarf", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("scarf_beginner", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("scarf_beginner_premium", result!.Id);
    }

    private static SzydelkoPomocnikDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new SzydelkoPomocnikDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "szydelko-pomocnik", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("szydelko-pomocnik data directory not found.");
    }

    private sealed class SzydelkoPomocnikDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
