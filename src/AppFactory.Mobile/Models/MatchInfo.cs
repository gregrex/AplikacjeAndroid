namespace AppFactory.Mobile.Models;

public sealed class MatchInfo
{
    public string RuleId { get; init; } = string.Empty;
    public int Score { get; init; }
    public string Reason { get; init; } = string.Empty;
    public List<string> MatchedConditions { get; init; } = new();
    public List<string> AlternativePremiumResultIds { get; init; } = new();

    public bool HasAnyInfo =>
        !string.IsNullOrWhiteSpace(RuleId)
        || Score > 0
        || !string.IsNullOrWhiteSpace(Reason)
        || MatchedConditions.Count > 0;
}
