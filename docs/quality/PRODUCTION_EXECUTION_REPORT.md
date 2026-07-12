# Raport przygotowania produkcyjnego — AppFactory Pomocniki 1.0

## Podsumowanie

Repozytorium zostało przekształcone z koncepcji wielu mikroaplikacji w jeden produkt Google Play:

```text
AppFactory Pomocniki
pl.gbcom.appfactory
1.0.0 / version code 1
Android API 35
AAB
```

Status: **release candidate w repozytorium; testy, podpis i Play Console pozostają do wykonania w środowisku właściciela**.

## Wykonane w repo

| Obszar | Status | Dowód |
| --- | --- | --- |
| 20 pomocników i pakiety danych | DONE_IN_CODE | `projects/`, `wwwroot/projects/` |
| 100 scenariuszy | DONE_IN_CODE | `production-scenarios.md` |
| Reguły, fallbacki i powody dopasowania | DONE_IN_CODE | `RuleEngineService`, testy jakości |
| Pełne wyniki bez reklamy | DONE_IN_CODE | `Result.razor` |
| Usunięcie mockowanych reklam | DONE_IN_CODE | brak `MockAdService` |
| SQLite i migracja | DONE_IN_CODE | `AppFactory.Persistence`, testy bazy |
| Historia i ulubione | DONE_IN_CODE | usługi SQLite i strony UI |
| Logi, retencja i maskowanie | DONE_IN_CODE | `LocalLogStore`, testy logów |
| Eksport diagnostyki | DONE_IN_CODE | `DiagnosticsExportService`, `Diagnostics.razor` |
| Local AI ukryte w Release | DONE_IN_CODE | `AppFeatureFlags`, `EnableLocalAiRelease=false` |
| Android API 35 i AAB | CONFIGURED | `AppFactory.Mobile.csproj` |
| Manifest prywatności | CONFIGURED | `AndroidManifest.xml` |
| Launcher icon i splash | DONE_IN_CODE | `Resources/AppIcon`, `Resources/Splash` |
| Google Play icon source | DONE | `store-icon.svg` |
| Feature graphic source | DONE | `feature-graphic.svg` |
| Generator PNG | DONE_IN_CODE | `generate-play-graphics.ps1` |
| Listing PL/EN/UK | READY_SOURCE | `listings.json`, `release-locales.json` |
| Draft listingów UE + UKR | READY_SOURCE | 25 przygotowanych zestawów |
| Publiczna polityka prywatności | READY_SOURCE | `site/privacy/` |
| Strona wsparcia | READY_SOURCE | `site/support/` |
| Warunki korzystania | READY_SOURCE | `site/terms/` |
| GitHub Pages workflow | CONFIGURED | `.github/workflows/pages.yml` |
| Data Safety guide | DONE | `DATA_SAFETY_DECLARATION.md` |
| Content rating guide | DONE | `CONTENT_RATING_GUIDE.md` |
| Play Console checklist | DONE | `PLAY_CONSOLE_CHECKLIST.md` |
| Release notes | DONE | `RELEASE_NOTES_1.0.0.md` |
| Upload-key helper | DONE_IN_CODE | `create-upload-keystore.ps1` |
| Podpisany AAB helper | DONE_IN_CODE | `build-play-aab.ps1` |
| Release workflow | CONFIGURED | `.github/workflows/release-aab.yml` |
| Kompletny package runner | DONE_IN_CODE | `prepare-google-play-package.ps1` |
| Gate Google Play | DONE_IN_CODE | `GooglePlayReleaseReadinessTests.cs` |
| Plan marketingowy | DONE | `marketing/LAUNCH_MARKETING_PLAN.md` |

## Świadome decyzje wydania

### Jedna aplikacja

Wydawany jest jeden katalog 20 pomocników. Osobne build profile pozostają narzędziem technicznym, ale nie są planem publikacji 20 podobnych produktów.

### Bez reklam w 1.0

Reklamy i płatności zostały wyłączone. Pełna rekomendacja jest dostępna bez blokady. Pozwala to wydać pierwszą stabilną wersję bez AdMob, UMP, identyfikatora reklamowego i dodatkowych deklaracji prywatności.

### Local AI wyłączone w Release

Kod eksperymentalny pozostaje dostępny w Debug. Wersja sklepowa nie pokazuje analizy zdjęć i dźwięku, dopóki realne modele, URL, SHA256, etykiety, wydajność i deklaracje prywatności nie zostaną potwierdzone.

### Trzy języki w pierwszym listingu

Teksty sklepu przygotowano dla wszystkich języków urzędowych UE i ukraińskiego. Publikowane są jednak tylko PL, EN i UK, ponieważ pełne wyniki aplikacji są dostępne właśnie w tych językach. Pozostałe teksty są materiałem na kolejne wydania.

## Czynności wymagające lokalnego środowiska

| Czynność | Status |
| --- | --- |
| `dotnet test` | NOT_RUN |
| Android Debug build | NOT_RUN |
| Android Release build | NOT_RUN |
| Generowanie finalnych PNG | NOT_RUN |
| 100 scenariuszy na urządzeniu | NOT_RUN |
| Migracja SQLite na Androidzie | NOT_RUN |
| Eksport diagnostyki offline | NOT_RUN |
| Sześć screenshotów | NOT_RUN |
| Utworzenie upload keystore | OWNER_ACTION |
| Podpisanie AAB | OWNER_ACTION |
| Włączenie GitHub Pages | OWNER_ACTION |
| Utworzenie aplikacji w Play Console | OWNER_ACTION |
| Data Safety i content rating | OWNER_ACTION |
| Internal/Closed testing | OWNER_ACTION |
| Publikacja produkcyjna | OWNER_ACTION |

## Polecenia wykonawcze

### 1. Testy

```powershell
pwsh ./tools/quality/run-local-test-plan.ps1 -RestoreWorkloads -IncludeReleaseBuild -WriteReport
```

### 2. Paczka sklepu bez podpisu

```powershell
pwsh ./tools/release/prepare-google-play-package.ps1
```

### 3. Upload key

```powershell
pwsh ./tools/release/create-upload-keystore.ps1
```

### 4. Podpisany AAB

```powershell
pwsh ./tools/release/build-play-aab.ps1 `
  -KeystorePath <path> `
  -KeyPasswordFile <path> `
  -StorePasswordFile <path>
```

## Kryterium końcowe

Status `production-ready` można nadać dopiero po uzyskaniu:

- zielonych testów i buildów,
- 100/100 PASS,
- sześciu zatwierdzonych screenshotów,
- aktywnych publicznych stron,
- podpisanego AAB zaakceptowanego przez Google Play,
- poprawnego pre-launch report,
- zakończonego wymaganego testu zamkniętego,
- braku defektów krytycznych i wysokich.

Nie wykonano ani nie zasymulowano czynności wymagających prywatnego klucza, urządzenia lub dostępu do Play Console.
