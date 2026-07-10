using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class ProjectUiProfileService
{
    public ProjectUiProfile GetProfile(string projectId) => projectId switch
    {
        "plama-ratownik" => Profile(projectId, "🧼", "Ratunek dla tkanin", "Usuń plamę bez pogarszania sytuacji", "Dobierz procedurę do rodzaju zabrudzenia, materiału i świeżości plamy.", "Wybierz rodzaj plamy", "Opowiedz o plamie", "Plan ratunkowy", "instruction-checklist", "Odblokuj pełną instrukcję", "Zapisz instrukcję", safety: true),
        "kolek-dobieracz" => Profile(projectId, "🧱", "Mocowanie bez zgadywania", "Dobierz kołek do ściany i obciążenia", "Przejdź przez materiał podłoża, ciężar i sposób montażu.", "Wybierz podłoże", "Dopasuj mocowanie", "Rekomendowane mocowanie", "technical-safety-checklist", "Pokaż pełną checklistę montażu", "Zapisz mocowanie", safety: true),
        "pies-trener-7dni" => Profile(projectId, "🐕", "Plan małych kroków", "Siedem dni spokojnego treningu", "Krótki plan dopasowany do wieku, energii i celu pracy z psem.", "Wybierz cel treningu", "Poznaj potrzeby psa", "Plan na 7 dni", "seven-day-plan", "Pokaż pełny plan 7 dni", "Zapisz plan", safety: true),
        "bajka-z-rysunku" => Profile(projectId, "🎨", "Spokojna opowieść", "Zamień pomysł lub rysunek w bajkę", "Wybierz bohatera, nastrój i miejsce akcji.", "Wybierz motyw bajki", "Zbuduj opowieść", "Twoja bajka", "story-page", "Pokaż pełną bajkę", "Zapisz bajkę"),
        "vinted-olx-opis" => Profile(projectId, "🏷️", "Opis gotowy do publikacji", "Napisz uczciwe ogłoszenie sprzedażowe", "Zbierz stan, cechy i najważniejsze informacje bez dopisywania faktów.", "Wybierz rodzaj przedmiotu", "Uzupełnij dane ogłoszenia", "Gotowy opis", "sales-copy-card", "Pokaż pełny opis", "Zapisz szablon", copy: true),
        "kot-bawi-sie" => Profile(projectId, "🐈", "Aktywność dla kota", "Dobierz zabawę do wieku i energii", "Krótka, bezpieczna aktywność z rzeczy dostępnych w domu.", "Wybierz typ aktywności", "Poznaj nastrój kota", "Plan zabawy", "pet-activity-plan", "Pokaż pełny plan zabawy", "Zapisz zabawę", safety: true),
        "barber-translator" => Profile(projectId, "💈", "Brief bez nieporozumień", "Pokaż barberowi dokładnie, o co chodzi", "Zbuduj jasny opis długości, przejścia i wykończenia.", "Wybierz kierunek fryzury", "Doprecyzuj brief", "Brief dla barbera", "style-checklist", "Pokaż pełny brief", "Zapisz brief", copy: true),
        "outfit-coach" => Profile(projectId, "👔", "Zestaw do okazji", "Dobierz strój do pogody i stylu", "Neutralna checklista elementów bez oceniania wyglądu osoby.", "Wybierz okazję", "Dopasuj zestaw", "Proponowany outfit", "style-checklist", "Pokaż pełną stylizację", "Zapisz zestaw"),
        "domfix" => Profile(projectId, "🛠️", "Bezpieczne drobne naprawy", "Najpierw oceń ryzyko, potem działaj", "Prosta diagnostyka domowego problemu z jasną granicą DIY.", "Wybierz problem", "Sprawdź warunki naprawy", "Plan działania", "technical-safety-checklist", "Pokaż pełną checklistę", "Zapisz naprawę", safety: true),
        "fryzury-proste" => Profile(projectId, "✨", "Fryzura bez komplikacji", "Proste uczesanie krok po kroku", "Dobierz efekt do czasu, okazji i dostępnych akcesoriów.", "Wybierz efekt", "Dopasuj uczesanie", "Instrukcja fryzury", "style-checklist", "Pokaż pełną instrukcję", "Zapisz fryzurę"),
        "rysunek-coach" => Profile(projectId, "✏️", "Ćwiczenie kreatywne", "Narysuj coś krok po kroku", "Lekcja dopasowana do tematu i poziomu trudności.", "Wybierz temat", "Ustaw poziom lekcji", "Lekcja rysowania", "creative-lesson", "Pokaż pełną lekcję", "Zapisz lekcję"),
        "bukietownik" => Profile(projectId, "💐", "Kompozycja na okazję", "Ułóż spójny bukiet bez chaosu", "Dobierz styl, kolorystykę i zakres budżetu.", "Wybierz okazję", "Dopasuj kompozycję", "Plan bukietu", "arrangement-plan", "Pokaż pełną kompozycję", "Zapisz bukiet"),
        "pokoj-makeover" => Profile(projectId, "🛋️", "Metamorfoza bez remontu", "Zmień pokój tym, co już masz", "Priorytety dla układu, światła, przechowywania i stylu.", "Wybierz cel zmiany", "Opisz pokój", "Plan metamorfozy", "arrangement-plan", "Pokaż pełny plan", "Zapisz metamorfozę"),
        "pakowanie-paczek" => Profile(projectId, "📦", "Bezpieczna wysyłka", "Spakuj przedmiot odpowiednio do ryzyka", "Checklista opakowania, wypełnienia i testu przed nadaniem.", "Wybierz przedmiot", "Oceń ryzyko wysyłki", "Checklista pakowania", "packing-checklist", "Pokaż pełną checklistę", "Zapisz checklistę", safety: true),
        "silikon-fuga-fix" => Profile(projectId, "🧽", "Ocena przed naprawą", "Wyczyścić czy wymienić?", "Rozpoznaj zabrudzenie, pleśń, pęknięcie lub odspojenie.", "Wybierz problem", "Oceń stan powierzchni", "Bezpieczna procedura", "technical-safety-checklist", "Pokaż pełną procedurę", "Zapisz procedurę", safety: true),
        "szydelko-pomocnik" => Profile(projectId, "🧶", "Warsztat szydełkowy", "Wzór, licznik rzędów i notatki", "Pomoc przy prostym wzorze oraz lokalny zapis postępu robótki.", "Wybierz rodzaj pomocy", "Dopasuj wzór", "Pomocnik robótki", "craft-helper", "Pokaż pełną pomoc", "Zapisz wzór", toolKind: "crochet-counter"),
        "chleb-zakwas-coach" => Profile(projectId, "🍞", "Diagnoza procesu", "Popraw zakwas i bochenek krok po kroku", "Oceń aktywność, fermentację i objawy bez ryzykowania bezpieczeństwa żywności.", "Wybierz problem", "Opisz zakwas lub chleb", "Plan korekty", "diagnostic-checklist", "Pokaż pełny plan", "Zapisz plan", safety: true),
        "zmywarka-diagnosta" => Profile(projectId, "🍽️", "Domowa diagnostyka", "Sprawdź objaw bez rozbierania urządzenia", "Bezpieczna checklista filtra, odpływu, osadu i typowych błędów.", "Wybierz objaw", "Doprecyzuj problem", "Checklista diagnostyczna", "diagnostic-checklist", "Pokaż pełną diagnostykę", "Zapisz wynik", safety: true),
        "krawat-garnitur-coach" => Profile(projectId, "🤵", "Formalność bez stresu", "Dobierz spójny zestaw do okazji", "Garnitur, koszula, krawat i dodatki w neutralnym języku.", "Wybierz okazję", "Dopasuj formalność", "Zestaw formalny", "style-checklist", "Pokaż pełny zestaw", "Zapisz zestaw"),
        "router-wifi-diagnosta" => Profile(projectId, "📶", "Lepszy zasięg i stabilność", "Znajdź źródło problemu z WiFi", "Oddziel problemy z zasięgiem, routerem i łączem operatora.", "Wybierz objaw", "Sprawdź sieć", "Plan diagnostyczny", "diagnostic-checklist", "Pokaż pełną diagnostykę", "Zapisz plan", safety: true),
        _ => Profile(projectId, "✨", "Praktyczny pomocnik", "Szybka pomoc krok po kroku", "Odpowiedz na kilka pytań i otrzymaj dopasowaną rekomendację.", "Wybierz temat", "Dopasuj rekomendację", "Twoja rekomendacja", "instruction-checklist", "Odblokuj pełną wersję", "Dodaj do ulubionych")
    };

    private static ProjectUiProfile Profile(
        string projectId,
        string icon,
        string badge,
        string heroTitle,
        string heroDescription,
        string categoryHeading,
        string quizHeading,
        string resultHeading,
        string resultViewType,
        string primaryAction,
        string secondaryAction,
        bool safety = false,
        bool copy = false,
        string toolKind = "") => new()
    {
        ProjectId = projectId,
        Icon = icon,
        Badge = badge,
        HeroTitle = heroTitle,
        HeroDescription = heroDescription,
        CategoryHeading = categoryHeading,
        QuizHeading = quizHeading,
        ResultHeading = resultHeading,
        ResultViewType = resultViewType,
        PrimaryActionLabel = primaryAction,
        SecondaryActionLabel = secondaryAction,
        ShowSafetyBlock = safety,
        ShowCopyAction = copy,
        ToolKind = toolKind
    };
}
