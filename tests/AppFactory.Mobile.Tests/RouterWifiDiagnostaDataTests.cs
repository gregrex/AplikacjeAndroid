using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class RouterWifiDiagnostaDataTests
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
    public void Scenario_WeakSignal_ReturnsPlacementPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "problem", Value = "weak_signal" },
            new() { QuestionId = "home_type", Value = "flat_large" },
            new() { QuestionId = "router_place", Value = "corner" },
            new() { QuestionId = "walls", Value = "two_three" },
            new() { QuestionId = "devices", Value = "few" }
        };

        var match = engine.Match("weak_signal", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("weak_signal_place", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("weak_signal_place_premium", result!.Id);
    }

    [Fact]
    public void Scenario_SlowSpeed_ReturnsSpeedTestPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "problem", Value = "slow_speed" },
            new() { QuestionId = "home_type", Value = "flat_small" },
            new() { QuestionId = "router_place", Value = "central" },
            new() { QuestionId = "walls", Value = "zero_one" },
            new() { QuestionId = "devices", Value = "few" }
        };

        var match = engine.Match("slow_speed", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("slow_speed_test", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("slow_speed_test_premium", result!.Id);
    }

    [Fact]
    public void Scenario_ManyDevices_ReturnsDeviceLoadPlan()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "problem", Value = "many_devices" },
            new() { QuestionId = "home_type", Value = "house" },
            new() { QuestionId = "router_place", Value = "central" },
            new() { QuestionId = "walls", Value = "two_three" },
            new() { QuestionId = "devices", Value = "many" }
        };

        var match = engine.Match("many_devices", answers, data.Rules);
        var result = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("many_devices_split", match.RuleId);
        Assert.NotNull(result);
        Assert.Equal("many_devices_split_premium", result!.Id);
    }

    private static RouterWifiDiagnostaDataPack LoadDataPack()
    {
        var dataDir = GetDataDir();
        return new RouterWifiDiagnostaDataPack
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
            var candidate = Path.Combine(dir.FullName, "projects", "router-wifi-diagnosta", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }
            dir = dir.Parent;
        }
        throw new DirectoryNotFoundException("router-wifi-diagnosta data directory not found.");
    }

    private sealed class RouterWifiDiagnostaDataPack
    {
        public List<CategoryDefinition> Categories { get; init; } = new();
        public List<QuestionDefinition> Questions { get; init; } = new();
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
