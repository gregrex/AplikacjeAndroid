using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class ZmywarkaDiagnostaDataTests
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
    public void Scenario_Leak_ReturnsSafetyStop()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "symptom", Value = "not_draining" },
            new() { QuestionId = "frequency", Value = "once" },
            new() { QuestionId = "last_change", Value = "none" },
            new() { QuestionId = "filter_state", Value = "unknown" },
            new() { QuestionId = "safety", Value = "leak" }
        };

        var match = engine.Match("not_draining", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("safety_leak_stop", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("safety_leak_stop_premium", result!.Id);
    }

    [Fact]
    public void Scenario_NotCleaning_ReturnsFilterChecklist()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "symptom", Value = "not_cleaning" },
            new() { QuestionId = "frequency", Value = "always" },
            new() { QuestionId = "last_change", Value = "moved_dishes" },
            new() { QuestionId = "filter_state", Value = "dirty" },
            new() { QuestionId = "safety", Value = "normal" }
        };

        var match = engine.Match("not_cleaning", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("not_cleaning_filter", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("not_cleaning_filter_premium", result!.Id);
    }

    [Fact]
    public void Scenario_ErrorCode_ReturnsManualChecklist()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "symptom", Value = "error_code" },
            new() { QuestionId = "frequency", Value = "sometimes" },
            new() { QuestionId = "last_change", Value = "none" },
            new() { QuestionId = "filter_state", Value = "clean" },
            new() { QuestionId = "safety", Value = "normal" }
        };

        var match = engine.Match("error_code", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("error_code_manual", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("error_code_manual_premium", result!.Id);
    }

    private static ZmywarkaDiagnostaDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new ZmywarkaDiagnostaDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "zmywarka-diagnosta", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("zmywarka-diagnosta data directory not found.");
    }

    private sealed class ZmywarkaDiagnostaDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
