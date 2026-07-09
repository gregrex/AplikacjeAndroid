using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class RuleEngineService
{
    public RuleMatch Match(string categoryId, IReadOnlyCollection<UserAnswer> answers, IReadOnlyCollection<RuleDefinition> rules)
    {
        var answerMap = answers
            .Where(x => !string.IsNullOrWhiteSpace(x.QuestionId))
            .GroupBy(x => x.QuestionId, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(x => x.Key, x => x.Last().Value, StringComparer.OrdinalIgnoreCase);

        var matchingRules = rules
            .Where(rule => CategoryMatches(rule.CategoryId, categoryId))
            .Where(rule => ConditionsMatch(rule.When, answerMap))
            .OrderByDescending(rule => rule.Score)
            .ToList();

        var best = matchingRules.FirstOrDefault()
                   ?? rules
                       .Where(rule => rule.CategoryId == "*" && rule.When.Count == 0)
                       .OrderByDescending(rule => rule.Score)
                       .FirstOrDefault();

        if (best is null)
        {
            return new RuleMatch();
        }

        var alternatives = matchingRules
            .Where(rule => !string.Equals(rule.Id, best.Id, StringComparison.OrdinalIgnoreCase))
            .Take(3)
            .ToList();

        return new RuleMatch
        {
            RuleId = best.Id,
            FreeResultId = best.FreeResultId,
            PremiumResultId = best.PremiumResultId,
            Score = best.Score,
            Reason = best.Reason,
            MatchedConditions = best.When.Select(x => $"{x.Key}={x.Value}").ToList(),
            AlternativeRuleIds = alternatives.Select(x => x.Id).ToList(),
            AlternativePremiumResultIds = alternatives.Select(x => x.PremiumResultId).ToList()
        };
    }

    private static bool CategoryMatches(string ruleCategoryId, string selectedCategoryId)
    {
        return ruleCategoryId == "*" || string.Equals(ruleCategoryId, selectedCategoryId, StringComparison.OrdinalIgnoreCase);
    }

    private static bool ConditionsMatch(Dictionary<string, string> conditions, Dictionary<string, string> answers)
    {
        foreach (var condition in conditions)
        {
            if (!answers.TryGetValue(condition.Key, out var answer))
            {
                return false;
            }

            if (!string.Equals(answer, condition.Value, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        return true;
    }
}
