using System.Text.Json;
using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Tests;

public sealed class RuleReasonsQualityTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly string[] ProjectsWithRequiredReasons =
    {
        "router-wifi-diagnosta",
        "zmywarka-diagnosta",
        "krawat-garnitur-coach",
        "pakowanie-paczek",
        "silikon-fuga-fix",
        "chleb-zakwas-coach",
        "domfix",
        "outfit-coach",
        "fryzury-proste"
    };

    [Fact]
    public void SelectedProjects_HaveReasonsForEverySourceRule()
    {
        var root = GetRepoRoot();
        var errors = new List<string>();

        foreach (var projectId in ProjectsWithRequiredReasons)
        {
            var rulesPath = Path.Combine(root, "projects", projectId, "data", "rules.json");
            Assert.True(File.Exists(rulesPath), $"Missing rules file: {rulesPath}");

            var rules = ReadJson<List<RuleDefinition>>(rulesPath);
            foreach (var rule in rules)
            {
                if (string.IsNullOrWhiteSpace(rule.Reason))
                {
                    errors.Add($"{projectId}: source rule '{rule.Id}' has empty reason");
                }
            }
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void SelectedProjects_HaveReasonsForEveryRuntimeRule()
    {
        var root = GetRepoRoot();
        var errors = new List<string>();

        foreach (var projectId in ProjectsWithRequiredReasons)
        {
            var rulesPath = Path.Combine(root, "src", "AppFactory.Mobile", "wwwroot", "projects", projectId, "rules.json");
            Assert.True(File.Exists(rulesPath), $"Missing runtime rules file: {rulesPath}");

            var rules = ReadJson<List<RuleDefinition>>(rulesPath);
            foreach (var rule in rules)
            {
                if (string.IsNullOrWhiteSpace(rule.Reason))
                {
                    errors.Add($"{projectId}: runtime rule '{rule.Id}' has empty reason");
                }
            }
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    private static T ReadJson<T>(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json, JsonOptions) ?? throw new InvalidOperationException($"Cannot read {path}");
    }

    private static string GetRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, "projects")) && Directory.Exists(Path.Combine(dir.FullName, "src")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("Repository root not found.");
    }
}
