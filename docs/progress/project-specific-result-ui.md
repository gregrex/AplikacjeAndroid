# Progress — Project-specific result UI

## Problem

Wcześniej projekty różniły się głównie danymi JSON i motywem kolorystycznym. To było za mało, bo każda aplikacja powinna mieć własny sposób prezentacji wyniku oraz własną logikę akcji.

## Zrobione

Dodano warstwę UI/UX dla wyników:

- `ProjectUiProfile`,
- `ProjectUiProfileService`,
- `ProjectResultView`,
- `ClipboardExportService`,
- rejestrację usług w `MauiProgram`,
- import komponentów w `_Imports.razor`,
- przepięcie `Result.razor` na projektowy komponent wyniku,
- style CSS dla różnych typów wyników,
- testy `ProjectUiProfileServiceTests`.

## Typy ekranów wyników

- `instruction-checklist` — instrukcje krok po kroku, np. Plama Ratownik.
- `technical-safety-checklist` — techniczna checklista z naciskiem na bezpieczeństwo, np. Kołek Dobieracz.
- `seven-day-plan` — plan dzienny, np. Pies Trener 7 Dni.
- `story-page` — karta bajki/opowieści, np. Bajka z rysunku.
- `sales-copy-card` — karta opisu sprzedażowego z akcją kopiowania, np. Opis Sprzedażowy.

## Efekt

Aplikacje nie są już tylko zestawem JSON. Ekran wyniku ma projektowy wariant UI oraz akcje dopasowane do projektu.

## Ważne

To nadal jest wspólny silnik. Różnice są sterowane profilem UI i stylem, a nie osobnym kodem każdej aplikacji od zera.

## Nadal brakuje

- uruchomienia builda Android,
- potwierdzenia działania schowka na telefonie,
- dopracowania osobnych ekranów startowych per projekt,
- osobnych mikrointerakcji dla kolejnych projektów,
- docelowego przeniesienia części profili UI wyłącznie do Core, żeby uniknąć duplikacji między Core i Mobile.
