# Progress — Dynamic project shell UI

## Problem

Po wyborze projektu aplikacja przechodziła do danych danego projektu, ale layout i motyw mogły pozostać z poprzedniego projektu, bo `MainLayout` ładował theme tylko przy starcie.

## Zrobione

Dodano realną logikę zmiany projektu w UI:

- `ProjectContextService` emituje `ProjectChanged`,
- `MainLayout` nasłuchuje zmiany projektu,
- `MainLayout` przeładowuje `theme.json`,
- topbar pokazuje nazwę aktualnej aplikacji i identyfikator projektu,
- ekran główny zmieniono na katalog mikroaplikacji,
- karty projektów pokazują typ doświadczenia UI,
- CSS dostał style dla katalogu projektów i dynamicznego topbara.

## Efekt

Wybór projektu nie zmienia już wyłącznie danych. Zmienia także shell aplikacji:

- nazwę w topbarze,
- kolory,
- identyfikator projektu,
- kontekst doświadczenia użytkownika.

## Pliki

- `src/AppFactory.Mobile/Services/ProjectContextService.cs`
- `src/AppFactory.Mobile/Layout/MainLayout.razor`
- `src/AppFactory.Mobile/Pages/Home.razor`
- `src/AppFactory.Mobile/wwwroot/css/app.css`

## Nadal brakuje

- uruchomienia aplikacji na Androidzie,
- potwierdzenia, że event `ProjectChanged` działa poprawnie w MAUI BlazorWebView,
- testów UI manualnych,
- trwałego zapamiętania ostatnio wybranego projektu.
