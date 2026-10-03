import os
import re
import shutil
import zipfile
from collections import Counter
from docx import Document
from docx.enum.table import WD_TABLE_ALIGNMENT
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.shared import Inches, Pt
import yaml

FOLDER_PRZEBIEGOW_INPUT = "INPUT"
FOLDER_WYNIKOWY = "OUTPUT"
FOLDER_PLIKOW_POSREDNICH = "Pliki pośrednie"
SZABLON_PRZYDZIALOW = "Przydzialy/przydzialy_klasa_{klasa}.txt"

KATALOG_SKRYPTU = os.path.dirname(os.path.abspath(__file__))


def wczytaj_konfiguracje(nazwa_pliku=".przebiegi rytmiczne.yml"):
    sciezka_pliku = os.path.join(KATALOG_SKRYPTU, nazwa_pliku)

    if not os.path.exists(sciezka_pliku):
        raise FileNotFoundError(
            f"Nie znaleziono pliku konfiguracyjnego pod ścieżką: {sciezka_pliku}"
        )

    with open(sciezka_pliku, "r", encoding="utf-8") as file:
        return yaml.safe_load(file)


def wczytaj_uczniow_z_pliku(sciezka_pliku):
    with open(sciezka_pliku, "r", encoding="utf-8") as f:
        uczniowie = [line.strip() for line in f if line.strip()]
    return uczniowie


def generuj_zestaw_rownomierny(liczba_uczniow, min_val, max_val):
    """
    Deterministycznie i idealnie równomiernie przydziela numery z zakresu min_val..max_val.
    Gwarantuje, że różnica w liczbie wystąpień między jakimikolwiek dwoma numerami
    nie przekroczy 1.
    """
    zakres = list(range(min_val, max_val + 1))
    len_z = len(zakres)

    wymagane_numery = liczba_uczniow * 2
    sekwencja_numerow = [zakres[i % len_z] for i in range(wymagane_numery)]

    zestawy = []
    uzyte_pary = set()

    for i in range(liczba_uczniow):
        l1 = sekwencja_numerow[2 * i]
        l2 = sekwencja_numerow[2 * i + 1]

        if l1 == l2 or tuple(sorted((l1, l2))) in uzyte_pary:
            offset = 1
            while (l1 == l2 or tuple(sorted((l1, l2))) in uzyte_pary) and (
                2 * i + 1 + offset
            ) < len(sekwencja_numerow):
                idx_alt = 2 * i + 1 + offset
                sekwencja_numerow[2 * i + 1], sekwencja_numerow[idx_alt] = (
                    sekwencja_numerow[idx_alt],
                    sekwencja_numerow[2 * i + 1],
                )
                l2 = sekwencja_numerow[2 * i + 1]
                offset += 1

        para = tuple(sorted((l1, l2)))
        uzyte_pary.add(para)
        zestawy.append([l1, l2])

    return zestawy


def znajdz_plik_klasy(folder_klas, nazwa_klasy):
    """
    Szuka pliku z listą uczniów w folderze klas.
    Sprawdza kolejno: nazwa.txt, Klasa_nazwa.txt, klasa_nazwa.txt
    """
    mozliwe_nazwy = [
        f"{nazwa_klasy}.txt",
        f"Klasa_{nazwa_klasy}.txt",
        f"klasa_{nazwa_klasy}.txt",
    ]

    for nazwa_pliku in mozliwe_nazwy:
        sciezka = os.path.join(folder_klas, nazwa_pliku)
        if os.path.exists(sciezka):
            return sciezka

    return None


def przydziel_numery_dla_klas(konfiguracja, folder_wynikowy):
    liczba_przebiegow = konfiguracja["liczba_przebiegow"]

    rel_folder_klas = konfiguracja.get("folder_klas", "../Listy uczniow")
    folder_klas = os.path.abspath(
        os.path.join(KATALOG_SKRYPTU, rel_folder_klas)
    )

    folder_plikow_posrednich = os.path.join(
        folder_wynikowy, FOLDER_PLIKOW_POSREDNICH
    )

    klasy = konfiguracja.get("klasy", [])
    if not klasy:
        print("Brak zdefiniowanych klas w pliku konfiguracyjnym (sekcja 'klasy').")
        return []

    polowa = liczba_przebiegow // 2
    pliki_przydzialow = []

    for nazwa_klasy in klasy:
        nazwa_klasy_str = str(nazwa_klasy).strip()
        
        # Oczyszczenie nazwy klasy z przedrostka Klasa_ na potrzeby zmiennej {klasa}
        czysta_nazwa_klasy = nazwa_klasy_str.replace("Klasa_", "").replace("klasa_", "")

        sciezka_pliku_wejsciowego = znajdz_plik_klasy(folder_klas, czysta_nazwa_klasy)

        if not sciezka_pliku_wejsciowego:
            print(
                f"Ostrzeżenie: Nie znaleziono pliku z listą uczniów dla klasy '{czysta_nazwa_klasy}' w folderze: {folder_klas}"
            )
            continue

        uczniowie = wczytaj_uczniow_z_pliku(sciezka_pliku_wejsciowego)

        if not uczniowie:
            print(f"Plik dla klasy '{czysta_nazwa_klasy}' jest pusty. Pomijam.")
            continue

        n = len(uczniowie)

        zestawy_A = generuj_zestaw_rownomierny(n, 1, polowa)
        zestawy_B = generuj_zestaw_rownomierny(n, polowa + 1, liczba_przebiegow)

        zawartosc_pliku = []
        licznik_numerow = Counter()

        for idx, uczeń in enumerate(uczniowie, start=1):
            A = zestawy_A[idx - 1]
            B = zestawy_B[idx - 1]

            if idx % 2 != 0:
                wynikowe = [A[0], B[0], A[1], B[1]]
            else:
                wynikowe = [B[0], A[0], B[1], A[1]]

            licznik_numerow.update(wynikowe)

            sformatowane_liczby = [f"{num:02d}" for num in wynikowe]
            ciag_liczb = ", ".join(sformatowane_liczby)

            uczen_format = f"{uczeń}:".ljust(35)
            linia = f"{uczen_format}{ciag_liczb}"
            zawartosc_pliku.append(linia)

        # Tworzenie ścieżki docelowej z podstawieniem czystej nazwy klasy (np. "5")
        wzgledna_sciezka_wynikowa = SZABLON_PRZYDZIALOW.replace("\\", "/").format(
            klasa=czysta_nazwa_klasy
        )
        sciezka_pliku_wynikowego = os.path.abspath(
            os.path.join(folder_plikow_posrednich, wzgledna_sciezka_wynikowa)
        )

        os.makedirs(os.path.dirname(sciezka_pliku_wynikowego), exist_ok=True)

        with open(sciezka_pliku_wynikowego, "w", encoding="utf-8") as f:
            f.write("\n".join(zawartosc_pliku))
        pliki_przydzialow.append(
            (sciezka_pliku_wynikowego, f"Klasa_{czysta_nazwa_klasy}")
        )

        # Raport w konsoli
        print(
            f"\nPrzetworzono klasę '{czysta_nazwa_klasy}' ({n} uczniów) -> Zapisano w '{sciezka_pliku_wynikowego}'"
        )
        print("=" * 50)
        print(f"RAPORT CZĘSTOTLIWOŚCI NUMERÓW DLA KLASY: {czysta_nazwa_klasy}")
        print("=" * 50)
        for num in range(1, liczba_przebiegow + 1):
            ilosc = licznik_numerow.get(num, 0)
            print(f"Numer {num:02d}: {ilosc} os.")
        print("-" * 50)

    return pliki_przydzialow


def znajdz_plik_word(path_input):
    """Zwraca jedyny plik DOCX w serii i zgłasza błąd dla kilku plików."""
    pliki_docx = []
    for root, dirs, files in os.walk(path_input):
        for nazwa_pliku in files:
            if nazwa_pliku.lower().endswith(".docx") and not nazwa_pliku.startswith("~$"):
                pliki_docx.append(os.path.join(root, nazwa_pliku))

    if len(pliki_docx) >= 2:
        lista_plikow = "\n".join(pliki_docx)
        raise ValueError(
            f"Błąd: znaleziono {len(pliki_docx)} pliki Word (.docx) w serii "
            f"'{path_input}'. Maksymalna liczba to 1:\n{lista_plikow}"
        )

    if not pliki_docx:
        raise FileNotFoundError(
            f"Nie znaleziono pliku Word (.docx) w serii: {path_input}"
        )

    return pliki_docx[0]


def pobierz_sciezki_obrazow_word(sciezka_docx):
    """Zwraca unikalne obrazy osadzone w dokumencie Word, w kolejności użycia."""
    doc = Document(sciezka_docx)
    xml_str = doc.part.element.xml
    image_rids_in_order = re.findall(r'r:embed="(rId\d+)"', xml_str)

    unique_rids = list(dict.fromkeys(image_rids_in_order))
    rid_to_path = {
        rel.rId: rel.target_ref
        for rel in doc.part.rels.values()
        if "image" in rel.target_ref
    }
    images_paths = []
    for rid in unique_rids:
        if rid in rid_to_path:
            target = rid_to_path[rid]
            if not target.startswith("word/"):
                target = "word/" + target
            images_paths.append(target)

    if not images_paths:
        raise ValueError(
            f"Nie znaleziono obrazów przebiegów w dokumencie: {sciezka_docx}"
        )

    return images_paths


def wyodrebnij_obrazy_przebiegow(sciezka_docx, path_przebiegi_img, images_paths):
    """Wypakowuje przekazane obrazy przebiegów z dokumentu Word."""
    os.makedirs(path_przebiegi_img, exist_ok=True)
    print(f"Przetwarzanie pliku z obrazami przebiegów: {sciezka_docx}")
    with zipfile.ZipFile(sciezka_docx, "r") as archive:
        for idx, img_path in enumerate(images_paths, start=1):
            ext = os.path.splitext(img_path)[1]
            out_filename = os.path.join(path_przebiegi_img, f"{idx}{ext}")
            with open(out_filename, "wb") as output_file:
                output_file.write(archive.read(img_path))
            print(
                f"  [Nutowe] Przebieg {idx:02d} -> wyciągnięto obraz: "
                f"{os.path.basename(img_path)}"
            )

    print(f"Wyodrębniono {len(images_paths)} obrazów przebiegów.")
    return len(images_paths)


def przygotuj_pliki_audio(path_input, path_przebiegi_audio, liczba_przebiegow):
    """Kopiuje nagrania MP3 z folderu wejściowego, także z archiwów ZIP."""
    os.makedirs(path_przebiegi_audio, exist_ok=True)
    audio_map = {}

    def rozpoznaj_numer(nazwa_pliku):
        nazwa_bez_ext = os.path.splitext(nazwa_pliku)[0]
        if nazwa_bez_ext.strip().isdigit():
            numer = int(nazwa_bez_ext.strip())
            if 1 <= numer <= liczba_przebiegow:
                return numer

        dopasowanie = re.search(
            r"(?:przebieg|nagranie|nr|zestaw)[\s_|-]*0*(\d{1,2})",
            nazwa_bez_ext,
            re.IGNORECASE,
        )
        if dopasowanie:
            numer = int(dopasowanie.group(1))
            if 1 <= numer <= liczba_przebiegow:
                return numer

        for numer_str in re.findall(r"\d+", nazwa_bez_ext):
            numer = int(numer_str)
            if 1 <= numer <= liczba_przebiegow:
                return numer
        return None

    print("\n--- Skanowanie plików audio MP3 ---")
    for root, dirs, files in os.walk(path_input):
        for nazwa_pliku in files:
            ext = os.path.splitext(nazwa_pliku)[1].lower()
            sciezka = os.path.join(root, nazwa_pliku)
            if ext == ".mp3":
                numer = rozpoznaj_numer(nazwa_pliku)
                if numer and numer not in audio_map:
                    sciezka_docelowa = os.path.join(
                        path_przebiegi_audio, f"{numer:02d}.mp3"
                    )
                    shutil.copy(sciezka, sciezka_docelowa)
                    audio_map[numer] = sciezka_docelowa
                    print(f"  [Audio] Przebieg {numer:02d} -> '{nazwa_pliku}'")
            elif ext == ".zip":
                try:
                    with zipfile.ZipFile(sciezka, "r") as archive:
                        for member in archive.namelist():
                            if not member.lower().endswith(".mp3"):
                                continue
                            nazwa_audio = os.path.basename(member)
                            numer = rozpoznaj_numer(nazwa_audio)
                            if numer and numer not in audio_map:
                                sciezka_docelowa = os.path.join(
                                    path_przebiegi_audio, f"{numer:02d}.mp3"
                                )
                                with open(sciezka_docelowa, "wb") as output_file:
                                    output_file.write(archive.read(member))
                                audio_map[numer] = sciezka_docelowa
                                print(
                                    f"  [Audio ZIP] Przebieg {numer:02d} "
                                    f"-> '{nazwa_audio}'"
                                )
                except Exception as error:
                    print(f"Błąd podczas odczytu ZIP {nazwa_pliku}: {error}")

    print(
        f"Przygotowano {len(audio_map)}/{liczba_przebiegow} nagrań audio.\n"
    )
    return audio_map


def przetworz_klasy(
    pliki_klas, path_output, path_przebiegi_img, path_przebiegi_audio
):
    """Tworzy dokumenty klas oraz foldery uczniów z przypisanymi nagraniami."""
    if not pliki_klas:
        print("Brak wygenerowanych plików przydziałów do przetworzenia.")
        return

    obrazy_przebiegow = {}
    for nazwa_pliku in os.listdir(path_przebiegi_img):
        nazwa, ext = os.path.splitext(nazwa_pliku)
        if nazwa.isdigit():
            obrazy_przebiegow[int(nazwa)] = os.path.join(
                path_przebiegi_img, nazwa_pliku
            )

    nagrania_audio = {}
    if os.path.exists(path_przebiegi_audio):
        for nazwa_pliku in os.listdir(path_przebiegi_audio):
            nazwa, ext = os.path.splitext(nazwa_pliku)
            if nazwa.isdigit() and ext.lower() == ".mp3":
                nagrania_audio[int(nazwa)] = os.path.join(
                    path_przebiegi_audio, nazwa_pliku
                )

    for sciezka_txt, nazwa_klasy in pliki_klas:
        folder_klasy_out = os.path.join(path_output, nazwa_klasy)
        os.makedirs(folder_klasy_out, exist_ok=True)
        doc = Document()
        print(f"\n--- Przetwarzanie: {nazwa_klasy} ---")

        with open(sciezka_txt, "r", encoding="utf-8") as file:
            linie = [line.strip() for line in file if line.strip()]

        for idx, linia in enumerate(linie):
            if ":" not in linia:
                continue
            uczen_info, numery_str = linia.split(":", 1)
            nazwa_ucznia = uczen_info.strip()
            nazwa_ucznia_bez_nr = re.sub(r"^\d+\.\s*", "", nazwa_ucznia).strip()
            numery = [
                int(value.strip())
                for value in numery_str.split(",")
                if value.strip().isdigit()
            ]

            folder_ucznia = os.path.join(folder_klasy_out, nazwa_ucznia)
            podfolder_audio = os.path.join(folder_ucznia, nazwa_ucznia_bez_nr)
            os.makedirs(podfolder_audio, exist_ok=True)
            kopiowane_audio_count = 0
            for numer_porzadkowy, numer_przebiegu in enumerate(numery, start=1):
                if numer_przebiegu in nagrania_audio:
                    shutil.copy(
                        nagrania_audio[numer_przebiegu],
                        os.path.join(podfolder_audio, f"{numer_porzadkowy}.mp3"),
                    )
                    kopiowane_audio_count += 1
            print(
                f" Uczeń: {nazwa_ucznia} -> przekopiowano "
                f"{kopiowane_audio_count}/{len(numery)} plików audio."
            )

            doc.add_heading(nazwa_ucznia, level=2)
            table = doc.add_table(rows=0, cols=2)
            table.alignment = WD_TABLE_ALIGNMENT.CENTER
            table.autofit = False
            for numer_przebiegu in numery:
                row_cells = table.add_row().cells
                row_cells[0].width = Inches(0.8)
                row_cells[1].width = Inches(5.2)
                paragraph_number = row_cells[0].paragraphs[0]
                paragraph_number.alignment = WD_ALIGN_PARAGRAPH.CENTER
                number_run = paragraph_number.add_run(f"{numer_przebiegu:02d}.")
                number_run.font.bold = True
                number_run.font.size = Pt(14)
                paragraph_content = row_cells[1].paragraphs[0]
                if numer_przebiegu in obrazy_przebiegow:
                    paragraph_content.add_run().add_picture(
                        obrazy_przebiegow[numer_przebiegu], width=Inches(5.0)
                    )
                else:
                    paragraph_content.add_run(
                        f"[Brak obrazka dla nr {numer_przebiegu:02d}]"
                    )

            if idx < len(linie) - 1:
                doc.add_page_break()

        sciezka_doc_out = os.path.join(folder_klasy_out, f"{nazwa_klasy}.docx")
        doc.save(sciezka_doc_out)
        print(f"Pomyślnie wygenerowano komplet dla klasy: {nazwa_klasy}")


def wybierz_serie(path_input):
    """Pyta o serię i zwraca ścieżkę do wybranego podfolderu."""
    serie = [
        nazwa
        for nazwa in os.listdir(path_input)
        if os.path.isdir(os.path.join(path_input, nazwa))
    ]
    serie.sort(
        key=lambda nazwa: (0, int(nazwa))
        if nazwa.isdecimal()
        else (1, nazwa.casefold())
    )

    if not serie:
        raise FileNotFoundError(
            f"Nie znaleziono folderów serii w katalogu: {path_input}"
        )

    print(f"Dostępne serie: {', '.join(serie)}")
    while True:
        wybor = input("Którą serię przetworzyć? Podaj jej nazwę: ").strip()
        if wybor in serie:
            return os.path.join(path_input, wybor)
        print(f"Nieprawidłowa nazwa serii. Wybierz jedną z: {', '.join(serie)}")


def przygotuj_materialy(
    konfiguracja, pliki_klas, path_input, path_output, sciezka_docx, images_paths
):
    """Uruchamia etap obrazów, audio i dokumentów po wygenerowaniu przydziałów."""
    liczba_przebiegow = konfiguracja["liczba_przebiegow"]
    folder_plikow_posrednich = os.path.join(
        path_output, FOLDER_PLIKOW_POSREDNICH
    )
    path_przebiegi_img = os.path.join(
        folder_plikow_posrednich, "Rozbicie przebiegów"
    )
    path_przebiegi_audio = os.path.join(
        folder_plikow_posrednich, "Rozbicie przebiegów audio"
    )

    if not os.path.isdir(path_input):
        raise FileNotFoundError(
            f"Nie znaleziono folderu wybranej serii: {path_input}"
        )
    os.makedirs(path_output, exist_ok=True)

    sukces = wyodrebnij_obrazy_przebiegow(
        sciezka_docx, path_przebiegi_img, images_paths
    )
    przygotuj_pliki_audio(
        path_input, path_przebiegi_audio, liczba_przebiegow
    )
    if sukces:
        przetworz_klasy(
            pliki_klas, path_output, path_przebiegi_img, path_przebiegi_audio
        )


def glowna_funkcja():
    konfiguracja = wczytaj_konfiguracje(".przebiegi rytmiczne.yml")
    folder_input = os.path.abspath(
        os.path.join(
            KATALOG_SKRYPTU, FOLDER_PRZEBIEGOW_INPUT
        )
    )
    if not os.path.isdir(folder_input):
        raise FileNotFoundError(f"Nie znaleziono folderu wejściowego: {folder_input}")

    path_input = wybierz_serie(folder_input)
    nazwa_serii = os.path.basename(path_input)
    path_output = os.path.abspath(
        os.path.join(KATALOG_SKRYPTU, FOLDER_WYNIKOWY, nazwa_serii)
    )

    sciezka_docx = znajdz_plik_word(path_input)
    images_paths = pobierz_sciezki_obrazow_word(sciezka_docx)
    konfiguracja["liczba_przebiegow"] = len(images_paths)
    print(
        f"Liczba przebiegów odczytana z dokumentu Word: "
        f"{konfiguracja['liczba_przebiegow']}"
    )

    wygenerowane_przydzialy = przydziel_numery_dla_klas(
        konfiguracja, path_output
    )
    przygotuj_materialy(
        konfiguracja,
        wygenerowane_przydzialy,
        path_input,
        path_output,
        sciezka_docx,
        images_paths,
    )


if __name__ == "__main__":
    glowna_funkcja()