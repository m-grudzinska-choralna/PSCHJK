import os
from collections import Counter
import yaml

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


def przydziel_numery_dla_klas(konfiguracja):
    liczba_przebiegow = konfiguracja.get("liczba_przebiegow", 20)

    rel_folder_klas = konfiguracja.get("folder_klas", "../Listy uczniow")
    folder_klas = os.path.abspath(
        os.path.join(KATALOG_SKRYPTU, rel_folder_klas)
    )

    rel_folder_wynikowy = konfiguracja.get("folder_wynikowy", "Wyniki")
    folder_wynikowy = os.path.abspath(
        os.path.join(KATALOG_SKRYPTU, rel_folder_wynikowy)
    )

    pliki_wynikowe_cfg = konfiguracja.get("pliki_wynikowe", {})
    szablon_przydzialow = pliki_wynikowe_cfg.get(
        "przydzialy_przebiegow_do_uczniow",
        "przydzialy_klasa_{klasa}.txt",
    )

    klasy = konfiguracja.get("klasy", [])
    if not klasy:
        print("Brak zdefiniowanych klas w pliku konfiguracyjnym (sekcja 'klasy').")
        return

    polowa = liczba_przebiegow // 2

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
        wzgledna_sciezka_wynikowa = szablon_przydzialow.replace("\\", "/").format(
            klasa=czysta_nazwa_klasy
        )
        sciezka_pliku_wynikowego = os.path.abspath(
            os.path.join(folder_wynikowy, wzgledna_sciezka_wynikowa)
        )

        os.makedirs(os.path.dirname(sciezka_pliku_wynikowego), exist_ok=True)

        with open(sciezka_pliku_wynikowego, "w", encoding="utf-8") as f:
            f.write("\n".join(zawartosc_pliku))

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


if __name__ == "__main__":
    config = wczytaj_konfiguracje(".przebiegi rytmiczne.yml")
    przydziel_numery_dla_klas(config)