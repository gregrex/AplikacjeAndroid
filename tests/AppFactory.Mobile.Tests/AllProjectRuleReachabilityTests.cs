using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class AllProjectRuleReachabilityTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public void EverySourceRule_IsExecutableAndReturnsExistingResults()
    {
        var root = GetRepoRoot();
        var engine = new RuleEngineService();
        var projects = new ProjectCatalogService().GetProjects();
        var errors = new List<string>();

        foreach (var project in projects)
        {
            var dataDir = Path.Combine(root, "projects", project.Id, "data");
            var categories = ReadJson<List<CategoryDefinition>>(Path.Combine(dataDir, "categories.json"));
            var rules = ReadJson<List<RuleDefinition>>(Path.Combine(dataDir, "rules.json"));
            var plResults = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.pl.json"));
            var enResults = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.en.json"));
            var ukResults = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.uk.json"));

            var plIds = plResults.Select(x => x.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var enIds = enResults.Select(x => x.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var ukIds = ukResults.Select(x => x.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var rule in rules)
            {
                var categoryId = rule.CategoryId == "*"
                    ? categories.FirstOrDefault()?.Id ?? "default"
                    : rule.CategoryId;

                var answers = rule.When
                    .Select(condition => new UserAnswer
                    {
                        QuestionId = condition.Key,
                        Value = condition.Value
                    })
                    .ToArray();

                var match = engine.Match(categoryId, answers, new[] { rule });

                if (!string.Equals(match.RuleId, rule.Id, StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add($"{project.Id}/{rule.Id}: rule is not executable from its own conditions");
                    continue;
                }

                if (!string.Equals(match.FreeResultId, rule.FreeResultId, StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add($"{project.Id}/{rule.Id}: free result mismatch");
                }

                if (!string.Equals(match.PremiumResultId, rule.PremiumResultId, StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add($"{project.Id}/{rule.Id}: premium result mismatch");
                }

                RequireResult(errors, project.Id, rule.Id, "free", rule.FreeResultId, plIds, enIds, ukIds);
                RequireResult(errors, project.Id, rule.Id, "premium", rule.PremiumResultId, plIds, enIds, ukIds);
            }
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void EveryCategory_HasAReachableRuleOrGlobalFallback()
    {
        var root = GetRepoRoot();
        var engine = new RuleEngineService();
        var projects = new ProjectCatalogService().GetProjects();
        var errors = new List<string>();

        foreach (var project in projects)
        {
            var dataDir = Path.Combine(root, "projects", project.Id, "data");
            var categories = ReadJson<List<CategoryDefinition>>(Path.Combine(dataDir, "categories.json"));
            var rules = ReadJson<List<RuleDefinition>>(Path.Combine(dataDir, "rules.json"));
            var fallback = rules
                .Where(rule => rule.CategoryId == "*" && rule.When.Count == 0)
                .OrderByDescending(rule => rule.Score)
                .FirstOrDefault();

            foreach (var category in categories)
            {
                var targetRule = rules
                    .Where(rule => rule.CategoryId == "*" || string.Equals(rule.CategoryId, category.Id, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(rule => rule.When.Count)
                    .ThenByDescending(rule => rule.Score)
                    .FirstOrDefault();

                if (targetRule is null && fallback is null)
                {
                    errors.Add($"{project.Id}/{category.Id}: no category rule and no global fallback");
                    continue;
                }

                var answers = targetRule?.When
                    .Select(condition => new UserAnswer { QuestionId = condition.Key, Value = condition.Value })
                    .ToArray() ?? Array.Empty<UserAnswer>();

                var match = engine.Match(category.Id, answers, rules);
                if (string.IsNullOrWhiteSpace(match.RuleId)
                    || string.IsNullOrWhiteSpace(match.FreeResultId)
                    || string.IsNullOrWhiteSpace(match.PremiumResultId))
                {
                    errors.Add($"{project.Id}/{category.Id}: category does not produce a complete result");
                }
            }
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    private static void RequireResult(
        List<string> errors,
        string projectId,
        string ruleId,
        string resultType,
        string resultId,
        HashSet<string> plIds,
        HashSet<string> enIds,
        HashSet<string> ukIds)
    {
        if (!plIds.Contains(resultId))
        {
            errors.Add($"{projectId}/{ruleId}: {resultType} result {resultId} missing in PL");
        }

        if (!enIds.Contains(resultId))
        {
            errors.Add($"{projectId}/{ruleId}: {resultType} result {resultId} missing in EN");
        }

        if (!ukIds.Contains(resultId))
        {
            errors.Add($"{projectId}/{ruleId}: {resultType} result {resultId} missing in UK");
        }
    }

    private static T ReadJson<T>(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json, JsonOptions)
               ?? throw new InvalidOperationException($"Cannot read {path}");
    }

    private static string GetRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, "projects"))
                && Directory.Exists(Path.Combine(dir.FullName, "src")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("Repository root not found.");
    }
}
