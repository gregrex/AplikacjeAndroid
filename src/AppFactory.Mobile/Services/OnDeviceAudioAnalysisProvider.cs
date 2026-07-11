using AppFactory.Mobile.Models;
using Microsoft.Extensions.Logging;

namespace AppFactory.Mobile.Services;

public sealed class OnDeviceAudioAnalysisProvider : IAudioAnalysisProvider
{
    private readonly LocalAiModelCatalogService _catalog;
    private readonly LocalAiModelStore _modelStore;
    private readonly ILocalAudioInferenceEngine _engine;
    private readonly ILogger<OnDeviceAudioAnalysisProvider> _logger;

    public OnDeviceAudioAnalysisProvider(
        LocalAiModelCatalogService catalog,
        LocalAiModelStore modelStore,
        ILocalAudioInferenceEngine engine,
        ILogger<OnDeviceAudioAnalysisProvider> logger)
    {
        _catalog = catalog;
        _modelStore = modelStore;
        _engine = engine;
        _logger = logger;
    }

    public async Task<AudioAnalysisResult> AnalyzeAsync(AudioAnalysisRequest request, AudioAnalysisProjectPolicy policy, CancellationToken cancellationToken = default)
    {
        var model = _catalog.FindByModality("audio");
        if (model is null)
        {
            return Block(request, policy, "Brak profilu lokalnego modelu dźwięku.");
        }

        var status = _modelStore.GetStatus(model);
        if (!status.IsVerified)
        {
            _logger.LogWarning(
                "Local audio model is not ready. Project={ProjectId} ModelId={ModelId} Configured={Configured} Downloaded={Downloaded}",
                request.ProjectId,
                model.ModelId,
                status.IsConfigured,
                status.IsDownloaded);
            return Block(request, policy, "Lokalny model dźwięku nie jest pobrany lub nie przeszedł weryfikacji.");
        }

        _logger.LogDebug("Starting local audio inference. Project={ProjectId} ModelId={ModelId}", request.ProjectId, model.ModelId);
        return await _engine.AnalyzeAsync(request, status.LocalPath, policy, cancellationToken);
    }

    private AudioAnalysisResult Block(AudioAnalysisRequest request, AudioAnalysisProjectPolicy policy, string message)
    {
        _logger.LogWarning("Local audio analysis blocked. Project={ProjectId} Reason={Reason}", request.ProjectId, message);
        return new AudioAnalysisResult
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
