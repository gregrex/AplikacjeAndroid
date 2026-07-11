# Status jakości projektów AppFactory

## Status produkcyjny

Aktualny status repo: **production-ready candidate**.

Kod zawiera komplet danych, scenariuszy, profili UI/UX, lokalną bazę SQLite i automatyczne gate'y implementacji. Finalny status `production-ready` wymaga zielonego CI oraz wykonania scenariuszy na Androidzie.

## Zakres katalogu

Katalog obejmuje 20 projektów.

## UI/UX wszystkich aplikacji

Każdy projekt ma dedykowany profil:

- ikonę,
- badge doświadczenia,
- własny hero title i opis,
- nagłówki kategorii, quizu i wyniku,
- typ widoku wyniku,
- etykiety akcji premium i zapisu,
- konfigurację safety, kopiowania lub narzędzi specjalnych.

Wspólny design system obejmuje:

- sticky topbar,
- dolną nawigację,
- safe-area Android,
- responsywne siatki,
- gradientowe hero,
- większe cele dotykowe,
- progress quizu,
- kafle odpowiedzi,
- stany puste,
- karty bezpieczeństwa,
- warianty wyniku zależne od projektu,
- `prefers-reduced-motion`.

Raport:

```text
docs/quality/UI_UX_AUDIT.md
```

## Lokalna baza danych

Dodano osobny projekt:

```text
src/AppFactory.Persistence/AppFactory.Persistence.csproj
```

Pakiet:

```text
sqlite-net-pcl 1.9.172
```

SQLite przechowuje:

- historię wyników,
- ulubione wyniki,
- wersję schematu.

Historia jest deduplikowana, sortowana i ograniczona do 100 rekordów. Ulubione są deduplikowane oraz można je usuwać pojedynczo lub czyścić w całości.

`HistoryService` i `FavoritesService` wykonują migrację wcześniejszych list JSON z `Preferences`.

W ustawieniach aplikacji widoczny jest health check bazy:

- stan,
- wersja schematu,
- liczba wpisów historii,
- liczba ulubionych.

Testy:

```text
tests/AppFactory.Mobile.Tests/AppDatabaseTests.cs
tests/AppFactory.Mobile.Tests/LocalDatabaseProductionTests.cs
```

Dokumentacja:

```text
docs/quality/LOCAL_DATABASE.md
```

## Scenariusze produkcyjne

Dla każdego projektu istnieje:

```text
projects/<projectId>/tests/production-scenarios.md
```

Łączne pokrycie:

- **20 projektów**,
- **5 scenariuszy na projekt**,
- **100 scenariuszy produkcyjnych**.

Testy:

```text
tests/AppFactory.Mobile.Tests/ProjectProductionScenariosTests.cs
tests/AppFactory.Mobile.Tests/AllProjectRuleReachabilityTests.cs
tests/AppFactory.Mobile.Tests/ScenarioImplementationAuditTests.cs
```

## Audyt akcji i logiki biznesowej

Każdy scenariusz jest mapowany na wymagane capabilities:

- katalog i motyw,
- quiz i silnik reguł,
- wynik free/premium,
- ulubione i historia,
- języki PL/EN/UK,
- fallback,
- alternatywy,
- kopiowanie,
- Local AI image/audio,
- trwałość danych,
- safety,
- narzędzia projektu.

Test sprawdza dowody implementacji w faktycznych stronach, usługach, danych i politykach projektu.

## Local AI on device

Runtime:

```text
Microsoft.ML.OnnxRuntime
```

Warstwa obejmuje downloader, SHA256, lokalny model store, tensor input, provider obrazu, provider audio, FilePicker oraz ręczne zatwierdzanie sugestii w quizie.

Aktualny kontrakt wejścia:

```text
input: float32[1,1,1,256]
```

Modele `local-vision-v1` i `local-audio-v1` wymagają skonfigurowania adresu pobierania, SHA256 i docelowych etykiet klas.

## Plan testów lokalnych

Dodano:

```text
docs/quality/LOCAL_TEST_PLAN.md
tools/quality/run-local-test-plan.ps1
```

Zalecane pierwsze uruchomienie:

```powershell
pwsh ./tools/quality/run-local-test-plan.ps1 -RestoreWorkloads -IncludeReleaseBuild -WriteReport
```

Runner zapisuje logi, pliki TRX i podsumowanie w:

```text
artifacts/local-test/<timestamp>
```

Tracker wykonania został rozszerzony o:

- dane urządzenia i buildu,
- automatyczne gate'y,
- testy SQLite i migracji,
- macierz 100 scenariuszy,
- regresję końcową,
- tabelę defektów.

## Weryfikacja wymagana przed publikacją

1. Uruchomić lokalny runner testów.
2. Naprawić pierwszy błąd kompilacji albo testu.
3. Wykonać smoke test na Androidzie.
4. Wykonać test nowej bazy i migracji `Preferences -> SQLite`.
5. Wykonać 100 scenariuszy na emulatorze lub urządzeniu.
6. Zapisać PASS/FAIL dla każdego scenariusza.
7. Sprawdzić FilePicker, klawiaturę, safe-area, systemowy back i długie tłumaczenia.
8. Naprawić wszystkie błędy krytyczne i wysokie.
9. Skonfigurować docelowe modele ONNX albo wyłączyć funkcje AI w buildzie publikacyjnym do czasu gotowości modeli.

## Uwagi

Nie uruchamiałem lokalnie `dotnet test`, ponieważ zmiany wykonuję przez GitHub connector. Kompilacja SQLite, Android build i migracja danych wymagają teraz potwierdzenia podczas Twojej lokalnej sesji testowej.
