# Uruchomienie AppFactory Pomocniki

## Wymagania

- .NET SDK 9,
- workload .NET MAUI/Android,
- Android SDK,
- zgodny JDK,
- emulator Android albo telefon z debugowaniem USB,
- PowerShell 7 dla skryptów jakości i wydania.

## Przygotowanie

```powershell
dotnet --info
dotnet workload list
dotnet workload restore .\src\AppFactory.Mobile\AppFactory.Mobile.csproj
adb devices
```

## Build Debug

```powershell
dotnet build .\src\AppFactory.Mobile\AppFactory.Mobile.csproj -f net9.0-android -c Debug
```

Debug zawiera:

- narzędzia diagnostyczne testera,
- eksperymentalny panel Local AI,
- lokalne logi od poziomu `Debug`.

## Build Release bez podpisu sklepowego

```powershell
dotnet build .\src\AppFactory.Mobile\AppFactory.Mobile.csproj -f net9.0-android -c Release
```

Domyślny Release 1.0:

- tworzy AAB,
- targetuje API 35,
- nie zawiera reklam,
- pokazuje pełne wyniki bez blokady,
- ukrywa Local AI do czasu skonfigurowania realnych modeli,
- ukrywa przyciski generujące testowe wyjątki,
- zachowuje ręczny eksport diagnostyki.

## Pełna lokalna sesja jakości

```powershell
pwsh .\tools\quality\run-local-test-plan.ps1 -RestoreWorkloads -IncludeReleaseBuild -WriteReport
```

Nie przechodź do screenshotów i podpisywania AAB, dopóki wszystkie testy oraz build Release nie przejdą.

## Podpisany AAB dla Google Play

Najpierw utwórz upload keystore poza repozytorium:

```powershell
pwsh .\tools\release\create-upload-keystore.ps1
```

Następnie zbuduj AAB:

```powershell
pwsh .\tools\release\build-play-aab.ps1 `
  -KeystorePath "$HOME\AppFactory-Secrets\appfactory-upload.keystore" `
  -KeyPasswordFile "$HOME\AppFactory-Secrets\key-password.txt" `
  -StorePasswordFile "$HOME\AppFactory-Secrets\store-password.txt"
```

Wynik:

```text
artifacts/google-play/release/appfactory-pomocniki-1.0.0-build1.aab
```

## Materiały sklepu

```powershell
pwsh .\tools\release\generate-play-graphics.ps1
pwsh .\tools\release\export-play-metadata.ps1
```

Screenshoty:

```text
marketing/google-play/SCREENSHOT_PLAN.md
```

## Testowane funkcje wersji 1.0

1. Katalog 20 pomocników.
2. Wyszukiwanie i filtry.
3. Kategorie i quiz.
4. Lokalny silnik reguł.
5. Pełne rekomendacje bez reklamy.
6. Wyjaśnienie dopasowania i alternatywy.
7. Historia i ulubione w SQLite.
8. Języki PL, EN i UK.
9. Narzędzia projektu, w tym licznik rzędów.
10. Lokalne logi i ręczny eksport ZIP.
11. Polityka prywatności i pomoc w aplikacji.

Dokument planu wydania:

```text
docs/release/GOOGLE_PLAY_RELEASE_PLAN.md
```
