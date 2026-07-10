using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class AudioAnalysisServiceTests
{
    [Fact]
    public void PolicyService_EnablesExpectedProjects()
    {
        var service = new AudioAnalysisPolicyService();

        var enabled = service.GetEnabledProjectIds();

        Assert.Contains("zmywarka-diagnosta", enabled);
        Assert.Contains("pies-trener-7dni", enabled);
        Assert.Contains("kot-bawi-sie", enabled);
    }

    [Fact]
    public async Task AnalyzeAsync_RejectsDisabledProject()
    {
        var service = CreateService();

        var result = await service.AnalyzeAsync(new AudioAnalysisRequest
        {
            ProjectId = "kolek-dobieracz",
            ContentType = "audio/wav",
            SizeBytes = 1024,
            Duration = TimeSpan.FromSeconds(2)
        });

        Assert.False(result.IsAccepted);
        Assert.False(result.IsEnabled);
        Assert.Contains("nie obsługuje", result.Summary, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task AnalyzeAsync_RejectsUnsupportedContentType()
    {
        var service = CreateService();

        var result = await service.AnalyzeAsync(new AudioAnalysisRequest
        {
            ProjectId = "zmywarka-diagnosta",
            ContentType = "video/mp4",
            SizeBytes = 1024,
            Duration = TimeSpan.FromSeconds(2)
        });

        Assert.False(result.IsAccepted);
        Assert.True(result.IsEnabled);
        Assert.Contains("Nieobsługiwany", result.Summary, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task AnalyzeAsync_RejectsTooLongAudio()
    {
        var service = CreateService();

        var result = await service.AnalyzeAsync(new AudioAnalysisRequest
        {
            ProjectId = "zmywarka-diagnosta",
            ContentType = "audio/wav",
            SizeBytes = 1024,
            Duration = TimeSpan.FromSeconds(30)
        });

        Assert.False(result.IsAccepted);
        Assert.Contains("za długie", result.Summary, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task AnalyzeAsync_BlocksWhenLocalAudioModelIsNotConfiguredOrVerified()
    {
        var service = CreateService();

        var result = await service.AnalyzeAsync(new AudioAnalysisRequest
        {
            ProjectId = "zmywarka-diagnosta",
            ContentType = "audio/wav",
            SizeBytes = 1024,
            Duration = TimeSpan.FromSeconds(3)
        });

        Assert.False(result.IsAccepted);
        Assert.True(result.IsEnabled);
        Assert.True(result.IsSafetySensitive);
        Assert.Contains("model dźwięku", result.Summary, StringComparison.OrdinalIgnoreCase);
    }

    private static AudioAnalysisService CreateService()
    {
        var catalog = new LocalAiModelCatalogService();
        var store = new LocalAiModelStore(modelDirectory: Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        var provider = new OnDeviceAudioAnalysisProvider(catalog, store, new LocalAudioInferenceEngine());
        return new AudioAnalysisService(new AudioAnalysisPolicyService(), provider);
    }
}
