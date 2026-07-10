# Scenariusze produkcyjne — Zmywarka Diagnosta

## SCN-01 — Osad na naczyniach

**Cel:** sprawdzić podstawową diagnostykę.

**Kroki:**
1. Otwórz projekt `zmywarka-diagnosta`.
2. Wybierz biały osad lub nalot.
3. Zaznacz brak wycieku i zapachu spalenizny.
4. Zakończ quiz.

**Oczekiwany wynik:** aplikacja pokazuje checklistę kontroli soli, nabłyszczacza, filtra i ustawień oraz uzasadnia wybór.

**Pokrycie:** reguła szczegółowa, wynik free, reason.

## SCN-02 — Woda nie odpompowuje się

**Cel:** sprawdzić wariant odpływu.

**Kroki:**
1. Wybierz wodę pozostającą na dnie.
2. Otwórz wynik premium.

**Oczekiwany wynik:** wynik zaczyna od bezpiecznej kontroli filtra i odpływu, a pełne kroki nie wymagają ingerencji w instalację elektryczną.

**Pokrycie:** alternatywna reguła, premium, bezpieczeństwo.

## SCN-03 — Wyciek lub zapach spalenizny

**Cel:** sprawdzić blokadę ryzykownej diagnostyki.

**Kroki:**
1. Wybierz wyciek, dym, zapach spalenizny albo problem elektryczny.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja zaleca przerwanie używania, odłączenie urządzenia w bezpieczny sposób i kontakt z serwisem; nie podaje instrukcji rozbierania urządzenia.

**Pokrycie:** safety fallback, krytyczne ostrzeżenie.

## SCN-04 — Lokalna analiza zdjęcia i dźwięku

**Cel:** sprawdzić oba moduły Local AI.

**Kroki:**
1. Sprawdź modele `local-vision-v1` i `local-audio-v1`.
2. Wybierz lokalne zdjęcie osadu oraz nagranie do 20 sekund.
3. Uruchom obie analizy.
4. Potwierdź sugestie ręcznie.

**Oczekiwany wynik:** analiza bez modelu jest blokowana; z modelami ONNX sugestie są lokalne, jawnie niepewne i nie zastępują oceny ryzyka.

**Pokrycie:** image, audio, ONNX, SHA256, safety-sensitive UX.

## SCN-05 — Alternatywy, historia i język

**Cel:** sprawdzić funkcje przekrojowe.

**Kroki:**
1. Wygeneruj wynik z alternatywami.
2. Odblokuj premium i wybierz alternatywę.
3. Dodaj wynik do ulubionych oraz sprawdź historię.
4. Otwórz wynik w EN lub UK.

**Oczekiwany wynik:** aktywny `ResultId` jest zapisany prawidłowo, a wersje językowe zachowują parytet.

**Pokrycie:** alternatywy, premium, historia, ulubione, PL/EN/UK.
