using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS2
{

    static class Parametry
    {
        public static int AutoScrollPosition_Y = 0;
        //TODO

        public static class Ograniczenie
        {
            /*
            public static string edycjaProgramu = "uczniowie";
            public static int max_rok = 2030;
            public static int max_miesiac = 11;
            */

            
            public static string edycjaProgramu = "";
            public static int max_rok = 2400;
            public static int max_miesiac = 11;
            
        }

        public static class Ogolne
        {
            public static bool kopiowanie =false;
            public static bool pasek_zwin = false;
        }

        public static class Konfiguracja
        {
            public static int nuta_min=0;
            public static int nuta_max = 44;
            public static int wiolinowy_min = 20;
            public static int wiolinowy_max = 44;
            public static int basowy_min = 0;
            public static int basowy_max = 26;
        }


        public class Tonacje
        {
            public static Tonacja tonacja = new Tonacja("C-dur");
            public static bool czy_pokazywac_terachordy = true;
            public static bool czy_pokazywac_dziubki_poltonowe = true;
            public static bool czy_pokazywac_podpisy_nut = true;
            public static bool czy_pokazywac_tytuly= true;
            public static string klucz = "wiol";
            //  public static List<string> sekcje= new List<string>(new string[] { "znaki przykluczowe", "odmiana naturalna"});
            public static Hashtable sekcje=new Hashtable()
                {
                    {"znaki przykluczowe", false},
                    {"odmiana naturalna", true},
                    {"odmiana harmoniczna", false},
                    {"odmiana dorycka", false},
                    {"odmiana melodyczna", false},
                };

        }

 
        public static class Panel
        {
            public static float powiekszenie = 1f;
            public static int margines_lewy_a = 10;
            public static int margines_prawy_a = 10;
            public static float margines_gorny_i = 0f;
            public static float margines_dolny_i = 0;
            public static float wspolczynnik_grubosci_linii = 0.12f;
            public static Color tlo = Color.White;//Color.BlanchedAlmond;
            //public static int minimalna_interlinia = 5;

            //public static int nuty_liczba_pol = 40;//15;
            //public static int symbole_liczba_pol = 8;

        }

        public static class Tryb
        {
            //public static string tryb = "rozpoznawanie";
            //public static string rodzaj_cwiczen = "klawisze";
            //public static string rodzaj_cwiczen = "interwaly_pieciolinia";
            // public static string rodzaj_cwiczen = "nuty";

            //public static string tryb = "generowanie";
            public static string tryb = "rozpoznawanie";
             public static string rodzaj_cwiczen = "nuty";
            // public static string rodzaj_cwiczen = "interwaly";
            //   public static string rodzaj_cwiczen = "interwalowy";
        }


        public static class ZakresPytan
        {
            public static bool czy_kolejne = false;
        }

        public static class ZakresNut
        {
            public static string klucz = "wiol";

            public static int wiolinowy_od = 23;
            public static int wiolinowy_do = 30;

            public static int basowy_od = 8;
            public static int basowy_do = 20;

            public static bool wiolinowy_OK()
            {                
                if (Parametry.ZakresNut.wiolinowy_od >= Parametry.ZakresNut.wiolinowy_do) return false;
                return true;
            }
            public static bool basowy_OK()
            {
                if (Parametry.ZakresNut.basowy_od >= Parametry.ZakresNut.basowy_do) return false;
                return true;
            }

            public static bool czy_parametryzuje()
            {
                bool parametryzuje = false;
                switch (Tryb.tryb + "_" + Tryb.rodzaj_cwiczen)
                {
                    case ("rozpoznawanie_nuty"): parametryzuje = true; break;
                    case ("rozpoznawanie_symbole"): parametryzuje = false; break;
                    case ("rozpoznawanie_klawisze"): parametryzuje = true; break;
                    case ("rozpoznawanie_interwaly_pieciolinia"): parametryzuje = true; break;
                    case ("rozpoznawanie_interwaly_klawiatura"): parametryzuje = true; break;
                    case ("generowanie_nuty"): parametryzuje = true; break;
                    case ("generowanie_interwaly"): parametryzuje = true; break;
                    case ("materialy_tonacje"): parametryzuje = false; break;
                    case ("materialy_interwaly"): parametryzuje = false; break;
                    case ("edytor_podstawowy"): parametryzuje = true; break;
                    case ("edytor_interwalowy"): parametryzuje = true; break;
                    default: throw new Exception("Nieznany tryb+rodzaj");
                }
                return parametryzuje;
            }

        }


        public static class UkladArkusza
        {
            public static int liczba_systemow = 1;
            public static float odstep_pionowy_przed_pierwszym_wierszem_nutowym_i=6f;            
            //public static int liczba_pol_dodatkowych_gora = 5;  //powinno być 5, by całość
            //public static int liczba_pol_dodatkowych_dol = 6; //powinno być 6, by całość
        }

        public static class GrafikaNut
        {
            public static float odstep_po_nucie_i = 2.5f;
            public static float odstep_po_kluczu_i = 0.3f;
            public static float odstep_po_znakach_przykluczowych_i = 2f;
            public static float odstep_po_kresce_taktowej_i = 2.5f;

            public static float odstep_przed_pierwszym_obiektem_i = 1f;

            public static float odstep_po_znaku_chromatycznym_i = 0.3f;            
            public static bool czy_podpisy_z_oznaczeniem_gamy = true;

            public static Color kolor_podpisu=Color.Black;            
            public static string font_podpis_nuty_zwykly = "Courier New";
            public static string font_podpis_nuty_interwal = "Arial";
            
        }

        public static class Klawiatura
        {
            public static float stosunek_szerokosci_klawisza_czarnego_do_bialego = 6 / 7.0f;
            public static float stosunek_wysokosci_klawisza_czarnego_do_bialego = 2 / 3.0f;
            public static float stosunek_szerokosci_do_wysokosci_klawisza_bialego= 1/4.50f;
            public static float stosunek_promienia_wybrania_do_szerokosc_klawisza_czarnego = 2 / 3.0f;

            public static string font_nazwa = "Courier New";

            public static bool czy_parametryzuje()
            {
                bool parametryzuje = false;
                switch (Tryb.tryb + "_" + Tryb.rodzaj_cwiczen)
                {
                    case ("rozpoznawanie_nuty"): parametryzuje = false; break;
                    case ("rozpoznawanie_symbole"): parametryzuje = false; break;
                    case ("rozpoznawanie_klawisze"): parametryzuje = false; break;
                    case ("rozpoznawanie_interwaly_pieciolinia"): parametryzuje = false; break;
                    case ("rozpoznawanie_interwaly_klawiatura"): parametryzuje = false; break;
                    case ("generowanie_nuty"): parametryzuje = false; break;
                    case ("generowanie_interwaly"): parametryzuje = false; break;
                    case ("materialy_tonacje"): parametryzuje = false; break;
                    case ("edytor_podstawowy"): parametryzuje = true; break;
                    case ("edytor_interwalowy"): parametryzuje = false; break;
                    default: throw new Exception("Nieznany tryb+rodzaj");
                }
                return parametryzuje;
            }
        }


        public static class ZnakiChromatyczne
        {
            public static Hashtable znaki = new Hashtable();

            static ZnakiChromatyczne()
            {
                List<string> etykiety = getEtykiety();
                for (int i = 0; i < etykiety.Count(); i++)
                {
                    znaki.Add(etykiety[i], i == 0 ? true : false);
                }
            }

            public static List<string> getEtykiety()
            {
                return new List<string> { "brak", "krzyzyk", "bemol", "podwojny_bemol", "podwojny_krzyzyk" };
            }

            public static bool co_najmniej_jeden_wybrany()
            {               
                bool czy_wybrany = false;
                foreach (string znak in znaki.Keys)
                {
                    if (!(Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen == "rozpoznawanie_klawisze" && znak != "brak" && znak != "krzyzyk" && znak != "bemol"))
                        czy_wybrany = czy_wybrany || (bool)(znaki[znak]);
                }
                if (!czy_wybrany) return false;
                return true;
            }


            public static bool co_najmniej_jeden_wybrany_poza_brak()
            {
                bool czy_wybrany = false;
                foreach (string znak in znaki.Keys)
                {
                    if(znak!="brak")
                      czy_wybrany = czy_wybrany || (bool)(znaki[znak]);
                }
                if (!czy_wybrany) return false;
                return true;
            }

            public static bool czy_parametryzuje()
            {
                bool parametryzuje = false;
                switch (Tryb.tryb + "_" + Tryb.rodzaj_cwiczen)
                {
                    case ("rozpoznawanie_nuty"): parametryzuje = true; break;
                    case ("rozpoznawanie_symbole"): parametryzuje = true; break;
                    case ("rozpoznawanie_klawisze"): parametryzuje = true; break;
                    case ("rozpoznawanie_interwaly_pieciolinia"): parametryzuje = true; break;
                    case ("rozpoznawanie_interwaly_klawiatura"): parametryzuje = true; break;
                    case ("generowanie_nuty"): parametryzuje = true; break;
                    case ("generowanie_interwaly"): parametryzuje = true; break;
                    case ("materialy_tonacje"): parametryzuje = false; break;
                    case ("edytor_podstawowy"): parametryzuje = false; break;
                    case ("edytor_interwalowy"): parametryzuje = false; break;
                    default: throw new Exception("Nieznany tryb+rodzaj");
                }
                return parametryzuje;
            }
        }

        public static class Rundy
        {
            public static bool czy_parametryzuje()
            {
                bool parametryzuje = false;
                switch (Tryb.tryb + "_" + Tryb.rodzaj_cwiczen)
                {
                    case ("rozpoznawanie_nuty"): parametryzuje = true; break;
                    case ("rozpoznawanie_symbole"): parametryzuje = true; break;
                    case ("rozpoznawanie_klawisze"): parametryzuje = true; break;
                    case ("rozpoznawanie_interwaly_pieciolinia"): parametryzuje = true; break;
                    case ("rozpoznawanie_interwaly_klawiatura"): parametryzuje = true; break;
                    case ("generowanie_nuty"): parametryzuje = true; break;
                    case ("materialy_tonacje"): parametryzuje = false; break;
                    case ("edytor_podstawowy"): parametryzuje = true; break;
                    case ("edytor_interwalowy"): parametryzuje = true; break;
                    default: throw new Exception("Nieznany tryb+rodzaj");
                }
                return parametryzuje;
            }
        }

        public static class Interwaly
        {
            public static Hashtable ile_wszystkich_interwalow = new Hashtable();

            public static void Przelicz()
            {
                foreach (Interwal interwal in Interwal.interwaly)
                {
                    GeneratorElementow generator = new GeneratorElementow("interwal", Parametry.ZakresNut.klucz, interwal.kod);
                    ile_wszystkich_interwalow[interwal.kod] = generator.wszystkie_mozliwe_elementy.Count(); ;
                }


            }
            static Interwaly()
            {
                Przelicz();
            }
        }


            public static class LiczebnoscInterwalow
        {
            public static Hashtable ile_interwalow_do_losowania = new Hashtable();
            

            static LiczebnoscInterwalow()
            {
                foreach (Interwal interwal in Interwal.interwaly)
                {
                    ile_interwalow_do_losowania[interwal.kod] = 0;
                }
            }
            public static bool czy_parametryzuje()
            {
                bool parametryzuje = false;
                switch (Tryb.tryb + "_" + Tryb.rodzaj_cwiczen)
                {
                    case ("rozpoznawanie_nuty"): parametryzuje = false; break;
                    case ("rozpoznawanie_symbole"): parametryzuje = false; break;
                    case ("rozpoznawanie_klawisze"): parametryzuje = false; break;
                    case ("rozpoznawanie_interwaly_pieciolinia"): parametryzuje = false; break;
                    case ("rozpoznawanie_interwaly_klawiatura"): parametryzuje = false; break;
                    case ("generowanie_nuty"): parametryzuje = false; break;
                    case ("generowanie_interwaly"): parametryzuje = true; break;
                    case ("materialy_tonacje"): parametryzuje = false; break;
                    case ("edytor_podstawowy"): parametryzuje = false; break;
                    case ("edytor_interwalowy"): parametryzuje = false; break;
                    default: throw new Exception("Nieznany tryb+rodzaj");
                }
                return parametryzuje;
            }
        }

        public static class ZaznaczanieInterwalow
        {
            public static Hashtable zaznaczenie_interwalow = new Hashtable();
            
            static ZaznaczanieInterwalow()
            {               
                foreach (Interwal interwal in Interwal.interwaly)
                {
                    zaznaczenie_interwalow[interwal.kod] = 0;                    
                }
                            
            }

            public static bool czy_parametryzuje()
            {
                bool parametryzuje = false;
                switch (Tryb.tryb + "_" + Tryb.rodzaj_cwiczen)
                {
                    case ("rozpoznawanie_nuty"): parametryzuje = false; break;
                    case ("rozpoznawanie_symbole"): parametryzuje = false; break;
                    case ("rozpoznawanie_klawisze"): parametryzuje = false; break;
                    case ("rozpoznawanie_interwaly_pieciolinia"): parametryzuje = true; break;
                    case ("rozpoznawanie_interwaly_klawiatura"): parametryzuje = true; break;
                    case ("generowanie_nuty"): parametryzuje = false; break;
                    case ("generowanie_interwaly"): parametryzuje = false; break;
                    case ("materialy_tonacje"): parametryzuje = false; break;
                    case ("edytor_podstawowy"): parametryzuje = false; break;
                    case ("edytor_interwalowy"): parametryzuje = false; break;
                    default: throw new Exception("Nieznany tryb+rodzaj");
                }
                return parametryzuje;
            }

            public static bool co_najmniej_jeden_wybrany()
            {
                bool czy_wybrany = false;
                foreach (string interwal_kod in zaznaczenie_interwalow.Keys)
                {

                    int ile_wszystkich = (int)(Parametry.Interwaly.ile_wszystkich_interwalow[interwal_kod]);
                    int czy_zaznaczony = (int)(zaznaczenie_interwalow[interwal_kod]);

                    if (czy_zaznaczony == 1 && ile_wszystkich>0)
                        czy_wybrany = true;
                }
                if (!czy_wybrany) return false;
                return true;
            }
        }



        public static class WartosciRytmiczne
        {

            public static Hashtable wartosci = new Hashtable();


            static WartosciRytmiczne()
            {
                List<string> etykiety = getEtykiety();
                for (int i=0;i < etykiety.Count();i++)
                {
                    wartosci.Add(etykiety[i], i == 0 ? true : false);
                }
            }

            public static List<string> getEtykiety()
            {
                return new List<string> { "cala_nuta", "polnuta", "cwiercnuta", "osemka" , "szesnastka" };
            }
           

            public static bool co_najmniej_jeden_wybrany()
            {
                bool czy_wybrany = false;
                foreach (string wartosc in wartosci.Keys)
                {
                    czy_wybrany = czy_wybrany || (bool)(wartosci[wartosc]);
                }
                if (!czy_wybrany) return false;
                return true;
            }

            public static bool czy_parametryzuje()
            {
                bool parametryzuje = false;
                switch (Tryb.tryb + "_" + Tryb.rodzaj_cwiczen)
                {
                    case ("rozpoznawanie_nuty"): parametryzuje = true; break;
                    case ("rozpoznawanie_symbole"): parametryzuje = true; break;
                    case ("rozpoznawanie_klawisze"): parametryzuje = false; break;
                    case ("rozpoznawanie_interwaly_pieciolinia"): parametryzuje = true; break;
                    case ("rozpoznawanie_interwaly_klawiatura"): parametryzuje = true; break;
                    case ("generowanie_nuty"): parametryzuje = true; break;
                    case ("generowanie_interwaly"): parametryzuje = true; break;
                    case ("materialy_tonacje"): parametryzuje = false; break;
                    case ("edytor_podstawowy"): parametryzuje = false; break;
                    case ("edytor_interwalowy"): parametryzuje = false; break;
                    default: throw new Exception("Nieznany tryb+rodzaj");
                }
                return parametryzuje;
            }

        }

        public static class Pauzy
        {
            public static Hashtable pauzy = new Hashtable();
            static Pauzy()
            {
                List<string> etykiety = getEtykiety();
                for (int i = 0; i < etykiety.Count(); i++)
                {
                    pauzy.Add(etykiety[i], i == 0 ? true : false);
                }
            }

            public static List<string> getEtykiety()
            {
                return new List<string> { "calonutowa", "polnutowa", "cwiercnutowa", "osemkowa", "szesnastkowa" };
            }

            public static bool co_najmniej_jeden_wybrany()
            {
                bool czy_wybrany = false;
                foreach (string pauza in pauzy.Keys)
                {
                    czy_wybrany = czy_wybrany || (bool)(pauzy[pauza]);
                }
                if (!czy_wybrany) return false;
                return true;
            }

            public static bool czy_parametryzuje()
            {
                bool parametryzuje = false;
                switch (Tryb.tryb + "_" + Tryb.rodzaj_cwiczen)
                {
                    case ("rozpoznawanie_nuty"): parametryzuje = false; break;
                    case ("rozpoznawanie_symbole"): parametryzuje = true; break;
                    case ("rozpoznawanie_klawisze"): parametryzuje = false; break;
                    case ("rozpoznawanie_interwaly_pieciolinia"): parametryzuje = false; break;
                    case ("rozpoznawanie_interwaly_klawiatura"): parametryzuje = false; break;
                    case ("generowanie_nuty"): parametryzuje = false; break;
                    case ("generowanie_interwaly"): parametryzuje = false; break;
                    case ("materialy_tonacje"): parametryzuje = false; break;
                    case ("edytor_podstawowy"): parametryzuje = false; break;
                    case ("edytor_interwalowy"): parametryzuje = false; break;
                    default: throw new Exception("Nieznany tryb+rodzaj");
                }
                return parametryzuje;
            }
        }

        public static class Cwiczenia
        {

            public static string nazwy_nut = "literowe";
            public static bool odpowiedzi = true;
            public static Color kolor_odpowiedzi1 = System.Drawing.Color.Blue;
            public static Color kolor_odpowiedzi2 = System.Drawing.Color.Red;
        }

        public static class LosoweNuty
        {
            public static int maksymalna_liczba_nut_w_systemie = 10;
            public static int liczba_nut_wiolinowy = 3;
            public static int liczba_nut_basowy= 3;
            public static Tonacja tonacja = new Tonacja("C-dur");
            public static int maksymalna_liczba_systemow = 10;
            public static int liczba_systemow = 3;
            public static bool pokaz_nute = true;
            public static bool pokaz_podpis = true;            
        }


        public class LosoweInterwaly
        {
            public static bool czy_pokazywac_pierwsza_nute = true;
            public static bool czy_pokazywac_druga_nute = true;
            public static bool czy_podpis = true;
            public static bool czy_pomijac_identyczne_znaki_chromatyczne = false;
            public static Tonacja tonacja = new Tonacja("C-dur");

        }

        public static class Edytor
        {
            public static Tonacja init_tonacja= new Tonacja("C-dur");
            public static string init_klucz = "wiol";

            public static Metrum init_metrum =new Metrum("1/1");

            /*
            public static int init_wiolinowy_od = 20;
            public static int init_wiolinowy_do = 44;
            public static int init_basowy_od = 0;
            public static int init_basowy_do = 26;
            */

            public static bool init_pokaz_nute = true;
            public static bool init_pokaz_podpis = true;
            public static bool init_pokaz_pierwsza_nute = true;
            public static bool init_pokaz_druga_nute = true;
            public static bool init_pokaz_klawiature = true;
            public static bool init_pokaz_pieciolinie = true;
            public static string init_podpisy_klawiszy = "brak";

         

        }

        public static string rodzaj_nazwa_wyswietlana(string p_tryb, string p_rodzaj)
        {
            string nazwa_wyswietlana = "";
            switch (p_tryb + "_" + p_rodzaj)
            {
                case ("rozpoznawanie_nuty"): nazwa_wyswietlana = "Rozpoznawanie nut"; break;
                case ("rozpoznawanie_symbole"): nazwa_wyswietlana = "Rozpoznawanie symboli"; break;
                case ("rozpoznawanie_klawisze"): nazwa_wyswietlana = "Rozpoznawanie klawiszy"; break;
                case ("rozpoznawanie_interwaly_pieciolinia"): nazwa_wyswietlana = "Rozpoznawanie interwałów"; break;
                case ("rozpoznawanie_interwaly_klawiatura"): nazwa_wyswietlana = "Rozpoznawanie interwałów"; break;
                case ("generowanie_nuty"): nazwa_wyswietlana = "Losowe nuty"; break;
                case ("generowanie_interwaly"): nazwa_wyswietlana = "Losowe interwały"; break;
                case ("materialy_tonacje"): nazwa_wyswietlana = "Materiały tonacje"; break;
                case ("edytor_podstawowy"): nazwa_wyswietlana = "Edytor nut"; break;
                case ("edytor_interwalowy"): nazwa_wyswietlana = "Edytor interwałów"; break;
                default: throw new Exception("Nieznany tryb+rodzaj");
            }
            return nazwa_wyswietlana;
        }


 

        public static bool czy_numerowanie_ekranow()
        {
            bool numerowanie = false;
            switch (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen)
            {
                case ("rozpoznawanie_nuty"): numerowanie = true; break;
                case ("rozpoznawanie_symbole"): numerowanie = true; break;
                case ("rozpoznawanie_klawisze"): numerowanie = true; break;
                case ("rozpoznawanie_interwaly_pieciolinia"): numerowanie = true; break;
                case ("rozpoznawanie_interwaly_klawiatura"): numerowanie = true; break;
                case ("generowanie_nuty"): numerowanie = true; break;
                case ("generowanie_interwaly"): numerowanie = true; break;
                case ("materialy_tonacje"): numerowanie = false; break;
                case ("edytor_podstawowy"): numerowanie = true; break;
                case ("edytor_interwalowy"): numerowanie = true; break;
                default: throw new Exception("Nieznany tryb+rodzaj");

            }
            return numerowanie;
        }


        public static bool czy_kolejne()
        {
            bool kolejne = false;
            switch (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen)
            {
                case ("rozpoznawanie_nuty"): kolejne = Parametry.ZakresPytan.czy_kolejne; break;
                case ("rozpoznawanie_symbole"): kolejne = Parametry.ZakresPytan.czy_kolejne; break;
                case ("rozpoznawanie_klawisze"): kolejne = Parametry.ZakresPytan.czy_kolejne; break;
                case ("rozpoznawanie_interwaly_pieciolinia"): kolejne =false; break;
                case ("rozpoznawanie_interwaly_klawiatura"): kolejne = false; break;
                case ("generowanie_nuty"): kolejne = false; break;
                case ("generowanie_interwaly"): kolejne = false; break;
                case ("materialy_tonacje"): kolejne = false; break;
                case ("edytor_podstawowy"): kolejne = false; break;
                case ("edytor_interwalowy"): kolejne = false; break;
                default: throw new Exception("Nieznany tryb+rodzaj");

            }
            return kolejne;
        }


        public static bool czy_odpowiedzi()
        {
            bool odpowiedzi = false;
            switch (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen)
            {
                case ("rozpoznawanie_nuty"): odpowiedzi = Parametry.Cwiczenia.odpowiedzi; break;
                case ("rozpoznawanie_symbole"): odpowiedzi = Parametry.Cwiczenia.odpowiedzi; break;
                case ("rozpoznawanie_klawisze"): odpowiedzi = Parametry.Cwiczenia.odpowiedzi; break;
                case ("rozpoznawanie_interwaly_pieciolinia"): odpowiedzi = Parametry.Cwiczenia.odpowiedzi; break;
                case ("rozpoznawanie_interwaly_klawiatura"): odpowiedzi = Parametry.Cwiczenia.odpowiedzi; break;
                case ("generowanie_nuty"): odpowiedzi = false; break;
                case ("generowanie_interwaly"): odpowiedzi = false; break;
                case ("materialy_tonacje"): odpowiedzi = false; break;
                case ("edytor_podstawowy"): odpowiedzi = false; break;
                case ("edytor_interwalowy"): odpowiedzi = false; break;
                default: throw new Exception("Nieznany tryb+rodzaj");

            }
            return odpowiedzi;
        }

        public static void Przelicz()
        {
            Parametry.Interwaly.Przelicz();
        }

        public static bool OK()
        {
            Hashtable alerty = new Hashtable();
            return OK(ref alerty);
        }


        public static bool OK(ref Hashtable alerty)
        {
            bool wynik = true;
            alerty["wartosci_rytmiczne"] = "";
            alerty["znaki_chromatyczne"] = "";
            alerty["klucz_wiolinowy"] = "";
            alerty["klucz_basowy"] = "";
            switch (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen)
            {
                case ("rozpoznawanie_nuty"):
                    if (!Parametry.ZakresNut.wiolinowy_OK())
                    {
                        alerty["klucz_wiolinowy"] = "Wartość \"do\" dla klucza wiolinowego musi być większa od wartości \"od\"";
                        wynik = false;
                    }
                    if (!Parametry.ZakresNut.basowy_OK())
                    {
                        alerty["klucz_basowy"] = "Wartość \"do\" dla klucza basowego musi być większa od wartości \"od\"";
                        wynik = false;
                    }
                    if (!ZnakiChromatyczne.co_najmniej_jeden_wybrany())
                    {
                        alerty["znaki_chromatyczne"] = "Wybierz znaki chromatyczne";
                        wynik = false;
                    }
                    if (!WartosciRytmiczne.co_najmniej_jeden_wybrany())
                    {
                        alerty["wartosci_rytmiczne"] = "Wybierz wartości rytmiczne";
                        wynik = false;
                    }
                    break;
                case ("rozpoznawanie_klawisze"):
                    if (!Parametry.ZakresNut.wiolinowy_OK())
                    {
                        alerty["klucz_wiolinowy"] = "Wartość \"do\" dla klucza wiolinowego musi być większa od wartości \"od\"";
                        wynik = false;
                    }
                    if (!Parametry.ZakresNut.basowy_OK())
                    {
                        alerty["klucz_basowy"] = "Wartość \"do\" dla klucza basowego musi być większa od wartości \"od\"";
                        wynik = false;
                    }
                    if (!ZnakiChromatyczne.co_najmniej_jeden_wybrany())
                    {
                        alerty["znaki_chromatyczne"] = "Wybierz znaki chromatyczne";
                        wynik = false;
                    }
                    break;
                case ("rozpoznawanie_symbole"):
                    if (!ZnakiChromatyczne.co_najmniej_jeden_wybrany_poza_brak() &&  !WartosciRytmiczne.co_najmniej_jeden_wybrany() && !Pauzy.co_najmniej_jeden_wybrany())
                    {
                        alerty["wartosci_rytmiczne"] = "Wybierz symbole";
                        wynik = false;
                    };
                    break;
                case ("generowanie_nuty"):
                case ("generowanie_interwaly"):
                    if (!Parametry.ZakresNut.wiolinowy_OK())
                    {
                        alerty["klucz_wiolinowy"] = "Wartość \"do\" dla klucza wiolinowego musi być większa od wartości \"od\"";
                        wynik = false;
                    }
                    if (!Parametry.ZakresNut.basowy_OK())
                    {
                        alerty["klucz_basowy"] = "Wartość \"do\" dla klucza basowego musi być większa od wartości \"od\"";
                        wynik = false;
                    }
                    if (!ZnakiChromatyczne.co_najmniej_jeden_wybrany())
                    {
                        alerty["znaki_chromatyczne"] = "Wybierz znaki chromatyczne";
                        wynik = false;
                    }
                    if (!WartosciRytmiczne.co_najmniej_jeden_wybrany())
                    {
                        alerty["wartosci_rytmiczne"] = "Wybierz wartości rytmiczne";
                        wynik = false;
                    }

                    break;
                case ("materialy_tonacje"):
                    break;
                case ("edytor_podstawowy"):
                    if (!Parametry.ZakresNut.wiolinowy_OK())
                    {
                        alerty["klucz_wiolinowy"] = "Wartość \"do\" dla klucza wiolinowego musi być większa od wartości \"od\"";
                        wynik = false;
                    }
                    if (!Parametry.ZakresNut.basowy_OK())
                    {
                        alerty["klucz_basowy"] = "Wartość \"do\" dla klucza basowego musi być większa od wartości \"od\"";
                        wynik = false;
                    }
                    break;
                case ("edytor_interwalowy"):
                    if (!Parametry.ZakresNut.wiolinowy_OK())
                    {
                        alerty["klucz_wiolinowy"] = "Wartość \"do\" dla klucza wiolinowego musi być większa od wartości \"od\"";
                        wynik = false;
                    }
                    if (!Parametry.ZakresNut.basowy_OK())
                    {
                        alerty["klucz_basowy"] = "Wartość \"do\" dla klucza basowego musi być większa od wartości \"od\"";
                        wynik = false;
                    }
                    break;
                case ("rozpoznawanie_interwaly_pieciolinia"):
                case ("rozpoznawanie_interwaly_klawiatura"):
                    if (!Parametry.ZakresNut.wiolinowy_OK())
                    {
                        alerty["klucz_wiolinowy"] = "Wartość \"do\" dla klucza wiolinowego musi być większa od wartości \"od\"";
                        wynik = false;
                    }
                    if (!Parametry.ZakresNut.basowy_OK())
                    {
                        alerty["klucz_basowy"] = "Wartość \"do\" dla klucza basowego musi być większa od wartości \"od\"";
                        wynik = false;
                    }
                    if (!ZnakiChromatyczne.co_najmniej_jeden_wybrany())
                    {
                        alerty["znaki_chromatyczne"] = "Wybierz znaki chromatyczne";
                        wynik = false;
                    }
                    if (!WartosciRytmiczne.co_najmniej_jeden_wybrany())
                    {
                        alerty["wartosci_rytmiczne"] = "Wybierz wartości rytmiczne";
                        wynik = false;
                    }
                    if (!ZaznaczanieInterwalow.co_najmniej_jeden_wybrany())
                    {
                        if (!ZnakiChromatyczne.co_najmniej_jeden_wybrany())
                            alerty["interwaly_zaznaczanie"] = "Wybierz znaki chromatyczne";
                        else alerty["interwaly_zaznaczanie"] = "Wybierz interwały";
                        wynik = false;
                    }
                    break;

                default: throw new Exception("Nieznany tryb+rodzaj");
            }
            return wynik;
        }


        /*
        public static bool OK()
        {
            if (ZakresNut.czy_parametryzuje())
            {
                if (!Parametry.ZakresNut.wiolinowy_OK()) return false;
                if (!Parametry.ZakresNut.basowy_OK()) return false;
            }
            if (Tryb.rodzaj_cwiczen != "symbole")
            {
                if (ZnakiChromatyczne.czy_parametryzuje() && !ZnakiChromatyczne.co_najmniej_jeden_wybrany()) return false;
                if (WartosciRytmiczne.czy_parametryzuje() && !WartosciRytmiczne.co_najmniej_jeden_wybrany()) return false;
            }
            else
            {
                if (!ZnakiChromatyczne.co_najmniej_jeden_wybrany() 
                    && !WartosciRytmiczne.co_najmniej_jeden_wybrany()
                     && !Pauzy.co_najmniej_jeden_wybrany()
                   ) return false;
            }
            return true;
        }
        */

    }



    public class Parametryzacja
    {
        public bool czy_pokazywac_terachordy = false;
        public bool czy_pokazywac_dziubki_poltonowe = false;
        public bool czy_pokazywac_podpisy_nut = false;
        public bool czy_pokazywac_tytuly = false;
        public string klucz = null;
        public Hashtable sekcje = null;
    }


}
 