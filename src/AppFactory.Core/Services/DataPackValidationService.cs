using System.Text.Json;
using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class DataPackValidationService
{
    public IReadOnlyList<string> ValidateAppConfig(AppConfig appConfig, string expectedProjectId)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(appConfig.AppId))
        {
            errors.Add("AppConfig appId cannot be empty.");
        }
        else if (!string.Equals(appConfig.AppId, expectedProjectId, StringComparison.OrdinalIgnoreCase))
        {
            errors.Add($"AppConfig appId '{appConfig.AppId}' does not match project id '{expectedProjectId}'.");
        }

        if (string.IsNullOrWhiteSpace(appConfig.AppName))
        {
            errors.Add($"AppConfig '{expectedProjectId}' appName cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(appConfig.DefaultLanguage))
        {
            errors.Add($"AppConfig '{expectedProjectId}' defaultLanguage cannot be empty.");
        }

        if (appConfig.SupportedLanguages.Count == 0)
        {
            errors.Add($"AppConfig '{expectedProjectId}' supportedLanguages cannot be empty.");
        }
        else
        {
            RequireUnique(appConfig.SupportedLanguages, $"{expectedProjectId} supported language", errors);

            if (!string.IsNullOrWhiteSpace(appConfig.DefaultLanguage) && !appConfig.SupportedLanguages.Contains(appConfig.DefaultLanguage, StringComparer.OrdinalIgnoreCase))
            {
                errors.Add($"AppConfig '{expectedProjectId}' defaultLanguage '{appConfig.DefaultLanguage}' is not listed in supportedLanguages.");
            }
        }

        if (string.IsNullOrWhiteSpace(appConfig.Theme.PrimaryColor))
        {
            errors.Add($"AppConfig '{expectedProjectId}' theme primaryColor cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(appConfig.Theme.SecondaryColor))
        {
            errors.Add($"AppConfig '{expectedProjectId}' theme secondaryColor cannot be empty.");
        }

        return errors;
    }

    public IReadOnlyList<string> ValidateTheme(JsonElement theme, string expectedProjectId)
    {
        var errors = new List<string>();

        RequireThemeProperty(theme, "themeId", expectedProjectId, errors);
        RequireThemeProperty(theme, "brandName", expectedProjectId, errors);
        RequireThemeProperty(theme, "targetAudience", expectedProjectId, errors);
        RequireThemeProperty(theme, "tone", expectedProjectId, errors);
        RequireThemeProperty(theme, "primaryColor", expectedProjectId, errors);
        RequireThemeProperty(theme, "secondaryColor", expectedProjectId, errors);
        RequireThemeProperty(theme, "backgroundColor", expectedProjectId, errors);
        RequireThemeProperty(theme, "surfaceColor", expectedProjectId, errors);
        RequireThemeProperty(theme, "textColor", expectedProjectId, errors);

        return errors;
    }

    public IReadOnlyList<string> Validate(
        IReadOnlyCollection<CategoryDefinition> categories,
        IReadOnlyCollection<QuestionDefinition> questions,
        IReadOnlyCollection<RuleDefinition> rules,
        IReadOnlyCollection<ResultDefinition> results)
    {
        var errors = new List<string>();

        if (categories.Count == 0)
        {
            errors.Add("Data pack must contain at least one category.");
        }

        if (questions.Count == 0)
        {
            errors.Add("Data pack must contain at least one question.");
        }

        if (rules.Count == 0)
        {
            errors.Add("Data pack must contain at least one rule.");
        }

        if (results.Count == 0)
        {
            errors.Add("Data pack must contain at least one result.");
        }

        RequireUnique(categories.Select(x => x.Id), "category", errors);
        RequireUnique(questions.Select(x => x.Id), "question", errors);
        RequireUnique(rules.Select(x => x.Id), "rule", errors);
        RequireUnique(results.Select(x => x.Id), "result", errors);

        var categoryIds = categories.Select(x => x.Id).Where(x => !string.IsNullOrWhiteSpace(x)).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var questionsById = questions
            .Where(x => !string.IsNullOrWhiteSpace(x.Id))
            .GroupBy(x => x.Id, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);
        var resultIds = results.Select(x => x.Id).Where(x => !string.IsNullOrWhiteSpace(x)).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var category in categories)
        {
            if (string.IsNullOrWhiteSpace(category.Id))
            {
                errors.Add("Category id cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(category.NameKey))
            {
                errors.Add($"Category '{category.Id}' nameKey cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(category.Icon))
            {
                errors.Add($"Category '{category.Id}' icon cannot be empty.");
            }
        }

        foreach (var question in questions)
        {
            if (string.IsNullOrWhiteSpace(question.Id))
            {
                errors.Add("Question id cannot be empty.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(question.Type))
            {
                errors.Add($"Question '{question.Id}' type cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(question.TextKey))
            {
                errors.Add($"Question '{question.Id}' textKey cannot be empty.");
            }

            if (string.Equals(question.Type, "single", StringComparison.OrdinalIgnoreCase) && question.Options.Count == 0)
            {
                errors.Add($"Question '{question.Id}' is single choice but has no options.");
            }

            RequireUnique(question.Options.Select(x => x.Value), $"question '{question.Id}' option", errors);

            foreach (var option in question.Options)
            {
                if (string.IsNullOrWhiteSpace(option.Value))
                {
                    errors.Add($"Question '{question.Id}' has option with empty value.");
                }

                if (string.IsNullOrWhiteSpace(option.LabelKey))
                {
                    errors.Add($"Question '{question.Id}' option '{option.Value}' labelKey cannot be empty.");
                }
            }
        }

        foreach (var rule in rules)
        {
            if (string.IsNullOrWhiteSpace(rule.Id))
            {
                errors.Add("Rule id cannot be empty.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(rule.CategoryId))
            {
                errors.Add($"Rule '{rule.Id}' categoryId cannot be empty.");
            }
            else if (rule.CategoryId != "*" && !categoryIds.Contains(rule.CategoryId))
            {
                errors.Add($"Rule '{rule.Id}' references missing category '{rule.CategoryId}'.");
            }

            if (string.IsNullOrWhiteSpace(rule.FreeResultId))
            {
                errors.Add($"Rule '{rule.Id}' freeResultId cannot be empty.");
            }
            else if (!resultIds.Contains(rule.FreeResultId))
            {
                errors.Add($"Rule '{rule.Id}' references missing free result '{rule.FreeResultId}'.");
            }

            if (string.IsNullOrWhiteSpace(rule.PremiumResultId))
            {
                errors.Add($"Rule '{rule.Id}' premiumResultId cannot be empty.");
            }
            else if (!resultIds.Contains(rule.PremiumResultId))
            {
                errors.Add($"Rule '{rule.Id}' references missing premium result '{rule.PremiumResultId}'.");
            }

            foreach (var condition in rule.When)
            {
                if (!questionsById.TryGetValue(condition.Key, out var question))
                {
                    errors.Add($"Rule '{rule.Id}' references missing question '{condition.Key}'.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(condition.Value))
                {
                    errors.Add($"Rule '{rule.Id}' has empty value for question '{condition.Key}'.");
                    continue;
                }

                if (question.Options.Count > 0 && !question.Options.Any(x => string.Equals(x.Value, condition.Value, StringComparison.OrdinalIgnoreCase)))
                {
                    errors.Add($"Rule '{rule.Id}' uses value '{condition.Value}' that is not present in question '{condition.Key}' options.");
                }
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

    private static void RequireThemeProperty(JsonElement theme, string propertyName, string expectedProjectId, List<string> errors)
    {
        if (!theme.TryGetProperty(propertyName, out var property) || string.IsNullOrWhiteSpace(property.GetString()))
        {
            errors.Add($"Theme '{expectedProjectId}' property '{propertyName}' cannot be empty.");
        }
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
