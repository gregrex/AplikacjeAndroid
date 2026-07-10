using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class AiSuggestionStateService
{
    private readonly Dictionary<string, List<AiSuggestedAnswer>> _suggestions = new(StringComparer.OrdinalIgnoreCase);

    public void StoreImageSuggestions(string projectId, IReadOnlyList<ImageAnswerSuggestion> suggestions)
    {
        Store(projectId, suggestions.Select(x => new AiSuggestedAnswer
        {
            QuestionId = x.QuestionId,
            Value = x.Value,
            Confidence = x.Confidence,
            Reason = x.Reason,
            Source = "image"
        }));
    }

    public void StoreAudioSuggestions(string projectId, IReadOnlyList<AudioAnswerSuggestion> suggestions)
    {
        Store(projectId, suggestions.Select(x => new AiSuggestedAnswer
        {
            QuestionId = x.QuestionId,
            Value = x.Value,
            Confidence = x.Confidence,
            Reason = x.Reason,
            Source = "audio"
        }));
    }

    public AiSuggestedAnswer? GetForQuestion(string projectId, string questionId)
    {
        if (!_suggestions.TryGetValue(projectId, out var values))
        {
            return null;
        }

        return values
            .Where(x => string.Equals(x.QuestionId, questionId, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.Confidence)
            .FirstOrDefault();
    }

    public void Remove(string projectId, string questionId)
    {
        if (!_suggestions.TryGetValue(projectId, out var values))
        {
            return;
        }

        values.RemoveAll(x => string.Equals(x.QuestionId, questionId, StringComparison.OrdinalIgnoreCase));
    }

    public void Clear(string projectId) => _suggestions.Remove(projectId);

    private void Store(string projectId, IEnumerable<AiSuggestedAnswer> suggestions)
    {
        if (!_suggestions.TryGetValue(projectId, out var values))
        {
            values = new List<AiSuggestedAnswer>();
            _suggestions[projectId] = values;
        }

        foreach (var suggestion in suggestions.Where(x => !string.IsNullOrWhiteSpace(x.QuestionId) && !string.IsNullOrWhiteSpace(x.Value)))
        {
            values.RemoveAll(x => string.Equals(x.QuestionId, suggestion.QuestionId, StringComparison.OrdinalIgnoreCase)
                                  && string.Equals(x.Source, suggestion.Source, StringComparison.OrdinalIgnoreCase));
            values.Add(suggestion);
        }
    }
}

public sealed class AiSuggestedAnswer
{
    public string QuestionId { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public double Confidence { get; init; }
    public string Reason { get; init; } = string.Empty;
    public string Source { get; init; } = string.Empty;
}
