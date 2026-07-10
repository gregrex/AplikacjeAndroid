using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class OnDeviceImageAnalysisProvider : IImageAnalysisProvider
{
    private readonly LocalAiModelCatalogService _catalog;
    private readonly LocalAiModelStore _modelStore;
    private readonly ILocalVisionInferenceEngine _engine;

    public OnDeviceImageAnalysisProvider(LocalAiModelCatalogService catalog, LocalAiModelStore modelStore, ILocalVisionInferenceEngine engine)
    {
        _catalog = catalog;
        _modelStore = modelStore;
        _engine = engine;
    }

    public async Task<ImageAnalysisResult> AnalyzeAsync(ImageAnalysisRequest request, ImageAnalysisProjectPolicy policy, CancellationToken cancellationToken = default)
    {
        var model = _catalog.FindByModality("image");
        if (model is null)
        {
            return Blocked(request, policy, "Brak profilu lokalnego modelu obrazu.");
        }

        var status = _modelStore.GetStatus(model);
        if (!status.IsVerified)
        {
            return Blocked(request, policy, "Lokalny model obrazu nie jest pobrany lub nie przeszedł weryfikacji.");
        }

        return await _engine.AnalyzeAsync(request, status.LocalPath, policy, cancellationToken);
    }

    private static ImageAnalysisResult Blocked(ImageAnalysisRequest request, ImageAnalysisProjectPolicy policy, string message) => new()
    {
        ProjectId = request.ProjectId,
        IsEnabled = policy.IsEnabled,
        IsAccepted = false,
        IsSafetySensitive = policy.IsSafetySensitive,
        Summary = message,
        Warnings = new[] { message }
    };
}
