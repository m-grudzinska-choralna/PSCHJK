using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KS2
{

   




    public class Tonacja
    {

        public static string[] lista_tonacji_durowych_bez_znakow = { "C-dur" };
        public static string[] lista_tonacji_durowych_krzyzykowych = { "G-dur", "D-dur", "A-dur", "E-dur", "H-dur", "Fis-dur", "Cis-dur" };
        public static string[] lista_tonacji_durowych_bemolowych = { "F-dur", "B-dur", "Es-dur", "As-dur", "Des-dur", "Ges-dur", "Ces-dur" };

        public static string[] lista_tonacji_mollowych_bez_znakow = { "a-moll" };
        public static string[] lista_tonacji_mollowych_krzyzykowych = { "e-moll", "h-moll", "fis-moll", "cis-moll", "gis-moll", "dis-moll", "ais-moll" };
        public static string[] lista_tonacji_mollowych_bemolowych = { "d-moll", "g-moll", "c-moll", "f-moll", "b-moll", "es-moll", "as-moll" };


        public static Interwal[] schemat_gamy_durowej_naturalnej = { Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 1, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 1, 1) };
        public static Interwal[] schemat_gamy_durowej_harmonicznej = { Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 1, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 1, 1), Interwal.getInterwal(2, 3, 1), Interwal.getInterwal(2, 1, 1) };
        public static Interwal[] schemat_gamy_mollowej_naturalnej = { Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 1, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 1, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 2, 1) };
        public static Interwal[] schemat_gamy_mollowej_harmonicznej = { Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 1, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 1, 1), Interwal.getInterwal(2, 3, 1), Interwal.getInterwal(2, 1, 1) };
        public static Interwal[] schemat_gamy_mollowej_doryckiej = { Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 1, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 2, 1), Interwal.getInterwal(2, 1, 1) };
        public static Interwal[] schemat_gamy_mollowej_melodycznej = { Interwal.getInterwal(2,2, 1), Interwal.getInterwal(2,1, 1), Interwal.getInterwal(2,2, 1), Interwal.getInterwal(2,2, 1), Interwal.getInterwal(2,2, 1), Interwal.getInterwal(2,2, 1), Interwal.getInterwal(2,1, 1),
                                                                              Interwal.getInterwal(2, 2,-1), Interwal.getInterwal(2, 2,-1), Interwal.getInterwal(2, 1,-1),  Interwal.getInterwal(2, 2,-1), Interwal.getInterwal(2, 2,-1), Interwal.getInterwal(2, 1,-1), Interwal.getInterwal(2, 2,-1) };
        


        /*
        public static Tuple<int, int>[] schemat_gamy_durowej = { new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 1), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 1) };
        public static Tuple<int, int>[] schemat_gamy_mollowej_naturalnej = { new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 1), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 1), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 2) };
        public static Tuple<int, int>[] schemat_gamy_mollowej_harmonicznej = { new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 1), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 1), new Tuple<int, int>(1, 3), new Tuple<int, int>(1, 1) };
        public static Tuple<int, int>[] schemat_gamy_mollowej_doryckiej = { new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 1), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 1) };
        public static Tuple<int, int>[] schemat_gamy_mollowej_melodycznej = { new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 1), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 1),
                                                                              new Tuple<int, int>(-1, 2), new Tuple<int, int>(-1, 2), new Tuple<int, int>(-1, 1),  new Tuple<int, int>(-1, 2), new Tuple<int, int>(-1, 2), new Tuple<int, int>(-1, 1), new Tuple<int, int>(-1, 2)
        
        */
    

        public string tryb;
        public string tonika_nazwa_literowa;


        public ZnakiPrzykluczowe znaki_przykluczowe;
        public string nazwa_tonacji;

        public override string ToString()
        {
            // Generates the text shown in the combo box
            return nazwa_tonacji.PadLeft(8, ' ');
        }

        public Tonacja(string p_nazwa_tonacji)
        {

            nazwa_tonacji = p_nazwa_tonacji;
            int ile_znakow = 0;
            string znak_chromatyczny = "";

            bool czy_znaleziona = false;

            for (int i = 0; i < lista_tonacji_durowych_bez_znakow.Count(); i++)
            {
                if (nazwa_tonacji == lista_tonacji_durowych_bez_znakow[i])
                {
                    czy_znaleziona = true;
                    ile_znakow = 0;
                    znak_chromatyczny = "brak";
                }
            }

            for (int i = 0; i < lista_tonacji_durowych_krzyzykowych.Count(); i++)
            {
                if (nazwa_tonacji == lista_tonacji_durowych_krzyzykowych[i])
                {
                    czy_znaleziona = true;
                    ile_znakow = i + 1;
                    znak_chromatyczny = "krzyzyk";
                }
            }

            for (int i = 0; i < lista_tonacji_durowych_bemolowych.Count(); i++)
            {
                if (nazwa_tonacji == lista_tonacji_durowych_bemolowych[i])
                {
                    czy_znaleziona = true;
                    ile_znakow = i + 1;
                    znak_chromatyczny = "bemol";
                }
            }

            for (int i = 0; i < lista_tonacji_mollowych_bez_znakow.Count(); i++)
            {
                if (nazwa_tonacji == lista_tonacji_mollowych_bez_znakow[i])
                {
                    czy_znaleziona = true;
                    ile_znakow = 0;
                    znak_chromatyczny = "brak";
                }
            }

            for (int i = 0; i < lista_tonacji_mollowych_krzyzykowych.Count(); i++)
            {
                if (nazwa_tonacji == lista_tonacji_mollowych_krzyzykowych[i])
                {
                    czy_znaleziona = true;
                    ile_znakow = i + 1;
                    znak_chromatyczny = "krzyzyk";
                }
            }

            for (int i = 0; i < lista_tonacji_mollowych_bemolowych.Count(); i++)
            {
                if (nazwa_tonacji == lista_tonacji_mollowych_bemolowych[i])
                {
                    czy_znaleziona = true;
                    ile_znakow = i + 1;
                    znak_chromatyczny = "bemol";
                }
            }

            if (!czy_znaleziona) throw new Exception("Nieznana tonacja:" + nazwa_tonacji);

            znaki_przykluczowe = new ZnakiPrzykluczowe(znak_chromatyczny, ile_znakow);

            tonika_nazwa_literowa = (nazwa_tonacji.Split('-')[0]).ToLower();
            tryb = nazwa_tonacji.Split('-')[1];
        }

        public static string NazwaTonacji(string tryb, int nr)
        {
            string nazwa_tonacji = "";
            if (tryb == "dur")
            {
                if (nr == 0) nazwa_tonacji = lista_tonacji_durowych_bez_znakow[0];
                if (nr > 0) nazwa_tonacji = lista_tonacji_durowych_krzyzykowych[nr - 1];
                if (nr < 0) nazwa_tonacji = lista_tonacji_durowych_bemolowych[-nr - 1];
            }
            if (tryb == "moll")
            {
                if (nr == 0) nazwa_tonacji = lista_tonacji_mollowych_bez_znakow[0];
                if (nr > 0) nazwa_tonacji = lista_tonacji_mollowych_krzyzykowych[nr - 1];
                if (nr < 0) nazwa_tonacji = lista_tonacji_mollowych_bemolowych[-nr - 1];
            }
            return nazwa_tonacji;
        }

        public List<Nuta> PodajCiagDzwiekow(string dzwiek_rozpoczynajacy, string nazwa_oktawy, Interwal[] schemat, string wartosc_rytmiczna)
        {
            List<Nuta> lista = new List<Nuta>();
            Nuta aktualny_dzwiek = new Nuta(dzwiek_rozpoczynajacy, nazwa_oktawy, wartosc_rytmiczna);
            lista.Add(aktualny_dzwiek);
            foreach (Interwal interwal in schemat)
            {
                aktualny_dzwiek = aktualny_dzwiek.PrzesunOInterwal(interwal,false);
                lista.Add(aktualny_dzwiek);
            }

            return lista;
        }

    }

    public class ZnakDodatkowy : ObiektNutowy
    {
        public string rodzaj;
        public int poczatek_z;
        public int koniec_z;

        public ZnakDodatkowy(string p_rodzaj, int p_poczatek_z, int p_koniec_z)
        {
            rodzaj = p_rodzaj;
            poczatek_z = p_poczatek_z;
            koniec_z = p_koniec_z;
        }
    }

    public class OdstepPoziomy: ObiektNutowy
    {
        public float dlugosc_i;
        public OdstepPoziomy(int p_dlugosc_i)
        {
            dlugosc_i = p_dlugosc_i;
        }
    }


    public class ZnakiPrzykluczowe : ObiektNutowy
    {

        public static string[] kolejne_krzyzyki = { "f", "c", "g", "d", "a", "e", "h" };
        public static string[] kolejne_bemole = { "h", "e", "a", "d", "g", "c", "f" };

        public Hashtable znaki = new Hashtable();
        public string znak_chromatyczny;
        public int liczba_znakow;


        public ZnakiPrzykluczowe(string p_znak_chromatyczny, int p_ile_znakow)
        {
            znak_chromatyczny = p_znak_chromatyczny;
            liczba_znakow = p_ile_znakow;
            if (liczba_znakow > 7)
            {
                throw new Exception("Nieznana liczba znakow: " + liczba_znakow);
            }

            for (int i = 0; i < liczba_znakow; i++)
            {
                if (znak_chromatyczny == "krzyzyk")
                    znaki.Add(kolejne_krzyzyki[i], "krzyzyk");
                else znaki.Add(kolejne_bemole[i], "bemol");
            }
            odstep_po_i = Parametry.GrafikaNut.odstep_po_znakach_przykluczowych_i;
        }
    }

    public class WierszArkusza
    {
        public int margines_lewy_i = 0;
        public int margines_prawy_i = 0;
        public int margines_gorny_i = 0;
        public int margines_dolny_i = 0;
        
    }

    public class WierszTekstowy : WierszArkusza
    {

        public string tekst;
        public float czcionka_rozmiar_i;
        public string wyrownanie;
        public string czcionka_nazwa;

        public WierszTekstowy(string p_tekst, string p_czcionka_nazwa, float p_czcionka_rozmiar_i, string p_wyrownanie)
        {
            tekst = p_tekst;
            czcionka_rozmiar_i = p_czcionka_rozmiar_i;
            wyrownanie = p_wyrownanie;
            czcionka_nazwa = p_czcionka_nazwa;
        }
    }

    public class WierszNutowy : WierszArkusza
    {
        public float interlinia_wspolczynnik = 1;
        public float grubosc_linii_wspolczynnik = 1;
        public float wysokosc_podpisow_l ;
        public int grubosc_linii = 2; //TODO

        public float odstep_przed_pierwszym_obiektem_i = Parametry.GrafikaNut.odstep_przed_pierwszym_obiektem_i;

        public int liczba_linii_wlasciwych = 5;
        //public int liczba_pol_dodatkowych_gora = Parametry.UkladArkusza.liczba_pol_dodatkowych_gora;
       // public int liczba_pol_dodatkowych_dol = Parametry.UkladArkusza.liczba_pol_dodatkowych_dol;
        public List<ObiektNutowy> lista_obiektow_nutowych;
        public List<ZnakDodatkowy> lista_znakow_dodatkowych;

        public void DodajDziubkiPolnutowe()
        {
            bool czy_poprzedni_nuta = false;
            bool czy_obecny_nuta = false;
            for (int i = 0; i < lista_obiektow_nutowych.Count(); i++)
            {
                czy_poprzedni_nuta = czy_obecny_nuta;
                Type t = lista_obiektow_nutowych[i].GetType();
                if (t.Equals(typeof(Nuta)))
                {
                    czy_obecny_nuta = true;
                }
                else czy_obecny_nuta = false;

                if (czy_poprzedni_nuta && czy_obecny_nuta)
                {
                    Nuta nuta1 = (Nuta)lista_obiektow_nutowych[i - 1];
                    Nuta nuta2 = (Nuta)lista_obiektow_nutowych[i];
                    if (nuta1.OdlegloscWPoltonach(nuta2) == 1)
                        lista_znakow_dodatkowych.Add(new ZnakDodatkowy("dziubek_poltonowy", i - 1, i));
                }
            }
        }


        public WierszNutowy()
        {
            lista_obiektow_nutowych = new List<ObiektNutowy>();
            lista_znakow_dodatkowych = new List<ZnakDodatkowy>();
        }

        public void Dodaj(ObiektNutowy obiekt_nutowy)
        {
            lista_obiektow_nutowych.Add(obiekt_nutowy);
        }

        public void Dodaj(List<Nuta> p_lista_obiektow_nutowych, string rodzaj_podpisu)
        {
            foreach (Nuta nuta in p_lista_obiektow_nutowych)
            {
                nuta.rodzaj_podpisu = rodzaj_podpisu;
                lista_obiektow_nutowych.Add(nuta);
            }
        }


        /*
        public void UstawWysokoscPodpisu()
                {
                    float min_wysokosc_l = 1.5f;
                    Klucz klucz = null;
                    foreach (ObiektNutowy obiekt in lista_obiektow_nutowych)
                    {

                        Type t = obiekt.GetType();
                        if (t.Equals(typeof(Nuta)))
                        {
                            Nuta nuta = (Nuta)obiekt;
                            if (nuta.NumerLinii(klucz) < min_wysokosc_l)
                                min_wysokosc_l = nuta.NumerLinii(klucz);
                        }
                        if (t.Equals(typeof(Klucz)))
                        {
                            klucz = (Klucz)obiekt;
                        }
                    }
                    this.wysokosc_podpisow_l = min_wysokosc_l - 1;

                }
        */
    }


    public class ObiektNutowy
    {
        public bool czy_obiekt_wyswietlany = true;

        public float wspolczynnik_odstepu_po = 1;
        public float odstep_po_i;
        public Color kolor_podpisu = Parametry.GrafikaNut.kolor_podpisu;
        public Color kolor_tla = Parametry.Panel.tlo;
        public string font_podpis = Parametry.GrafikaNut.font_podpis_nuty_zwykly;
        public Color kolor = Color.Black;//Color.Purple;
        public ObiektNutowy()
        {

        }
    }




    public class KreskaTaktowa : ObiektNutowy
    {
        public string kod;
        public KreskaTaktowa(string p_kod, float p_wspolczynnik_odstepu_po = 1)
        {
            kod = p_kod;
            wspolczynnik_odstepu_po = p_wspolczynnik_odstepu_po;
            odstep_po_i = Parametry.GrafikaNut.odstep_po_kresce_taktowej_i;
        }
    }


    public class Oktawa
    {
        public int nr;
        public string nazwa;
        public string nazwa_nut;


        public Oktawa(int pnr, string pnazwa, string pnazwa_nut)
        {
            nr = pnr;
            nazwa = pnazwa;
            nazwa_nut = pnazwa_nut;
        }
    }



    public class Interwal
    {
        public string kod;
        public string symbol;
        public int liczba_stopni;
        public int liczba_poltonow;
        public string pelna_nazwa;
        public string nazwa;
        public string rodzaj;
        public int kierunek;
        public int lp;

        public string nazwa_wyswietlana;

        public Interwal Clone(int p_kierunek)
        {            
            return new Interwal(nazwa,rodzaj, liczba_stopni, liczba_poltonow, p_kierunek, symbol,lp);            
        }

        public Interwal(string p_nazwa, string p_rodzaj, int p_liczba_stopni, int p_liczba_poltonow, int p_kierunek, string p_symbol, int p_lp)
        {
            if (p_liczba_stopni < 0) throw new Exception("ujemna liczba stopni");
            if (p_liczba_poltonow < 0)throw new Exception("ujemna liczba poltonow");
            if (p_liczba_stopni==1 && p_liczba_poltonow==0 && p_kierunek!=0) throw new Exception("Kierunek dla prymy czystej musi być równy 0");
            liczba_stopni = Math.Abs(p_liczba_stopni);
            liczba_poltonow = p_liczba_poltonow;
            kierunek = 1;
            nazwa = p_nazwa;
            rodzaj = p_rodzaj;
            symbol = p_symbol;
            kierunek = p_kierunek;
            pelna_nazwa = p_nazwa + " " + p_rodzaj;
            kod = p_liczba_stopni + "_" + p_liczba_poltonow;
            lp = p_lp;              
            switch(kierunek)
            {
                case 1: nazwa_wyswietlana = symbol + "" + char.ConvertFromUtf32(0x2191);break;
                case -1: nazwa_wyswietlana = symbol + "" + char.ConvertFromUtf32(0x2193); break;
                case 0: nazwa_wyswietlana = symbol;break;
                default: throw new Exception("nieprawidłowy kierunek");
            }            
        }

        public static List<Interwal> interwaly = new List<Interwal>()
           {
            new Interwal("pryma","czysta",1,0,0,"1",3),
            new Interwal("pryma","zwiększona",1,1,1,"1<",4),

            new Interwal("sekunda","zmniejszona",2,0,1,"2>>",1),
            new Interwal("sekunda","mała",2,1,1,"2>",2),
            new Interwal("sekunda","wielka",2,2,1,"2",3),
            new Interwal("sekunda","zwiększona",2,3,1,"2<",4),

            new Interwal("tercja","zmniejszona",3,2,1,"3>>",1),
            new Interwal("tercja","mała",3,3,1,"3>",2),
            new Interwal("tercja","wielka",3,4,1,"3",3),
            new Interwal("tercja","zwiększona",3,5,1,"3<",4),

            new Interwal("kwarta","zmniejszona",4,4,1,"4>",1),
            new Interwal("kwarta","czysta",4,5,1,"4",2),
            new Interwal("kwarta","zwiększona",4,6,1,"4<",3),

            
            new Interwal("kwinta","zmniejszona",5,6,1,"5>",2),
            new Interwal("kwinta","czysta",5,7,1,"5",3),
            new Interwal("kwinta","zwiększona",5,8,1,"5<",4),

            new Interwal("seksta","zmniejszona",6,7,1,"6>>",1),
            new Interwal("seksta","mała",6,8,1,"6>",2),
            new Interwal("seksta","wielka",6,9,1,"6",3),
            new Interwal("seksta","zwiększona",6,10,1,"6<",4),

            new Interwal("septyma","zmniejszona",7,9,1,"7>",1),
            new Interwal("septyma","mała",7,10,1,"7",2),
            new Interwal("septyma","wielka",7,11,1,"7<",3),
            new Interwal("septyma","zwiększona",7,12,1,"7<<",4),

            new Interwal("oktawa","zmniejszona",8,11,1,"8>",2),
            new Interwal("oktawa","czysta",8,12,1,"8",3),
            new Interwal("oktawa","zwiększona",8,13,1,"8<",4)
        };


        public static int getLiczbaPoltonow(string p_kod)
        {
            Interwal interwal = null;
            int wynik = -1;

            foreach (Interwal i in interwaly)
            {
                if (i.kod == p_kod)
                {
                    wynik = i.liczba_poltonow;
                    break;
                }

            }
            if (wynik == -1)
                throw new Exception("Nieznany interwał");
            return wynik;
        }



        public static Interwal getInterwal(string p_kod,int p_kierunek)
        {
            Interwal interwal=null;

            foreach (Interwal i  in interwaly)
            {
                if (i.kod == p_kod)
                {
                    interwal = i.Clone(p_kierunek);
                    break;
                }

            }
            if (interwal==null)
                throw new Exception("Nieznany interwał");
            return interwal;
        }

        public static Interwal getInterwal(int p_liczba_stopni, int p_liczba_poltonow,int p_kierunek)
        {

            Interwal interwal = null;                    
            foreach (Interwal i in interwaly)
            {
                if (i.liczba_stopni == p_liczba_stopni && i.liczba_poltonow == p_liczba_poltonow)
                {
                    interwal = i.Clone(p_kierunek);
                    break;
                }

            }

            if (interwal == null)
                throw new Exception("Nieznany interwał");
            return interwal;
        }

        

        public static Interwal getInterwal(string p_nazwa, string p_rodzaj, int p_kierunek)
        {

            Interwal interwal = null;
            foreach (Interwal i in interwaly)
            {
                if (i.nazwa == p_nazwa && i.rodzaj == p_rodzaj)
                {
                    interwal = i.Clone(p_kierunek);
                    break;
                }

            }
            if (interwal == null)
                throw new Exception("Nieznany interwał");
            return interwal;
        }


    }



    public class Klucz : ObiektNutowy
    {
        public string kod;
        public string symbol_kod;
        public string pelna_nazwa;
        public ZnakiPrzykluczowe znaki_przykluczowe;
        public Klucz(string p_kod, Tonacja tonacja, float p_wspolczynnik_odstepu_po = 1)
        {
            kod = p_kod;
            switch (kod)
            {
                case "wiol": pelna_nazwa = "wiolinowy"; symbol_kod = "klucz_g"; break;
                case "bas": pelna_nazwa = "basowy"; symbol_kod = "klucz_f"; break;
                default: throw new Exception("Nieznany klucz:" + p_kod);
            }
            wspolczynnik_odstepu_po = p_wspolczynnik_odstepu_po;
            odstep_po_i = Parametry.GrafikaNut.odstep_po_kluczu_i;
            znaki_przykluczowe = tonacja.znaki_przykluczowe;
        }
    }



    public class WartoscRytmiczna
    {
        public string nazwa;
        public float odwrotnosc_dlugosci;
        public float dlugosc;
        public Obrazek obrazek1;
        public Obrazek obrazek2;

        public static Hashtable wartosci = new Hashtable()
         {
           { "cala_nuta",new WartoscRytmiczna {nazwa="cala_nuta", odwrotnosc_dlugosci=1,dlugosc=1,obrazek1=(Obrazek)Rysowanie.obrazki["cala_nuta"],obrazek2=(Obrazek)Rysowanie.obrazki["cala_nuta"]}  },
           { "polnuta",new WartoscRytmiczna { nazwa="polnuta", odwrotnosc_dlugosci=2,dlugosc=0.5f,obrazek1=(Obrazek)Rysowanie.obrazki["polnuta_w_gore"],obrazek2=(Obrazek)Rysowanie.obrazki["polnuta_w_dol"]} },
           { "cwiercnuta",new WartoscRytmiczna { nazwa="cwiercnuta", odwrotnosc_dlugosci=4,dlugosc=0.25f,obrazek1=(Obrazek)Rysowanie.obrazki["cwiercnuta_w_gore"],obrazek2=(Obrazek)Rysowanie.obrazki["cwiercnuta_w_dol"]} },
           { "osemka",new WartoscRytmiczna { nazwa="osemka", odwrotnosc_dlugosci=8,dlugosc=0.125f,obrazek1=(Obrazek)Rysowanie.obrazki["osemka_w_gore"],obrazek2=(Obrazek)Rysowanie.obrazki["osemka_w_dol"]} },
           { "szesnastka",new WartoscRytmiczna {nazwa="szesnastka",  odwrotnosc_dlugosci=16,dlugosc=1/16.0f,obrazek1=(Obrazek)Rysowanie.obrazki["szesnastka_w_gore"],obrazek2=(Obrazek)Rysowanie.obrazki["szesnastka_w_dol"]} },
         };

        public static WartoscRytmiczna wartosc(string p_nazwa)
        {
            WartoscRytmiczna wartosc = (WartoscRytmiczna)(wartosci[p_nazwa]);
            if (wartosc is null) throw new Exception("Nieznana wartość rytmiczna");
            return wartosc;
        }
    }

    
    public class Nuta:ObiektNutowy
    {

         static string[] nazwy_literowe = new string[7] { "c", "d", "e", "f", "g", "a", "h" };
         static string[] nazwy_literowe_bemol = new string[7] { "ces", "des", "es", "fes", "ges", "as", "b" };
         static string[] nazwy_literowe_podwojny_bemol = new string[7] { "ceses", "deses", "eses", "feses", "geses", "asas", "heses" };
         static string[] nazwy_literowe_krzyzyk = new string[7] { "cis", "dis", "eis", "fis", "gis", "ais", "his" };
         static string[] nazwy_literowe_podwojny_krzyzyk = new string[7] { "cisis", "disis", "eisis", "fisis", "gisis", "aisis", "hisis" };
        private static string[] nazwy_solmizacyjne = new string[7] { "do", "re", "mi", "fa", "sol", "la", "si" };

        public static Oktawa[] oktawy = new Oktawa[9] {
            new Oktawa(0,"subkontra","subkontra"),
            new Oktawa(1,"kontra","kontra"),
            new Oktawa(2,"wielka","wielkie"),
            new Oktawa(3,"mała","małe"),
            new Oktawa(4,"razkreślna","razkreślne"),
            new Oktawa(5,"dwukreślna","dwukreślne"),
            new Oktawa(6,"trzykreślna","trzykreślne"),
            new Oktawa(7,"czterokreślna","czterokreślne"),
            new Oktawa(8,"pięciokreślna","pięciokreślne")
        };

        public int nr { get; set; } 
        public string pelna_nazwa { get; set; }
        public string bazowa_nazwa_literowa;
        public string nazwa_literowa;        
        public string nazwa_solmizacyjna;        
        public int oktawa_nr;
        public Oktawa oktawa;        
        public string znak_chromatyczny ;
        public WartoscRytmiczna wartosc_rytmiczna;
        public int nr_w_oktawie;
        public int odleglosc_do_nastepnego;
        public int odleglosc_do_poprzedniego;
        public float rozmiar_podpisu_i= 1.1f;



        public string rodzaj_podpisu="brak";
        public string podpis_inny = null;



        

        public void UstawWartosci(int p_nr, string p_wartosc_rytmiczna,  float p_wspolczynnik_odstepu_po , string p_znak_chromatyczny)
        {
            nr = p_nr;            
            znak_chromatyczny = p_znak_chromatyczny;
            wartosc_rytmiczna = WartoscRytmiczna.wartosc(p_wartosc_rytmiczna);

            wspolczynnik_odstepu_po = p_wspolczynnik_odstepu_po;
            odstep_po_i = Parametry.GrafikaNut.odstep_po_nucie_i;

            oktawa_nr = oktawy[(nr + 5) / 7].nr;
            oktawa = Nuta.oktawy[oktawa_nr];
            nr_w_oktawie = (nr + 5) % 7;
            

            string[] zestaw_nazw = null;

            switch (p_znak_chromatyczny)
            {
                case "brak":
                    zestaw_nazw = nazwy_literowe;
                    break;
                case "krzyzyk":
                    zestaw_nazw = nazwy_literowe_krzyzyk;
                    break;
                case "bemol":
                    zestaw_nazw = nazwy_literowe_bemol;
                    break;
                case "podwojny_krzyzyk":
                    zestaw_nazw = nazwy_literowe_podwojny_krzyzyk;
                    break;
                case "podwojny_bemol":
                    zestaw_nazw = nazwy_literowe_podwojny_bemol;
                    break;
            }
            
            nazwa_literowa = zestaw_nazw[nr_w_oktawie];
            bazowa_nazwa_literowa = nazwy_literowe[nr_w_oktawie];
            nazwa_solmizacyjna = nazwy_solmizacyjne[nr_w_oktawie];
            pelna_nazwa = nazwa_literowa + " " + oktawy[oktawa_nr].nazwa_nut;

            if (nr_w_oktawie == 2 || nr_w_oktawie == 6)
                odleglosc_do_nastepnego = 1;
            else odleglosc_do_nastepnego = 2;

            if (nr_w_oktawie == 3 || nr_w_oktawie == 0)
                odleglosc_do_poprzedniego = 1;
            else odleglosc_do_poprzedniego = 2;
        }




        public Nuta(string p_nazwa_literowa, string p_oktawa_nazwa, string p_wartosc_rytmiczna="cala_nuta", float p_wspolczynnik_odstepu_po = 1)
        {

            UstawWartosci(NumerNuty(p_nazwa_literowa, p_oktawa_nazwa),
                          p_wartosc_rytmiczna,
                          p_wspolczynnik_odstepu_po,
                          ZnakChromatycznyDzwieku(p_nazwa_literowa));

        }


        public Nuta(int p_nr, string p_wartosc_rytmiczna = "cala_nuta", string p_znak_chromatyczny = "brak",float p_wspolczynnik_odstepu_po=1)

        {
            UstawWartosci(p_nr, p_wartosc_rytmiczna, p_wspolczynnik_odstepu_po, p_znak_chromatyczny);
        }


        public Nuta NutaPrzesun(Nuta nuta, int ile)
        {
            return new Nuta(nuta.nr + ile, nuta.wartosc_rytmiczna.nazwa, nuta.znak_chromatyczny);
        }

        public float NumerLinii(Klucz klucz)
        {
            float nr_linii=0;
            switch (klucz.kod)
            {
                case "wiol":
                    nr_linii=(nr - 23) / 2.0f;
                    break;
                case "bas":
                    nr_linii = (nr - 23) / 2.0f+6f;
                    break;
            }
            return nr_linii;
        }

        public Klawisz getKlawisz()
        {
            int nuta_efektywna_bazowa_nr = -1;
            bool nuta_efektywna_podwyzszenie = false;
            switch (znak_chromatyczny)
            {
                case "brak":
                      nuta_efektywna_bazowa_nr = nr;
                      nuta_efektywna_podwyzszenie = false;
                      break;
                case "krzyzyk": if(bazowa_nazwa_literowa=="e" || bazowa_nazwa_literowa == "h")
                    {
                        nuta_efektywna_bazowa_nr = nr + 1;
                        nuta_efektywna_podwyzszenie = false;
                    }
                else
                    {
                        nuta_efektywna_bazowa_nr = nr;
                        nuta_efektywna_podwyzszenie = true;
                    }
                    break;
                case "podwojny_krzyzyk":
                    if (bazowa_nazwa_literowa == "e" || bazowa_nazwa_literowa == "h")
                    {
                        nuta_efektywna_bazowa_nr = nr + 1;
                        nuta_efektywna_podwyzszenie = true;
                    }
                    else
                    {
                        nuta_efektywna_bazowa_nr = nr+1;
                        nuta_efektywna_podwyzszenie = false;
                    }                    
                    break;
                case "bemol":
                    if (bazowa_nazwa_literowa == "f" || bazowa_nazwa_literowa == "c")
                    {
                        nuta_efektywna_bazowa_nr = nr - 1;
                        nuta_efektywna_podwyzszenie = false;
                    }
                    else
                    {
                        nuta_efektywna_bazowa_nr = nr-1;
                        nuta_efektywna_podwyzszenie = true;
                    }
                    break;
                case "podwojny_bemol":
                    if (bazowa_nazwa_literowa == "f" || bazowa_nazwa_literowa == "c")
                    {
                        nuta_efektywna_bazowa_nr = nr - 2;
                        nuta_efektywna_podwyzszenie = true;
                    }
                    else
                    {
                        nuta_efektywna_bazowa_nr = nr - 1;
                        nuta_efektywna_podwyzszenie = false;
                    }
                    break;
            }

            int nuta_efektywna_nr_oktawy = (nuta_efektywna_bazowa_nr + 5) / 7;
            int nuta_efektywna_nr_w_oktawie = (nuta_efektywna_bazowa_nr + 5) % 7;

            int klawisz_nr=-1;
            switch(nuta_efektywna_nr_w_oktawie)
            {
                case 0:
                    klawisz_nr = 0;break;
                case 1:
                    klawisz_nr = 2; break;
                case 2:
                    klawisz_nr = 4; break;
                case 3:
                    klawisz_nr = 5; break;
                case 4:
                    klawisz_nr = 7; break;
                case 5:
                    klawisz_nr = 9; break;
                case 6:
                    klawisz_nr = 11; break;            
            }
            if (nuta_efektywna_podwyzszenie) klawisz_nr++;
            return new Klawisz(nuta_efektywna_nr_oktawy, klawisz_nr);
        }


        public float NumerLiniiEfektywny(Klucz klucz, ref int przenosnik)
        {
            float numerLinii = NumerLinii(klucz);

            if (klucz.kod == "wiol" && numerLinii >= 9.5f)
            {
                numerLinii = numerLinii - 3.5f;
                przenosnik = 1;
            }

            if (klucz.kod == "bas" && numerLinii <= -5f)
            {
                numerLinii = numerLinii + 3.5f;
                przenosnik = -1;
            }
            return numerLinii;
        }

        public static int NumerOktawy(string p_nazwa)
        {
            int nr=-1;
            for (int i = 0; i < oktawy.Length; i++)
                if (oktawy[i].nazwa == p_nazwa) nr = i;

            if (nr==-1) throw new Exception("Nieznana oktawa:" + p_nazwa);
            return nr;
        }

        public static int NumerDzwiekuWGamie(string p_nazwa_literowa)
        {
            int nr_w_gamie = -1;
            for (int i = 0; i < nazwy_literowe.Length; i++)
            {
                if (nazwy_literowe[i] == p_nazwa_literowa)
                {
                    nr_w_gamie = i;
                }
            }

            for (int i = 0; i < nazwy_literowe_krzyzyk.Length; i++)
            {
                if (nazwy_literowe_krzyzyk[i] == p_nazwa_literowa)
                {
                    nr_w_gamie = i;
                }
            }


            for (int i = 0; i < nazwy_literowe_bemol.Length; i++)
            {
                if (nazwy_literowe_bemol[i] == p_nazwa_literowa)
                {
                    nr_w_gamie = i;
                }
            }

            for (int i = 0; i < nazwy_literowe_podwojny_krzyzyk.Length; i++)
            {
                if (nazwy_literowe_podwojny_krzyzyk[i] == p_nazwa_literowa)
                {
                    nr_w_gamie = i;
                }
            }


            for (int i = 0; i < nazwy_literowe_podwojny_bemol.Length; i++)
            {
                if (nazwy_literowe_podwojny_bemol[i] == p_nazwa_literowa)
                {
                    nr_w_gamie = i;
                }
            }

            if (nr_w_gamie == -1) throw new Exception("Nieznana nazwa literowa:" + p_nazwa_literowa);
            return nr_w_gamie;
        }

        public static string ZnakChromatycznyDzwieku(string p_nazwa_literowa)
        {
            string znak_chromatyczny = "";
            for (int i = 0; i < nazwy_literowe.Length; i++)
            {
                if (nazwy_literowe[i] == p_nazwa_literowa)
                {
                    znak_chromatyczny = "brak";
                }
            }

            for (int i = 0; i < nazwy_literowe_krzyzyk.Length; i++)
            {
                if (nazwy_literowe_krzyzyk[i] == p_nazwa_literowa)
                {
                    znak_chromatyczny = "krzyzyk";
                }
            }


            for (int i = 0; i < nazwy_literowe_bemol.Length; i++)
            {
                if (nazwy_literowe_bemol[i] == p_nazwa_literowa)
                {
                    znak_chromatyczny = "bemol";
                }
            }


            for (int i = 0; i < nazwy_literowe_podwojny_krzyzyk.Length; i++)
            {
                if (nazwy_literowe_podwojny_krzyzyk[i] == p_nazwa_literowa)
                {
                    znak_chromatyczny = "podwojny_krzyzyk";
                }
            }


            for (int i = 0; i < nazwy_literowe_podwojny_bemol.Length; i++)
            {
                if (nazwy_literowe_podwojny_bemol[i] == p_nazwa_literowa)
                {
                    znak_chromatyczny = "podwojny_bemol";
                }
            }

            if (znak_chromatyczny == "") throw new Exception("Nieznana nazwa literowa:" + p_nazwa_literowa);
            return znak_chromatyczny;
        }


        public static int NumerNuty(string nazwa_literowa,string oktawa)
        {
            return (NumerOktawy(oktawa)-1) * 7+ NumerDzwiekuWGamie(nazwa_literowa)+ 2;
        }

        public Nuta PrzesunOInterwal(Interwal interwal, bool null_if_impossible)
        {            

            Nuta nuta_przesunieta=new Nuta(this.nr+interwal.kierunek*(interwal.liczba_stopni-1), this.wartosc_rytmiczna.nazwa);
            int odleglosc_w_poltonach = OdlegloscWPoltonach(nuta_przesunieta);
            string znak_chromatyczny = "";
            int kierunek;
            if (interwal.liczba_poltonow != 0) kierunek = interwal.kierunek;   
              else kierunek = Math.Sign(odleglosc_w_poltonach); 
            bool impossible = false;                   
               
            //interwal.liczba_poltonow - kierunek * odleglosc_w_poltonach
            switch (kierunek*interwal.liczba_poltonow - odleglosc_w_poltonach)
            {
                case 0: znak_chromatyczny = "brak";break;
                case 1: znak_chromatyczny = "krzyzyk"; break;
                case 2: znak_chromatyczny = "podwojny_krzyzyk"; break;
                case -1: znak_chromatyczny = "bemol"; break;
                case -2: znak_chromatyczny = "podwojny_bemol"; break;
                default:
                    if (null_if_impossible) impossible = true;
                    else  throw new Exception("zbyt duza odleglosc");
                    break;
            }
            if (impossible) nuta_przesunieta = null;
            else nuta_przesunieta = new Nuta(nuta_przesunieta.nr, this.wartosc_rytmiczna.nazwa, znak_chromatyczny, this.wspolczynnik_odstepu_po);
            return nuta_przesunieta;
        }

        public Interwal InterwalDoNuty(Nuta nuta2)
        {
            int liczba_stopni;
            liczba_stopni = nuta2.nr - this.nr + 1;

            if (nuta2.nr >= this.nr) liczba_stopni = nuta2.nr - this.nr + 1;
            else liczba_stopni = this.nr - nuta2.nr + 1;

            int odlegloscWPoltonach = this.OdlegloscWPoltonach(nuta2);
            int liczba_poltonow= Math.Abs(odlegloscWPoltonach);

            int kierunek=0;
            if (nuta2.nr > this.nr)
            {
                if (odlegloscWPoltonach < 0) throw new Exception("nieprawidłowy interwał");
                kierunek = 1;
            }
            else
            {
                if (nuta2.nr < this.nr)
                {
                    if (odlegloscWPoltonach > 0) throw new Exception("nieprawidłowy interwał");
                    kierunek = -1;

                }
                else
                {
                    if (odlegloscWPoltonach > 0) kierunek = 1;
                    if (odlegloscWPoltonach == 0) kierunek = 0;
                    if (odlegloscWPoltonach < 0) kierunek = -1;
                }
            }

            Interwal interwal =Interwal.getInterwal(liczba_stopni, liczba_poltonow,kierunek);
            return interwal;
        }

        public int OdlegloscWPoltonach(Nuta dzwiek)
        {
            int odleglosc=0;
            Nuta nuta1;
            Nuta nuta2;
            int znak = 0;
            if (nr<=dzwiek.nr)
            {
                nuta1 = this;
                nuta2 = dzwiek;
                znak = 1;
            }
            else
            {
                nuta1 = dzwiek;
                nuta2 = this;
                znak = -1;
            }

            switch(nuta1.znak_chromatyczny)
            {
                case "krzyzyk": odleglosc -= 1;break;
                case "podwojny_krzyzyk": odleglosc -= 2; break;
                case "bemol": odleglosc += 1; break;
                case "podwojny_bemol": odleglosc += 2; break;
            }

            switch (nuta2.znak_chromatyczny)
            {
                case "krzyzyk": odleglosc += 1; break;
                case "podwojny_krzyzyk": odleglosc += 2; break;
                case "bemol": odleglosc -= 1; break;
                case "podwojny_bemol": odleglosc -= 2; break;
            }

            for (int i=nuta1.nr;i<nuta2.nr;i++)
            {
                odleglosc += nuta1.odleglosc_do_nastepnego;
                nuta1 = new Nuta(nuta1.nr + 1, this.wartosc_rytmiczna.nazwa);
            }


            

            return odleglosc*znak;
        }

        public static float OIleTonowZmienia(string znak_chromatyczny)
        {
            float wynik = 0;
            switch(znak_chromatyczny)
            {
                case ("brak"): wynik = 0;break;
                case ("bemol"): wynik = -0.5f; break;
                case ("podwojny_bemol"): wynik = -1; break;
                case ("krzyzyk"): wynik = 0.5f; break;
                case ("podwojny_krzyzyk"): wynik = 1; break;
                default: throw new Exception("Nieznany znak chromatyczny: " + znak_chromatyczny);
            }
            return wynik;
        }

    }

    

    public class AktualneZnakiChromatyczne
    {
        public Hashtable znaki;
        public AktualneZnakiChromatyczne(Klucz klucz)
        {
            znaki = new Hashtable();
            
            for (int nr=Parametry.Konfiguracja.nuta_min;nr<= Parametry.Konfiguracja.nuta_max; nr++)
            {
                Nuta nuta = new Nuta(nr);
                string znak_chromatyczny = (string)(klucz.znaki_przykluczowe.znaki[nuta.bazowa_nazwa_literowa]);
                if (znak_chromatyczny == null) znak_chromatyczny = "brak";
                znaki[nr] = znak_chromatyczny;
            }
        }
    }

    public class Metrum
    {
        public string kod;
        public int wartosc_gorna;
        public int wartosc_dolna;

        public static string[] lista_metrum=new string[] {"1/1","2/1","3/1","4/1","5/1"};

        public Metrum(string p_kod)
        {
            kod = p_kod;
            wartosc_gorna= Int32.Parse(p_kod.Split('/')[0]);
            wartosc_dolna= Int32.Parse(p_kod.Split('/')[1]);
        }


    }

}



