using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class ImageAnalysisServiceTests
{
    [Fact]
    public void PolicyService_EnablesExpectedProjects()
    {
        var service = new ImageAnalysisPolicyService();

        var enabled = service.GetEnabledProjectIds();

        Assert.Contains("plama-ratownik", enabled);
        Assert.Contains("pokoj-makeover", enabled);
        Assert.Contains("rysunek-coach", enabled);
        Assert.Contains("outfit-coach", enabled);
        Assert.Contains("fryzury-proste", enabled);
        Assert.Contains("barber-translator", enabled);
        Assert.Contains("zmywarka-diagnosta", enabled);
        Assert.Contains("silikon-fuga-fix", enabled);
    }

    [Fact]
    public async Task AnalyzeAsync_RejectsDisabledProject()
    {
        var service = CreateService();

        var result = await service.AnalyzeAsync(new ImageAnalysisRequest
        {
            ProjectId = "kolek-dobieracz",
            ContentType = "image/jpeg",
            SizeBytes = 1024
        });

        Assert.False(result.IsAccepted);
        Assert.False(result.IsEnabled);
        Assert.Contains("nie obsługuje", result.Summary, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task AnalyzeAsync_RejectsUnsupportedContentType()
    {
        var service = CreateService();

        var result = await service.AnalyzeAsync(new ImageAnalysisRequest
        {
            ProjectId = "plama-ratownik",
            ContentType = "application/pdf",
            SizeBytes = 1024
        });

        Assert.False(result.IsAccepted);
        Assert.True(result.IsEnabled);
        Assert.Contains("Nieobsługiwany", result.Summary, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task AnalyzeAsync_RejectsOversizedFile()
    {
        var service = CreateService();

        var result = await service.AnalyzeAsync(new ImageAnalysisRequest
        {
            ProjectId = "plama-ratownik",
            ContentType = "image/png",
            SizeBytes = 6 * 1024 * 1024
        });

        Assert.False(result.IsAccepted);
        Assert.Contains("za duży", result.Summary, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task AnalyzeAsync_BlocksWhenLocalModelIsNotConfiguredOrVerified()
    {
        var service = CreateService();

        var result = await service.AnalyzeAsync(new ImageAnalysisRequest
        {
            ProjectId = "plama-ratownik",
            CategoryId = "coffee",
            FileName = "stain.jpg",
            ContentType = "image/jpeg",
            SizeBytes = 1024
        });

        Assert.False(result.IsAccepted);
        Assert.True(result.IsEnabled);
        Assert.True(result.IsSafetySensitive);
        Assert.Contains("model obrazu", result.Summary, StringComparison.OrdinalIgnoreCase);
    }

    private static ImageAnalysisService CreateService()
    {
        var catalog = new LocalAiModelCatalogService();
        var store = new LocalAiModelStore(modelDirectory: Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        var provider = new OnDeviceImageAnalysisProvider(catalog, store, new LocalVisionInferenceEngine());
        return new ImageAnalysisService(new ImageAnalysisPolicyService(), provider);
    }
}
