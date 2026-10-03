import glob
import os
import random
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


def generuj_zestaw_unikalny(
    liczba_uczniow, min_val, max_val, wykorzystane_pary
):
    zakres = list(range(min_val, max_val + 1))

    for proby_calosci in range(100):
        lokalne_wykorzystane = set(wykorzystane_pary)
        zestawy = []
        licznik = Counter({num: 0 for num in zakres})
        sukces = True

        for _ in range(liczba_uczniow):
            znaleziono = False
            podejscia = 0

            while not znaleziono and podejscia < 200:
                podejscia += 1
                najrzadsze = sorted(
                    zakres, key=lambda x: (licznik[x], random.random())
                )
                l1, l2 = najrzadsze[0], najrzadsze[1]
                para = tuple(sorted((l1, l2)))

                if para not in lokalne_wykorzystane and l1 != l2:
                    lokalne_wykorzystane.add(para)
                    licznik[l1] += 1
                    licznik[l2] += 1
                    zestawy.append([l1, l2])
                    znaleziono = True

            if not znaleziono:
                sukces = False
                break

        if sukces:
            wykorzystane_pary.update(lokalne_wykorzystane)
            return zestawy

    raise ValueError(
        f"Nie można wygenerować unikalnej pary z zakresu {min_val}-{max_val}."
    )


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

    os.makedirs(folder_wynikowy, exist_ok=True)

    wzorzec_szukania = os.path.join(folder_klas, "*.txt")
    sciezki_plikow = glob.glob(wzorzec_szukania)

    if not sciezki_plikow:
        print(f"Nie znaleziono żadnych plików .txt w folderze: {folder_klas}")
        return

    polowa = liczba_przebiegow // 2

    for sciezka_pliku_wejsciowego in sciezki_plikow:
        nazwa_pliku_bez_ext = os.path.splitext(
            os.path.basename(sciezka_pliku_wejsciowego)
        )[0]

        uczniowie = wczytaj_uczniow_z_pliku(sciezka_pliku_wejsciowego)

        if not uczniowie:
            print(f"Plik '{nazwa_pliku_wejsciowego}' jest pusty. Pomijam.")
            continue

        n = len(uczniowie)

        wykorzystane_pary_A = set()
        wykorzystane_pary_B = set()

        zestawy_A = generuj_zestaw_unikalny(
            n, 1, polowa, wykorzystane_pary_A
        )
        zestawy_B = generuj_zestaw_unikalny(
            n, polowa + 1, liczba_przebiegow, wykorzystane_pary_B
        )

        zawartosc_pliku = []

        for idx, uczeń in enumerate(uczniowie, start=1):
            A = zestawy_A[idx - 1]
            B = zestawy_B[idx - 1]

            if idx % 2 != 0:
                wynikowe = [A[0], B[0], A[1], B[1]]
            else:
                wynikowe = [B[0], A[0], B[1], A[1]]

            sformatowane_liczby = [f"{num:02d}" for num in wynikowe]
            ciag_liczb = ", ".join(sformatowane_liczby)

            # Wyrównanie imienia i nazwiska do szerokości 50 znaków z dwukropkiem
            uczen_format = f"{uczeń}:".ljust(35)
            linia = f"{uczen_format}{ciag_liczb}"
            zawartosc_pliku.append(linia)

        nazwa_pliku_wyjsciowego = f"przydzialy_{nazwa_pliku_bez_ext}.txt"
        sciezka_pliku_wynikowego = os.path.join(
            folder_wynikowy, nazwa_pliku_wyjsciowego
        )

        with open(sciezka_pliku_wynikowego, "w", encoding="utf-8") as f:
            f.write("\n".join(zawartosc_pliku))

        print(
            f"Przetworzono '{nazwa_pliku_bez_ext}' ({n} uczniów) -> Zapisano w '{sciezka_pliku_wynikowego}'"
        )


if __name__ == "__main__":
    config = wczytaj_konfiguracje(".przebiegi rytmiczne.yml")
    przydziel_numery_dla_klas(config)