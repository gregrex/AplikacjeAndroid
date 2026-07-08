namespace AppFactory.Mobile.Services;

public sealed class ProjectCatalogService
{
    public IReadOnlyList<ProjectCatalogItem> GetProjects()
    {
        return new List<ProjectCatalogItem>
        {
            new("plama-ratownik", "Plama Ratownik", "Szybka pomoc przy plamach"),
            new("kolek-dobieracz", "Kołek Dobieracz", "Dobór mocowania do ściany")
        };
    }
}

public sealed record ProjectCatalogItem(string Id, string Name, string Description);
