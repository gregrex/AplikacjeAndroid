namespace AppFactory.Mobile.Models;

public sealed class AudioAnalysisRequest
{
    public string ProjectId { get; init; } = string.Empty;
    public string CategoryId { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
    public long SizeBytes { get; init; }
    public TimeSpan Duration { get; init; }
    public IReadOnlyDictionary<string, string> UserContext { get; init; } = new Dictionary<string, string>();
}

public sealed class AudioAnalysisResult
{
    public string ProjectId { get; init; } = string.Empty;
    public bool IsEnabled { get; init; }
    public bool IsAccepted { get; init; }
    public bool IsSafetySensitive { get; init; }
    public string Summary { get; init; } = string.Empty;
    public IReadOnlyList<AudioAnswerSuggestion> SuggestedAnswers { get; init; } = Array.Empty<AudioAnswerSuggestion>();
    public IReadOnlyList<string> Warnings { get; init; } = Array.Empty<string>();
}

public sealed class AudioAnswerSuggestion
{
    public string QuestionId { get; init; } = string.Empty;
    public string Value { get; init; }
        = string.Empty;
    public double Confidence { get; init; }
    public string Reason { get; init; } = string.Empty;
}

public sealed class AudioAnalysisProjectPolicy
{
    public string ProjectId { get; init; } = string.Empty;
    public bool IsEnabled { get; init; }
    public bool IsSafetySensitive { get; init; }
    public long MaxSizeBytes { get; init; } = 10 * 1024 * 1024;
    public TimeSpan MaxDuration { get; init; } = TimeSpan.FromSeconds(20);
    public IReadOnlyList<string> AcceptedContentTypes { get; init; } = new[] { "audio/wav", "audio/mpeg", "audio/mp4", "audio/aac", "audio/ogg" };
}
