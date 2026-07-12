# Plan wydania Google Play — AppFactory Pomocniki 1.0

## Decyzja produktu

Publikowany jest jeden produkt:

- nazwa: `AppFactory Pomocniki`,
- package ID: `pl.gbcom.appfactory`,
- wersja: `1.0.0`,
- version code: `1`,
- jedna aplikacja katalogowa z 20 pomocnikami,
- kategoria Google Play: `Tools`,
- grupa docelowa: osoby dorosłe,
- brak reklam, płatności i konta,
- pełne wyniki bez blokady,
- Local AI wyłączone w domyślnym Release do czasu dostarczenia zweryfikowanych modeli.

## Lokalizacje

Przygotowano szkice listingów dla 24 języków urzędowych UE oraz ukraińskiego.

Wydanie 1.0 publikuje wyłącznie:

- `pl-PL`,
- `en-US`,
- `uk-UA`.

Tylko te języki mają komplet treści wynikowych w aplikacji. Pozostałych 22 listingów nie wolno publikować przed przetłumaczeniem i przetestowaniem interfejsu oraz danych projektów.

Źródła:

```text
marketing/google-play/listings.json
marketing/google-play/release-locales.json
marketing/google-play/release-notes.json
```

## Gotowe w repozytorium

### Produkt

- AAB Release i API 35,
- finalna nazwa oraz numer wersji,
- pełne wyniki bez mockowanych reklam,
- feature flags Release,
- SQLite i migracja danych,
- lokalne logi z ręcznym eksportem,
- polityka prywatności i pomoc w aplikacji,
- Android manifest z wyłączonym backupem oraz cleartext traffic.

### Marka i materiały sklepu

- launcher icon i splash,
- źródło ikony Google Play,
- źródło feature graphic,
- generator PNG 512×512 i 1024×500,
- exporter metadanych PL/EN/UK,
- plan sześciu screenshotów,
- release notes,
- przewodnik marki i plan marketingowy.

### Strona publiczna

- strona produktu,
- polityka prywatności,
- strona pomocy,
- warunki korzystania,
- workflow GitHub Pages.

### Release engineering

- pipeline gotowości Release bez podpisu,
- generator paczki Google Play,
- walidator artefaktów,
- helper tworzenia upload keystore,
- helper eksportu keystore do GitHub Secrets,
- budowa podpisanego AAB,
- workflow podpisanego AAB,
- checklista Play Console,
- Data Safety i content rating guide,
- instrukcja Internal oraz Closed testing.

## Etap 1 — zielony kod

Uruchom:

```powershell
pwsh ./tools/quality/run-local-test-plan.ps1 -RestoreWorkloads -IncludeReleaseBuild -WriteReport
```

Wymagane:

1. wszystkie testy automatyczne PASS,
2. Android Debug build PASS,
3. Android Release build PASS,
4. działająca SQLite i migracja,
5. działający eksport diagnostyki,
6. smoke test urządzenia,
7. 100/100 scenariuszy PASS,
8. brak defektów krytycznych i wysokich.

Nie przechodź do finalnych screenshotów ani podpisu, dopóki ten etap nie jest zielony.

## Etap 2 — paczka sklepu bez podpisu

Uruchom:

```powershell
pwsh ./tools/release/prepare-google-play-package.ps1
```

Skrypt:

- uruchamia gate’y gotowości wydania,
- buduje Android Release z `EnableLocalAiRelease=false`,
- generuje ikonę i feature graphic,
- eksportuje listing oraz changelogi PL/EN/UK,
- sprawdza źródła publicznej strony,
- raportuje brakujące screenshoty i podpis,
- waliduje wygenerowane artefakty.

Wyniki:

```text
artifacts/google-play/generated/
artifacts/google-play/metadata/
artifacts/google-play/package-summary.md
```

To samo może wykonać workflow:

```text
.github/workflows/release-readiness.yml
```

## Etap 3 — publiczna strona

1. W repozytorium otwórz `Settings -> Pages`.
2. Wybierz źródło `GitHub Actions`.
3. Uruchom workflow `Publish AppFactory Site`.
4. Sprawdź bez logowania:

```text
https://gregrex.github.io/AplikacjeAndroid/
https://gregrex.github.io/AplikacjeAndroid/privacy/
https://gregrex.github.io/AplikacjeAndroid/support/
https://gregrex.github.io/AplikacjeAndroid/terms/
```

5. Sprawdź linki na telefonie i komputerze.
6. Nie wpisuj URL do Play Console, dopóki strony nie odpowiadają publicznie.

## Etap 4 — screenshoty finalnego buildu

1. Zainstaluj finalny Release candidate.
2. Ustaw ekran pionowo i usuń prywatne dane testowe.
3. Wykonaj sześć ekranów według:

```text
marketing/google-play/SCREENSHOT_PLAN.md
```

4. Zapisz zatwierdzone pliki w:

```text
artifacts/google-play/screenshots/final/
```

5. Ponownie uruchom walidator:

```powershell
pwsh ./tools/release/verify-google-play-package.ps1 -RequireScreenshots
```

Screenshoty nie mogą pokazywać Local AI, reklam, przycisków testowych, błędów ani danych prywatnych.

## Etap 5 — upload key i podpisany AAB

Utwwórz klucz poza repozytorium:

```powershell
pwsh ./tools/release/create-upload-keystore.ps1
```

Zabezpiecz:

- co najmniej dwie zaszyfrowane kopie,
- hasła w menedżerze haseł,
- brak klucza oraz haseł w repo, logach i komunikatorach.

Zbuduj AAB:

```powershell
pwsh ./tools/release/build-play-aab.ps1 `
  -KeystorePath <path> `
  -KeyPasswordFile <path> `
  -StorePasswordFile <path>
```

Albo skonfiguruj GitHub Secrets i uruchom:

```text
.github/workflows/release-aab.yml
```

Wymagane secrets:

- `ANDROID_KEYSTORE_BASE64`,
- `ANDROID_KEYSTORE_PASSWORD`,
- `ANDROID_KEY_PASSWORD`,
- `ANDROID_KEY_ALIAS`.

Po podpisaniu:

```powershell
pwsh ./tools/release/verify-google-play-package.ps1 -RequireScreenshots -RequireSignedAab
```

## Etap 6 — Play Console

1. Utwórz aplikację `AppFactory Pomocniki`.
2. Ustaw package ID `pl.gbcom.appfactory`.
3. Włącz Play App Signing.
4. Ustaw aplikację jako bezpłatną.
5. Dodaj listing PL/EN/UK i grafiki.
6. Ustaw publiczną politykę prywatności oraz stronę wsparcia.
7. Uzupełnij App content według:

```text
docs/release/PLAY_CONSOLE_CHECKLIST.md
docs/release/DATA_SAFETY_DECLARATION.md
docs/release/CONTENT_RATING_GUIDE.md
```

8. Sprawdź każdą odpowiedź względem finalnego AAB i aktualnego formularza konsoli.
9. Wgraj AAB do `Internal testing`.
10. Sprawdź App Bundle Explorer oraz pre-launch report.

## Etap 7 — Internal i Closed testing

Instrukcja:

```text
docs/release/GOOGLE_PLAY_TESTING_GUIDE.md
```

Kolejność:

1. Internal testing na małej grupie,
2. test instalacji i aktualizacji przez Google Play,
3. sprawdzenie danych SQLite po aktualizacji,
4. naprawa crashy i ANR,
5. Closed testing, jeżeli wymaga tego konto,
6. utrzymanie wymaganej liczby testerów i okresu widocznego w Play Console,
7. build naprawczy zawsze z wyższym version code.

## Etap 8 — produkcja

Produkcja jest dopuszczalna dopiero gdy:

- podpisany AAB jest zaakceptowany,
- publiczne strony działają,
- Data Safety odpowiada binarce,
- 100/100 scenariuszy ma PASS,
- screenshoty pochodzą z finalnego buildu,
- pre-launch report nie zawiera crashy ani ANR,
- wymagany test zamknięty jest zakończony,
- brak otwartych defektów krytycznych i wysokich,
- właściciel konta zaakceptował finalne treści prawne i sklepowe.

Pierwsze wdrożenie wykonaj jako kontrolowany, etapowy rollout.

## Czynności, których repo nie może wykonać

- rejestracja i opłacenie konta Google Play Developer,
- weryfikacja tożsamości oraz urządzenia właściciela,
- akceptacja umów Google,
- wybór i przechowywanie prywatnych haseł,
- wykonanie prawdziwych screenshotów uruchomionej aplikacji,
- prawne zatwierdzenie formularzy Play Console,
- udział realnych testerów,
- kliknięcie publikacji.

Te pozycje są oznaczone jako `OWNER_ACTION` i nie mogą zostać bezpiecznie zasymulowane w repozytorium.
