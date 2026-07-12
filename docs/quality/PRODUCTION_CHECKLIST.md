# Checklista produkcyjna — AppFactory Pomocniki 1.0

## 1. Tożsamość wydania

- nazwa: `AppFactory Pomocniki`,
- package ID: `pl.gbcom.appfactory`,
- display version: `1.0.0`,
- version code: `1`,
- format: Android App Bundle,
- target API: 35,
- jedna aplikacja katalogowa z 20 pomocnikami,
- bez konta, reklam i płatności,
- pełne wyniki dostępne bez blokady,
- Local AI wyłączone w domyślnym buildzie Release.

## 2. Dane 20 pomocników

Każdy pomocnik musi mieć komplet source i runtime:

```text
projects/<projectId>/data/app.json
projects/<projectId>/data/categories.json
projects/<projectId>/data/questions.json
projects/<projectId>/data/rules.json
projects/<projectId>/data/results.pl.json
projects/<projectId>/data/results.en.json
projects/<projectId>/data/results.uk.json
projects/<projectId>/theme.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/*.json
```

Wymagania:

- zgodne identyfikatory source/runtime,
- każda reguła ma `reason`,
- każda reguła wskazuje istniejące wyniki,
- PL/EN/UK mają parytet identyfikatorów wyników,
- globalny bezpieczny fallback,
- ostrzeżenia dla projektów technicznych i bezpieczeństwa.

Synchronizacja:

```powershell
pwsh ./tools/quality/sync-runtime-packs.ps1
```

## 3. Testy automatyczne

Pełna lokalna sesja:

```powershell
pwsh ./tools/quality/run-local-test-plan.ps1 -RestoreWorkloads -IncludeReleaseBuild -WriteReport
```

Release gate:

```powershell
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter "FullyQualifiedName~GooglePlayReleaseReadinessTests|FullyQualifiedName~ProductionReadinessTests"
```

Wymagane obszary:

- walidacja 20 pakietów danych,
- osiągalność reguł,
- 100 scenariuszy,
- SQLite i migracja,
- logowanie i maskowanie danych,
- UI/UX i dostępność,
- brak mockowanych reklam,
- branding, listing, polityka i pliki wydania,
- konfiguracja AAB i API 35.

## 4. Lokalna baza SQLite

SQLite przechowuje:

- historię wyników,
- ulubione,
- wersję schematu.

Wymagania:

- wersjonowany schemat,
- sortowanie i limit 100 wpisów historii,
- deduplikacja ulubionych,
- usuwanie i czyszczenie,
- jednorazowa migracja starszych danych z `Preferences`,
- test nowej instalacji oraz aktualizacji bez czyszczenia danych.

Dokumentacja:

```text
docs/quality/LOCAL_DATABASE.md
```

## 5. Logi i diagnostyka

Wersja Release zapisuje lokalne logi od poziomu `Information`:

- JSONL,
- retencja 7 dni,
- maksymalnie 12 plików,
- maksymalnie 2 MB na plik,
- maskowanie e-maili, tokenów, haseł i sekretów,
- brak automatycznej transmisji,
- ręczny eksport ZIP.

W buildzie Release nie mogą być widoczne przyciski generowania znaczników i testowych wyjątków. Eksport, podgląd i czyszczenie logów pozostają dostępne dla użytkownika.

Dokumentacja:

```text
docs/quality/LOCAL_LOGGING.md
```

## 6. Funkcje wersji 1.0

Wymagane:

- katalog 20 pomocników,
- wyszukiwanie i filtry,
- kategorie,
- quiz z postępem i cofnięciem,
- lokalny silnik reguł,
- pełna rekomendacja bez reklamy i płatności,
- wyjaśnienie dopasowania,
- alternatywne rekomendacje,
- historia i ulubione,
- języki PL, EN i UK,
- kopiowanie treści w odpowiednich projektach,
- licznik rzędów i notatki szydełkowe,
- polityka prywatności,
- pomoc i kontakt,
- diagnostyka użytkownika.

Local AI:

- kod może pozostać w repo,
- panel nie może być widoczny w domyślnym Release,
- `EnableLocalAiRelease=false`,
- funkcji nie wolno promować w listingu i screenshotach 1.0.

## 7. Branding i materiały Google Play

Wymagane źródła:

```text
src/AppFactory.Mobile/Resources/AppIcon/
src/AppFactory.Mobile/Resources/Splash/
marketing/brand/BRAND_GUIDE.md
marketing/google-play/source/store-icon.svg
marketing/google-play/source/feature-graphic.svg
```

Generowanie:

```powershell
pwsh ./tools/release/generate-play-graphics.ps1
```

Wynik:

- ikona PNG 512×512,
- feature graphic 1024×500,
- sześć prawdziwych screenshotów finalnej aplikacji.

Screenshoty muszą odpowiadać aktualnej binarce i nie mogą pokazywać Local AI, mocków, danych prywatnych ani kontrolek testowych.

Plan:

```text
marketing/google-play/SCREENSHOT_PLAN.md
```

## 8. Listing i lokalizacje

Źródło:

```text
marketing/google-play/listings.json
```

Przygotowano 25 zestawów tekstów: 24 języki urzędowe UE plus ukraiński.

Pierwszy release eksportuje tylko:

- `pl-PL`,
- `en-US`,
- `uk-UA`.

Powód: pełna zawartość aplikacji jest obecnie dostępna w tych językach. Pozostałych lokalizacji nie wolno publikować przed ukończeniem i testem treści w aplikacji.

Eksport:

```powershell
pwsh ./tools/release/export-play-metadata.ps1
```

## 9. Publiczna strona i dokumenty prawne

Wymagane publiczne adresy HTTPS:

- strona produktu,
- polityka prywatności,
- pomoc,
- warunki korzystania.

Źródła:

```text
site/index.html
site/privacy/index.html
site/support/index.html
site/terms/index.html
```

Workflow:

```text
.github/workflows/pages.yml
```

Właściciel repozytorium musi włączyć GitHub Pages z użyciem GitHub Actions i sprawdzić wszystkie adresy bez logowania.

## 10. Play Console

Właściciel konta musi uzupełnić:

- politykę prywatności,
- Data Safety,
- deklarację braku reklam,
- App access — bez specjalnego dostępu,
- grupę docelową — osoby pełnoletnie,
- content rating,
- kategorię `Tools`,
- dane kontaktowe,
- listing i grafiki,
- release notes.

Dokumenty robocze:

```text
docs/release/PLAY_CONSOLE_CHECKLIST.md
docs/release/DATA_SAFETY_DECLARATION.md
docs/release/CONTENT_RATING_GUIDE.md
```

Odpowiedzi muszą zostać ponownie sprawdzone względem finalnego AAB i aktualnego formularza Play Console.

## 11. Podpis i AAB

Upload keystore musi powstać poza repozytorium:

```powershell
pwsh ./tools/release/create-upload-keystore.ps1
```

Budowa:

```powershell
pwsh ./tools/release/build-play-aab.ps1 `
  -KeystorePath <path> `
  -KeyPasswordFile <path> `
  -StorePasswordFile <path>
```

Wymagania:

- Play App Signing włączone,
- upload key ma co najmniej dwie zaszyfrowane kopie,
- hasła i klucz nie trafiają do repo, logów ani artefaktów publicznych,
- AAB jest przyjęty przez App Bundle Explorer,
- hash SHA256 jest zapisany w dokumentacji wydania.

## 12. Testy Android i Google Play

Przed produkcją:

- wszystkie automaty zielone,
- Debug i Release build przechodzą,
- smoke test na urządzeniu,
- 100/100 scenariuszy PASS,
- test migracji SQLite,
- eksport logów offline,
- systemowy Wstecz, obrót, tło/wznowienie,
- długi tekst i powiększona czcionka,
- Internal testing przez Play Console,
- pre-launch report bez crashy i ANR,
- Closed testing, jeżeli wymaga tego konto,
- brak otwartych defektów krytycznych i wysokich.

Tracker:

```text
docs/quality/SCENARIO_EXECUTION_TRACKER.md
```

## 13. Kryterium publikacji

Aplikację można skierować na produkcję, gdy:

- finalny podpisany AAB jest zaakceptowany,
- publiczna polityka działa,
- Data Safety odpowiada binarce,
- grafiki i screenshoty są zatwierdzone,
- wszystkie testy i scenariusze przechodzą,
- wymagany test zamknięty jest ukończony,
- właściciel konta zaakceptował dokumenty prawne i listing,
- wdrożenie zaczyna się od kontrolowanego rollout’u.

Pełny plan:

```text
docs/release/GOOGLE_PLAY_RELEASE_PLAN.md
```
