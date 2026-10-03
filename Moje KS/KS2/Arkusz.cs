using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS2
{
    public class Arkusz
    {
        public List<WierszArkusza> wiersze;
        public float odstep_pionowy_przed_pierwszym_wierszem_nutowym_i = Parametry.UkladArkusza.odstep_pionowy_przed_pierwszym_wierszem_nutowym_i;
        public float odstep_pionowy_przed_wierszem_nutowym_i;
        public float odstep_pionowy_po_wierszu_nutowym_i;
        public float wysokosc_podpisow_l;
        public Arkusz()
        {
            wiersze = new List<WierszArkusza>();
        }

        public void DodajWiersz(WierszArkusza wiersz)
        {

            wiersze.Add(wiersz);
        }

        public void UstawWysokosciPodpisuIOdlegloscMiedzyWierszami()
        {
            float nr_linii_min = 1000;
            float nr_linii_max = -1000;
            Klucz klucz = null;

            for (int i = 0; i < wiersze.Count(); i++)
            {
                WierszArkusza wiersz = wiersze[i];
                Type t = wiersz.GetType();
                if (t.Equals(typeof(WierszNutowy)))
                {
                    WierszNutowy wierszNutowy = (WierszNutowy)wiersz;
                    foreach (ObiektNutowy obiektNutowy in wierszNutowy.lista_obiektow_nutowych)
                    {
                        Type typ = obiektNutowy.GetType();
                        if (typ.Equals(typeof(Nuta)))
                        {
                            Nuta nuta = (Nuta)obiektNutowy;
                            int przenosnik = 0;
                            float nr_linii = nuta.NumerLiniiEfektywny(klucz, ref przenosnik);
                            if (przenosnik == 1) nr_linii = 9.5f;
                            if (przenosnik == -1) nr_linii = -4.5f;

                            if (nr_linii < nr_linii_min) nr_linii_min = nr_linii;
                            if (nr_linii > nr_linii_max) nr_linii_max = nr_linii;
                        }
                        if (typ.Equals(typeof(Klucz)))
                        {
                            klucz = (Klucz)obiektNutowy;
                        }
                    }
                }
            }
            if (nr_linii_min > 0) nr_linii_min = 0;
            if (nr_linii_max < 4.5f) nr_linii_max = 4.5f;

            odstep_pionowy_przed_wierszem_nutowym_i = nr_linii_max - 3f;
            odstep_pionowy_po_wierszu_nutowym_i = -nr_linii_min + 1.5f;

            if ((Parametry.Tryb.tryb + "." + Parametry.Tryb.rodzaj_cwiczen != "rozpoznawanie.nuty")
              && (Parametry.Tryb.tryb + "." + Parametry.Tryb.rodzaj_cwiczen != "rozpoznawanie.interwaly_pieciolinia"))
            {
                odstep_pionowy_po_wierszu_nutowym_i += 5.5f;
            }
            wysokosc_podpisow_l = nr_linii_min - 1;


            for (int i = 0; i < wiersze.Count(); i++)
            {
                WierszArkusza wiersz = wiersze[i];
                Type t = wiersz.GetType();
                if (t.Equals(typeof(WierszNutowy)))
                {
                    WierszNutowy wierszNutowy = (WierszNutowy)wiersz;
                    wierszNutowy.wysokosc_podpisow_l = wysokosc_podpisow_l;
                }

            }
        }

        public static Arkusz GetArkusz_generowanie_interwaly(ZawartoscEkranu_GenerowanieInterwaly zawartosc)
        {
            Arkusz arkusz = new Arkusz(); ;
            Tonacja tonacja = zawartosc.tonacja;
            bool pokaz_podpis = Parametry.LosoweInterwaly.czy_podpis;
            bool pokaz_pierwsza_nute = Parametry.LosoweInterwaly.czy_pokazywac_pierwsza_nute;
            bool pokaz_druga_nute = Parametry.LosoweInterwaly.czy_pokazywac_druga_nute;

            WierszNutowy wierszNutowy = new WierszNutowy();
            // wierszNutowy.liczba_pol_dodatkowych_gora = 10;
            // wierszNutowy.margines_gorny_i = 4;            
            Klucz klucz = new Klucz(zawartosc.klucz, tonacja);
            wierszNutowy.lista_obiektow_nutowych.Add(klucz);

            List<Nuta> nuty = zawartosc.nuty;
            int ile_nut = nuty.Count;

            for (int i = 0; i < nuty.Count(); i++)
            {
                Nuta nuta = new Nuta(nuty[i].nr, nuty[i].wartosc_rytmiczna.nazwa, nuty[i].znak_chromatyczny);

                if ((pokaz_podpis) && (i % 2 == 0))
                {
                    nuta.font_podpis = Parametry.GrafikaNut.font_podpis_nuty_interwal;
                    nuta.rodzaj_podpisu = "inny";
                    if (i == ile_nut - 1) nuta.podpis_inny = "";
                    else
                    {
                        Nuta nastepnaNuta = (Nuta)(nuty[i + 1]);
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


                wierszNutowy.Dodaj(nuta);
                if ((i + 1) % 2 == 0 && i < ile_nut - 1)
                    wierszNutowy.Dodaj(new KreskaTaktowa("zwykła"));
                if (i == ile_nut - 1)
                {
                    wierszNutowy.Dodaj(new KreskaTaktowa("koniec"));
                }
            }
            arkusz.DodajWiersz(wierszNutowy);
            arkusz.UstawWysokosciPodpisuIOdlegloscMiedzyWierszami();
            return arkusz;
        }



        public static Arkusz GetArkusz_generowanie_nuty(ZawartoscEkranu_GenerowanieNuty zawartosc)
        {
            Arkusz arkusz = new Arkusz(); ;
            Tonacja tonacja = zawartosc.tonacja;

            WierszNutowy system_nutowy = new WierszNutowy();
            Klucz klucz = null;
            if (zawartosc.nuty_wiolinowy.Count > 0)
            {

                klucz = new Klucz("wiol", tonacja);
                system_nutowy.Dodaj(klucz);

                for (int i = 0; i < zawartosc.nuty_wiolinowy.Count; i++)
                {
                    Nuta nuta = zawartosc.nuty_wiolinowy[i];
                    if (Parametry.LosoweNuty.pokaz_podpis)
                        nuta.rodzaj_podpisu = "nazwa_literowa";
                    else nuta.rodzaj_podpisu = "brak";
                    nuta.czy_obiekt_wyswietlany = Parametry.LosoweNuty.pokaz_nute;
                    system_nutowy.Dodaj(nuta);

                    if (i != zawartosc.nuty_wiolinowy.Count - 1)
                        system_nutowy.Dodaj(new KreskaTaktowa("zwykła"));
                    else system_nutowy.Dodaj(new KreskaTaktowa("koniec"));
                }
            }

            if (zawartosc.nuty_basowy.Count > 0)
            {

                klucz = new Klucz("bas", tonacja);
                system_nutowy.Dodaj(klucz);

                for (int i = 0; i < zawartosc.nuty_basowy.Count; i++)
                {
                    Nuta nuta = zawartosc.nuty_basowy[i];
                    if (Parametry.LosoweNuty.pokaz_podpis)
                        nuta.rodzaj_podpisu = "nazwa_literowa";
                    else nuta.rodzaj_podpisu = "brak";
                    nuta.czy_obiekt_wyswietlany = Parametry.LosoweNuty.pokaz_nute;
                    system_nutowy.Dodaj(nuta);

                    if (i != zawartosc.nuty_basowy.Count - 1)
                        system_nutowy.Dodaj(new KreskaTaktowa("zwykła"));
                    else system_nutowy.Dodaj(new KreskaTaktowa("koniec"));
                }
            }

            arkusz.DodajWiersz(system_nutowy);
            arkusz.UstawWysokosciPodpisuIOdlegloscMiedzyWierszami();
            return arkusz;
        }


        public static Arkusz GetArkusz_rozpoznawanie_interwaly_pieciolinia(ZawartoscEkranu_RozpoznawanieInterwaly zawartoscEkranu, bool czy_odpowiedz)
        {
            Arkusz arkusz = new Arkusz();
            WierszNutowy wiersz_nutowy = new WierszNutowy();
            wiersz_nutowy.Dodaj(new Klucz(zawartoscEkranu.klucz, zawartoscEkranu.tonacja));
            Nuta nuta1 = zawartoscEkranu.dwie_nuty[0];
            Nuta nuta1x = new Nuta(nuta1.nr, nuta1.wartosc_rytmiczna.nazwa, nuta1.znak_chromatyczny);
            if (czy_odpowiedz)
            {
                switch (Parametry.Cwiczenia.nazwy_nut)
                {
                    case "literowe": nuta1x.rodzaj_podpisu = "nazwa_literowa"; break;
                    case "solmizacyjne": nuta1x.rodzaj_podpisu = "nazwa_solmizacyjna"; break;
                }
                nuta1x.kolor_podpisu = Parametry.Cwiczenia.kolor_odpowiedzi1;
                nuta1x.rozmiar_podpisu_i = 1.7f;
            }
            wiersz_nutowy.Dodaj(nuta1x);
            wiersz_nutowy.Dodaj(new OdstepPoziomy(3));

            Nuta nuta2 = zawartoscEkranu.dwie_nuty[1];
            Nuta nuta2x = new Nuta(nuta2.nr, nuta2.wartosc_rytmiczna.nazwa, nuta2.znak_chromatyczny);
            if (czy_odpowiedz)
            {
                switch (Parametry.Cwiczenia.nazwy_nut)
                {
                    case "literowe": nuta2x.rodzaj_podpisu = "nazwa_literowa"; break;
                    case "solmizacyjne": nuta2x.rodzaj_podpisu = "nazwa_solmizacyjna"; break;
                }
                nuta2x.kolor_podpisu = Parametry.Cwiczenia.kolor_odpowiedzi1;
                nuta2x.rozmiar_podpisu_i = 1.7f;
            }
            wiersz_nutowy.Dodaj(nuta2x);
            wiersz_nutowy.Dodaj(new OdstepPoziomy(1));

            wiersz_nutowy.wysokosc_podpisow_l = zawartoscEkranu.klucz == "wiol" ? -2 : 11;
            arkusz.DodajWiersz(wiersz_nutowy);
            //arkusz.UstawWysokosciPodpisuIOdlegloscMiedzyWierszami();            
            return arkusz;
        }


        public static Arkusz GetArkusz_rozpoznawanie_nuta(ZawartoscEkranu_RozpoznawanieNuty zawartoscEkranu_RozpoznawanieNuty)
        {
            Nuta nuta = zawartoscEkranu_RozpoznawanieNuty.nuta;
            Arkusz arkusz = new Arkusz();
            Tonacja C_dur = new Tonacja("C-dur");
            WierszNutowy wiersz_nutowy = new WierszNutowy();
            wiersz_nutowy.Dodaj(new Klucz(Parametry.ZakresNut.klucz, C_dur));
            wiersz_nutowy.Dodaj(nuta);
            wiersz_nutowy.Dodaj(new OdstepPoziomy(1));
            arkusz.DodajWiersz(wiersz_nutowy);
            arkusz.UstawWysokosciPodpisuIOdlegloscMiedzyWierszami();
            return arkusz;
        }


        public static Arkusz GetArkusz_rozpoznawanie_klawisze(ZawartoscEkranu_RozpoznawanieKlawisze zawartoscEkranu_RozpoznawanieKlawisze)
        {

            Arkusz arkusz = new Arkusz();
            WierszKlawiaturowy wierszKlawiaturowy = new WierszKlawiaturowy(zawartoscEkranu_RozpoznawanieKlawisze.klawisz_od.klawisz_nr,
                                                                           zawartoscEkranu_RozpoznawanieKlawisze.klawisz_do.klawisz_nr, "caly_ekran");
            
            wierszKlawiaturowy.wyroznieniaKlawiszy[zawartoscEkranu_RozpoznawanieKlawisze.nuta.getKlawisz().klawisz_nr] = new WyroznienieKlawiszaKolko(Parametry.Klawiatura.stosunek_promienia_wybrania_do_szerokosc_klawisza_czarnego, 
                                                                                                                          Color.Red);
                                    
            arkusz.DodajWiersz(wierszKlawiaturowy);            
            return arkusz;
        }


        public static Arkusz GetArkusz_materialy_tonacje(Tonacja tonacja, Parametryzacja parametryzacja)
        {

            Arkusz arkusz = new Arkusz();
            WierszTekstowy wierszTekstowy = null;
            WierszNutowy wierszNutowy;
            /*


               foreach (Interwal interwal in Interwal.interwaly)
                {
                    string t =  interwal.symbol+"      "+ interwal.pelna_nazwa + ": " + interwal.liczba_stopni.ToString() + "," + interwal.liczba_poltonow.ToString();
                    wierszTekstowy = new WierszTekstowy(t, "Arial",2f, "left");
                    arkusz.DodajWiersz(wierszTekstowy);
                }
                */
            /*
        wierszNutowy = new WierszNutowy();
        wierszNutowy.Dodaj(new Klucz("wiol", tonacja));
        Nuta nuta1 = new Nuta("eses", "razkreślna");
        Interwal interwal = Interwal.getInterwal("1_0",1);
        Nuta nuta2 = nuta1.PrzesunOInterwal(interwal, true);
        wierszNutowy.Dodaj(nuta1);
        wierszNutowy.Dodaj(nuta2);
        arkusz.DodajWiersz(wierszNutowy);
        */

            if (parametryzacja.czy_pokazywac_tytuly)
            {
                wierszTekstowy = new WierszTekstowy("Tonacja " + tonacja.nazwa_tonacji, "Arial", 3, "center");
                arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 4, "center"));
                arkusz.DodajWiersz(wierszTekstowy);
                arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 2, "center"));
            }

            // if (parametryzacja.sekcje.Find(x => x.Equals("znaki przykluczowe")) != null)
            if ((bool)parametryzacja.sekcje["znaki przykluczowe"])
            {

                wierszNutowy = new WierszNutowy();
                if (parametryzacja.klucz == "wiol" || parametryzacja.klucz == "wiolbas")
                    wierszNutowy.Dodaj(new Klucz("wiol", tonacja));
                if (parametryzacja.klucz == "bas" || parametryzacja.klucz == "wiolbas")
                    wierszNutowy.Dodaj(new Klucz("bas", tonacja));
                wierszNutowy.Dodaj(tonacja.znaki_przykluczowe);
                // wierszNutowy.liczba_pol_dodatkowych_gora = 3;
                arkusz.DodajWiersz(wierszNutowy);
            }
            //Nuta nuta = Dane.aktualne_pytanie.nuta1;
            //nuta.rodzaj_podpisu = "nazwa_literowa";

            Tonacja C_dur = new Tonacja("C-dur");
            Klucz klucz;

            string rodzaj_podpisu = "brak";
            if (parametryzacja.czy_pokazywac_podpisy_nut)
                rodzaj_podpisu = "nazwa_literowa";
            Interwal[] schemat;
            if (tonacja.tryb == "dur")
            {


                string oktawa_wiol = "";
                if (new Nuta(tonacja.tonika_nazwa_literowa, "mała", "cala_nuta").nr_w_oktawie >= 5)
                    oktawa_wiol = "mała";
                else oktawa_wiol = "razkreślna";
                string oktawa_bas = "";
                if (new Nuta(tonacja.tonika_nazwa_literowa, "mała", "cala_nuta").nr_w_oktawie >= 4)
                    oktawa_bas = "wielka";
                else oktawa_bas = "mała";

                if ((bool)parametryzacja.sekcje["odmiana naturalna"])
                {
                    schemat = Tonacja.schemat_gamy_durowej_naturalnej;

                    if (parametryzacja.czy_pokazywac_tytuly)
                    {
                        arkusz.DodajWiersz(new WierszTekstowy("Odmiana naturalna", "Arial", 2, "center"));
                        arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 4, "center"));
                    }

                    if (parametryzacja.klucz == "wiol" || parametryzacja.klucz == "wiolbas")
                    {
                        List<Nuta> ciagDzwiekow = tonacja.PodajCiagDzwiekow(tonacja.tonika_nazwa_literowa, oktawa_wiol, schemat, "cala_nuta");
                        wierszNutowy = new WierszNutowy();
                        klucz = new Klucz("wiol", tonacja);
                        wierszNutowy.Dodaj(klucz);
                        wierszNutowy.Dodaj(ciagDzwiekow, rodzaj_podpisu);
                        // wierszNutowy.liczba_pol_dodatkowych_gora = 4;                        
                        if (parametryzacja.czy_pokazywac_dziubki_poltonowe) wierszNutowy.DodajDziubkiPolnutowe();
                        if (parametryzacja.czy_pokazywac_terachordy)
                        {
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 1, 4));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 5, 8));
                        }
                        arkusz.DodajWiersz(wierszNutowy);

                        arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 5, "center"));
                    }
                    if (parametryzacja.klucz == "bas" || parametryzacja.klucz == "wiolbas")
                    {
                        List<Nuta> ciagDzwiekow = tonacja.PodajCiagDzwiekow(tonacja.tonika_nazwa_literowa, oktawa_bas, schemat, "cala_nuta");
                        wierszNutowy = new WierszNutowy();
                        klucz = new Klucz("bas", tonacja);
                        wierszNutowy.Dodaj(klucz);
                        wierszNutowy.Dodaj(ciagDzwiekow, rodzaj_podpisu);
                        if (parametryzacja.czy_pokazywac_dziubki_poltonowe) wierszNutowy.DodajDziubkiPolnutowe();
                        if (parametryzacja.czy_pokazywac_terachordy)
                        {
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 1, 4));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 5, 8));
                        }
                        //wierszNutowy.liczba_pol_dodatkowych_gora = 4;
                        arkusz.DodajWiersz(wierszNutowy);
                    }
                }

                if ((bool)parametryzacja.sekcje["odmiana harmoniczna"])
                {
                    schemat = Tonacja.schemat_gamy_durowej_harmonicznej;

                    if (parametryzacja.czy_pokazywac_tytuly)
                    {
                        arkusz.DodajWiersz(new WierszTekstowy("Odmiana harmoniczna", "Arial", 2, "center"));
                        arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 4, "center"));
                    }

                    if (parametryzacja.klucz == "wiol" || parametryzacja.klucz == "wiolbas")
                    {
                        List<Nuta> ciagDzwiekow = tonacja.PodajCiagDzwiekow(tonacja.tonika_nazwa_literowa, oktawa_wiol, schemat, "cala_nuta");
                        wierszNutowy = new WierszNutowy();
                        klucz = new Klucz("wiol", tonacja);
                        wierszNutowy.Dodaj(klucz);
                        wierszNutowy.Dodaj(ciagDzwiekow, rodzaj_podpisu);
                        //wierszNutowy.liczba_pol_dodatkowych_gora = 4;                        
                        if (parametryzacja.czy_pokazywac_dziubki_poltonowe) wierszNutowy.DodajDziubkiPolnutowe();
                        if (parametryzacja.czy_pokazywac_terachordy)
                        {
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 1, 4));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 5, 8));
                        }
                        arkusz.DodajWiersz(wierszNutowy);

                        arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 5, "center"));
                    }
                    if (parametryzacja.klucz == "bas" || parametryzacja.klucz == "wiolbas")
                    {
                        List<Nuta> ciagDzwiekow = tonacja.PodajCiagDzwiekow(tonacja.tonika_nazwa_literowa, oktawa_bas, schemat, "cala_nuta");
                        wierszNutowy = new WierszNutowy();
                        klucz = new Klucz("bas", tonacja);
                        wierszNutowy.Dodaj(klucz);
                        wierszNutowy.Dodaj(ciagDzwiekow, rodzaj_podpisu);
                        if (parametryzacja.czy_pokazywac_dziubki_poltonowe) wierszNutowy.DodajDziubkiPolnutowe();
                        if (parametryzacja.czy_pokazywac_terachordy)
                        {
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 1, 4));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 5, 8));
                        }
                        //wierszNutowy.liczba_pol_dodatkowych_gora = 4;
                        arkusz.DodajWiersz(wierszNutowy);
                    }
                }
            }

            if (tonacja.tryb == "moll")
            {

                schemat = Tonacja.schemat_gamy_mollowej_naturalnej;

                string oktawa_wiol = "";
                if (new Nuta(tonacja.tonika_nazwa_literowa, "mała", "cala_nuta").nr_w_oktawie >= 5)
                    oktawa_wiol = "mała";
                else oktawa_wiol = "razkreślna";
                string oktawa_bas = "";
                if (new Nuta(tonacja.tonika_nazwa_literowa, "mała", "cala_nuta").nr_w_oktawie >= 4)
                    oktawa_bas = "wielka";
                else oktawa_bas = "mała";
                List<Nuta> ciagDzwiekow;


                if ((bool)parametryzacja.sekcje["odmiana naturalna"])
                {
                    ciagDzwiekow = tonacja.PodajCiagDzwiekow(tonacja.tonika_nazwa_literowa, oktawa_wiol, schemat, "cala_nuta");

                    if (parametryzacja.czy_pokazywac_tytuly)
                    {
                        arkusz.DodajWiersz(new WierszTekstowy("Odmiana naturalna", "Arial", 2, "center"));
                        arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 4, "center"));
                    }
                    if (parametryzacja.klucz == "wiol" || parametryzacja.klucz == "wiolbas")
                    {
                        wierszNutowy = new WierszNutowy();
                        klucz = new Klucz("wiol", tonacja);
                        wierszNutowy.Dodaj(klucz);
                        wierszNutowy.Dodaj(ciagDzwiekow, rodzaj_podpisu);
                        //wierszNutowy.liczba_pol_dodatkowych_gora = 4;
                        if (parametryzacja.czy_pokazywac_dziubki_poltonowe) wierszNutowy.DodajDziubkiPolnutowe();
                        if (parametryzacja.czy_pokazywac_terachordy)
                        {
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 1, 4));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 5, 8));
                        }
                        arkusz.DodajWiersz(wierszNutowy);
                        arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 5, "center"));
                    }

                    if (parametryzacja.klucz == "bas" || parametryzacja.klucz == "wiolbas")
                    {
                        ciagDzwiekow = tonacja.PodajCiagDzwiekow(tonacja.tonika_nazwa_literowa, oktawa_bas, schemat, "cala_nuta");
                        wierszNutowy = new WierszNutowy();
                        klucz = new Klucz("bas", tonacja);
                        wierszNutowy.Dodaj(klucz);
                        wierszNutowy.Dodaj(ciagDzwiekow, rodzaj_podpisu);
                        // wierszNutowy.liczba_pol_dodatkowych_gora = 4;                        
                        if (parametryzacja.czy_pokazywac_dziubki_poltonowe) wierszNutowy.DodajDziubkiPolnutowe();
                        if (parametryzacja.czy_pokazywac_terachordy)
                        {
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 1, 4));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 5, 8));
                        }
                        arkusz.DodajWiersz(wierszNutowy);
                        arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 5, "center"));
                    }
                }
                if ((bool)parametryzacja.sekcje["odmiana harmoniczna"])
                {
                    if (parametryzacja.czy_pokazywac_tytuly)
                    {
                        arkusz.DodajWiersz(new WierszTekstowy("Odmiana harmoniczna", "Arial", 2, "center"));
                        arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 4, "center"));
                    }
                    schemat = Tonacja.schemat_gamy_mollowej_harmonicznej;

                    if (parametryzacja.klucz == "wiol" || parametryzacja.klucz == "wiolbas")
                    {
                        ciagDzwiekow = tonacja.PodajCiagDzwiekow(tonacja.tonika_nazwa_literowa, oktawa_wiol, schemat, "cala_nuta");
                        wierszNutowy = new WierszNutowy();
                        klucz = new Klucz("wiol", tonacja);
                        wierszNutowy.Dodaj(klucz);
                        wierszNutowy.Dodaj(ciagDzwiekow, rodzaj_podpisu);
                        // wierszNutowy.liczba_pol_dodatkowych_gora = 4;                        
                        if (parametryzacja.czy_pokazywac_dziubki_poltonowe) wierszNutowy.DodajDziubkiPolnutowe();
                        if (parametryzacja.czy_pokazywac_terachordy)
                        {
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 1, 4));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 5, 8));
                        }
                        arkusz.DodajWiersz(wierszNutowy);

                        wierszTekstowy = new WierszTekstowy(" ", "Arial", 5, "center");
                        arkusz.DodajWiersz(wierszTekstowy);
                    }

                    if (parametryzacja.klucz == "bas" || parametryzacja.klucz == "wiolbas")
                    {
                        ciagDzwiekow = tonacja.PodajCiagDzwiekow(tonacja.tonika_nazwa_literowa, oktawa_bas, schemat, "cala_nuta");
                        wierszNutowy = new WierszNutowy();
                        klucz = new Klucz("bas", tonacja);
                        wierszNutowy.Dodaj(klucz);
                        wierszNutowy.Dodaj(ciagDzwiekow, rodzaj_podpisu);
                        //wierszNutowy.liczba_pol_dodatkowych_gora = 4;                        
                        if (parametryzacja.czy_pokazywac_dziubki_poltonowe) wierszNutowy.DodajDziubkiPolnutowe();
                        if (parametryzacja.czy_pokazywac_terachordy)
                        {
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 1, 4));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 5, 8));
                        }
                        arkusz.DodajWiersz(wierszNutowy);
                        arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 5, "center"));
                    }
                }

                if ((bool)parametryzacja.sekcje["odmiana dorycka"])
                {
                    if (parametryzacja.czy_pokazywac_tytuly)
                    {
                        arkusz.DodajWiersz(new WierszTekstowy("Odmiana dorycka", "Arial", 2, "center"));
                        arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 4, "center"));
                    }

                    schemat = Tonacja.schemat_gamy_mollowej_doryckiej;

                    if (parametryzacja.klucz == "wiol" || parametryzacja.klucz == "wiolbas")
                    {
                        ciagDzwiekow = tonacja.PodajCiagDzwiekow(tonacja.tonika_nazwa_literowa, oktawa_wiol, schemat, "cala_nuta");
                        wierszNutowy = new WierszNutowy();
                        klucz = new Klucz("wiol", tonacja);
                        wierszNutowy.Dodaj(klucz);
                        wierszNutowy.Dodaj(ciagDzwiekow, rodzaj_podpisu);
                        // wierszNutowy.liczba_pol_dodatkowych_gora = 4;                        
                        if (parametryzacja.czy_pokazywac_dziubki_poltonowe) wierszNutowy.DodajDziubkiPolnutowe();
                        if (parametryzacja.czy_pokazywac_terachordy)
                        {
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 1, 4));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 5, 8));
                        }
                        arkusz.DodajWiersz(wierszNutowy);
                        wierszTekstowy = new WierszTekstowy(" ", "Arial", 5, "center");
                        arkusz.DodajWiersz(wierszTekstowy);
                    }
                    if (parametryzacja.klucz == "bas" || parametryzacja.klucz == "wiolbas")
                    {
                        ciagDzwiekow = tonacja.PodajCiagDzwiekow(tonacja.tonika_nazwa_literowa, oktawa_bas, schemat, "cala_nuta");
                        wierszNutowy = new WierszNutowy();
                        klucz = new Klucz("bas", tonacja);
                        wierszNutowy.Dodaj(klucz);
                        wierszNutowy.Dodaj(ciagDzwiekow, rodzaj_podpisu);
                        //  wierszNutowy.liczba_pol_dodatkowych_gora = 4;                        
                        if (parametryzacja.czy_pokazywac_dziubki_poltonowe) wierszNutowy.DodajDziubkiPolnutowe();
                        if (parametryzacja.czy_pokazywac_terachordy)
                        {
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 1, 4));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 5, 8));
                        }
                        arkusz.DodajWiersz(wierszNutowy);
                        arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 5, "center"));
                    }
                }

                if ((bool)parametryzacja.sekcje["odmiana melodyczna"])
                {
                    if (parametryzacja.czy_pokazywac_tytuly)
                    {
                        arkusz.DodajWiersz(new WierszTekstowy("Odmiana melodyczna", "Arial", 2, "center"));
                        arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 4, "center"));
                    }
                    schemat = Tonacja.schemat_gamy_mollowej_melodycznej;

                    if (parametryzacja.klucz == "wiol" || parametryzacja.klucz == "wiolbas")
                    {
                        ciagDzwiekow = tonacja.PodajCiagDzwiekow(tonacja.tonika_nazwa_literowa, oktawa_wiol, schemat, "cala_nuta");
                        wierszNutowy = new WierszNutowy();
                        klucz = new Klucz("wiol", tonacja);
                        wierszNutowy.Dodaj(klucz);
                        wierszNutowy.Dodaj(ciagDzwiekow, rodzaj_podpisu);
                        //   wierszNutowy.liczba_pol_dodatkowych_gora = 4;                        
                        if (parametryzacja.czy_pokazywac_dziubki_poltonowe) wierszNutowy.DodajDziubkiPolnutowe();
                        if (parametryzacja.czy_pokazywac_terachordy)
                        {
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 1, 4));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 5, 8));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 8, 11));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 12, 15));
                        }
                        arkusz.DodajWiersz(wierszNutowy);
                        arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 5, "center"));
                    }

                    if (parametryzacja.klucz == "bas" || parametryzacja.klucz == "wiolbas")
                    {
                        ciagDzwiekow = tonacja.PodajCiagDzwiekow(tonacja.tonika_nazwa_literowa, oktawa_bas, schemat, "cala_nuta");
                        wierszNutowy = new WierszNutowy();
                        klucz = new Klucz("bas", tonacja);
                        wierszNutowy.Dodaj(klucz);
                        wierszNutowy.Dodaj(ciagDzwiekow, rodzaj_podpisu);
                        // wierszNutowy.liczba_pol_dodatkowych_gora = 4;                        
                        if (parametryzacja.czy_pokazywac_dziubki_poltonowe) wierszNutowy.DodajDziubkiPolnutowe();
                        if (parametryzacja.czy_pokazywac_terachordy)
                        {
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 1, 4));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 5, 8));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 8, 11));
                            wierszNutowy.lista_znakow_dodatkowych.Add(new ZnakDodatkowy("klamra_tetrachord", 12, 15));
                        }
                        arkusz.DodajWiersz(wierszNutowy);
                        arkusz.DodajWiersz(new WierszTekstowy(" ", "Arial", 5, "center"));
                    }
                }

            }
            arkusz.UstawWysokosciPodpisuIOdlegloscMiedzyWierszami();
            return arkusz;
        }
    }
}
