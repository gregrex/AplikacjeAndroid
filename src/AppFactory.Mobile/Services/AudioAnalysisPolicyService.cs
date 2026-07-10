using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class AudioAnalysisPolicyService
{
    private static readonly HashSet<string> EnabledProjects = new(StringComparer.OrdinalIgnoreCase)
    {
        "zmywarka-diagnosta",
        "pies-trener-7dni",
        "kot-bawi-sie"
    };

    private static readonly HashSet<string> SafetySensitiveProjects = new(StringComparer.OrdinalIgnoreCase)
    {
        "zmywarka-diagnosta"
    };

    public AudioAnalysisProjectPolicy GetPolicy(string projectId)
    {
        return new AudioAnalysisProjectPolicy
        {
            ProjectId = projectId,
            IsEnabled = EnabledProjects.Contains(projectId),
            IsSafetySensitive = SafetySensitiveProjects.Contains(projectId),
            MaxSizeBytes = 10 * 1024 * 1024,
            MaxDuration = TimeSpan.FromSeconds(20),
            AcceptedContentTypes = new[] { "audio/wav", "audio/mpeg", "audio/mp4", "audio/aac", "audio/ogg" }
        };
    }

    public IReadOnlyList<string> GetEnabledProjectIds() =>
        EnabledProjects.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToArray();
}
