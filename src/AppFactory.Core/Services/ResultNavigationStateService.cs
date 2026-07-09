using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class ResultNavigationStateService
{
    private readonly Dictionary<string, MatchInfo> _stateByKey = new(StringComparer.OrdinalIgnoreCase);

    public string Store(string projectId, string categoryId, string freeResultId, string premiumResultId, RuleMatch match)
    {
        var key = BuildKey(projectId, categoryId, freeResultId, premiumResultId);
        _stateByKey[key] = new MatchInfo
        {
            RuleId = match.RuleId,
            Score = match.Score,
            Reason = match.Reason,
            MatchedConditions = match.MatchedConditions.ToList(),
            AlternativePremiumResultIds = match.AlternativePremiumResultIds.ToList()
        };

        return key;
    }

    public MatchInfo GetOrDefault(string projectId, string categoryId, string freeResultId, string premiumResultId)
    {
        var key = BuildKey(projectId, categoryId, freeResultId, premiumResultId);
        return _stateByKey.TryGetValue(key, out var info) ? info : new MatchInfo();
    }

    public void Clear(string projectId, string categoryId, string freeResultId, string premiumResultId)
    {
        _stateByKey.Remove(BuildKey(projectId, categoryId, freeResultId, premiumResultId));
    }

    private static string BuildKey(string projectId, string categoryId, string freeResultId, string premiumResultId) =>
        string.Join("|", projectId, categoryId, freeResultId, premiumResultId);
}
