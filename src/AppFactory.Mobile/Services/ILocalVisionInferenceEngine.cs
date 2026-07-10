using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public interface ILocalVisionInferenceEngine
{
    Task<ImageAnalysisResult> AnalyzeAsync(ImageAnalysisRequest request, string modelPath, ImageAnalysisProjectPolicy policy, CancellationToken cancellationToken = default);
}
