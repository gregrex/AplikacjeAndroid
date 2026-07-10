using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class LocalAiModelCatalogService
{
    private static readonly LocalAiModelProfile VisionModel = new()
    {
        ModelId = "local-vision-v1",
        Modality = "image",
        Version = "0.1.0",
        FileName = "local-vision-v1.onnx",
        DownloadUrl = "",
        Sha256 = "",
        SizeBytes = 0,
        IsRequiredForProduction = true
    };

    private static readonly LocalAiModelProfile AudioModel = new()
    {
        ModelId = "local-audio-v1",
        Modality = "audio",
        Version = "0.1.0",
        FileName = "local-audio-v1.onnx",
        DownloadUrl = "",
        Sha256 = "",
        SizeBytes = 0,
        IsRequiredForProduction = true
    };

    public IReadOnlyList<LocalAiModelProfile> GetAll() => new[] { VisionModel, AudioModel };

    public LocalAiModelProfile? FindById(string modelId) =>
        GetAll().FirstOrDefault(x => string.Equals(x.ModelId, modelId, StringComparison.OrdinalIgnoreCase));

    public LocalAiModelProfile? FindByModality(string modality) =>
        GetAll().FirstOrDefault(x => string.Equals(x.Modality, modality, StringComparison.OrdinalIgnoreCase));
}
