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

Docelowe użycie w `rules.json`:

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

## Co można podpiąć w UI

Na ekranie wyniku można dodać sekcję:

### Dlaczego taki wynik?

Źródła danych:

- `RuleMatch.Reason`,
- `RuleMatch.Score`,
- `RuleMatch.MatchedConditions`.

### Zobacz też

Źródła danych:

- `RuleMatch.AlternativePremiumResultIds`.

## Następny krok

1. Stopniowo dodawać `reason` do najważniejszych reguł w projektach.
2. Dodać sekcję `Dlaczego taki wynik?` w ekranie wyniku.
3. Dodać sekcję alternatywnych wyników.
4. Dodać testy UI albo snapshoty dla ekranu wyniku.
