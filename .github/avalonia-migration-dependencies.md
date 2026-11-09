# Mapa zależności UI i domeny przed migracją do Avalonia

## Cel dokumentu

Dokument rozdziela elementy obecnej aplikacji Windows Forms na:

- logikę domenową możliwą do współdzielenia;
- logikę aplikacyjną wymagającą refaktoryzacji;
- kod zależny od Windows Forms;
- kod zależny od GDI+/`System.Drawing`;
- kod, który powinien zostać zastąpiony przez Avalonia.

Analiza dotyczy projektu `Moje KS\KS2`, biblioteki `Moje KS\AvaloniaApp\Core` i prototypu `Moje KS\AvaloniaApp`.

## 1. Obecny układ projektów

### `KS2`

`Moje KS\KS2\Program Moje KS.csproj` jest aplikacją `net8.0-windows` z `UseWindowsForms=true`. Zawiera zarówno logikę domenową, jak i cały interfejs WinForms.

Punkt startowy:

```text
Program.Main()
	→ Application.Run(new OknoGlowne())
```

Projekt wykonuje również podpisywanie artefaktu przez `SignApp.ps1` po buildzie.

### `Core`

`Moje KS\AvaloniaApp\Core\Core.csproj` jest biblioteką `net8.0-windows`, ale nadal ma:

- `UseWindowsForms=true`;
- zależność `System.Drawing.Common`;
- pliki linkowane bezpośrednio z `KS2`.

Obecnie linkowane są:

- `Arkusz.cs`;
- `Dane.cs`;
- `Klawiatura.cs`;
- `ObrobkaGrafiki.cs`;
- `Parametry.cs`;
- `Nuta.cs`;
- `Rysowanie.cs`;
- `Symbol.cs`;
- `Zawartosc.cs`.

To nie jest jeszcze czysta warstwa domenowa. Część plików zawiera zależności UI lub renderowania.

### `AvaloniaApp`

`Moje KS\AvaloniaApp\AvaloniaApp.csproj` jest aplikacją `net8.0-windows` z referencją do `Core`. Zawiera obecnie prototypowe widoki:

- `MainWindow.axaml`;
- `OknoGlowne.axaml`;
- `StaffControl.cs`;
- `App.axaml`.

Projekt nie jest jeszcze dodany do `KS7.sln`.

## 2. Klasyfikacja plików

### A. Kod UI WinForms — nie przenosić bezpośrednio

Te pliki powinny zostać zastąpione widokami, ViewModelami, komendami lub serwisami Avalonia:

- `OknoGlowne.cs`;
- `OknoGlowne.Designer.cs`;
- `Menu.cs`;
- `Kontrolki.cs`;
- `Ograniczenia.cs`;
- `Puste.cs`;
- `Zdarzenia.cs`;
- `ZakresNutDialog.cs`;
- `ZakresNutDialog.Designer.cs`;
- `AppStyle.cs`;
- `Program.cs` — tylko część startowa i ograniczenia uruchomienia.

Główne zależności tych plików:

- `Form`;
- `Panel`;
- `Control`;
- `CheckBox`;
- `ComboBox`;
- `Button`;
- `Label`;
- `NumericUpDown`;
- `PaintEventArgs`;
- `KeyEventArgs`;
- `Clipboard`;
- `Application.Run`;
- `Controls.Add`;
- `ShowDialog`.

### B. Logika domenowa — kandydat do współdzielenia

Najlepszymi kandydatami do wspólnej biblioteki są klasy opisujące muzykę, ćwiczenia i dane:

- `Nuta`;
- `Tonacja`;
- `Symbol`;
- `Metrum`;
- `Interwal`;
- `ZawartoscEkranu` i klasy pochodne;
- `Element` oraz `GeneratorElementow`;
- `Ekran`;
- `Dane` — po usunięciu zależności od UI;
- `Parametry` — po zastąpieniu typów UI/GDI+ typami neutralnymi;
- `Arkusz` oraz struktury nutowe.

Te klasy powinny docelowo nie znać Avalonia ani Windows Forms.

### C. Logika aplikacyjna — wymaga wydzielenia

Następujące operacje są obecnie rozproszone między formularzem i stanem globalnym:

- przełączanie trybu ćwiczeń;
- resetowanie danych przez `Zeruj()`;
- walidacja `Parametry.OK()`;
- przechodzenie między ekranami;
- obsługa odpowiedzi;
- aktualizacja parametrów edytora;
- generowanie kolejnych ekranów;
- zarządzanie widocznością paneli.

Powinny zostać wydzielone do neutralnych serwisów lub ViewModeli, np.:

- `ExerciseSession` — aktywna sesja i bieżący ekran;
- `ExerciseMode` — typ i rodzaj ćwiczenia;
- `ExerciseNavigationService` — `Next`, `Previous`, odpowiedzi;
- `ExerciseSettings` — ustawienia ćwiczeń;
- `ExerciseValidationService` — walidacja;
- `EditorState` — stan edytora;
- `MenuState` — dostępność pozycji menu.

Nazwy są propozycją organizacyjną; przed implementacją należy sprawdzić, czy nie ma już odpowiedników w kodzie domenowym.

## 3. Zależności `System.Windows.Forms`

### Pliki z bezpośrednią zależnością

Wystąpienia `System.Windows.Forms` znaleziono w szczególności w:

- `Klawiatura.cs`;
- `Edytor.cs`;
- `Dane.cs`;
- `Kontrolki.cs`;
- `AppStyle.cs`;
- `Nuta.cs`;
- `Ograniczenia.cs`;
- `ObrobkaGrafiki.cs`;
- `OknoGlowne.cs`;
- `Parametry.cs`;
- `Program.cs`;
- `Puste.cs`;
- `Rysowanie.cs`;
- `Zdarzenia.cs`;
- `ZakresNutDialog.cs`.

### Zależności wymagające usunięcia lub abstrakcji

#### `Dane.cs`

- używa typów Windows Forms w zakresie obsługi UI lub wejścia;
- zawiera stan ekranów, generowanie zawartości i nawigację;
- powinien zostać rozdzielony na neutralny model sesji oraz adapter UI.

#### `Nuta.cs`

- zawiera zależności związane z prezentacją lub obsługą platformową;
- model nuty powinien pozostać niezależny od frameworka UI.

#### `Edytor.cs`

- korzysta z `KeyEventArgs`, `Control.ModifierKeys` oraz `OknoGlowne`;
- logika edytora jest domenowo cenna, ale obsługę klawiatury należy przepisać na neutralny model poleceń, np. `EditorKeyCommand`.

#### `Rysowanie.cs` i `Klawiatura.cs`

- przyjmują `Graphics`;
- rysują bezpośrednio przez GDI+;
- wymagają oddzielenia obliczeń geometrii od konkretnego backendu renderowania.

## 4. Zależności `System.Drawing` i GDI+

### Główne miejsca

- `Rysowanie.cs` — centralny renderer muzyczny oparty o `Graphics`, `Image`, `Bitmap`, pióra, czcionki i pędzle;
- `Klawiatura.cs` — renderer klawiatury;
- `ObrobkaGrafiki.cs` — konwersja panelu do bitmapy, kadrowanie i zmiana koloru pikseli;
- `OknoGlowne.cs` — bezpośrednie rysowanie komunikatów i używanie `Graphics`;
- `Kontrolki.cs` — obrazy kontrolek i konfiguracja renderowania;
- `Parametry.cs` — kolory i parametry graficzne;
- `Nuta.cs` oraz `Arkusz.cs` — potencjalne typy prezentacyjne lub kolorystyczne.

### Wniosek architektoniczny

Nie należy zamieniać mechanicznie `Graphics` na `DrawingContext` w całym kodzie. Należy rozdzielić:

```text
model muzyczny
	↓
obliczenia układu i geometrii
	↓
neutralne polecenia rysowania
	↓
renderer Avalonia oparty na DrawingContext
```

Proponowany neutralny kontrakt może obejmować polecenia typu:

- `DrawLine`;
- `DrawEllipse`;
- `DrawImage`;
- `DrawText`;
- `DrawRectangle`;
- transformacje i skalowanie.

Alternatywnie geometria może być obliczana przez domenę, a bezpośredni rendering przeniesiony do `StaffControl`/`MusicCanvas`.

## 5. Zależności obrazów i zasobów

Obecny kod używa:

- `Image`;
- `Bitmap`;
- `BackgroundImage`;
- `ImageLayout.Zoom`;
- `Obrazek.getImage(Color)`;
- zasobów PNG osadzonych w projekcie WinForms.

W Avalonia należy ustalić jeden model zasobów:

- pliki obrazów jako `AvaloniaResource`;
- `Bitmap` Avalonia dla ikon i symboli;
- konwersja `Obrazek` do neutralnego identyfikatora zasobu;
- kolory jako `Avalonia.Media.Color` dopiero w warstwie UI/renderera.

Nie należy przenosić `System.Drawing.Image` do ViewModeli.

## 6. Zależność od globalnego stanu

Najważniejsze globalne punkty stanu:

- `Parametry.Tryb` — bieżący tryb i rodzaj ćwiczenia;
- `Parametry.ZakresNut` — klucz i zakresy nut;
- `Parametry.Cwiczenia` — odpowiedzi i nazwy nut;
- `Parametry.LosoweNuty` — konfiguracja losowania nut;
- `Parametry.LosoweInterwaly` — konfiguracja losowania interwałów;
- `Parametry.Tonacje` — opcje prezentacji tonacji;
- `Parametry.Edytor` — ustawienia startowe edytora;
- `Parametry.GrafikaNut` — parametry wizualne;
- `Dane.ekrany` — lista ekranów;
- `Dane.nr_pokazywanego_ekranu` — bieżący ekran;
- `Dane.czy_odpowiedz` — stan wyświetlania odpowiedzi.

Globalny stan może być wykorzystany przejściowo, ale docelowy interfejs Avalonia powinien otrzymać kontrolowany model sesji i ViewModele z powiadomieniami o zmianach.

## 7. Proponowane granice projektów

### Warstwa domenowa

Powinna zawierać:

- modele nut, tonacji, interwałów i symboli;
- generator elementów;
- tworzenie zawartości ekranów;
- reguły walidacji parametrów;
- geometrię i układ muzyczny, jeśli nie zależy od frameworka UI.

Nie powinna zawierać:

- `System.Windows.Forms`;
- `System.Drawing.Graphics`;
- `Bitmap` WinForms;
- `Control`, `Form`, `Panel`;
- `Clipboard`;
- `KeyEventArgs`.

### Warstwa Avalonia

Powinna zawierać:

- `App.axaml` i konfigurację aplikacji;
- okna i widoki XAML;
- ViewModele i komendy;
- adapter wejścia klawiatury/myszy;
- renderer `DrawingContext`;
- zasoby i style Fluent;
- obsługę schowka Avalonia/platformy.

### Warstwa kompatybilności WinForms

Do czasu zakończenia migracji może pozostać:

- obecny formularz `OknoGlowne`;
- `ZakresNutDialog`;
- istniejący renderer GDI+;
- adaptery wywołujące wspólną logikę domenową.

## 8. Kolejność refaktoryzacji wynikająca z mapy

1. Usunąć zależność od `OknoGlowne` z kodu domenowego.
2. Wydzielić obsługę klawiatury z `Edytor` do neutralnych poleceń.
3. Wydzielić obliczenia układu muzycznego z `Rysowanie` i `Klawiatura`.
4. Zastąpić typy `System.Drawing.Color` w modelu neutralnym typem własnym lub prostą reprezentacją koloru.
5. Wydzielić operacje bitmapowe i schowka do adapterów UI.
6. Przenieść modele i generatory do czystego projektu współdzielonego.
7. Zbudować ViewModele Avalonia na wspólnych modelach.
8. Dopiero potem zastąpić renderer GDI+ rendererem Avalonia.

## 9. Ryzyka

- `Core` nie jest obecnie niezależny od Windows; samo przeniesienie plików do biblioteki nie wystarczy.
- `Rysowanie.cs` łączy obliczenia, model i rendering, więc migracja będzie wymagała rozdzielenia odpowiedzialności.
- `Edytor` bezpośrednio odwołuje się do formularza i zdarzeń WinForms.
- `ObrobkaGrafiki.PanelToBitmap` nie ma bezpośredniego odpowiednika między WinForms a Avalonia.
- Globalne statyczne parametry mogą utrudniać testowanie ViewModeli i uruchomienie kilku sesji.
- Projekt Avalonia ma obecnie TFM `net8.0-windows`, więc usunięcie WinForms z `Core` powinno być osobnym sprawdzonym etapem, a nie założeniem.

## Podsumowanie

Największą granicą migracji nie jest XAML, lecz obecne połączenie trzech odpowiedzialności w tych samych klasach:

```text
stan ćwiczeń + logika muzyczna + rendering/UI
```

Docelowo należy je rozdzielić na:

```text
model i logika domenowa
	+
serwisy sesji i walidacji
	+
ViewModele Avalonia
	+
renderer Avalonia
```
