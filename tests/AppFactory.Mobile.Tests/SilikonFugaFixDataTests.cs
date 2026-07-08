using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class SilikonFugaFixDataTests
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
    public void Scenario_SuspectedLeak_ReturnsStopPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "problem", Value = "suspected_leak" },
            new() { QuestionId = "place", Value = "shower" },
            new() { QuestionId = "dirt_level", Value = "heavy" },
            new() { QuestionId = "material", Value = "unknown" },
            new() { QuestionId = "risk", Value = "medium" }
        };

        var match = engine.Match("suspected_leak", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("leak_stop", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("leak_stop_premium", result!.Id);
    }

    [Fact]
    public void Scenario_DirtyGrout_ReturnsCleaningPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "problem", Value = "dirty_grout" },
            new() { QuestionId = "place", Value = "floor" },
            new() { QuestionId = "dirt_level", Value = "medium" },
            new() { QuestionId = "material", Value = "ceramic" },
            new() { QuestionId = "risk", Value = "low" }
        };

        var match = engine.Match("dirty_grout", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("dirty_grout_clean", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("dirty_grout_clean_premium", result!.Id);
    }

    [Fact]
    public void Scenario_OldSilicone_ReturnsReplacementPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "problem", Value = "old_silicone" },
            new() { QuestionId = "place", Value = "bath" },
            new() { QuestionId = "dirt_level", Value = "heavy" },
            new() { QuestionId = "material", Value = "acrylic" },
            new() { QuestionId = "risk", Value = "low" }
        };

        var match = engine.Match("old_silicone", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("old_silicone_replace", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("old_silicone_replace_premium", result!.Id);
    }

    private static SilikonFugaFixDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new SilikonFugaFixDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "silikon-fuga-fix", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("silikon-fuga-fix data directory not found.");
    }

    private sealed class SilikonFugaFixDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
