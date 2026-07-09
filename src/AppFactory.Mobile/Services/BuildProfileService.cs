using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class BuildProfileService
{
    public BuildProfile GetCatalogProfile() => new()
    {
        ProjectId = "catalog",
        ApplicationTitle = "AppFactory",
        ApplicationId = "pl.gbcom.appfactory",
        DisplayVersion = "0.1.0",
        Version = 1,
        IsCatalogBuild = true
    };

    public BuildProfile GetProjectProfile(ProjectCatalogItem project)
    {
        var safeId = project.Id.Replace("-", string.Empty, StringComparison.OrdinalIgnoreCase);
        return new BuildProfile
        {
            ProjectId = project.Id,
            ApplicationTitle = project.Name,
            ApplicationId = $"pl.gbcom.appfactory.{safeId}",
            DisplayVersion = "0.1.0",
            Version = 1,
            IsCatalogBuild = false
        };
    }

    public IReadOnlyList<BuildProfile> GetAllProfiles(IReadOnlyList<ProjectCatalogItem> projects)
    {
        var profiles = new List<BuildProfile> { GetCatalogProfile() };
        profiles.AddRange(projects.Select(GetProjectProfile));
        return profiles;
    }
}
