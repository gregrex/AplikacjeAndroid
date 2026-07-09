using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public interface IImageAnalysisProvider
{
    Task<ImageAnalysisResult> AnalyzeAsync(ImageAnalysisRequest request, ImageAnalysisProjectPolicy policy, CancellationToken cancellationToken = default);
}
