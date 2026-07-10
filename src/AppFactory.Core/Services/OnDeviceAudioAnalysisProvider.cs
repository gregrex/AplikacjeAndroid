using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class OnDeviceAudioAnalysisProvider : IAudioAnalysisProvider
{
    private readonly LocalAiModelCatalogService _catalog;
    private readonly LocalAiModelStore _modelStore;
    private readonly ILocalAudioInferenceEngine _engine;

    public OnDeviceAudioAnalysisProvider(LocalAiModelCatalogService catalog, LocalAiModelStore modelStore, ILocalAudioInferenceEngine engine)
    {
        _catalog = catalog;
        _modelStore = modelStore;
        _engine = engine;
    }

    public async Task<AudioAnalysisResult> AnalyzeAsync(AudioAnalysisRequest request, AudioAnalysisProjectPolicy policy, CancellationToken cancellationToken = default)
    {
        var model = _catalog.FindByModality("audio");
        if (model is null)
        {
            return Blocked(request, policy, "Brak profilu lokalnego modelu dźwięku.");
        }

        var status = _modelStore.GetStatus(model);
        if (!status.IsVerified)
        {
            return Blocked(request, policy, "Lokalny model dźwięku nie jest pobrany lub nie przeszedł weryfikacji.");
        }

        return await _engine.AnalyzeAsync(request, status.LocalPath, policy, cancellationToken);
    }

    private static AudioAnalysisResult Blocked(AudioAnalysisRequest request, AudioAnalysisProjectPolicy policy, string message) => new()
    {
        ProjectId = request.ProjectId,
        IsEnabled = policy.IsEnabled,
        IsAccepted = false,
        IsSafetySensitive = policy.IsSafetySensitive,
        Summary = message,
        Warnings = new[] { message }
    };
}
