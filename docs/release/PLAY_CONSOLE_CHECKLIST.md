# Play Console — checklista AppFactory Pomocniki 1.0

## Dane aplikacji

| Pole | Wartość | Status |
| --- | --- | --- |
| Nazwa | AppFactory Pomocniki | READY |
| Package ID | `pl.gbcom.appfactory` | READY |
| Domyślny język | polski (`pl-PL`) | READY |
| Typ | aplikacja | READY |
| Cena | bezpłatna | OWNER_ACTION |
| Kategoria | Tools | OWNER_ACTION |
| E-mail | `gbosko@gbcom.pl` | READY |
| Strona produktu | `https://gregrex.github.io/AplikacjeAndroid/` | PENDING_PAGES |
| Polityka prywatności | `https://gregrex.github.io/AplikacjeAndroid/privacy/` | PENDING_PAGES |
| Pomoc | `https://gregrex.github.io/AplikacjeAndroid/support/` | PENDING_PAGES |
| Warunki | `https://gregrex.github.io/AplikacjeAndroid/terms/` | PENDING_PAGES |

## Kolejność przygotowania

- [ ] Workflow `Quality Checks` jest zielony.
- [ ] Workflow `Google Play Release Readiness` jest zielony.
- [ ] `run-local-test-plan.ps1` przechodzi lokalnie.
- [ ] `prepare-google-play-package.ps1` przechodzi.
- [ ] Sześć screenshotów znajduje się w `artifacts/google-play/screenshots/final`.
- [ ] `verify-google-play-package.ps1 -RequireScreenshots` przechodzi.
- [ ] Publiczne strony GitHub Pages działają.
- [ ] Dopiero potem utworzono i użyto upload key.

## Konfiguracja konta

- [ ] Konto Google Play Developer jest aktywne.
- [ ] Tożsamość właściciela została zweryfikowana.
- [ ] Urządzenie właściciela zostało zweryfikowane, jeżeli konsola tego wymaga.
- [ ] Publiczne dane dewelopera są poprawne.
- [ ] Zaakceptowano aktualne umowy.
- [ ] Włączono uwierzytelnianie wieloskładnikowe.

## GitHub Pages

- [ ] W `Settings -> Pages` wybrano `GitHub Actions`.
- [ ] Workflow `Publish AppFactory Site` zakończył się sukcesem.
- [ ] Strona główna działa bez logowania.
- [ ] Polityka prywatności działa bez logowania.
- [ ] Strona wsparcia działa bez logowania.
- [ ] Warunki korzystania działają bez logowania.
- [ ] Wszystkie adresy sprawdzono na telefonie.

## Store listing

- [ ] Wgrano ikonę PNG 512×512, maksymalnie 1 MB.
- [ ] Wgrano feature graphic 1024×500 bez przezroczystości.
- [ ] Wgrano sześć screenshotów telefonu z finalnego release candidate.
- [ ] Uzupełniono tekst alternatywny grafik.
- [ ] Opublikowano listing `pl-PL`.
- [ ] Opublikowano listing `en-US`.
- [ ] Opublikowano listing `uk-UA`.
- [ ] Nie opublikowano pozostałych 22 szkiców przed ukończeniem lokalizacji aplikacji.
- [ ] Dodano changelog version code `1` w PL/EN/UK.
- [ ] Teksty nie zawierają obietnic „najlepsza”, „numer 1” ani sztucznego upychania słów kluczowych.
- [ ] Screenshoty nie pokazują Local AI, reklam, danych prywatnych ani kontrolek testowych.

Źródła:

```text
marketing/google-play/listings.json
marketing/google-play/release-locales.json
marketing/google-play/release-notes.json
marketing/google-play/SCREENSHOT_PLAN.md
```

## App content

### Polityka prywatności

- [ ] Wstawiono działający publiczny URL.
- [ ] Polityka odpowiada zachowaniu finalnego AAB.
- [ ] Kontakt w polityce jest aktualny.

### Reklamy

- [ ] Wybrano `No, my app does not contain ads`.
- [ ] Potwierdzono brak AdMob, UMP i identyfikatora reklamowego.
- [ ] Pełna rekomendacja jest dostępna bez reklamy.

### App access

- [ ] Wybrano `All functionality is available without special access`.
- [ ] Brak logowania, kodu zaproszenia i płatnej blokady.

### Target audience

- [ ] Wybrano wyłącznie grupy pełnoletnie.
- [ ] Nie zaznaczono kierowania aplikacji do dzieci.
- [ ] Grafiki i opis nie przedstawiają produktu jako aplikacji dziecięcej.

### Content rating

- [ ] Kwestionariusz wypełniono zgodnie z `CONTENT_RATING_GUIDE.md`.
- [ ] Sprawdzono otrzymaną klasyfikację dla głównych rynków.
- [ ] Zachowano zapis odpowiedzi do dokumentacji wydania.

### Data Safety

- [ ] Odpowiedzi przygotowano na podstawie `DATA_SAFETY_DECLARATION.md`.
- [ ] Sprawdzono biblioteki, manifest i ruch sieciowy finalnego Release.
- [ ] Potwierdzono `EnableLocalAiRelease=false`.
- [ ] Potwierdzono brak automatycznej transmisji logów.
- [ ] Potwierdzono, że ręczny arkusz udostępniania nie ma domyślnego odbiorcy.

### Pozostałe deklaracje

- [ ] Aplikacja nie jest aplikacją rządową.
- [ ] Aplikacja nie jest aplikacją newsową.
- [ ] Aplikacja nie jest wyrobem medycznym ani aplikacją zdrowotną.
- [ ] Aplikacja nie zawiera publicznego UGC.
- [ ] Aplikacja nie zawiera zakupów cyfrowych.

## Podpis i AAB

- [ ] Włączono Play App Signing.
- [ ] Upload key utworzono poza repozytorium.
- [ ] Istnieją co najmniej dwie zaszyfrowane kopie klucza.
- [ ] Hasła znajdują się w menedżerze haseł.
- [ ] Klucz i hasła nie występują w repo, logach ani artefaktach publicznych.
- [ ] Podpisany AAB zbudowano przez `build-play-aab.ps1` albo workflow `Build Signed Google Play AAB`.
- [ ] `verify-google-play-package.ps1 -RequireScreenshots -RequireSignedAab` przechodzi.
- [ ] SHA256 AAB zapisano w dokumentacji wydania.
- [ ] App Bundle Explorer przyjął AAB.
- [ ] Finalny AAB ma package ID `pl.gbcom.appfactory`.
- [ ] Finalny AAB ma version code `1`.
- [ ] Finalny AAB targetuje co najmniej API 35.

## Testy Google Play

- [ ] Internal testing został opublikowany.
- [ ] Instalacja z linku opt-in działa.
- [ ] Aktualizacja przez Google Play zachowuje SQLite.
- [ ] Pre-launch report nie zawiera crashy ani ANR.
- [ ] Sprawdzono problemy dostępności zgłoszone przez crawler.
- [ ] Closed testing wykonano, jeżeli konto tego wymaga.
- [ ] Spełniono dokładną liczbę testerów i okres pokazywany w Play Console.
- [ ] Każdy kolejny AAB ma wyższy version code.

Instrukcja:

```text
docs/release/GOOGLE_PLAY_TESTING_GUIDE.md
```

## Kryterium publikacji

Nie używać `Start rollout to production`, dopóki:

- testy automatyczne i 100 scenariuszy nie mają PASS,
- nie ma defektów krytycznych ani wysokich,
- polityka, Data Safety, listing i binarka nie są zgodne,
- publiczne strony nie działają,
- screenshoty nie pochodzą z finalnego release candidate,
- podpisany AAB nie został sprawdzony w Internal testing,
- pre-launch report zawiera crash, ANR lub bloker,
- wymagany test zamknięty nie jest zakończony,
- właściciel konta nie zatwierdził treści prawnych i marketingowych.
