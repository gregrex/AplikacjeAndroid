using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class PlamaRatownikScenarioTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public void Scenario_CoffeeCottonFresh_ReturnsCoffeeResults()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "material", Value = "cotton" },
            new() { QuestionId = "fresh", Value = "yes" }
        };

        var match = engine.Match("coffee", answers, data.Rules);
        var free = resultService.FindResult(data.Results, match.FreeResultId);
        var premium = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("coffee_cotton_fresh", match.RuleId);
        Assert.NotNull(free);
        Assert.NotNull(premium);
        Assert.Contains("Kawa", free!.Title, StringComparison.OrdinalIgnoreCase);
        Assert.NotEmpty(free.Steps);
        Assert.NotEmpty(premium!.Steps);
        Assert.NotEmpty(premium.Needed);
    }

    [Fact]
    public void Scenario_UnknownCombination_ReturnsDefaultResults()
    {
        var data = LoadDataPack();
        var engine = new RuleEngineService();
        var resultService = new ResultService();

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "material", Value = "linen" },
            new() { QuestionId = "fresh", Value = "unknown" }
        };

        var match = engine.Match("unknown-stain", answers, data.Rules);
        var free = resultService.FindResult(data.Results, match.FreeResultId);
        var premium = resultService.FindResult(data.Results, match.PremiumResultId);

        Assert.Equal("default_any", match.RuleId);
        Assert.NotNull(free);
        Assert.NotNull(premium);
        Assert.Equal("default_free", free!.Id);
        Assert.Equal("default_premium", premium!.Id);
    }

    private static PlamaDataPack LoadDataPack()
    {
        var root = FindRepositoryRoot();
        var dataDir = Path.Combine(root, "src", "AppFactory.Mobile", "wwwroot", "projects", "plama-ratownik");

        return new PlamaDataPack
        {
            Rules = ReadJson<List<RuleDefinition>>(Path.Combine(dataDir, "rules.json")),
            Results = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.pl.json"))
        };
    }

    private static T ReadJson<T>(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json, JsonOptions) ?? throw new InvalidOperationException($"Cannot read {path}");
    }

    private static string FindRepositoryRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, "src")) && Directory.Exists(Path.Combine(dir.FullName, "tests")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("Repository root not found.");
    }

    private sealed class PlamaDataPack
    {
        public List<RuleDefinition> Rules { get; init; } = new();
        public List<ResultDefinition> Results { get; init; } = new();
    }
}
