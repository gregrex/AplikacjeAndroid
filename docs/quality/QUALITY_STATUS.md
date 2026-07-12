# Status jakości — AppFactory Pomocniki

## Status

Aktualny status repozytorium: **Google Play release candidate — wymagane wykonanie testów i czynności właściciela**.

Kod i dokumentacja zostały przygotowane dla jednej aplikacji:

- nazwa: `AppFactory Pomocniki`,
- package ID: `pl.gbcom.appfactory`,
- wersja: `1.0.0`,
- version code: `1`,
- Android target API: 35,
- format Release: AAB,
- 20 pomocników w jednym katalogu,
- bez reklam, płatności i konta,
- pełne rekomendacje dostępne bez blokady,
- Local AI wyłączone w domyślnym Release 1.0.

## Gotowe w kodzie

### Dane i logika

- komplet source/runtime dla 20 pomocników,
- reguły z wyjaśnieniem `reason`,
- wyniki PL/EN/UK,
- globalne fallbacki,
- alternatywy,
- 100 scenariuszy produkcyjnych,
- automaty osiągalności reguł.

### Interfejs

- katalog, wyszukiwanie i filtry,
- dedykowane profile 20 pomocników,
- kategorie i quiz z postępem,
- pełny wynik i wyjaśnienie dopasowania,
- historia oraz ulubione,
- narzędzia projektowe,
- polityka prywatności i pomoc,
- responsywny design oraz safe-area.

### Dane lokalne

- SQLite dla historii i ulubionych,
- wersjonowanie schematu,
- migracja z wcześniejszego JSON w `Preferences`,
- health check w ustawieniach,
- testy integracyjne bazy.

### Diagnostyka

- lokalne logi JSONL,
- rotacja 7 dni / 12 plików / 2 MB,
- maskowanie sekretów i e-maili,
- rejestr wyjątków,
- ręczny eksport ZIP,
- collector ADB dla Debug,
- przyciski testowego markera i wyjątku ukryte w Release.

### Bezpieczna konfiguracja Release

- usunięty `MockAdService`,
- brak blokady reklamowej,
- `AdsEnabled=false`,
- `EnableLocalAiRelease=false`,
- `android:allowBackup=false`,
- `android:usesCleartextTraffic=false`,
- marka, launcher icon i splash.

## Pakiet Google Play przygotowany w repo

### Marka i grafiki

```text
marketing/brand/BRAND_GUIDE.md
marketing/google-play/source/store-icon.svg
marketing/google-play/source/feature-graphic.svg
tools/release/generate-play-graphics.ps1
```

Generator przygotowuje:

- ikonę 512×512 PNG,
- feature graphic 1024×500 PNG.

Prawdziwe screenshoty finalnego buildu wykonuje się według:

```text
marketing/google-play/SCREENSHOT_PLAN.md
```

### Listing

```text
marketing/google-play/listings.json
marketing/google-play/release-locales.json
tools/release/export-play-metadata.ps1
```

Przygotowano teksty dla 24 języków urzędowych UE oraz ukraińskiego. Release 1.0 eksportuje wyłącznie `pl-PL`, `en-US` i `uk-UA`, ponieważ tylko te języki mają pełną treść wyników w aplikacji. Pozostałe zestawy pozostają materiałem planowanym.

### Strona publiczna

```text
site/index.html
site/privacy/index.html
site/support/index.html
site/terms/index.html
.github/workflows/pages.yml
```

Docelowe adresy:

- `https://gregrex.github.io/AplikacjeAndroid/`,
- `https://gregrex.github.io/AplikacjeAndroid/privacy/`,
- `https://gregrex.github.io/AplikacjeAndroid/support/`.

Publikacja wymaga włączenia GitHub Pages z użyciem GitHub Actions.

### Dokumenty Play Console

```text
docs/release/GOOGLE_PLAY_RELEASE_PLAN.md
docs/release/PLAY_CONSOLE_CHECKLIST.md
docs/release/DATA_SAFETY_DECLARATION.md
docs/release/CONTENT_RATING_GUIDE.md
docs/release/RELEASE_NOTES_1.0.0.md
```

### Podpis i AAB

```text
tools/release/create-upload-keystore.ps1
tools/release/build-play-aab.ps1
tools/release/prepare-google-play-package.ps1
.github/workflows/release-aab.yml
```

Sekrety podpisu muszą znajdować się poza repozytorium albo w GitHub Actions Secrets.

## Automatyczne gate’y

```text
tests/AppFactory.Mobile.Tests/ProductionReadinessTests.cs
tests/AppFactory.Mobile.Tests/GooglePlayReleaseReadinessTests.cs
tests/AppFactory.Mobile.Tests/ScenarioImplementationAuditTests.cs
tests/AppFactory.Mobile.Tests/UiUxProductionTests.cs
tests/AppFactory.Mobile.Tests/AppDatabaseTests.cs
tests/AppFactory.Mobile.Tests/LocalLogStoreTests.cs
```

Pierwsze polecenie:

```powershell
pwsh ./tools/quality/run-local-test-plan.ps1 -RestoreWorkloads -IncludeReleaseBuild -WriteReport
```

Przygotowanie paczki sklepu:

```powershell
pwsh ./tools/release/prepare-google-play-package.ps1
```

## Wymagane potwierdzenia przed publikacją

1. Zielony `dotnet test`.
2. Zielony Android Debug i Release build.
3. Smoke test na urządzeniu.
4. 100/100 scenariuszy PASS.
5. Test nowej instalacji i migracji SQLite.
6. Test eksportu diagnostyki offline.
7. Wygenerowanie PNG ikony i feature graphic.
8. Wykonanie sześciu screenshotów finalnego release candidate.
9. Włączenie GitHub Pages i sprawdzenie publicznych adresów.
10. Utworzenie oraz zabezpieczenie upload key.
11. Zbudowanie podpisanego AAB.
12. Akceptacja AAB w Internal testing.
13. Uzupełnienie Data Safety, content rating i target audience przez właściciela konta.
14. Pre-launch report bez crashy i ANR.
15. Closed testing, gdy jest wymagany dla konta.
16. Brak otwartych defektów krytycznych i wysokich.

## Informacja o tej sesji

Zmiany zostały wprowadzone przez GitHub connector. Nie wykonano lokalnie `dotnet test`, Android builda, generowania PNG, podpisania AAB ani testów Play Console. Tych wyników nie można uczciwie oznaczyć jako PASS bez lokalnego środowiska, prywatnego klucza oraz dostępu właściciela do Play Console.
