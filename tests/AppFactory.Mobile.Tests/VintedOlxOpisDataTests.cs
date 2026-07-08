using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class VintedOlxOpisDataTests
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
    public void Scenario_FriendlyTone_ReturnsFriendlyTemplate()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "condition", Value = "very_good" },
            new() { QuestionId = "tone", Value = "friendly" },
            new() { QuestionId = "shipping", Value = "both" }
        };

        var match = engine.Match("clothes", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("friendly_any", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("friendly_premium", result!.Id);
    }

    [Fact]
    public void Scenario_ElectronicsGood_ReturnsElectronicsTemplate()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "condition", Value = "good" },
            new() { QuestionId = "tone", Value = "detailed" },
            new() { QuestionId = "shipping", Value = "yes" }
        };

        var match = engine.Match("electronics", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("electronics_good", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("electronics_premium", result!.Id);
    }

    private static SalesDescriptionDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new SalesDescriptionDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "vinted-olx-opis", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("vinted-olx-opis data directory not found.");
    }

    private sealed class SalesDescriptionDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
