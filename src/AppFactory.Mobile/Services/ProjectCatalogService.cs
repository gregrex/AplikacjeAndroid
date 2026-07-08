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
            new("fryzury-proste", "Fryzury Proste", "Neutralne instrukcje prostych uczesań"),
            new("rysunek-coach", "Rysunek Coach", "Proste lekcje rysowania krok po kroku"),
            new("bukietownik", "Bukietownik", "Proste kompozycje kwiatowe do okazji"),
            new("pokoj-makeover", "Pokój Makeover", "Metamorfoza pokoju bez remontu"),
            new("pakowanie-paczek", "Pakowanie Paczek", "Checklisty zabezpieczenia przedmiotów do wysyłki"),
            new("silikon-fuga-fix", "Silikon Fuga Fix", "Bezpieczne czyszczenie, ocena i wymiana silikonu lub fug"),
            new("szydelko-pomocnik", "Szydełko Pomocnik", "Offline licznik rzędów, notatki i proste wzory szydełkowe"),
            new("chleb-zakwas-coach", "Chleb Zakwas Coach", "Prowadzenie zakwasu i korekta podstawowych problemów z chlebem"),
            new("zmywarka-diagnosta", "Zmywarka Diagnosta", "Bezpieczna checklista typowych problemów ze zmywarką")
        };
    }
}

public sealed record ProjectCatalogItem(string Id, string Name, string Description);
