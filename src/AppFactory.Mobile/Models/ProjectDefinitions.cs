namespace AppFactory.Mobile.Models;

public sealed class CategoryDefinition
{
    public string Id { get; set; } = string.Empty;
    public string NameKey { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}

public sealed class QuestionDefinition
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = "single";
    public string TextKey { get; set; } = string.Empty;
    public bool Required { get; set; }
    public List<QuestionOption> Options { get; set; } = new();
}

public sealed class QuestionOption
{
    public string Value { get; set; } = string.Empty;
    public string LabelKey { get; set; } = string.Empty;
}

public sealed class UserAnswer
{
    public string QuestionId { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public sealed class RuleDefinition
{
    public string Id { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public Dictionary<string, string> When { get; set; } = new();
    public int Score { get; set; }
    public string FreeResultId { get; set; } = string.Empty;
    public string PremiumResultId { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
}

public sealed class RuleMatch
{
    public string FreeResultId { get; set; } = string.Empty;
    public string PremiumResultId { get; set; } = string.Empty;
    public string RuleId { get; set; } = string.Empty;
    public int Score { get; set; }
    public string Reason { get; set; } = string.Empty;
    public List<string> MatchedConditions { get; set; } = new();
    public List<string> AlternativeRuleIds { get; set; } = new();
    public List<string> AlternativePremiumResultIds { get; set; } = new();
}
