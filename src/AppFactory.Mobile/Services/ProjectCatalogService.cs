namespace AppFactory.Mobile.Services;

public sealed class ProjectCatalogService
{
    public IReadOnlyList<ProjectCatalogItem> GetProjects()
    {
        return new List<ProjectCatalogItem>
        {
            new("plama-ratownik", "Plama Ratownik", "Szybka pomoc przy plamach"),
            new("kolek-dobieracz", "Kołek Dobieracz", "Dobór mocowania do ściany"),
            new("pies-trener-7dni", "Pies Trener 7 Dni", "Prosty plan treningu psa"),
            new("bajka-z-rysunku", "Bajka z rysunku", "Spokojne bajki z lokalnych szablonów")
        };
    }
}

public sealed record ProjectCatalogItem(string Id, string Name, string Description);
