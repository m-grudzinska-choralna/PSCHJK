# Inwentaryzacja interfejsu WinForms przed migracją do Avalonia

## Zakres

Inwentaryzacja obejmuje projekt `Moje KS\KS2`, istniejący prototyp `Moje KS\AvaloniaApp` oraz główne przepływy interfejsu aplikacji „Program Moje KS”. Dokument jest punktem odniesienia dla migracji UI do Avalonia i nie opisuje jeszcze docelowej implementacji.

## 1. Tryby dostępne z menu

Logika menu znajduje się w `Moje KS\KS2\Menu.cs`, a deklaracje kontrolek w `Moje KS\KS2\OknoGlowne.Designer.cs`.

### Rozpoznawanie

- rozpoznawanie nut;
- rozpoznawanie symboli;
- rozpoznawanie klawiszy;
- rozpoznawanie interwałów na pięciolinii;
- rozpoznawanie interwałów na klawiaturze.

### Materiały

- tonacje.

### Losowe

- losowe nuty;
- losowe interwały.

### Edytor

- edytor nut;
- edytor interwałów.

Po wybraniu pozycji menu ustawiane są `Parametry.Tryb.tryb` i `Parametry.Tryb.rodzaj_cwiczen`, a następnie wykonywana jest metoda `Zeruj()`.

## 2. Panele głównego okna

Główne okno jest zdefiniowane w `Moje KS\KS2\OknoGlowne.cs` oraz `OknoGlowne.Designer.cs`. Występują w nim następujące obszary UI:

- panel parametrów ćwiczenia;
- panel losowania nut;
- panel generowania interwałów;
- panel materiałów — tonacje;
- panel edytora;
- panel wyboru klucza wiolinowego/basowego;
- panel znaków chromatycznych;
- panel wartości rytmicznych;
- panel pauz;
- panel określania liczebności interwałów;
- panel zaznaczania interwałów;
- panel nawigacji rund;
- główny panel z renderowanym arkuszem nutowym;
- panel komunikatów i alertów.

Widoczność paneli zależy od kombinacji:

```text
Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen
```

Obsługiwane kombinacje obejmują między innymi:

- `rozpoznawanie_nuty`;
- `rozpoznawanie_symbole`;
- `rozpoznawanie_klawisze`;
- `rozpoznawanie_interwaly_pieciolinia`;
- `rozpoznawanie_interwaly_klawiatura`;
- `generowanie_nuty`;
- `generowanie_interwaly`;
- `materialy_tonacje`;
- `edytor_podstawowy`;
- `edytor_interwalowy`.

## 3. Dynamiczne kontrolki

Dynamiczne kontrolki są tworzone głównie w `Moje KS\KS2\Kontrolki.cs`.

Tworzone są między innymi:

- przyciski znaków chromatycznych;
- przyciski wartości rytmicznych;
- przyciski pauz;
- przyciski interwałów;
- kontrolki liczebności interwałów;
- listy tonacji;
- listy zakresów nut;
- listy kluczy;
- opcje edytora.

Obecnie kontrolki są dodawane przez `Controls.Add`, mają nadawane nazwy i są obsługiwane przez zdarzenia. W Avalonia należy zastąpić ten mechanizm kolekcjami ViewModeli, np. `ObservableCollection<T>`, oraz wiązaniami danych.

## 4. Nawigacja i obsługa klawiatury

Obsługa nawigacji znajduje się w `Moje KS\KS2\Zdarzenia.cs` i `OknoGlowne.cs`.

### Przyciski

- `Wstecz` wywołuje `EkranWstecz()`;
- `Dalej` wywołuje `EkranDalej()`;
- dla materiałów — tonacji zmieniana jest wybrana tonacja;
- dla pozostałych trybów zmieniany jest aktualny ekran w `Dane`.

### Skróty klawiaturowe

- `Space` — przejście do następnego ekranu;
- `Backspace` — przejście do poprzedniego ekranu;
- `F12` — odświeżenie widoku;
- `F1`/`F2` — zmiana odstępu między nutami;
- `F3`/`F4` — zmiana odstępu po kluczu;
- `+` — powiększenie grafiki;
- `-` — pomniejszenie grafiki.

Edytor przechwytuje dodatkowo własne zdarzenia klawiatury przez `Edytor.NacisniecieKlawisza`.

## 5. Dialog zakresu nut

Dialog znajduje się w:

- `Moje KS\KS2\ZakresNutDialog.cs`;
- `Moje KS\KS2\ZakresNutDialog.Designer.cs`;
- `Moje KS\KS2\ZakresNutDialog.resx`.

Obsługuje:

- wybór klucza;
- zakres nut wiolinowych;
- zakres nut basowych;
- znaki chromatyczne;
- wartości rytmiczne;
- pauzy;
- walidację zakresów;
- wymaganie zaznaczenia przynajmniej jednej opcji;
- przyciski `OK` i `Anuluj`.

Dialog powinien zostać zastąpiony osobnym widokiem Avalonia, np. `ZakresNutDialog.axaml`, z ViewModelem zawierającym stan formularza i walidację.

## 6. Renderowanie muzyczne

Renderowanie odbywa się głównie w `Moje KS\KS2\OknoGlowne.cs` i `Moje KS\KS2\Rysowanie.cs`.

Renderowane są:

- pięciolinia;
- nuty;
- symbole;
- klucze;
- podpisy nut;
- odpowiedzi do ćwiczeń;
- interwały;
- komunikat informacyjny.

Aktualnie wykorzystywane są API GDI+:

```csharp
System.Drawing.Graphics
System.Drawing.Pen
System.Drawing.Font
System.Drawing.SolidBrush
```

W Avalonia wymagany będzie osobny renderer oparty na `Avalonia.Media.DrawingContext`. Należy uwzględnić skalowanie, zmianę rozmiaru okna, przewijanie i różnice w pomiarze tekstu.

## 7. Odświeżanie stanu

Najważniejszy obecny przepływ działania wygląda następująco:

```text
zmiana ustawienia
	↓
aktualizacja Parametry
	↓
Parametry.OK()
	↓
Zeruj() lub OdswiezPanel()
	↓
UstawKontrolki()
	↓
renderowanie panelu
```

W Avalonia przepływ powinien zostać oparty na:

```text
zmiana właściwości ViewModelu
	↓
walidacja
	↓
aktualizacja stanu ćwiczenia
	↓
komenda lub obserwacja zmiany
	↓
odświeżenie renderera
```

## 8. Funkcje dodatkowe

- zwijanie i rozwijanie panelu parametrów przez `zwin_button`;
- pokazywanie odpowiedzi przez `odpowiedzi_checkBox`;
- kopiowanie wyrenderowanego obrazu do schowka przez `kopiuj_button`;
- automatyczne kopiowanie przez `Parametry.Ogolne.kopiowanie`;
- ograniczanie pozycji menu dla trybu `edycjaProgramu == "uczniowie"`;
- dynamiczne komunikaty walidacyjne dla kluczy, znaków chromatycznych, wartości rytmicznych i interwałów.

## 9. Obecny prototyp Avalonia

Prototyp znajduje się w `Moje KS\AvaloniaApp`.

Obecne elementy:

- `MainWindow.axaml` — uproszczone okno główne;
- `OknoGlowne.axaml` — częściowe odwzorowanie menu i paneli;
- `StaffControl.cs` — przykładowy renderer pięciolinii i jednej nuty;
- `App.axaml` — `FluentTheme`;
- `Core\Core.csproj` — współdzielona część logiki.

Prototyp nie odwzorowuje jeszcze pełnej funkcjonalności WinForms. Projekty `AvaloniaApp` i `Core` nie są obecnie wpisane do `KS7.sln`.

## 10. Wnioski dla migracji

1. Nie należy przenosić `OknoGlowne.cs` bezpośrednio do Avalonia.
2. Logikę domenową trzeba oddzielić od kontrolek WinForms.
3. Największe obszary migracji to:
   - renderer `System.Drawing`;
   - dynamiczne kontrolki;
   - edytor;
   - dialog zakresu nut;
   - globalny stan `Parametry` i `Dane`.
4. Główny widok powinien zostać podzielony na mniejsze widoki Avalonia i ViewModele.
5. Wersja WinForms powinna pozostać uruchamialna do porównań regresyjnych do czasu osiągnięcia parytetu funkcjonalnego.
6. Kolejnym krokiem jest mapowanie zależności UI i domeny oraz wskazanie kodu, który można przenieść bez zmian, a który wymaga abstrakcji renderowania lub wejścia użytkownika.
