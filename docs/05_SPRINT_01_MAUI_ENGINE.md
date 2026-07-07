# Sprint 01 — MAUI Blazor Hybrid Engine

## Cel sprintu

Zbudować minimalny działający silnik aplikacji Android, który potrafi załadować dane projektu `plama-ratownik` i przejść pełny scenariusz użytkownika.

## Zakres sprintu

### 1. Utworzenie solution

Struktura docelowa:

```text
src/
  AppFactory.Mobile/
    AppFactory.Mobile.csproj
    MauiProgram.cs
    MainPage.xaml
    wwwroot/
    Components/
    Pages/
    Layout/
    Services/
    Models/
    Data/
    Rules/
    Localization/
    Storage/
```

### 2. Modele danych

Utworzyć modele:

- AppConfig
- CategoryDefinition
- QuestionDefinition
- QuestionOption
- UserAnswer
- RuleDefinition
- ResultDefinition
- HistoryEntry
- FavoriteEntry

### 3. Loader JSON

Utworzyć serwis `ProjectDataService`, który ładuje dane z:

```text
wwwroot/projects/plama-ratownik/
```

Na tym etapie można skopiować dane z `projects/plama-ratownik/data/` do katalogu aplikacji.

### 4. Ekrany Blazor

Minimalne ekrany:

- Home
- Categories
- Quiz
- Result
- History
- Favorites
- Settings

### 5. Silnik reguł

`RuleEngineService` ma:

- przyjąć kategorię i odpowiedzi,
- znaleźć pasujące reguły,
- wybrać regułę z najwyższym score,
- zwrócić `freeResultId` i `premiumResultId`.

### 6. Wynik

`ResultService` ma:

- załadować wynik podstawowy,
- załadować wynik pełny,
- obsłużyć mock odblokowania pełnego wyniku.

### 7. Mock reklam

Na start reklamy są mockiem:

- kliknięcie „odblokuj” zawsze zwraca sukces,
- w logu aplikacji zapisać nazwę placementu,
- później podmienić na AdMob.

### 8. Historia i ulubione

Na MVP sprintu można użyć lokalnego zapisu w pliku lub SQLite. Preferowane SQLite, ale jeśli spowolni sprint, użyć prostego storage i dodać TODO.

## Definicja ukończenia

Sprint jest ukończony, gdy:

1. Aplikacja startuje.
2. Pokazuje nazwę Plama Ratownik.
3. Pokazuje listę kategorii.
4. Pozwala przejść pytania.
5. Dopasowuje regułę.
6. Pokazuje wynik podstawowy.
7. Pozwala odblokować wynik pełny mock reklamą.
8. Wynik można dodać do ulubionych.
9. Repo zawiera instrukcję uruchomienia.

## Zakaz rozszerzania zakresu

W tym sprincie nie dodawać:

- prawdziwego AdMob,
- logowania,
- backendu,
- AI,
- synchronizacji chmurowej,
- zbyt rozbudowanego UI.

Najpierw ma działać silnik.
