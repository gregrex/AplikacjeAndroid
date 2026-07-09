# Build profiles AppFactory

Ten raport opisuje katalogowy build oraz osobne profile buildów dla mikroaplikacji.

## Catalog build

| ProjectId | ApplicationTitle | ApplicationId |
| --- | --- | --- |
| catalog | AppFactory | `pl.gbcom.appfactory` |

## Project builds

| ProjectId | ApplicationTitle | ApplicationId |
| --- | --- | --- |
| `plama-ratownik` | Plama Ratownik | `pl.gbcom.appfactory.plamaratownik` |
| `kolek-dobieracz` | Kołek Dobieracz | `pl.gbcom.appfactory.kolekdobieracz` |
| `pies-trener-7dni` | Pies Trener 7 Dni | `pl.gbcom.appfactory.piestrener7dni` |
| `bajka-z-rysunku` | Bajka z rysunku | `pl.gbcom.appfactory.bajkazrysunku` |
| `vinted-olx-opis` | Opis Sprzedażowy | `pl.gbcom.appfactory.vintedolxopis` |
| `kot-bawi-sie` | Kot Bawi się | `pl.gbcom.appfactory.kotbawisie` |
| `barber-translator` | Barber Translator | `pl.gbcom.appfactory.barbertranslator` |
| `outfit-coach` | Outfit Coach | `pl.gbcom.appfactory.outfitcoach` |
| `domfix` | DomFix | `pl.gbcom.appfactory.domfix` |
| `fryzury-proste` | Fryzury Proste | `pl.gbcom.appfactory.fryzuryproste` |
| `rysunek-coach` | Rysunek Coach | `pl.gbcom.appfactory.rysunekcoach` |
| `bukietownik` | Bukietownik | `pl.gbcom.appfactory.bukietownik` |
| `pokoj-makeover` | Pokój Makeover | `pl.gbcom.appfactory.pokojmakeover` |
| `pakowanie-paczek` | Pakowanie Paczek | `pl.gbcom.appfactory.pakowaniepaczek` |
| `silikon-fuga-fix` | Silikon Fuga Fix | `pl.gbcom.appfactory.silikonfugafix` |
| `szydelko-pomocnik` | Szydełko Pomocnik | `pl.gbcom.appfactory.szydelkopomocnik` |
| `chleb-zakwas-coach` | Chleb Zakwas Coach | `pl.gbcom.appfactory.chlebzakwascoach` |
| `zmywarka-diagnosta` | Zmywarka Diagnosta | `pl.gbcom.appfactory.zmywarkadiagnosta` |
| `krawat-garnitur-coach` | Krawat Garnitur Coach | `pl.gbcom.appfactory.krawatgarniturcoach` |
| `router-wifi-diagnosta` | Router WiFi Diagnosta | `pl.gbcom.appfactory.routerwifidiagnosta` |

## Użycie

Docelowy build osobnej aplikacji powinien przekazywać `ProjectId`, `ApplicationTitle` i `ApplicationId` z powyższej tabeli do pipeline publikacyjnego.

Na tym etapie raport jest źródłem prawdy dla wariantów buildów i punktem startowym do konfiguracji flavorów / pipeline per aplikacja.
