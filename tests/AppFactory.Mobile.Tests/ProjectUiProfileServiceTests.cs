using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class ProjectUiProfileServiceTests
{
    [Theory]
    [InlineData("plama-ratownik", "instruction-checklist")]
    [InlineData("kolek-dobieracz", "technical-safety-checklist")]
    [InlineData("pies-trener-7dni", "seven-day-plan")]
    [InlineData("bajka-z-rysunku", "story-page")]
    [InlineData("vinted-olx-opis", "sales-copy-card")]
    [InlineData("kot-bawi-sie", "pet-activity-plan")]
    [InlineData("barber-translator", "style-checklist")]
    [InlineData("outfit-coach", "style-checklist")]
    [InlineData("domfix", "technical-safety-checklist")]
    [InlineData("fryzury-proste", "style-checklist")]
    [InlineData("rysunek-coach", "creative-lesson")]
    [InlineData("bukietownik", "arrangement-plan")]
    [InlineData("pokoj-makeover", "arrangement-plan")]
    [InlineData("pakowanie-paczek", "packing-checklist")]
    [InlineData("silikon-fuga-fix", "technical-safety-checklist")]
    [InlineData("szydelko-pomocnik", "craft-helper")]
    [InlineData("chleb-zakwas-coach", "diagnostic-checklist")]
    [InlineData("zmywarka-diagnosta", "diagnostic-checklist")]
    [InlineData("krawat-garnitur-coach", "style-checklist")]
    [InlineData("router-wifi-diagnosta", "diagnostic-checklist")]
    public void GetProfile_ReturnsExpectedResultViewType(string projectId, string expectedViewType)
    {
        var profile = new ProjectUiProfileService().GetProfile(projectId);

        Assert.Equal(projectId, profile.ProjectId);
        Assert.Equal(expectedViewType, profile.ResultViewType);
        Assert.False(string.IsNullOrWhiteSpace(profile.Icon));
        Assert.False(string.IsNullOrWhiteSpace(profile.Badge));
        Assert.False(string.IsNullOrWhiteSpace(profile.HeroTitle));
    }

    [Fact]
    public void GetProfile_CopyProjects_EnableCopyAction()
    {
        var service = new ProjectUiProfileService();

        Assert.True(service.GetProfile("vinted-olx-opis").ShowCopyAction);
        Assert.True(service.GetProfile("barber-translator").ShowCopyAction);
    }

    [Fact]
    public void GetProfile_CrochetProject_EnablesPersistentTool()
    {
        var profile = new ProjectUiProfileService().GetProfile("szydelko-pomocnik");

        Assert.Equal("crochet-counter", profile.ToolKind);
    }

    [Fact]
    public void GetProfile_UnknownProject_ReturnsInstructionChecklistFallback()
    {
        var profile = new ProjectUiProfileService().GetProfile("unknown-project");

        Assert.Equal("instruction-checklist", profile.ResultViewType);
        Assert.Equal("unknown-project", profile.ProjectId);
    }
}
