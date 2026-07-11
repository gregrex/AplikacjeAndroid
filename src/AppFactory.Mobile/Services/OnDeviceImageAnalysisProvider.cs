using AppFactory.Mobile.Models;
using Microsoft.Extensions.Logging;

namespace AppFactory.Mobile.Services;

public sealed class OnDeviceImageAnalysisProvider : IImageAnalysisProvider
{
    private readonly LocalAiModelCatalogService _catalog;
    private readonly LocalAiModelStore _modelStore;
    private readonly ILocalVisionInferenceEngine _engine;
    private readonly ILogger<OnDeviceImageAnalysisProvider> _logger;

    public OnDeviceImageAnalysisProvider(
        LocalAiModelCatalogService catalog,
        LocalAiModelStore modelStore,
        ILocalVisionInferenceEngine engine,
        ILogger<OnDeviceImageAnalysisProvider> logger)
    {
        _catalog = catalog;
        _modelStore = modelStore;
        _engine = engine;
        _logger = logger;
    }

    public async Task<ImageAnalysisResult> AnalyzeAsync(ImageAnalysisRequest request, ImageAnalysisProjectPolicy policy, CancellationToken cancellationToken = default)
    {
        var model = _catalog.FindByModality("image");
        if (model is null)
        {
            return Block(request, policy, "Brak profilu lokalnego modelu obrazu.");
        }

        var status = _modelStore.GetStatus(model);
        if (!status.IsVerified)
        {
            _logger.LogWarning(
                "Local vision model is not ready. Project={ProjectId} ModelId={ModelId} Configured={Configured} Downloaded={Downloaded}",
                request.ProjectId,
                model.ModelId,
                status.IsConfigured,
                status.IsDownloaded);
            return Block(request, policy, "Lokalny model obrazu nie jest pobrany lub nie przeszedł weryfikacji.");
        }

        _logger.LogDebug("Starting local vision inference. Project={ProjectId} ModelId={ModelId}", request.ProjectId, model.ModelId);
        return await _engine.AnalyzeAsync(request, status.LocalPath, policy, cancellationToken);
    }

    private ImageAnalysisResult Block(ImageAnalysisRequest request, ImageAnalysisProjectPolicy policy, string message)
    {
        _logger.LogWarning("Local vision analysis blocked. Project={ProjectId} Reason={Reason}", request.ProjectId, message);
        return new ImageAnalysisResult
        {
            ProjectId = request.ProjectId,
            IsEnabled = policy.IsEnabled,
            IsAccepted = false,
            IsSafetySensitive = policy.IsSafetySensitive,
            Summary = message,
            Warnings = new[] { message }
        };
    }
}
