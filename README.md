# AppFactory Pomocniki

Jedna aplikacja Android zawierająca 20 praktycznych pomocników do domu, stylu, hobby, zwierząt, pakowania i codziennych diagnoz.

## Produkt

Przepływ aplikacji:

```text
użytkownik wybiera pomocnik i kategorię
→ odpowiada na kilka pytań
→ lokalny silnik reguł dobiera rekomendację
→ aplikacja pokazuje pełny wynik, kroki i ostrzeżenia
→ wynik można zapisać w lokalnej historii lub ulubionych
```

Wersja Google Play 1.0:

- package ID: `pl.gbcom.appfactory`,
- nazwa: `AppFactory Pomocniki`,
- wersja: `1.0.0`,
- jedna aplikacja katalogowa, nie 20 powtarzalnych aplikacji,
- bez konta,
- bez reklam i płatności,
- bez zewnętrznej analityki,
- pełne wyniki dostępne bez blokady,
- dane użytkownika przechowywane lokalnie,
- Local AI wyłączone w domyślnym buildzie Release do czasu dostarczenia i testów prawdziwych modeli.

## Technologie

```text
.NET 9
.NET MAUI Blazor Hybrid
Android API 35
SQLite
lokalne pakiety JSON
Microsoft.Extensions.Logging
ONNX Runtime — infrastruktura eksperymentalna, wyłączona w Release 1.0
```

## Języki

Pełna zawartość wyników jest dostępna w:

- polskim,
- angielskim,
- ukraińskim.

Teksty listingów są przygotowane dla 24 języków urzędowych UE oraz ukraińskiego, ale pierwszy release eksportuje wyłącznie PL/EN/UK. Pozostałe listingi mogą zostać opublikowane dopiero po ukończeniu odpowiadających im tłumaczeń wewnątrz aplikacji.

## Struktura

```text
docs/                     dokumentacja jakości i wydania
projects/                 dane 20 pomocników
src/AppFactory.Core/      reguły i modele domenowe
src/AppFactory.Mobile/    aplikacja MAUI Android
src/AppFactory.Persistence/ SQLite i lokalne logi
tests/                    testy automatyczne
marketing/                marka i materiały Google Play
site/                     publiczna strona, polityka i pomoc
tools/quality/            automaty jakości
tools/release/            grafiki, metadane, podpis i AAB
```

## Testy lokalne

```powershell
pwsh ./tools/quality/run-local-test-plan.ps1 -RestoreWorkloads -IncludeReleaseBuild -WriteReport
```

Plan:

```text
docs/quality/LOCAL_TEST_PLAN.md
```

## Przygotowanie Google Play

Plan wykonania:

```text
docs/release/GOOGLE_PLAY_RELEASE_PLAN.md
```

Najważniejsze polecenia:

```powershell
pwsh ./tools/release/generate-play-graphics.ps1
pwsh ./tools/release/export-play-metadata.ps1
pwsh ./tools/release/create-upload-keystore.ps1
pwsh ./tools/release/build-play-aab.ps1 -KeystorePath <path> -KeyPasswordFile <path> -StorePasswordFile <path>
```

## Status

Repozytorium jest kandydatem wydania. Status produkcyjny wymaga zielonych testów, podpisanego AAB, prawdziwych screenshotów finalnego buildu, poprawnie opublikowanej strony prywatności oraz zakończonych testów w Google Play.
