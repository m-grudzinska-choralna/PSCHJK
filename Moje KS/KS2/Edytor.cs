using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KS2
{
    public class SystemNutowyDefinicja
    {
        public string klucz;
        public Tonacja tonacja;
        public List<ObiektNutowy> lista_obiektow_nutowych;
        

        public SystemNutowyDefinicja(string p_klucz,Tonacja p_tonacja)
        {
            tonacja = p_tonacja;
            lista_obiektow_nutowych = new List<ObiektNutowy>();
            klucz = p_klucz;
        }
    }
    public class Edytor : ZawartoscEkranu
    {
        public List<SystemNutowyDefinicja> lista_definicji_systemow_nutowych = null;
        public Metrum metrum;
        public int aktualny_system_nutowy = -1;
        public int aktualna_nuta_indeks = -1;
        public int wiol_od;
        public int wiol_do;
        public int bas_od;
        public int bas_do;

        public bool pokaz_nute ;
        public bool pokaz_podpis ;
        public bool pokaz_pierwsza_nute ;
        public bool pokaz_druga_nute ;

        public bool pokaz_klawiature ;
        public bool pokaz_pieciolinie ;
        public string podpisy_klawiszy_rodzaj;

        public Edytor(string p_klucz, Tonacja p_tonacja, Metrum p_metrum, int p_wiol_od, int p_wiol_do, int p_bas_od, int p_bas_do,
                      bool p_pokaz_nute, bool p_pokaz_podpis, bool p_pokaz_pierwsza_nute, bool p_pokaz_druga_nute, bool p_pokaz_klawiature, bool p_pokaz_pieciolinie,
                      string p_podpisy_klawiszy)
        {
            lista_definicji_systemow_nutowych = new List<SystemNutowyDefinicja>();
            SystemNutowyDefinicja systemNutowyDefinicja = new SystemNutowyDefinicja(p_klucz, p_tonacja);
            lista_definicji_systemow_nutowych.Add(systemNutowyDefinicja);
            aktualny_system_nutowy = 0;
            metrum = p_metrum;
            wiol_od = p_wiol_od;
            wiol_do = p_wiol_do;
            bas_od = p_bas_od;
            bas_do = p_bas_do;
            pokaz_nute = p_pokaz_nute;
            pokaz_podpis = p_pokaz_podpis;
            pokaz_pierwsza_nute = p_pokaz_pierwsza_nute;
            pokaz_druga_nute = p_pokaz_druga_nute;
            pokaz_klawiature = p_pokaz_klawiature;
            pokaz_pieciolinie = p_pokaz_pieciolinie;
            podpisy_klawiszy_rodzaj = p_podpisy_klawiszy;
            UstawKlucz(p_klucz);
        }

        public SystemNutowyDefinicja aktualnaDefinicjaSystemuNutowego()
        {
            return lista_definicji_systemow_nutowych[aktualny_system_nutowy];
        }

        public void NacisniecieKlawisza(OknoGlowne okno_glowne, object sender, KeyEventArgs e)
        {
            SystemNutowyDefinicja systemNutowyDefinicja = lista_definicji_systemow_nutowych[aktualny_system_nutowy];

            Nuta aktualnaNuta = null;
            if (aktualna_nuta_indeks != -1)
            {
                aktualnaNuta = (Nuta)(systemNutowyDefinicja.lista_obiektow_nutowych[aktualna_nuta_indeks]);
            }
            if (!(Control.ModifierKeys == Keys.Shift))
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                    case Keys.D:
                    case Keys.E:
                    case Keys.F:
                    case Keys.G:
                    case Keys.A:
                    case Keys.H:
                        string nazwa_literowa = e.KeyCode.ToString().ToLower();
                        string oktawa = "";

                        if (aktualnaNuta == null)
                        {
                            Nuta nuta2 = new Nuta(systemNutowyDefinicja.klucz == "wiol" ? (wiol_od + wiol_do) / 2 : (bas_od + bas_do) / 2);
                            oktawa = nuta2.oktawa.nazwa;
                        }
                        else
                        {
                            oktawa = aktualnaNuta.oktawa.nazwa;
                        }
                        Nuta nowaNuta = new Nuta(nazwa_literowa, oktawa);

                        if (nowaNuta.nr < (systemNutowyDefinicja.klucz == "wiol" ? wiol_od : bas_od))
                            nowaNuta = new Nuta(nowaNuta.nr + 7, nowaNuta.wartosc_rytmiczna.nazwa, nowaNuta.znak_chromatyczny);

                        if (nowaNuta.nr > (systemNutowyDefinicja.klucz == "wiol" ? wiol_do : bas_do))
                            nowaNuta = new Nuta(nowaNuta.nr - 7, nowaNuta.wartosc_rytmiczna.nazwa, nowaNuta.znak_chromatyczny);

                        systemNutowyDefinicja.lista_obiektow_nutowych.Add(nowaNuta);
                        aktualna_nuta_indeks = systemNutowyDefinicja.lista_obiektow_nutowych.Count - 1;
                        okno_glowne.OdswiezPanel();
                        break;

                    case Keys.Up:
                    case Keys.Down:
                        if (aktualnaNuta != null)
                        {
                            int aktualnaNutaNowyNr = aktualnaNuta.nr + (e.KeyCode == Keys.Up ? 1 : -1);
                            if ((aktualnaNutaNowyNr >= (systemNutowyDefinicja.klucz == "wiol" ? Parametry.Konfiguracja.wiolinowy_min : Parametry.Konfiguracja.basowy_min)
                                &&
                                (aktualnaNutaNowyNr <= (systemNutowyDefinicja.klucz == "wiol" ? Parametry.Konfiguracja.wiolinowy_max : Parametry.Konfiguracja.basowy_max))))

                                systemNutowyDefinicja.lista_obiektow_nutowych[aktualna_nuta_indeks] =
                                    new Nuta(aktualnaNutaNowyNr,
                                             aktualnaNuta.wartosc_rytmiczna.nazwa,
                                             aktualnaNuta.znak_chromatyczny
                                             );

                        };
                        okno_glowne.OdswiezPanel();
                        break;
                    case Keys.Back:
                        if (aktualnaNuta != null)
                        {
                            systemNutowyDefinicja.lista_obiektow_nutowych.RemoveAt(aktualna_nuta_indeks);
                            aktualna_nuta_indeks--;
                            okno_glowne.OdswiezPanel();
                        }
                        break;


                }
            }

            if ((Control.ModifierKeys == Keys.Shift))
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                    case Keys.Down:
                        if (aktualnaNuta != null)
                        {
                            string nowyZnakChromatyczny = "";
                            if (e.KeyCode == Keys.Up)
                                switch (aktualnaNuta.znak_chromatyczny)
                                {
                                    case ("podwojny_bemol"):
                                        nowyZnakChromatyczny = "bemol";
                                        break;
                                    case ("bemol"):
                                        nowyZnakChromatyczny = "brak";
                                        break;
                                    case ("brak"):
                                        nowyZnakChromatyczny = "krzyzyk";
                                        break;
                                    case ("krzyzyk"):
                                        nowyZnakChromatyczny = "podwojny_krzyzyk";
                                        break;
                                }
                            if (e.KeyCode == Keys.Down)
                                switch (aktualnaNuta.znak_chromatyczny)
                                {
                                    case ("bemol"):
                                        nowyZnakChromatyczny = "podwojny_bemol";
                                        break;
                                    case ("brak"):
                                        nowyZnakChromatyczny = "bemol";
                                        break;
                                    case ("krzyzyk"):
                                        nowyZnakChromatyczny = "brak";
                                        break;
                                    case ("podwojny_krzyzyk"):
                                        nowyZnakChromatyczny = "krzyzyk";
                                        break;
                                }

                            if (nowyZnakChromatyczny != "")
                                systemNutowyDefinicja.lista_obiektow_nutowych[aktualna_nuta_indeks] =
                                       new Nuta(aktualnaNuta.nr,
                                                aktualnaNuta.wartosc_rytmiczna.nazwa,
                                                nowyZnakChromatyczny
                                                );
                            okno_glowne.OdswiezPanel();
                        }
                        break;
                }
            }
        }

        public void UstawKlucz(string p_klucz)
        {
            aktualnaDefinicjaSystemuNutowego().klucz = p_klucz;
        }

        public void UstawMetrum(string p_metrum)
        {
            metrum = new Metrum(p_metrum);
        }

        public void UstawTonacja(Tonacja p_tonacja)
        {

            aktualnaDefinicjaSystemuNutowego().tonacja = p_tonacja;
        }

        public void UstawPokazNute(bool p_pokaz_nute)
        {

            pokaz_nute = p_pokaz_nute;
        }


        public void UstawPokazPierwszaNute(bool p_pokaz_pierwsza_nute)
        {

            pokaz_pierwsza_nute = p_pokaz_pierwsza_nute;
        }


        public void UstawPokazDrugaNute(bool p_pokaz_druga_nute)
        {

            pokaz_druga_nute = p_pokaz_druga_nute;
        }

        public void UstawPokazPodpis(bool p_pokaz_podpis)
        {

            pokaz_podpis = p_pokaz_podpis;
        }

        public void UstawPodpisyKlawiszy(string p_podpisy_klawiszy)
        {
            podpisy_klawiszy_rodzaj = p_podpisy_klawiszy;
        }


        public Arkusz UtworzEkran(Panel panel)
        {
            Arkusz arkusz = new Arkusz();
            Tonacja tonacja;
            Klucz klucz;
            int ileObiektowNutowych;
            foreach (SystemNutowyDefinicja systemNutowyDefinicja in lista_definicji_systemow_nutowych)
            {
                tonacja = systemNutowyDefinicja.tonacja;
                klucz = new Klucz(systemNutowyDefinicja.klucz, tonacja);
                ileObiektowNutowych = systemNutowyDefinicja.lista_obiektow_nutowych.Count();

                if (pokaz_pieciolinie)
                {
                    WierszNutowy wierszNutowy = new WierszNutowy();
                    // wierszNutowy.liczba_pol_dodatkowych_gora = 10;
                    // wierszNutowy.margines_gorny_i = 4;                    
                    wierszNutowy.lista_obiektow_nutowych.Add(klucz);

                    for (int i = 0; i < ileObiektowNutowych; i++)
                    {
                        Nuta nuta = (Nuta)(systemNutowyDefinicja.lista_obiektow_nutowych[i]);

                        switch (klucz.kod)
                        {
                            case "wiol":
                                if (nuta.nr < wiol_od || nuta.nr > wiol_do)
                                    nuta.kolor = Color.Red;
                                else nuta.kolor = Color.Black;
                                break;
                            case "bas":
                                if (nuta.nr < bas_od || nuta.nr > bas_do)
                                    nuta.kolor = Color.Red;
                                else nuta.kolor = Color.Black;
                                break;

                        }
                        //nuta.wartosc_rytmiczna=WartoscRytmiczna.wartosc("ca");

                        if (Parametry.Tryb.rodzaj_cwiczen == "podstawowy")
                        {
                            if (pokaz_podpis)
                                nuta.rodzaj_podpisu = "nazwa_literowa";
                            else nuta.rodzaj_podpisu = "brak";
                            nuta.czy_obiekt_wyswietlany = pokaz_nute;
                        }
                        if (Parametry.Tryb.rodzaj_cwiczen == "interwalowy")
                        {
                            if ((pokaz_podpis) && (i % 2 == 0))
                            {
                                nuta.font_podpis = Parametry.GrafikaNut.font_podpis_nuty_interwal;
                                nuta.rodzaj_podpisu = "inny";
                                if (i == ileObiektowNutowych - 1) nuta.podpis_inny = "";
                                else
                                {
                                    Nuta nastepnaNuta = (Nuta)(systemNutowyDefinicja.lista_obiektow_nutowych[i + 1]);
                                    try
                                    {
                                        Interwal interwal = nuta.InterwalDoNuty(nastepnaNuta);
                                        nuta.podpis_inny = interwal.nazwa_wyswietlana;
                                    }
                                    catch
                                    {
                                        nuta.podpis_inny = "?";
                                    }
                                }
                            }
                            else nuta.rodzaj_podpisu = "brak";
                            if (i % 2 == 0)
                            {
                                nuta.czy_obiekt_wyswietlany = pokaz_pierwsza_nute;
                            }
                            else nuta.czy_obiekt_wyswietlany = pokaz_druga_nute;
                        }

                        wierszNutowy.Dodaj(nuta);
                        if ((i + 1) % metrum.wartosc_gorna == 0 && i < ileObiektowNutowych - 1)
                            wierszNutowy.Dodaj(new KreskaTaktowa("zwykła"));
                        if (i == ileObiektowNutowych - 1)
                        {
                            wierszNutowy.Dodaj(new KreskaTaktowa("koniec"));
                        }

                    }

                    arkusz.DodajWiersz(wierszNutowy);
                }

            }

            //KLAWIATURA - tylko pierwszy system
            SystemNutowyDefinicja pierwszySystemNutowyDefinicja = lista_definicji_systemow_nutowych[0];
            tonacja = pierwszySystemNutowyDefinicja.tonacja;
            klucz = new Klucz(pierwszySystemNutowyDefinicja.klucz, tonacja);
            ileObiektowNutowych = pierwszySystemNutowyDefinicja.lista_obiektow_nutowych.Count();
            if (pokaz_klawiature)
            {
                int klawisz_nuta_nr_od;
                int klawisz_nuta_nr_do;

                switch (klucz.kod)
                {
                    case "wiol":
                        klawisz_nuta_nr_od = wiol_od;
                        klawisz_nuta_nr_do = wiol_do;
                        break;
                    case "bas":
                        klawisz_nuta_nr_od = bas_od;
                        klawisz_nuta_nr_do = bas_do;
                        break;
                    default: throw new Exception("Nieznany kod" + klucz.kod);
                }

                string polozenie_pionowe = pokaz_pieciolinie ? "u_dolu" : "caly_ekran";

                WierszKlawiaturowy wierszKlawiaturowy = new WierszKlawiaturowy(new Nuta(klawisz_nuta_nr_od), new Nuta(klawisz_nuta_nr_do), polozenie_pionowe);
                arkusz.DodajWiersz(wierszKlawiaturowy);


                //zaznaczenie klawiszy z pieciolinii
                for (int i = 0; i < ileObiektowNutowych; i++)
                {
                    Nuta nuta = (Nuta)(pierwszySystemNutowyDefinicja.lista_obiektow_nutowych[i]);
                    wierszKlawiaturowy.wyroznieniaKlawiszy[nuta.getKlawisz().klawisz_nr] = new WyroznienieKlawiszaKolko(Parametry.Klawiatura.stosunek_promienia_wybrania_do_szerokosc_klawisza_czarnego,
                                                                                                                   Color.Red);
                }

                List<PodpisKlawisza> podpisyKlawisza;
                if (podpisy_klawiszy_rodzaj == "krzyzykowe" || podpisy_klawiszy_rodzaj == "krzyzykowe i bemolowe")
                {
                    for (int nuta_nr= klawisz_nuta_nr_od-1; nuta_nr <= klawisz_nuta_nr_do; nuta_nr++)
                    {
                        if (nuta_nr >= 0)
                        {

                            Nuta nuta = new Nuta(nuta_nr, "cala_nuta", "krzyzyk");
                            if (nuta.getKlawisz().klawisz_nr >= wierszKlawiaturowy.nr_klawisza_od && nuta.getKlawisz().klawisz_nr <= wierszKlawiaturowy.nr_klawisza_do)
                            {
                                wierszKlawiaturowy.podpisyKlawiszy.TryGetValue(nuta.getKlawisz().klawisz_nr, out podpisyKlawisza);
                                if (podpisyKlawisza == null) podpisyKlawisza = new List<PodpisKlawisza>();
                                podpisyKlawisza.Add(new PodpisKlawisza(0, nuta, "nazwa_literowa"));
                                wierszKlawiaturowy.podpisyKlawiszy[nuta.getKlawisz().klawisz_nr] = podpisyKlawisza;
                            }
                        }
                    }
                }


                if (podpisy_klawiszy_rodzaj == "bemolowe" || podpisy_klawiszy_rodzaj == "krzyzykowe i bemolowe")
                {
                    for (int nuta_nr = klawisz_nuta_nr_od-1; nuta_nr <= klawisz_nuta_nr_do+1; nuta_nr++)
                    {
                        if (nuta_nr >= 0)
                        {
                            Nuta nuta = new Nuta(nuta_nr, "cala_nuta", "bemol");
                            if (nuta.getKlawisz().klawisz_nr >= wierszKlawiaturowy.nr_klawisza_od && nuta.getKlawisz().klawisz_nr <= wierszKlawiaturowy.nr_klawisza_do)
                            {
                                wierszKlawiaturowy.podpisyKlawiszy.TryGetValue(nuta.getKlawisz().klawisz_nr, out podpisyKlawisza);
                                if (podpisyKlawisza == null) podpisyKlawisza = new List<PodpisKlawisza>();
                                podpisyKlawisza.Add(new PodpisKlawisza(1, nuta, "nazwa_literowa"));
                                wierszKlawiaturowy.podpisyKlawiszy[nuta.getKlawisz().klawisz_nr] = podpisyKlawisza;
                            }
                        }

                    }
                }

            }

            arkusz.UstawWysokosciPodpisuIOdlegloscMiedzyWierszami();
            return arkusz;
        }
    
    }
}


