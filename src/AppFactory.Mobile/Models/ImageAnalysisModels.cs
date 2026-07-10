namespace AppFactory.Mobile.Models;

public sealed class ImageAnalysisRequest
{
    public string ProjectId { get; init; } = string.Empty;
    public string CategoryId { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public string LocalFilePath { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
    public long SizeBytes { get; init; }
    public IReadOnlyDictionary<string, string> UserContext { get; init; } = new Dictionary<string, string>();
}

public sealed class ImageAnalysisResult
{
    public string ProjectId { get; init; } = string.Empty;
    public bool IsEnabled { get; init; }
    public bool IsAccepted { get; init; }
    public bool IsSafetySensitive { get; init; }
    public string Summary { get; init; } = string.Empty;
    public IReadOnlyList<ImageAnswerSuggestion> SuggestedAnswers { get; init; } = Array.Empty<ImageAnswerSuggestion>();
    public IReadOnlyList<string> Warnings { get; init; } = Array.Empty<string>();
}

public sealed class ImageAnswerSuggestion
{
    public string QuestionId { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public double Confidence { get; init; }
    public string Reason { get; init; } = string.Empty;
}

public sealed class ImageAnalysisProjectPolicy
{
    public string ProjectId { get; init; } = string.Empty;
    public bool IsEnabled { get; init; }
    public bool IsSafetySensitive { get; init; }
    public long MaxSizeBytes { get; init; } = 5 * 1024 * 1024;
    public IReadOnlyList<string> AcceptedContentTypes { get; init; } = new[] { "image/jpeg", "image/png", "image/webp" };
    public IReadOnlyList<string> SupportedCategoryIds { get; init; } = Array.Empty<string>();
}
