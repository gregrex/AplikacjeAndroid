using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class RuleEngineServiceTests
{
    [Fact]
    public void Match_ReturnsSpecificRule_WhenCategoryAndAnswersMatch()
    {
        var service = new RuleEngineService();
        var rules = new List<RuleDefinition>
        {
            new()
            {
                Id = "default_any",
                CategoryId = "*",
                Score = 1,
                FreeResultId = "default_free",
                PremiumResultId = "default_premium"
            },
            new()
            {
                Id = "coffee_cotton_fresh",
                CategoryId = "coffee",
                Score = 100,
                When = new Dictionary<string, string>
                {
                    ["material"] = "cotton",
                    ["fresh"] = "yes"
                },
                FreeResultId = "coffee_free",
                PremiumResultId = "coffee_premium"
            }
        };

        var answers = new List<UserAnswer>
        {
            new() { QuestionId = "material", Value = "cotton" },
            new() { QuestionId = "fresh", Value = "yes" }
        };

        var match = service.Match("coffee", answers, rules);

        Assert.Equal("coffee_cotton_fresh", match.RuleId);
        Assert.Equal("coffee_free", match.FreeResultId);
        Assert.Equal("coffee_premium", match.PremiumResultId);
    }

    [Fact]
    public void Match_ReturnsDefaultRule_WhenNoSpecificRuleMatches()
    {
        var service = new RuleEngineService();
        var rules = new List<RuleDefinition>
        {
            new()
            {
                Id = "default_any",
                CategoryId = "*",
                Score = 1,
                FreeResultId = "default_free",
                PremiumResultId = "default_premium"
            }
        };

        var match = service.Match("unknown", Array.Empty<UserAnswer>(), rules);

        Assert.Equal("default_any", match.RuleId);
        Assert.Equal("default_free", match.FreeResultId);
        Assert.Equal("default_premium", match.PremiumResultId);
    }
}
