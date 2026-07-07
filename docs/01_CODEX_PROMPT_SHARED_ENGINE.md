# Prompt dla Codexa — wspólny silnik .NET MAUI Blazor Hybrid

## Rola

Jesteś agentem kodującym odpowiedzialnym za zbudowanie wspólnego silnika aplikacji Android w .NET MAUI Blazor Hybrid.

Nie budujesz jednej aplikacji na sztywno. Budujesz silnik, który obsługuje wiele aplikacji poprzez pliki JSON.

## Cel

Zbuduj aplikację bazową działającą lokalnie bez serwera.

Schemat działania:

```text
wybór języka
→ ekran główny
→ wybór kategorii
→ quiz
→ dopasowanie reguł
→ wynik podstawowy
→ reklama rewarded / premium
→ wynik pełny
→ historia / ulubione
```

## Technologia

- .NET 9 lub aktualny stabilny .NET wspierający MAUI,
- .NET MAUI Blazor Hybrid,
- Android jako priorytet,
- SQLite lokalnie,
- JSON jako źródło danych projektowych,
- bez własnego backendu.

## Wymagana architektura

```text
src/AppFactory.Mobile/
  Components/
  Pages/
  Layout/
  Services/
  Models/
  Data/
  Rules/
  Localization/
  Monetization/
  Storage/
  Assets/
```

## Moduły

1. `ProjectConfigService` — ładuje `app.json`.
2. `LocalizationService` — ładuje tłumaczenia z `i18n/*.json`.
3. `QuestionFlowService` — obsługuje pytania i odpowiedzi.
4. `RuleEngineService` — dopasowuje reguły do odpowiedzi.
5. `ResultService` — buduje wynik darmowy i pełny.
6. `HistoryService` — zapisuje historię w SQLite.
7. `FavoritesService` — zapisuje ulubione.
8. `AdService` — abstrakcja reklam rewarded/interstitial.
9. `PremiumService` — abstrakcja przyszłych zakupów.
10. `PhotoService` — opcjonalne zdjęcie bez analizy AI.

## Modele danych

Zaimplementuj modele:

- `AppConfig`
- `LanguageConfig`
- `CategoryDefinition`
- `QuestionDefinition`
- `QuestionOption`
- `UserAnswer`
- `RuleDefinition`
- `RuleCondition`
- `ResultDefinition`
- `ResultStep`
- `HistoryEntry`
- `FavoriteEntry`

## JSON

Silnik ma ładować dane z katalogu:

```text
wwwroot/projects/{projectId}/
  app.json
  categories.json
  questions.json
  rules.json
  results.pl.json
  results.en.json
  results.uk.json
  i18n/pl.json
  i18n/en.json
  i18n/uk.json
```

## Języki

Na start pełna obsługa:

- pl,
- en,
- uk.

Przygotować strukturę pod wszystkie języki UE.

Fallback:

```text
wybrany język → en → pl
```

## Reklamy

Na MVP przygotuj interfejs i tryb mock:

```csharp
public interface IAdService
{
    Task<bool> ShowRewardedAsync(string placement);
    Task ShowInterstitialAsync(string placement);
}
```

W pierwszej wersji `MockAdService` zwraca sukces i loguje akcję. Integracja AdMob ma być osobnym etapem.

## Zasady kodowania

- Nie hardkoduj tekstów w komponentach.
- Nie hardkoduj projektów.
- Nie twórz backendu.
- Nie dodawaj logowania użytkownika.
- Dane użytkownika tylko lokalnie.
- Każdy ekran ma być prosty i mobilny.
- Kod ma być czytelny, mały i testowalny.

## Pierwszy milestone

Zbuduj minimalną aplikację obsługującą projekt `plama-ratownik`:

- lista kategorii,
- quiz,
- dopasowanie reguły,
- wynik darmowy,
- przycisk odblokowania pełnego wyniku,
- mock reklamy,
- wynik pełny,
- historia,
- ulubione,
- wybór języka.

## Zakaz

Nie trenuj AI. Nie integruj dużych modeli. Nie buduj serwera. Nie twórz rozbudowanego UI zanim silnik działa.
