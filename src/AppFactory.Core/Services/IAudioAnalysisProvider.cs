using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public interface IAudioAnalysisProvider
{
    Task<AudioAnalysisResult> AnalyzeAsync(AudioAnalysisRequest request, AudioAnalysisProjectPolicy policy, CancellationToken cancellationToken = default);
}
