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
    public void GetProfile_ReturnsExpectedResultViewType(string projectId, string expectedViewType)
    {
        var service = new ProjectUiProfileService();

        var profile = service.GetProfile(projectId);

        Assert.Equal(expectedViewType, profile.ResultViewType);
    }

    [Fact]
    public void GetProfile_SalesDescriptionProject_EnablesCopyAction()
    {
        var service = new ProjectUiProfileService();

        var profile = service.GetProfile("vinted-olx-opis");

        Assert.True(profile.ShowCopyAction);
    }

    [Fact]
    public void GetProfile_UnknownProject_ReturnsInstructionChecklistFallback()
    {
        var service = new ProjectUiProfileService();

        var profile = service.GetProfile("unknown-project");

        Assert.Equal("instruction-checklist", profile.ResultViewType);
    }
}
