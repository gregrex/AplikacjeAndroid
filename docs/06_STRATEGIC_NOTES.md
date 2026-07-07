# Uwagi strategiczne — AplikacjeAndroid

## Decyzja

Repo `AplikacjeAndroid` traktujemy jako fabrykę mikroaplikacji Android bez serwera. Celem nie jest jeden duży system, tylko szybkie wypuszczanie małych aplikacji, testowanie rynku i budowa przychodu reklamowego.

## Relacja do stealth startupów

Po przeglądzie repozytoriów głównym kandydatem na większy stealth startup pozostaje `Agro-Parts`. `AplikacjeAndroid` ma pełnić rolę niezależnego toru cashflow i laboratorium MVP.

## Zasada pracy

1. Najpierw wspólny silnik MAUI Blazor Hybrid.
2. Potem pierwsza aplikacja produkcyjna: `plama-ratownik`.
3. Następnie kolejne projekty na tym samym silniku.
4. Nie trenować AI w MVP.
5. Nie budować backendu w MVP.
6. Dane trzymać lokalnie w JSON/SQLite.
7. Reklamy rewarded jako główny model odblokowania pełnego wyniku.

## Priorytet 20 projektów

Fala 1:

1. plama-ratownik
2. kolek-dobieracz
3. pies-trener-7dni
4. bajka-z-rysunku
5. vinted-olx-opis

Fala 2:

6. kot-bawi-sie
7. barber-translator
8. outfit-coach
9. pakowanie-paczek
10. pokoj-makeover

Fala 3:

11. domfix
12. zmywarka-diagnosta
13. router-wifi-diagnosta
14. szydelko-pomocnik
15. rysunek-coach

Fala 4:

16. bukietownik
17. chleb-zakwas-coach
18. silikon-fuga-fix
19. krawat-garnitur-coach
20. fryzury-proste

## Uwaga o projekcie fryzur

Wcześniejszy katalog `fryzury-do-szkoly` był blokowany przez narzędzie zapisu. Zmieniamy nazwę roboczą na `fryzury-proste` i opisujemy projekt neutralnie jako instrukcje uczesań, bez oceniania wyglądu i bez rankingów.

## Następny krok

Dokończyć katalogi dla wszystkich 20 projektów oraz przygotować backlog implementacyjny dla Sprint 01.
