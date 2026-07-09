using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class ResultNavigationStateServiceTests
{
    [Fact]
    public void StoreAndGetOrDefault_ReturnsStoredMatchInfo()
    {
        var service = new ResultNavigationStateService();
        var match = new RuleMatch
        {
            RuleId = "router_rule",
            FreeResultId = "free",
            PremiumResultId = "premium",
            Score = 88,
            Reason = "Because router placement matched.",
            MatchedConditions = new List<string> { "problem=weak_signal" },
            AlternativePremiumResultIds = new List<string> { "alt_premium" }
        };

        service.Store("router-wifi-diagnosta", "weak_signal", "free", "premium", match);

        var info = service.GetOrDefault("router-wifi-diagnosta", "weak_signal", "free", "premium");

        Assert.True(info.HasAnyInfo);
        Assert.Equal("router_rule", info.RuleId);
        Assert.Equal(88, info.Score);
        Assert.Equal("Because router placement matched.", info.Reason);
        Assert.Equal(new[] { "problem=weak_signal" }, info.MatchedConditions);
        Assert.Equal(new[] { "alt_premium" }, info.AlternativePremiumResultIds);
    }

    [Fact]
    public void GetOrDefault_ReturnsEmptyInfo_ForMissingState()
    {
        var service = new ResultNavigationStateService();

        var info = service.GetOrDefault("project", "category", "free", "premium");

        Assert.False(info.HasAnyInfo);
    }

    [Fact]
    public void Clear_RemovesStoredState()
    {
        var service = new ResultNavigationStateService();
        var match = new RuleMatch
        {
            RuleId = "rule",
            FreeResultId = "free",
            PremiumResultId = "premium",
            Score = 10,
            Reason = "reason"
        };

        service.Store("project", "category", "free", "premium", match);
        service.Clear("project", "category", "free", "premium");

        var info = service.GetOrDefault("project", "category", "free", "premium");
        Assert.False(info.HasAnyInfo);
    }
}
