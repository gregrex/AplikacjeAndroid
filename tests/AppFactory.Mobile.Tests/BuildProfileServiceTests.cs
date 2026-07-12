using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class BuildProfileServiceTests
{
    [Fact]
    public void GetCatalogProfile_ReturnsCatalogBuild()
    {
        var service = new BuildProfileService();

        var profile = service.GetCatalogProfile();

        Assert.True(profile.IsCatalogBuild);
        Assert.Equal("catalog", profile.ProjectId);
        Assert.Equal("AppFactory Pomocniki", profile.ApplicationTitle);
        Assert.Equal("pl.gbcom.appfactory", profile.ApplicationId);
        Assert.Equal("1.0.0", profile.DisplayVersion);
        Assert.Equal(1, profile.Version);
    }

    [Fact]
    public void GetProjectProfile_ReturnsStableApplicationId()
    {
        var service = new BuildProfileService();
        var project = new ProjectCatalogItem("router-wifi-diagnosta", "Router WiFi Diagnosta", "WiFi checklist");

        var profile = service.GetProjectProfile(project);

        Assert.False(profile.IsCatalogBuild);
        Assert.Equal("router-wifi-diagnosta", profile.ProjectId);
        Assert.Equal("Router WiFi Diagnosta", profile.ApplicationTitle);
        Assert.Equal("pl.gbcom.appfactory.routerwifidiagnosta", profile.ApplicationId);
    }

    [Fact]
    public void GetAllProfiles_ReturnsCatalogAndEveryProject()
    {
        var service = new BuildProfileService();
        var projects = new[]
        {
            new ProjectCatalogItem("a-one", "A One", "First"),
            new ProjectCatalogItem("b-two", "B Two", "Second")
        };

        var profiles = service.GetAllProfiles(projects);

        Assert.Equal(3, profiles.Count);
        Assert.Contains(profiles, x => x.IsCatalogBuild && x.ProjectId == "catalog");
        Assert.Contains(profiles, x => x.ProjectId == "a-one" && x.ApplicationId == "pl.gbcom.appfactory.aone");
        Assert.Contains(profiles, x => x.ProjectId == "b-two" && x.ApplicationId == "pl.gbcom.appfactory.btwo");
    }
}
