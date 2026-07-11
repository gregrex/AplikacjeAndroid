# Plan testów lokalnych AppFactory

## Cel

Celem pierwszej lokalnej sesji jest ustalenie rzeczywistego stanu repozytorium przed oznaczeniem aplikacji jako `production-ready`.

Plan obejmuje:

- kompilację i testy automatyczne,
- SQLite i migrację danych,
- build Android Debug i Release,
- smoke test wspólnego UI,
- 100 scenariuszy produkcyjnych,
- Local AI obrazu i dźwięku,
- testy offline, restartu i uprawnień,
- rejestrację defektów oraz kryteria wydania.

## 1. Przygotowanie środowiska

Wymagane:

- Windows 11,
- .NET SDK 9,
- workload .NET MAUI/Android,
- Android SDK i zgodny JDK,
- PowerShell 7,
- emulator Android albo telefon z włączonym debugowaniem USB.

Kontrola:

```powershell
dotnet --info
dotnet workload list
adb devices
```

Jeżeli brakuje workloadu:

```powershell
dotnet workload restore src/AppFactory.Mobile/AppFactory.Mobile.csproj
```

## 2. Czysty punkt startowy

```powershell
git status
git pull
dotnet nuget locals all --clear
```

Przed pierwszym testem urządzenia:

- odinstaluj starszą wersję tylko wtedy, gdy nie testujesz migracji,
- pozostaw starszą wersję i jej dane, jeżeli testujesz migrację `Preferences -> SQLite`,
- zapisz model telefonu, wersję Androida i orientację ekranu.

## 3. Szybka sesja automatyczna

Najpierw uruchom gotowy skrypt:

```powershell
pwsh ./tools/quality/run-local-test-plan.ps1
```

Pełniejsza wersja z workload restore i buildem Release:

```powershell
pwsh ./tools/quality/run-local-test-plan.ps1 -RestoreWorkloads -IncludeReleaseBuild -WriteReport
```

Oczekiwany wynik:

- restore bez błędów,
- wszystkie testy `PASS`,
- Android Debug build bez błędów,
- opcjonalny Android Release build bez błędów,
- raport testów w `artifacts/local-test/<timestamp>`.

## 4. Testy automatyczne według obszaru

### 4.1 Wszystkie testy

```powershell
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release
```

### 4.2 SQLite

```powershell
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter AppDatabaseTests
```

Sprawdza:

- inicjalizację schematu,
- health check,
- sortowanie i deduplikację historii,
- limit 100 wpisów,
- deduplikację i usuwanie ulubionych.

### 4.3 Reguły biznesowe

```powershell
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter AllProjectRuleReachabilityTests
```

### 4.4 Scenariusze i akcje UI

```powershell
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter ProjectProductionScenariosTests
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter ScenarioImplementationAuditTests
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter UiUxProductionTests
```

### 4.5 Local AI

```powershell
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter "ImageAnalysisServiceTests|AudioAnalysisServiceTests|LocalAiModelStoreTests|LocalAiInputTensorFactoryTests|AiSuggestionWorkflowTests"
```

## 5. Build Android

### Debug

```powershell
dotnet build src/AppFactory.Mobile/AppFactory.Mobile.csproj -f net9.0-android -c Debug
```

### Release

```powershell
dotnet build src/AppFactory.Mobile/AppFactory.Mobile.csproj -f net9.0-android -c Release
```

Kryterium PASS:

- brak błędów kompilacji,
- brak błędów linkera i natywnych bibliotek SQLite/ONNX,
- APK/AAB powstaje w katalogu wynikowym,
- aplikacja uruchamia się bez crasha na ekranie startowym.

## 6. Smoke test wspólnego UI — około 15 minut

Wykonaj przed 100 scenariuszami:

1. Uruchom katalog.
2. Sprawdź wyszukiwanie i filtr typu aplikacji.
3. Otwórz dowolne trzy projekty o różnych profilach UI.
4. Sprawdź topbar, dolną nawigację i przycisk systemowy Wstecz.
5. Przejdź jeden quiz do wyniku.
6. Odblokuj pełny wynik.
7. Dodaj wynik do ulubionych.
8. Otwórz historię i ulubione.
9. Zamknij proces aplikacji i uruchom go ponownie.
10. Sprawdź, czy historia i ulubione nadal istnieją.

Jeżeli smoke test nie przechodzi, nie rozpoczynaj pełnych 100 scenariuszy.

## 7. Priorytetowa kolejność projektów

### Grupa A — bezpieczeństwo i diagnostyka

Testuj jako pierwsze:

- `zmywarka-diagnosta`,
- `silikon-fuga-fix`,
- `domfix`,
- `kolek-dobieracz`,
- `chleb-zakwas-coach`,
- `plama-ratownik`,
- `pakowanie-paczek`,
- `router-wifi-diagnosta`.

Każdy błąd prowadzący do ryzykownej instrukcji ma priorytet krytyczny.

### Grupa B — Local AI

- `plama-ratownik`,
- `pokoj-makeover`,
- `rysunek-coach`,
- `outfit-coach`,
- `fryzury-proste`,
- `barber-translator`,
- `zmywarka-diagnosta`,
- `silikon-fuga-fix`,
- `pies-trener-7dni`,
- `kot-bawi-sie`.

### Grupa C — pozostałe aplikacje

Testuj po przejściu grup A i B.

## 8. Wykonanie 100 scenariuszy

Źródło scenariuszy:

```text
projects/<projectId>/tests/production-scenarios.md
```

Tracker:

```text
docs/quality/SCENARIO_EXECUTION_TRACKER.md
```

Dla każdego scenariusza zapisz:

- `PASS`, `FAIL`, `BLOCKED` albo `NOT_RUN`,
- urządzenie i wersję Androida,
- datę,
- numer buildu/commit,
- numer defektu,
- screenshot albo log dla `FAIL` i `BLOCKED`.

Scenariusz jest `PASS`, gdy:

- wszystkie kroki można wykonać,
- akcje istnieją i reagują,
- wynik odpowiada oczekiwaniu,
- nie występuje crash,
- nie ma utraty danych,
- ostrzeżenia bezpieczeństwa są widoczne tam, gdzie są wymagane.

## 9. Test lokalnej bazy SQLite

### Nowa instalacja

1. Wyczyść dane aplikacji.
2. Uruchom aplikację.
3. Wygeneruj trzy różne wyniki.
4. Dodaj dwa wyniki do ulubionych.
5. Zamknij proces aplikacji.
6. Uruchom ponownie.
7. Sprawdź historię i ulubione.
8. Usuń jedno ulubione.
9. Wyczyść historię.
10. Uruchom aplikację ponownie i sprawdź stan.

### Migracja ze starszej wersji

1. Na starszym buildzie zapisz historię i ulubione.
2. Zainstaluj nowy build bez usuwania danych.
3. Otwórz historię i ulubione.
4. Sprawdź możliwość ponownego otwarcia każdego wyniku.
5. Uruchom aplikację ponownie.
6. Potwierdź, że dane nie zostały zduplikowane.

Kryterium PASS:

- brak utraty danych,
- brak duplikatów,
- poprawna kolejność historii,
- poprawne usuwanie i czyszczenie,
- brak crasha przy uszkodzonym starszym JSON.

## 10. Local AI

Przed testem sprawdź:

- skonfigurowany `DownloadUrl`,
- poprawny `Sha256`,
- dostępność modelu,
- wystarczającą ilość miejsca na urządzeniu.

Testuj:

1. Brak modelu — analiza powinna być zablokowana czytelnym komunikatem.
2. Błędny SHA256 — plik powinien zostać odrzucony i usunięty.
3. Poprawny model — analiza powinna uruchomić ONNX.
4. Zdjęcie/nagranie nieobsługiwanego typu — wejście powinno zostać odrzucone.
5. Plik zbyt duży albo nagranie zbyt długie — wejście powinno zostać odrzucone.
6. Sugestia AI — nie może zmienić quizu bez kliknięcia `Użyj tej sugestii`.
7. Tryb offline po pobraniu — analiza powinna działać bez sieci.

Jeżeli docelowe modele nie są jeszcze skonfigurowane, scenariusze AI oznacz `BLOCKED`, a nie `PASS`.

## 11. Testy niefunkcjonalne

Sprawdź minimum:

- start bez internetu,
- utratę internetu podczas pobierania modelu,
- małą ilość miejsca,
- odmowę dostępu do plików,
- anulowanie FilePicker,
- obrót ekranu,
- przejście aplikacji do tła i powrót,
- systemowy przycisk Wstecz,
- bardzo długie tłumaczenia,
- powiększony rozmiar tekstu systemowego,
- tryb ciemny systemu,
- szybkie wielokrotne kliknięcia przycisków,
- urządzenie o niższej wydajności.

## 12. Klasyfikacja defektów

### Krytyczny

- crash aplikacji,
- utrata lub uszkodzenie danych,
- niebezpieczna porada,
- brak możliwości uruchomienia aplikacji,
- niesprawny build Release.

### Wysoki

- scenariusz główny nie może zostać ukończony,
- wynik wskazuje złą regułę,
- historia lub ulubione nie działają,
- AI omija ręczne potwierdzenie,
- podstawowa nawigacja jest uszkodzona.

### Średni

- błąd alternatywnej ścieżki,
- problem z tłumaczeniem,
- nieprawidłowy stan pusty,
- problem z układem utrudniający obsługę.

### Niski

- kosmetyka, odstęp, literówka lub drobna niespójność bez wpływu na wykonanie scenariusza.

## 13. Kryterium zakończenia testów

Status `production-ready` można nadać dopiero gdy:

- wszystkie testy automatyczne są zielone,
- Debug i Release Android build przechodzą,
- smoke test przechodzi,
- 100/100 scenariuszy ma `PASS`,
- brak otwartych defektów krytycznych i wysokich,
- migracja SQLite jest potwierdzona,
- Local AI ma realne modele i przechodzi testy albo jest wyłączone w buildzie wydaniowym,
- wykonano regresję po ostatniej poprawce.

## 14. Pierwsza sesja testowa — zalecany zakres

Na pierwszą sesję wykonaj:

1. `run-local-test-plan.ps1`,
2. smoke test,
3. pełny test SQLite,
4. pięć scenariuszy `zmywarka-diagnosta`,
5. pięć scenariuszy `silikon-fuga-fix`,
6. pięć scenariuszy `szydelko-pomocnik`,
7. zapis defektów i decyzję, czy repo nadaje się do dalszej pełnej rundy.
