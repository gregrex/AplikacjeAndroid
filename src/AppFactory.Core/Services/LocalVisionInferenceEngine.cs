using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class LocalVisionInferenceEngine : ILocalVisionInferenceEngine
{
    public Task<ImageAnalysisResult> AnalyzeAsync(ImageAnalysisRequest request, string modelPath, ImageAnalysisProjectPolicy policy, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new ImageAnalysisResult
        {
            ProjectId = request.ProjectId,
            IsEnabled = policy.IsEnabled,
            IsAccepted = false,
            IsSafetySensitive = policy.IsSafetySensitive,
            Summary = "Lokalny model obrazu jest gotowy, ale natywny runtime inferencji nie został jeszcze podłączony.",
            Warnings = new[] { "Podłącz implementację ILocalVisionInferenceEngine dla Android ONNX/TFLite przed produkcyjnym rozpoznawaniem obrazu." }
        });
    }
}
