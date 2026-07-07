using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class DataPackValidationService
{
    public IReadOnlyList<string> Validate(
        IReadOnlyCollection<CategoryDefinition> categories,
        IReadOnlyCollection<QuestionDefinition> questions,
        IReadOnlyCollection<RuleDefinition> rules,
        IReadOnlyCollection<ResultDefinition> results)
    {
        var errors = new List<string>();

        RequireUnique(categories.Select(x => x.Id), "category", errors);
        RequireUnique(questions.Select(x => x.Id), "question", errors);
        RequireUnique(rules.Select(x => x.Id), "rule", errors);
        RequireUnique(results.Select(x => x.Id), "result", errors);

        var categoryIds = categories.Select(x => x.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var questionIds = questions.Select(x => x.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var resultIds = results.Select(x => x.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var rule in rules)
        {
            if (string.IsNullOrWhiteSpace(rule.Id))
            {
                errors.Add("Rule id cannot be empty.");
                continue;
            }

            if (rule.CategoryId != "*" && !categoryIds.Contains(rule.CategoryId))
            {
                errors.Add($"Rule '{rule.Id}' references missing category '{rule.CategoryId}'.");
            }

            if (!resultIds.Contains(rule.FreeResultId))
            {
                errors.Add($"Rule '{rule.Id}' references missing free result '{rule.FreeResultId}'.");
            }

            if (!resultIds.Contains(rule.PremiumResultId))
            {
                errors.Add($"Rule '{rule.Id}' references missing premium result '{rule.PremiumResultId}'.");
            }

            foreach (var condition in rule.When.Keys)
            {
                if (!questionIds.Contains(condition))
                {
                    errors.Add($"Rule '{rule.Id}' references missing question '{condition}'.");
                }
            }
        }

        foreach (var question in questions)
        {
            if (string.Equals(question.Type, "single", StringComparison.OrdinalIgnoreCase) && question.Options.Count == 0)
            {
                errors.Add($"Question '{question.Id}' is single choice but has no options.");
            }
        }

        foreach (var result in results)
        {
            if (string.IsNullOrWhiteSpace(result.Id))
            {
                errors.Add("Result id cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(result.Title))
            {
                errors.Add($"Result '{result.Id}' title cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(result.Summary))
            {
                errors.Add($"Result '{result.Id}' summary cannot be empty.");
            }

            if (result.Steps.Count == 0 || result.Steps.Any(string.IsNullOrWhiteSpace))
            {
                errors.Add($"Result '{result.Id}' must have non-empty steps.");
            }
        }

        return errors;
    }

    private static void RequireUnique(IEnumerable<string> ids, string entityName, List<string> errors)
    {
        var duplicates = ids
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key);

        foreach (var duplicate in duplicates)
        {
            errors.Add($"Duplicate {entityName} id: {duplicate}.");
        }
    }
}
