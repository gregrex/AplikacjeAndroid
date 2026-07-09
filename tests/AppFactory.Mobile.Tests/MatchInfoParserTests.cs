using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class MatchInfoParserTests
{
    [Fact]
    public void Parse_ReturnsEmptyInfo_ForEmptyUri()
    {
        var info = MatchInfoParser.Parse(string.Empty);

        Assert.False(info.HasAnyInfo);
        Assert.Equal(0, info.Score);
        Assert.Empty(info.MatchedConditions);
        Assert.Empty(info.AlternativePremiumResultIds);
    }

    [Fact]
    public void Parse_ReadsFullAbsoluteUri()
    {
        var uri = "https://app.local/result/wifi/free/premium?ruleId=weak_signal_place&score=95&reason=router%20centralnie&matched=problem%3Dweak_signal%7Cwalls%3Dmany&alternatives=mesh_premium%7Cplace_premium";

        var info = MatchInfoParser.Parse(uri);

        Assert.True(info.HasAnyInfo);
        Assert.Equal("weak_signal_place", info.RuleId);
        Assert.Equal(95, info.Score);
        Assert.Equal("router centralnie", info.Reason);
        Assert.Equal(new[] { "problem=weak_signal", "walls=many" }, info.MatchedConditions);
        Assert.Equal(new[] { "mesh_premium", "place_premium" }, info.AlternativePremiumResultIds);
    }

    [Fact]
    public void ParseQuery_ReadsQueryWithoutQuestionMark()
    {
        var info = MatchInfoParser.ParseQuery("ruleId=default_wifi&score=1&matched=&alternatives=");

        Assert.True(info.HasAnyInfo);
        Assert.Equal("default_wifi", info.RuleId);
        Assert.Equal(1, info.Score);
        Assert.Empty(info.MatchedConditions);
        Assert.Empty(info.AlternativePremiumResultIds);
    }

    [Fact]
    public void ParseQuery_IgnoresInvalidScore()
    {
        var info = MatchInfoParser.ParseQuery("ruleId=test&score=abc");

        Assert.Equal("test", info.RuleId);
        Assert.Equal(0, info.Score);
    }

    [Fact]
    public void ParseQuery_IsCaseInsensitiveForKeys()
    {
        var info = MatchInfoParser.ParseQuery("RULEID=case_rule&SCORE=10&REASON=ok");

        Assert.Equal("case_rule", info.RuleId);
        Assert.Equal(10, info.Score);
        Assert.Equal("ok", info.Reason);
    }
}
