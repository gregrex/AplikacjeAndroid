# Rule Engine v2 — notatki techniczne

## Cel

Celem zmian jest przygotowanie silnika reguł do lepszego UX:

- pokazania użytkownikowi, dlaczego dostał dany wynik,
- pokazania alternatywnych wyników,
- pokazania dopasowanych warunków,
- zachowania kompatybilności z obecnymi plikami `rules.json`.

## Zmienione modele

### `RuleDefinition`

Dodano opcjonalne pole:

```csharp
public string Reason { get; set; } = string.Empty;
```

Pole jest opcjonalne. Obecne pliki JSON nie muszą go mieć.

Przykład użycia w `rules.json`:

```json
{
  "id": "weak_signal_place",
  "categoryId": "weak_signal",
  "when": { "problem": "weak_signal" },
  "score": 95,
  "freeResultId": "weak_signal_place_free",
  "premiumResultId": "weak_signal_place_premium",
  "reason": "Wybrano tę regułę, bo użytkownik wskazał słaby zasięg WiFi."
}
```

### `RuleMatch`

Dodano:

```csharp
public int Score { get; set; }
public string Reason { get; set; } = string.Empty;
public List<string> MatchedConditions { get; set; } = new();
public List<string> AlternativeRuleIds { get; set; } = new();
public List<string> AlternativePremiumResultIds { get; set; } = new();
```

## Zmienione działanie silnika

`RuleEngineService.Match(...)` nadal zwraca najlepsze dopasowanie, ale dodatkowo zwraca:

- wynikowy `Score`,
- `Reason`, jeśli reguła go ma,
- listę warunków, które dopasowała reguła,
- maksymalnie 3 alternatywne reguły,
- maksymalnie 3 alternatywne wyniki premium.

## Bezpieczny fallback

Poprzednio fallback mógł wybrać wysokopunktową regułę wildcard nawet wtedy, gdy jej warunki nie były spełnione.

Teraz fallback wybiera tylko regułę:

```text
categoryId = *
when = {}
```

czyli prawdziwą regułę domyślną.

## Status wdrożenia UI

Wdrożono przekazanie metadanych z `Quiz.razor` do `Result.razor` przez query string.

Ekran wyniku pokazuje teraz sekcję:

### Dlaczego taki wynik?

Źródła danych:

- `RuleMatch.Reason`,
- `RuleMatch.Score`,
- `RuleMatch.MatchedConditions`.

Po odblokowaniu premium ekran może pokazać także:

### Alternatywne rekomendacje

Źródła danych:

- `RuleMatch.AlternativePremiumResultIds`.

## Projekty z pełnym `reason`

Pierwsze projekty, w których wszystkie reguły mają `reason` w źródle i runtime:

- `router-wifi-diagnosta`,
- `zmywarka-diagnosta`,
- `krawat-garnitur-coach`.

Jest to pilnowane testem:

- `tests/AppFactory.Mobile.Tests/RuleReasonsQualityTests.cs`.

## Następny krok

1. Dodać `reason` do kolejnych projektów.
2. Rozważyć przeniesienie query parsing z `Result.razor` do małego serwisu testowalnego jednostkowo.
3. Dodać testy/snapshoty dla ekranu wyniku.
4. Dodać klikane alternatywy zamiast samej listy alternatywnych wyników.
