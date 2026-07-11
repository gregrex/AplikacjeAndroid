# Play Console checklist — AppFactory Pomocniki

## Dane aplikacji

| Pole | Wartość | Status |
| --- | --- | --- |
| Nazwa | AppFactory Pomocniki | READY |
| Package ID | `pl.gbcom.appfactory` | READY |
| Domyślny język | polski (`pl-PL`) | READY |
| Typ | aplikacja | READY |
| Bezpłatna/płatna | bezpłatna | OWNER_ACTION |
| Kategoria | Tools | OWNER_ACTION |
| E-mail kontaktowy | `gbosko@gbcom.pl` | READY |
| Strona produktu | `https://gregrex.github.io/AplikacjeAndroid/` | PENDING_PAGES |
| Polityka prywatności | `https://gregrex.github.io/AplikacjeAndroid/privacy/` | PENDING_PAGES |
| Pomoc | `https://gregrex.github.io/AplikacjeAndroid/support/` | PENDING_PAGES |

## Konfiguracja konta

- [ ] Konto Google Play Developer jest aktywne.
- [ ] Tożsamość właściciela została zweryfikowana.
- [ ] Zweryfikowano urządzenie, jeżeli Play Console tego wymaga.
- [ ] Dane dewelopera wyświetlane publicznie są poprawne.
- [ ] Zaakceptowano aktualne umowy dystrybucyjne.
- [ ] Włączono uwierzytelnianie wieloskładnikowe.

## GitHub Pages

- [ ] W repozytorium otwarto `Settings -> Pages`.
- [ ] Jako źródło wybrano `GitHub Actions`.
- [ ] Workflow `Publish AppFactory Site` zakończył się sukcesem.
- [ ] Strona główna działa bez logowania.
- [ ] Polityka prywatności działa bez logowania.
- [ ] Strona wsparcia działa bez logowania.
- [ ] Linki zostały otwarte na telefonie.

## Store listing

- [ ] Wgrano ikonę PNG 512×512, maks. 1 MB.
- [ ] Wgrano feature graphic PNG/JPEG 1024×500 bez alpha.
- [ ] Wgrano minimum 4, docelowo 6 screenshotów telefonu.
- [ ] Każdy screenshot przedstawia aktualną wersję aplikacji.
- [ ] Uzupełniono tekst alternatywny grafik.
- [ ] Zaimportowano tytuł, krótki i pełny opis dla domyślnego języka.
- [ ] Zaimportowano lokalizacje UE i ukraińską.
- [ ] Teksty nie zawierają obietnic typu „najlepsza”, „numer 1” ani sztucznych słów kluczowych.

## App content

### Privacy policy

- [ ] Wstawiono publiczny URL polityki.
- [ ] Polityka odpowiada zachowaniu finalnego AAB.

### Ads

- [ ] Wybrano `No, my app does not contain ads` dla wersji 1.0.
- [ ] Potwierdzono brak AdMob i identyfikatora reklamowego w AAB.

### App access

- [ ] Wybrano `All functionality is available without special access`.
- [ ] Potwierdzono brak logowania, kodu zaproszenia i płatnej blokady.

### Target audience

- [ ] Wybrano wyłącznie grupy pełnoletnie.
- [ ] Nie zaznaczono kierowania aplikacji do dzieci.
- [ ] Listing i grafiki nie są projektowane jako produkt dla dzieci.

### Content rating

- [ ] Wypełniono kwestionariusz zgodnie z `CONTENT_RATING_GUIDE.md`.
- [ ] Sprawdzono otrzymaną klasyfikację.

### Data Safety

- [ ] Odpowiedzi przepisano z `DATA_SAFETY_DECLARATION.md`.
- [ ] Sprawdzono wszystkie biblioteki i uprawnienia finalnego AAB.
- [ ] Potwierdzono brak zewnętrznej transmisji danych użytkownika.

### Government apps / News apps / Health apps

- [ ] Aplikacja nie jest aplikacją rządową.
- [ ] Aplikacja nie jest aplikacją informacyjną/newsową.
- [ ] Aplikacja nie jest wyrobem medycznym ani aplikacją zdrowotną.

## Release

- [ ] Włączono Play App Signing.
- [ ] Upload key utworzono poza repozytorium.
- [ ] Backup klucza jest zaszyfrowany i przechowywany w co najmniej dwóch bezpiecznych miejscach.
- [ ] Hasła nie znajdują się w repo ani logach CI.
- [ ] Podpisany AAB został przyjęty przez App Bundle Explorer.
- [ ] Target API finalnego AAB wynosi co najmniej 35.
- [ ] Pre-launch report nie zawiera crashy, ANR ani blokerów dostępności.
- [ ] Internal testing zakończono smoke testem.
- [ ] Closed testing wykonano, jeżeli wymagane dla konta.
- [ ] Release notes są uzupełnione.
- [ ] Managed publishing jest skonfigurowane świadomie.

## Kryterium publikacji

Nie naciskać `Start rollout to production`, dopóki:

- checklista techniczna nie jest kompletna,
- 100 scenariuszy nie ma PASS,
- nie ma defektów krytycznych ani wysokich,
- polityka, Data Safety i binarka są spójne,
- screenshoty pochodzą z finalnego release candidate,
- właściciel konta zaakceptował finalne treści prawne i marketingowe.
