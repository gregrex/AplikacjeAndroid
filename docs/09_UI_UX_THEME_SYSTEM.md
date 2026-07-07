# UI UX Theme System

## Zasada

Każda aplikacja w repo `AplikacjeAndroid` ma mieć własny styl wizualny. Styl ma wynikać z funkcji aplikacji, grupy docelowej i sposobu użycia.

## Wspólna baza

Wszystkie aplikacje korzystają ze wspólnego minimalistycznego systemu UI:

- prosty układ mobilny,
- duże karty,
- czytelne przyciski,
- krótka ścieżka do wyniku,
- czytelna typografia,
- jasna hierarchia informacji.

## Różnice między aplikacjami

Każda aplikacja ma własny plik `theme.json`, który określa:

- kolory,
- nastrój marki,
- grupę docelową,
- ton komunikacji,
- styl kart,
- styl przycisków,
- styl ilustracji,
- styl ekranu wyniku.

## Przykłady kierunku

- Plama Ratownik: czystość, pomoc, dom.
- Kołek Dobieracz: technicznie, narzędziowo, bezpiecznie.
- Pies Trener: przyjaźnie, energicznie, treningowo.
- Bajka z rysunku: kreatywnie, miękko, rodzinnie.
- Barber Translator: kontrastowo, nowocześnie, premium.

## Pliki techniczne

- `ThemeDefinition`
- `ThemeCssBuilder`
- `theme.json` dla każdego projektu

## Warunek MVP UI

Projekt ma gotowy poziom MVP UI, gdy ma:

1. własny `theme.json`,
2. opis grupy docelowej,
3. opis tonu,
4. kolory,
5. styl kart,
6. styl przycisków,
7. styl ekranu wyniku.
