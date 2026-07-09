using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class ImageAnalysisPolicyService
{
    private static readonly HashSet<string> EnabledProjects = new(StringComparer.OrdinalIgnoreCase)
    {
        "plama-ratownik",
        "pokoj-makeover",
        "rysunek-coach",
        "outfit-coach",
        "fryzury-proste",
        "barber-translator",
        "zmywarka-diagnosta",
        "silikon-fuga-fix"
    };

    private static readonly HashSet<string> SafetySensitiveProjects = new(StringComparer.OrdinalIgnoreCase)
    {
        "plama-ratownik",
        "zmywarka-diagnosta",
        "silikon-fuga-fix"
    };

    public ImageAnalysisProjectPolicy GetPolicy(string projectId)
    {
        return new ImageAnalysisProjectPolicy
        {
            ProjectId = projectId,
            IsEnabled = EnabledProjects.Contains(projectId),
            IsSafetySensitive = SafetySensitiveProjects.Contains(projectId),
            MaxSizeBytes = 5 * 1024 * 1024,
            AcceptedContentTypes = new[] { "image/jpeg", "image/png", "image/webp" }
        };
    }

    public IReadOnlyList<string> GetEnabledProjectIds() =>
        EnabledProjects.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToArray();
}
