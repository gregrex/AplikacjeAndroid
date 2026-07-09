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
    public async Task AnalyzeAsync_ReturnsMockSuggestions_ForEnabledProject()
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

        Assert.True(result.IsAccepted);
        Assert.True(result.IsEnabled);
        Assert.True(result.IsSafetySensitive);
        Assert.Contains(result.SuggestedAnswers, x => x.QuestionId == "material" && x.Value == "cotton");
        Assert.Contains(result.Warnings, x => x.Contains("podpowiedzi", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task AnalyzeAsync_ReturnsSafetyWarning_ForSafetySensitiveProject()
    {
        var service = CreateService();

        var result = await service.AnalyzeAsync(new ImageAnalysisRequest
        {
            ProjectId = "zmywarka-diagnosta",
            ContentType = "image/webp",
            SizeBytes = 2048
        });

        Assert.True(result.IsSafetySensitive);
        Assert.Contains(result.Warnings, x => x.Contains("safety-sensitive", StringComparison.OrdinalIgnoreCase) || x.Contains("ostrożności", StringComparison.OrdinalIgnoreCase));
    }

    private static ImageAnalysisService CreateService() =>
        new(new ImageAnalysisPolicyService(), new MockImageAnalysisProvider());
}
