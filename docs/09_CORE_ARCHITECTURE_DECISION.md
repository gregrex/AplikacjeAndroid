# Decyzja architektoniczna — AppFactory.Core

## Problem

Pierwszy projekt testowy referencjonował bezpośrednio projekt `AppFactory.Mobile`. To jest niepraktyczne, bo projekt MAUI Android wymaga workloadów i środowiska mobilnego. Testy logiki reguł i integralności danych powinny działać szybko bez emulatora i bez Android SDK.

## Decyzja

Wydzielamy projekt:

```text
src/AppFactory.Core/AppFactory.Core.csproj
```

Core zawiera:

- modele danych,
- silnik reguł,
- serwis wyników,
- docelowo walidatory danych,
- docelowo parsery lokalnych paczek JSON.

Mobile zawiera:

- MAUI,
- Blazor Hybrid UI,
- dostęp do plików aplikacji,
- integracje Android,
- reklamy,
- storage lokalny,
- nawigację.

## Zasada

Wszystko, co da się testować bez telefonu, ma trafiać do `AppFactory.Core`.

Wszystko, co zależy od MAUI, Androida albo BlazorWebView, zostaje w `AppFactory.Mobile`.

## Testy

Projekt testowy:

```text
tests/AppFactory.Mobile.Tests
```

referencjonuje teraz `AppFactory.Core`, a nie `AppFactory.Mobile`.

## CI

Dodany workflow:

```text
.github/workflows/core-tests.yml
```

Uruchamia testy Core na `windows-latest` bez potrzeby emulatora Android.

## Następny krok

Docelowo usunąć duplikację modeli i serwisów między `AppFactory.Mobile` i `AppFactory.Core`, a projekt Mobile powinien referencjonować Core.

Najpierw jednak należy uzyskać zielone testy Core.
