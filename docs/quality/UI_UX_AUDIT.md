# Audyt UI/UX — 20 projektów AppFactory

## Zakres

Audyt wykonano na wspólnych stronach Blazor Hybrid, profilach UI, motywach source/runtime oraz akcjach wymaganych przez scenariusze produkcyjne.

Nie jest to audyt screenshotów z fizycznego urządzenia. Finalna ocena odstępów, klawiatury, bezpiecznych obszarów i wydajności animacji wymaga uruchomienia aplikacji na Androidzie.

## Wspólne poprawki graficzne

Dodano lub przebudowano:

- sticky topbar z ikoną i nazwą aktywnego projektu,
- dolną nawigację: katalog, historia, ulubione, ustawienia,
- bezpieczne odstępy `safe-area`,
- responsywne siatki kart,
- gradientowe hero z grafiką projektu,
- większe cele dotykowe i przyciski blokowe,
- czytelny progress quizu,
- kafle odpowiedzi z numerem i strzałką,
- karty wyniku zależne od rodzaju doświadczenia,
- karty ostrzeżeń, potrzebnych rzeczy i zakazów,
- metryki reguły, punktów i języka,
- puste stany historii, ulubionych i kategorii,
- obsługę `prefers-reduced-motion`,
- spójny design system oparty o kolory `theme.json`.

## Profile wszystkich aplikacji

| Projekt | Ikona | Profil wyniku | Główna cecha UX |
| --- | --- | --- | --- |
| `plama-ratownik` | 🧼 | instrukcja | szybki plan ratunkowy i ostrzeżenia |
| `kolek-dobieracz` | 🧱 | techniczna checklista | podłoże, obciążenie i bezpieczeństwo |
| `pies-trener-7dni` | 🐕 | plan 7 dni | etapy treningu w kaflach |
| `bajka-z-rysunku` | 🎨 | bajka | typograficzna karta opowieści |
| `vinted-olx-opis` | 🏷️ | opis sprzedażowy | pole tekstowe i kopiowanie |
| `kot-bawi-sie` | 🐈 | aktywność zwierzaka | plan zabawy i audio AI |
| `barber-translator` | 💈 | checklista stylu | kopiowalny brief dla barbera |
| `outfit-coach` | 👔 | checklista stylu | neutralny zestaw do okazji |
| `domfix` | 🛠️ | techniczna checklista | wyraźna granica bezpiecznego DIY |
| `fryzury-proste` | ✨ | checklista stylu | duże, czytelne etapy uczesania |
| `rysunek-coach` | ✏️ | lekcja kreatywna | ćwiczenie krok po kroku |
| `bukietownik` | 💐 | plan aranżacji | kompozycja, budżet i kolorystyka |
| `pokoj-makeover` | 🛋️ | plan aranżacji | wizualny plan metamorfozy |
| `pakowanie-paczek` | 📦 | checklista pakowania | zabezpieczenia i test końcowy |
| `silikon-fuga-fix` | 🧽 | techniczna checklista | safety-first i analiza zdjęcia |
| `szydelko-pomocnik` | 🧶 | pomocnik craft | licznik rzędów i lokalne notatki |
| `chleb-zakwas-coach` | 🍞 | diagnostyka | objawy, proces i bezpieczeństwo żywności |
| `zmywarka-diagnosta` | 🍽️ | diagnostyka | obraz, dźwięk i krytyczne ostrzeżenia |
| `krawat-garnitur-coach` | 🤵 | checklista stylu | poziom formalności bez oceniania osoby |
| `router-wifi-diagnosta` | 📶 | diagnostyka | rozdzielenie zasięgu, routera i operatora |

## Naprawione luki scenariuszy

### Local AI

`LocalAiPanel.razor` zapewnia faktyczne akcje:

- wybór lokalnego zdjęcia,
- wybór lokalnego nagrania,
- podanie czasu nagrania,
- skopiowanie pliku do cache aplikacji,
- przekazanie `LocalFilePath` do serwisu analizy,
- pokazanie wyniku, confidence i ostrzeżeń.

### Historia i ulubione

Wpisy przechowują teraz pełną trasę wyniku:

- `ProjectId`,
- `CategoryId`,
- `FreeResultId`,
- `PremiumResultId`.

Historia i ulubione mają akcję ponownego otwarcia wyniku.

### Kopiowanie

Kopiowanie działa nie tylko dla `vinted-olx-opis`, ale również dla `barber-translator` przez wspólny `ProjectResultView`.

### Szydełko

Dodano `ProjectTools.razor` i `ProjectToolStateService`:

- zwiększanie i zmniejszanie licznika rzędów,
- reset licznika,
- zapis do `Preferences`,
- lokalne notatki robótki.

## Automatyczne gate'y

- `UiUxProductionTests.cs` — pełne profile wszystkich projektów, motywy i wspólne akcje UI,
- `ScenarioImplementationAuditTests.cs` — mapowanie każdego scenariusza na faktyczne akcje i logikę,
- `ProjectProductionScenariosTests.cs` — struktura 100 scenariuszy,
- `AllProjectRuleReachabilityTests.cs` — osiągalność reguł i wyników.

## Elementy wymagające testu urządzenia

- zachowanie FilePicker na docelowych wersjach Androida,
- wygląd przy bardzo długich tłumaczeniach,
- klawiatura ekranowa w notatkach i ustawieniach,
- wydajność ONNX na urządzeniach klasy low-end,
- kontrast i czytelność dla wszystkich 20 palet na rzeczywistym ekranie,
- nawigacja wstecz systemowym przyciskiem Android,
- rezultat po zmianie orientacji i wznowieniu procesu aplikacji.
