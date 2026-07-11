using System.Diagnostics;
using AppFactory.Mobile.Models;
using Microsoft.Extensions.Logging;

namespace AppFactory.Mobile.Services;

public sealed class ImageAnalysisService
{
    private readonly ImageAnalysisPolicyService _policyService;
    private readonly IImageAnalysisProvider _provider;
    private readonly ILogger<ImageAnalysisService> _logger;

    public ImageAnalysisService(
        ImageAnalysisPolicyService policyService,
        IImageAnalysisProvider provider,
        ILogger<ImageAnalysisService> logger)
    {
        _policyService = policyService;
        _provider = provider;
        _logger = logger;
    }

    public async Task<ImageAnalysisResult> AnalyzeAsync(ImageAnalysisRequest request, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation(
            "Image analysis started. Project={ProjectId} ContentType={ContentType} SizeBytes={SizeBytes}",
            request.ProjectId,
            request.ContentType,
            request.SizeBytes);

        var policy = _policyService.GetPolicy(request.ProjectId);
        if (!policy.IsEnabled)
        {
            return Reject(request.ProjectId, policy, "Ten projekt nie obsługuje analizy obrazu.");
        }

        if (!policy.AcceptedContentTypes.Contains(request.ContentType, StringComparer.OrdinalIgnoreCase))
        {
            return Reject(request.ProjectId, policy, $"Nieobsługiwany typ pliku: {request.ContentType}.");
        }

        if (request.SizeBytes <= 0)
        {
            return Reject(request.ProjectId, policy, "Plik obrazu jest pusty.");
        }

        if (request.SizeBytes > policy.MaxSizeBytes)
        {
            return Reject(request.ProjectId, policy, $"Plik obrazu jest za duży. Limit: {policy.MaxSizeBytes} bajtów.");
        }

        try
        {
            var result = await _provider.AnalyzeAsync(request, policy, cancellationToken);
            stopwatch.Stop();
            _logger.LogInformation(
                "Image analysis completed. Project={ProjectId} Accepted={Accepted} Suggestions={SuggestionCount} DurationMs={DurationMs}",
                request.ProjectId,
                result.IsAccepted,
                result.SuggestedAnswers.Count,
                stopwatch.ElapsedMilliseconds);
            return result;
        }
        catch (OperationCanceledException)
        {
            stopwatch.Stop();
            _logger.LogWarning(
                "Image analysis cancelled. Project={ProjectId} DurationMs={DurationMs}",
                request.ProjectId,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(
                ex,
                "Image analysis failed unexpectedly. Project={ProjectId} DurationMs={DurationMs}",
                request.ProjectId,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    private ImageAnalysisResult Reject(string projectId, ImageAnalysisProjectPolicy policy, string message)
    {
        _logger.LogWarning("Image analysis rejected. Project={ProjectId} Reason={Reason}", projectId, message);
        return new ImageAnalysisResult
        {
            ProjectId = projectId,
            IsEnabled = policy.IsEnabled,
            IsAccepted = false,
            IsSafetySensitive = policy.IsSafetySensitive,
            Summary = message,
            Warnings = new[] { message }
        };
    }
}
