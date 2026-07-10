using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public interface ILocalAudioInferenceEngine
{
    Task<AudioAnalysisResult> AnalyzeAsync(AudioAnalysisRequest request, string modelPath, AudioAnalysisProjectPolicy policy, CancellationToken cancellationToken = default);
}
