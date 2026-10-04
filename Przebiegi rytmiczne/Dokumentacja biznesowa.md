# Dokumentacja biznesowa programu „Generuj serie”

## Cel programu

Program przygotowuje komplet materiałów do pracy z przebiegami rytmicznymi dla
wybranej serii i wskazanych klas. Dla każdego ucznia przydziela numery
przebiegów, tworzy plik z przydziałami, kopiuje odpowiadające im nagrania audio
oraz generuje dokument dla nauczyciela z zapisami nutowymi.

## Użytkownicy i rezultat

- **Nauczyciel** otrzymuje dokument Word dla każdej klasy, zawierający nazwiska
  uczniów oraz przypisane im zapisy nutowe.
- **Uczeń** otrzymuje folder ze swoimi nagraniami MP3. Pliki audio są nazywane
  według kolejności przebiegów w przydziale: `1.mp3`, `2.mp3` itd.
- **Osoba przygotowująca serię** otrzymuje tekstowe pliki przydziałów, raport
  statystyczny, log oraz kopie danych użytych i wygenerowanych podczas pracy.

## Dane wejściowe

Program odczytuje ustawienia z pliku `.Generuj serie.yml`. Konfiguracja
wskazuje:

- folder z seriami wejściowymi;
- folder, w którym mają powstać wyniki;
- folder z listami uczniów;
- klasy, dla których mają być przygotowane materiały.

W folderze wejściowym każda seria jest osobnym podfolderem. Użytkownik wybiera
jedną z dostępnych serii po jej nazwie.

Wybrana seria powinna zawierać:

1. Jeden dokument Word (`.docx`) z tabelami przebiegów. Numer przebiegu jest
   odczytywany z pierwszej kolumny tabeli, a odpowiadający mu zapis nutowy
   powinien znajdować się jako obraz w drugiej kolumnie.
2. Nagrania MP3 dla wszystkich przebiegów z dokumentu. Program rozpoznaje
   numery z nazw plików MP3, a także z plików ZIP zawierających MP3.

Dla każdej klasy konfiguracja wskazuje plik tekstowy z listą uczniów. Plik
zawiera nazwisko i imię każdego ucznia w osobnym wierszu; puste wiersze są
pomijane. Program akceptuje nazwy takie jak `5.txt`, `Klasa_5.txt` lub
`klasa_5.txt`.

## Przebieg procesu

1. Program wczytuje konfigurację i pokazuje dostępne foldery serii.
2. Użytkownik wybiera serię oraz potwierdza kontynuację, jeśli istnieją już
   wyniki dla tej serii.
3. Program odczytuje dokument Word, sprawdza numery i obrazy przebiegów, po
   czym pyta, ile numerów ma otrzymać każdy uczeń. Domyślna wartość to **4**
   (wybrana przez naciśnięcie Enter). Można podać inną dodatnią liczbę, nie
   większą niż liczba przebiegów w serii.
4. Program sprawdza, czy dostępne są nagrania MP3 do wszystkich przebiegów.
   Jeśli któregoś brakuje, przerywa przygotowanie materiałów i wyświetla listę
   brakujących numerów.
5. Dla każdej skonfigurowanej klasy program odczytuje listę uczniów i tworzy
   przydział.
6. Program kopiuje obrazy i nagrania do folderu nowej wersji, tworzy materiały
   dla uczniów i dokumenty dla nauczyciela, a następnie zapisuje kopie wejścia
   i wyników.
7. Na zakończenie wyświetla statystyki przydziałów.

## Zasady przydzielania

- Przydziały są deterministyczne: te same listy uczniów, numery przebiegów
  i wybrana liczba numerów dają ten sam wynik.
- Każdy uczeń otrzymuje wybraną liczbę **różnych** przebiegów.
- Algorytm rozkłada przydziały cyklicznie z całego zakresu numerów, starając
  się równomiernie rozdzielić zarówno łączną liczbę wystąpień numerów, jak
  i ich wystąpienia na poszczególnych pozycjach zestawu.
- Kolejność numerów w pliku przydziałów jest zachowana przy tworzeniu listy
  plików audio dla ucznia i dokumentu Word.

## Wyniki

Dla wybranej serii program tworzy folder wynikowy o nazwie tej serii. Każde
uruchomienie zapisuje materiały w kolejnej wersji (`Wersje/Wersja 1`,
`Wersje/Wersja 2` itd.). Wersja zawiera między innymi:

- `Przydzialy/przydzialy_klasa_<klasa>.txt` — nazwiska uczniów z przypisanymi
  numerami;
- `Rozbicie przebiegów` — obrazy zapisów nutowych wyodrębnione z dokumentu
  Word;
- `Rozbicie przebiegów audio` — skopiowane MP3 nazwane numerem przebiegu;
- `Input` — kopię wybranej serii wejściowej;
- `Output` — kopię przygotowanych materiałów wynikowych;
- `Log.txt` — zapis komunikatów programu.

W głównym folderze wynikowym znajdują się materiały do dalszego wykorzystania:

- `Odpowiedzi - tylko dla nauczyciela` — osobny dokument Word dla każdej klasy,
  z uczniami, numerami przebiegów i odpowiadającymi im zapisami nutowymi;
- `Dla uczniów - do skopiowania na chmurę` — foldery klas i uczniów z
  przypisanymi nagraniami MP3.

Raport końcowy podaje liczbę przebiegów, lokalizację materiałów uczniowskich,
liczbę uczniów w każdej klasie, częstotliwość użycia numerów oraz numery
powtarzające się na tych samych pozycjach u różnych uczniów.

## Warunki i ograniczenia

- W wybranej serii powinien znajdować się dokładnie jeden dokument Word.
- Dokument musi zawierać numerowany przebieg i obraz zapisu nutowego dla
  każdego przebiegu.
- Dla każdego przebiegu z dokumentu musi być dostępne nagranie MP3, również
  jeśli MP3 znajduje się wewnątrz archiwum ZIP.
- Dla każdej klasy z konfiguracji musi istnieć niepusta lista uczniów.
- Uczeń nie może otrzymać więcej różnych numerów niż liczba przebiegów
  dostępnych w serii.
- Jeśli folder wynikowy serii zawiera już dane, program pyta przed
  kontynuowaniem; nowa generacja jest zapisywana do kolejnego folderu wersji.
