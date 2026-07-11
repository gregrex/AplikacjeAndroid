using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class DataPackValidationServiceTests
{
    [Fact]
    public void Validate_ReturnsNoErrors_ForValidMinimalPack()
    {
        var service = new DataPackValidationService();

        var categories = new List<CategoryDefinition>
        {
            new() { Id = "coffee", NameKey = "stain.coffee", Icon = "coffee" }
        };

        var questions = new List<QuestionDefinition>
        {
            new()
            {
                Id = "fresh",
                Type = "single",
                TextKey = "question.fresh",
                Options = new List<QuestionOption>
                {
                    new() { Value = "yes", LabelKey = "common.yes" }
                }
            }
        };

        var rules = new List<RuleDefinition>
        {
            new()
            {
                Id = "coffee_fresh",
                CategoryId = "coffee",
                When = new Dictionary<string, string> { ["fresh"] = "yes" },
                FreeResultId = "free",
                PremiumResultId = "premium"
            }
        };

        var results = new List<ResultDefinition>
        {
            new() { Id = "free", Title = "Free", Summary = "Summary", Steps = new List<string> { "Step" } },
            new() { Id = "premium", Title = "Premium", Summary = "Summary", Steps = new List<string> { "Step" } }
        };

        var errors = service.Validate(categories, questions, rules, results);

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_ReturnsErrors_ForBrokenReferences()
    {
        var service = new DataPackValidationService();

        var errors = service.Validate(
            categories: Array.Empty<CategoryDefinition>(),
            questions: Array.Empty<QuestionDefinition>(),
            rules: new List<RuleDefinition>
            {
                new()
                {
                    Id = "broken_rule",
                    CategoryId = "missing_category",
                    When = new Dictionary<string, string> { ["missing_question"] = "yes" },
                    FreeResultId = "missing_free",
                    PremiumResultId = "missing_premium"
                }
            },
            results: Array.Empty<ResultDefinition>());

        Assert.Contains(errors, x => x.Contains("missing category", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(errors, x => x.Contains("missing question", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(errors, x => x.Contains("missing free result", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(errors, x => x.Contains("missing premium result", StringComparison.OrdinalIgnoreCase));
    }
}
