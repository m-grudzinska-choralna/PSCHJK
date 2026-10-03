import os
import re
import shutil
import zipfile
from docx import Document
from docx.enum.table import WD_TABLE_ALIGNMENT
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.shared import Inches, Pt


def wyodrebnij_obrazy_przebiegow(path_input, path_przebiegi_img):
    """Wypakowuje obrazki przebiegów (1-20) w dokładnej kolejności z dokumentu DOCX."""
    os.makedirs(path_przebiegi_img, exist_ok=True)

    sciezka_docx = None
    for root, dirs, files in os.walk(path_input):
        for f in files:
            if f.endswith(".docx") and not f.startswith("~$"):
                sciezka_docx = os.path.join(root, f)
                break
        if sciezka_docx:
            break

    if not sciezka_docx:
        print(f"Brak plików .docx z przebiegami w folderze '{path_input}'!")
        return False

    print(f"Przetwarzanie pliku z obrazami przebiegów: {sciezka_docx}")

    doc = Document(sciezka_docx)
    r_ids = []

    # 1. Odczytujemy ID obrazów z całego dokumentu w prawidłowej kolejności
    for rel in doc.part.rels.values():
        if "image" in rel.target_ref:
            # Szukamy powiązania R_ID w pliku xml
            r_ids.append((rel.rId, rel.target_ref))

    # Pobieramy elementy graficzne z ciała dokumentu w fizycznej kolejności występowania
    xml_str = doc.part.element.xml
    image_rids_in_order = re.findall(r'r:embed="(rId\d+)"', xml_str)

    # Usuwamy ewentualne powtórzenia zachowując kolejność
    seen = set()
    unique_rids = []
    for rid in image_rids_in_order:
        if rid not in seen:
            seen.add(rid)
            unique_rids.append(rid)

    # Mapowanie rId -> wewnętrzna ścieżka w archiwum zip
    rid_to_path = {rel.rId: rel.target_ref for rel in doc.part.rels.values() if "image" in rel.target_ref}

    images_paths = []
    for rid in unique_rids:
        if rid in rid_to_path:
            target = rid_to_path[rid]
            # Przekształcenie ścieżki na wewnętrzną ścieżkę w pliku ZIP
            if not target.startswith("word/"):
                target = "word/" + target
            images_paths.append(target)

    if not images_paths:
        print("Nie znaleziono obrazów w dokumencie Word!")
        return False

    # Przycinamy do pierwszych 20 obrazów
    images_paths = images_paths[:20]

    with zipfile.ZipFile(sciezka_docx, "r") as z:
        for idx, img_path in enumerate(images_paths, start=1):
            ext = os.path.splitext(img_path)[1]
            out_filename = os.path.join(path_przebiegi_img, f"{idx}{ext}")

            with open(out_filename, "wb") as f_out:
                f_out.write(z.read(img_path))

            print(f"  [Nutowe] Przebieg {idx:02d} -> wyciągnięto obraz: {os.path.basename(img_path)}")

    print(f"Wyodrębniono i poprawnie przypisano {len(images_paths)} obrazów przebiegów.")
    return True


def przygotuj_pliki_audio(path_input, path_przebiegi_audio):
    """Przeszukuje INPUT w poszukiwaniu nagrań MP3 i zapisuje je w folderze tymczasowym pod nr przebiegów (01.mp3 - 20.mp3)."""
    os.makedirs(path_przebiegi_audio, exist_ok=True)
    audio_map = {}

    def rozpoznaj_numer(nazwa_pliku):
        nazwa_bez_ext = os.path.splitext(nazwa_pliku)[0]

        if nazwa_bez_ext.strip().isdigit():
            nr = int(nazwa_bez_ext.strip())
            if 1 <= nr <= 20:
                return nr

        m = re.search(
            r"(?:przebieg|nagranie|nr|zestaw)[\s_|-]*0*(\d{1,2})",
            nazwa_bez_ext,
            re.IGNORECASE,
        )
        if m:
            nr = int(m.group(1))
            if 1 <= nr <= 20:
                return nr

        numery = re.findall(r"\d+", nazwa_bez_ext)
        for num_str in numery:
            nr = int(num_str)
            if 1 <= nr <= 20:
                return nr

        return None

    print("\n--- Skanowanie plików audio MP3 ---")

    for root, dirs, files in os.walk(path_input):
        for file in files:
            ext = os.path.splitext(file)[1].lower()

            if ext == ".mp3":
                nr = rozpoznaj_numer(file)
                if nr and nr not in audio_map:
                    src_path = os.path.join(root, file)
                    dst_path = os.path.join(
                        path_przebiegi_audio, f"{nr:02d}.mp3"
                    )
                    shutil.copy(src_path, dst_path)
                    audio_map[nr] = dst_path
                    print(
                        f"  [Audio] Dopasowano Przebieg {nr:02d} -> Plik: '{file}'"
                    )

            elif ext == ".zip":
                zip_path = os.path.join(root, file)
                try:
                    with zipfile.ZipFile(zip_path, "r") as z:
                        for filename in z.namelist():
                            if filename.lower().endswith(".mp3"):
                                base_name = os.path.basename(filename)
                                if not base_name:
                                    continue

                                nr = rozpoznaj_numer(base_name)
                                if nr and nr not in audio_map:
                                    out_path = os.path.join(
                                        path_przebiegi_audio, f"{nr:02d}.mp3"
                                    )
                                    with open(out_path, "wb") as f_out:
                                        f_out.write(z.read(filename))
                                    audio_map[nr] = out_path
                                    print(
                                        f"  [Audio ZIP] Dopasowano Przebieg {nr:02d} -> Plik: '{base_name}'"
                                    )
                except Exception as e:
                    print(f"Błąd podczas odczytu ZIP {file}: {e}")

    print(
        f"Przygotowano łącznie {len(audio_map)}/20 unikalnych nagrań w 'Rozbicie przebiegów audio'.\n"
    )
    return audio_map


def przetworz_klasy(
    path_input, path_output, path_przebiegi_img, path_przebiegi_audio
):
    """Generuje plik .docx oraz foldery i podfoldery uczniów z nagraniami 1.mp3, 2.mp3 itd."""
    pliki_klas = []
    for root, dirs, files in os.walk(path_input):
        for f in files:
            if f.startswith("Klasa_") and f.endswith(".txt"):
                pliki_klas.append(os.path.join(root, f))

    if not pliki_klas:
        print(
            f"Brak plików klas (zaczynających się od 'Klasa_') w folderze '{path_input}'."
        )
        return

    obrazy_przebiegow = {}
    for plik in os.listdir(path_przebiegi_img):
        nazwa, ext = os.path.splitext(plik)
        if nazwa.isdigit():
            obrazy_przebiegow[int(nazwa)] = os.path.join(
                path_przebiegi_img, plik
            )

    nagrania_audio = {}
    if os.path.exists(path_przebiegi_audio):
        for plik in os.listdir(path_przebiegi_audio):
            nazwa, ext = os.path.splitext(plik)
            if nazwa.isdigit() and ext.lower() == ".mp3":
                nagrania_audio[int(nazwa)] = os.path.join(
                    path_przebiegi_audio, plik
                )

    for sciezka_txt in pliki_klas:
        plik_klasy = os.path.basename(sciezka_txt)
        nazwa_klasy = os.path.splitext(plik_klasy)[0]

        folder_klasy_out = os.path.join(path_output, nazwa_klasy)
        os.makedirs(folder_klasy_out, exist_ok=True)

        doc = Document()
        print(f"\n--- Przetwarzanie: {nazwa_klasy} ---")

        with open(sciezka_txt, "r", encoding="utf-8") as f:
            linie = [l.strip() for l in f if l.strip()]

        for i, linia in enumerate(linie):
            if ":" in linia:
                uczen_info, numery_str = linia.split(":", 1)
                nazwa_ucznia = uczen_info.strip()

                nazwa_ucznia_bez_nr = re.sub(
                    r"^\d+\.\s*", "", nazwa_ucznia
                ).strip()

                numery = [
                    int(n.strip())
                    for n in numery_str.split(",")
                    if n.strip().isdigit()
                ]

                # 1. Tworzenie folderu ucznia i podfolderu na nagrania
                folder_ucznia = os.path.join(folder_klasy_out, nazwa_ucznia)
                podfolder_audio = os.path.join(
                    folder_ucznia, nazwa_ucznia_bez_nr
                )
                os.makedirs(podfolder_audio, exist_ok=True)

                kopiowane_audio_count = 0
                for idx, nr in enumerate(numery, start=1):
                    if nr in nagrania_audio:
                        src_audio = nagrania_audio[nr]
                        nazwa_pliku_out = f"{idx}.mp3"

                        dst_audio = os.path.join(
                            podfolder_audio, nazwa_pliku_out
                        )
                        shutil.copy(src_audio, dst_audio)
                        kopiowane_audio_count += 1

                print(
                    f" Uczeń: {nazwa_ucznia} -> przekopiowano {kopiowane_audio_count}/{len(numery)} plików audio."
                )

                # 2. Generowanie karty w pliku .docx
                doc.add_heading(nazwa_ucznia, level=2)

                table = doc.add_table(rows=0, cols=2)
                table.alignment = WD_TABLE_ALIGNMENT.CENTER
                table.autofit = False

                for nr in numery:
                    row_cells = table.add_row().cells
                    row_cells[0].width = Inches(0.8)
                    row_cells[1].width = Inches(5.2)

                    p0 = row_cells[0].paragraphs[0]
                    p0.alignment = WD_ALIGN_PARAGRAPH.CENTER
                    run0 = p0.add_run(f"{nr:02d}.")
                    run0.font.bold = True
                    run0.font.size = Pt(14)

                    p1 = row_cells[1].paragraphs[0]
                    if nr in obrazy_przebiegow:
                        sciezka_obrazka = obrazy_przebiegow[nr]
                        p1.add_run().add_picture(
                            sciezka_obrazka, width=Inches(5.0)
                        )
                    else:
                        p1.add_run(f"[Brak obrazka dla nr {nr:02d}]")

                if i < len(linie) - 1:
                    doc.add_page_break()

        sciezka_doc_out = os.path.join(folder_klasy_out, f"{nazwa_klasy}.docx")
        doc.save(sciezka_doc_out)
        print(f"Pomyślnie wygenerowano komplet dla klasy: {nazwa_klasy}")


def glowna_funkcja():
    glowny_folder = "DANE"
    folder_input = "INPUT"
    folder_output = "OUTPUT"

    path_input = os.path.join(glowny_folder, folder_input)
    path_output = os.path.join(glowny_folder, folder_output)
    path_przebiegi_img = os.path.join(path_output, "Rozbicie przebiegów")
    path_przebiegi_audio = os.path.join(
        path_output, "Rozbicie przebiegów audio"
    )

    os.makedirs(path_input, exist_ok=True)
    os.makedirs(path_output, exist_ok=True)

    # 1. Wyodrębnij obrazy z .docx
    sukces = wyodrebnij_obrazy_przebiegow(path_input, path_przebiegi_img)

    # 2. Przygotuj pliki audio MP3
    przygotuj_pliki_audio(path_input, path_przebiegi_audio)

    # 3. Wygeneruj dokumenty oraz podfoldery klas i uczniów
    if sukces:
        przetworz_klasy(
            path_input, path_output, path_przebiegi_img, path_przebiegi_audio
        )


if __name__ == "__main__":
    glowna_funkcja()