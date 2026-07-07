# Progress — Plama Ratownik MVP data expanded

## Status

Projekt `plama-ratownik` został rozbudowany o większą paczkę danych MVP.

## Zrobione

- Rozszerzono `rules.json` do obsługi głównych kategorii plam.
- Rozszerzono `results.pl.json` o wyniki darmowe i premium dla nowych reguł.
- Dodano testy jakości danych.
- Dodano testy scenariuszy biznesowych.
- Dodano dokument `docs/08_DEFINITION_OF_DONE.md`.

## Obsługiwane kategorie w danych MVP

- kawa
- herbata
- tłuszcz
- czerwone wino
- krew
- trawa
- błoto
- długopis
- kosmetyki
- rdza
- fallback ogólny

## Aktualna ocena ukończenia

`plama-ratownik`: około 55%.

## Dlaczego nie 100%

Brakuje jeszcze:

- uruchomionych testów lokalnie lub w CI,
- zielonego buildu Android,
- poprawienia ewentualnych błędów MAUI,
- SQLite lub decyzji o dev-only in-memory storage,
- tłumaczeń EN/UK albo pełnego fallbacku,
- integracji prawdziwych reklam albo przygotowania adaptera AdMob,
- checklisty Google Play,
- manualnego testu na Androidzie.

## Następny krok

Uruchomić:

```powershell
dotnet test .\tests\AppFactory.Mobile.Tests\AppFactory.Mobile.Tests.csproj
```

Potem:

```powershell
dotnet build .\src\AppFactory.Mobile\AppFactory.Mobile.csproj -f net9.0-android
```
