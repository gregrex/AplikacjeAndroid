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
                Id = "coffee_cotton_backup",
                CategoryId = "coffee",
                Score = 50,
                When = new Dictionary<string, string>
                {
                    ["material"] = "cotton"
                },
                FreeResultId = "coffee_backup_free",
                PremiumResultId = "coffee_backup_premium"
            },
            new()
            {
                Id = "coffee_cotton_fresh",
                CategoryId = "coffee",
                Score = 100,
                Reason = "Fresh cotton coffee stain has a dedicated rule.",
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
        Assert.Equal(100, match.Score);
        Assert.Equal("Fresh cotton coffee stain has a dedicated rule.", match.Reason);
        Assert.Contains("material=cotton", match.MatchedConditions);
        Assert.Contains("fresh=yes", match.MatchedConditions);
        Assert.Contains("coffee_cotton_backup", match.AlternativeRuleIds);
        Assert.Contains("coffee_backup_premium", match.AlternativePremiumResultIds);
    }

    [Fact]
    public void Match_ReturnsDefaultRule_WhenNoSpecificRuleMatches()
    {
        var service = new RuleEngineService();
        var rules = new List<RuleDefinition>
        {
            new()
            {
                Id = "conditional_any",
                CategoryId = "*",
                Score = 100,
                When = new Dictionary<string, string>
                {
                    ["material"] = "wool"
                },
                FreeResultId = "conditional_free",
                PremiumResultId = "conditional_premium"
            },
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
        Assert.Equal(1, match.Score);
        Assert.Empty(match.MatchedConditions);
    }
}
