using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class LocalAudioInferenceEngine : ILocalAudioInferenceEngine
{
    public Task<AudioAnalysisResult> AnalyzeAsync(AudioAnalysisRequest request, string modelPath, AudioAnalysisProjectPolicy policy, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new AudioAnalysisResult
        {
            ProjectId = request.ProjectId,
            IsEnabled = policy.IsEnabled,
            IsAccepted = false,
            IsSafetySensitive = policy.IsSafetySensitive,
            Summary = "Lokalny model dźwięku jest gotowy, ale natywny runtime inferencji nie został jeszcze podłączony.",
            Warnings = new[] { "Podłącz implementację ILocalAudioInferenceEngine dla Android ONNX/TFLite przed produkcyjnym rozpoznawaniem dźwięku." }
        });
    }
}
