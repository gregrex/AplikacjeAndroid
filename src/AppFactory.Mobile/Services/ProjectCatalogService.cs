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
            new("bajka-z-rysunku", "Bajka z rysunku", "Spokojne bajki z lokalnych szablonów"),
            new("vinted-olx-opis", "Opis Sprzedażowy", "Szablony opisów do sprzedaży online"),
            new("kot-bawi-sie", "Kot Bawi się", "Zabawy i aktywności dla kota domowego"),
            new("barber-translator", "Barber Translator", "Instrukcja fryzury do pokazania barberowi"),
            new("outfit-coach", "Outfit Coach", "Dobór stroju do okazji, pogody i stylu"),
            new("domfix", "DomFix", "Bezpieczne drobne naprawy domowe"),
            new("fryzury-proste", "Fryzury Proste", "Neutralne instrukcje prostych uczesań")
        };
    }
}

public sealed record ProjectCatalogItem(string Id, string Name, string Description);
