# Progress — EU languages plus Ukrainian

## Zrobione

Poprawiono wymaganie językowe: aplikacja ma obsługiwać wszystkie języki krajów Unii Europejskiej oraz ukraiński.

## Zmienione elementy

- `LanguageService` obsługuje pełną listę języków UE + `uk`.
- `Settings.razor` pokazuje dynamiczną listę języków z serwisu.
- `Result.razor` używa fallbacku wyników: wybrany język -> EN -> PL.
- `LanguageServiceTests` sprawdza pełną listę języków i fallback.

## Lista języków

- bg — bułgarski
- hr — chorwacki
- cs — czeski
- da — duński
- nl — niderlandzki
- en — angielski
- et — estoński
- fi — fiński
- fr — francuski
- de — niemiecki
- el — grecki
- hu — węgierski
- ga — irlandzki
- it — włoski
- lv — łotewski
- lt — litewski
- mt — maltański
- pl — polski
- pt — portugalski
- ro — rumuński
- sk — słowacki
- sl — słoweński
- es — hiszpański
- sv — szwedzki
- uk — ukraiński

## Ważna decyzja

Nie generujemy od razu pełnych wyników dla wszystkich języków, bo to mocno zwiększy objętość danych. Architektura już obsługuje wybór wszystkich języków, ale dopóki dany język nie ma pliku wyników, używany jest fallback do EN, a potem PL.

## Status `plama-ratownik`

Około 62%.

## Nadal brakuje

- runtime tłumaczeń kategorii i pytań,
- trwałego storage,
- zielonych testów CI,
- zielonego builda Android,
- checklisty Google Play,
- decyzji, które języki tłumaczymy w pełni w pierwszej publikacji.
