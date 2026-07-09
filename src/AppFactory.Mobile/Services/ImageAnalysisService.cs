using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class ImageAnalysisService
{
    private readonly ImageAnalysisPolicyService _policyService;
    private readonly IImageAnalysisProvider _provider;

    public ImageAnalysisService(ImageAnalysisPolicyService policyService, IImageAnalysisProvider provider)
    {
        _policyService = policyService;
        _provider = provider;
    }

    public async Task<ImageAnalysisResult> AnalyzeAsync(ImageAnalysisRequest request, CancellationToken cancellationToken = default)
    {
        var policy = _policyService.GetPolicy(request.ProjectId);
        if (!policy.IsEnabled)
        {
            return Rejected(request.ProjectId, policy, "Ten projekt nie obsługuje analizy obrazu.");
        }

        if (!policy.AcceptedContentTypes.Contains(request.ContentType, StringComparer.OrdinalIgnoreCase))
        {
            return Rejected(request.ProjectId, policy, $"Nieobsługiwany typ pliku: {request.ContentType}.");
        }

        if (request.SizeBytes <= 0)
        {
            return Rejected(request.ProjectId, policy, "Plik obrazu jest pusty.");
        }

        if (request.SizeBytes > policy.MaxSizeBytes)
        {
            return Rejected(request.ProjectId, policy, $"Plik obrazu jest za duży. Limit: {policy.MaxSizeBytes} bajtów.");
        }

        return await _provider.AnalyzeAsync(request, policy, cancellationToken);
    }

    private static ImageAnalysisResult Rejected(string projectId, ImageAnalysisProjectPolicy policy, string message) => new()
    {
        ProjectId = projectId,
        IsEnabled = policy.IsEnabled,
        IsAccepted = false,
        IsSafetySensitive = policy.IsSafetySensitive,
        Summary = message,
        Warnings = new[] { message }
    };
}
