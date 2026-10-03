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


    public class KształtKlawisza
    {
        public string kod;
        public string rodzaj;
        public bool wciecie_lewe;
        public bool wciecie_prawe;
        public string polowki;

        public void UstawDane(string p_kod, string p_rodzaj, bool p_wciecie_lewe, bool p_wciecie_prawe, string p_polowki)
        {
            kod = p_kod;
            rodzaj = p_rodzaj;
            wciecie_lewe = p_wciecie_lewe;
            wciecie_prawe = p_wciecie_prawe;
            polowki = p_polowki;
        }


        public KształtKlawisza(string p_kod)
        {
            switch (p_kod)
            {
                case "L":
                    UstawDane(p_kod, "biały", true, false, "LP"); break;
                case "P":
                    UstawDane(p_kod, "biały", false, true, "LP"); break;
                case "LP":
                    UstawDane(p_kod, "biały", true, true, "LP"); break;
                case "CZ":
                    UstawDane(p_kod, "czarny", false, false, "LP"); break;
                default:
                    throw new Exception("Nieznany kod" + p_kod);
            }


        }

    }

    public class Klawisz
    {

        public int klawisz_nr;

        public KształtKlawisza kształtKlawisza;
        public float nuta_nr;
        public int oktawa_nr;
        public int nr_klawisza_w_oktawie;
        public Oktawa oktawa;

        /*
        public string nazwa_krzyzykowa;
        public string nazwa_bemolowa;
        public string symbol_krzyzykowy;
        public string symbol_bemolowy;
        */


        public string font_nazwa = Parametry.Klawiatura.font_nazwa;

        public void UstawWartosci(int p_oktawa_nr, int p_nr_w_oktawie)
        {
            oktawa_nr = p_oktawa_nr;
            nr_klawisza_w_oktawie = p_nr_w_oktawie;

            oktawa = Nuta.oktawy[oktawa_nr];
            klawisz_nr = 3 + 12 * (oktawa_nr - 1) + p_nr_w_oktawie;

            switch (nr_klawisza_w_oktawie)
            {
                case 0:
                    kształtKlawisza = new KształtKlawisza("P");
                    nuta_nr = 2 + 7 * (oktawa_nr - 1);
                    //nazwa_krzyzykowa = "c";
                    break;
                case 1:
                    kształtKlawisza = new KształtKlawisza("CZ");
                    nuta_nr = 2.5f + 7 * (oktawa_nr - 1);
                   // nazwa_krzyzykowa = "cis";
                    break;
                case 2:
                    kształtKlawisza = new KształtKlawisza("LP");
                    nuta_nr = 3f + 7 * (oktawa_nr - 1);
                    //nazwa_krzyzykowa = "d";
                    break;
                case 3:
                    kształtKlawisza = new KształtKlawisza("CZ");
                    nuta_nr = 3.5f + 7 * (oktawa_nr - 1);
                    //nazwa_krzyzykowa = "dis";
                    break;
                case 4:
                    kształtKlawisza = new KształtKlawisza("L");
                    nuta_nr = 4f + 7 * (oktawa_nr - 1);
                    //nazwa_krzyzykowa = "e";
                    break;
                case 5:
                    kształtKlawisza = new KształtKlawisza("P");
                    nuta_nr = 5f + 7 * (oktawa_nr - 1);
                    //nazwa_krzyzykowa = "f";
                    break;
                case 6:
                    kształtKlawisza = new KształtKlawisza("CZ");
                    nuta_nr = 5.5f + 7 * (oktawa_nr - 1);
                    //nazwa_krzyzykowa = "fis";
                    break;
                case 7:
                    kształtKlawisza = new KształtKlawisza("LP");
                    nuta_nr = 6f + 7 * (oktawa_nr - 1);
                    //nazwa_krzyzykowa = "g";
                    break;
                case 8:
                    kształtKlawisza = new KształtKlawisza("CZ");
                    nuta_nr = 6.5f + 7 * (oktawa_nr - 1);
                   // nazwa_krzyzykowa = "gis";
                    break;
                case 9:
                    kształtKlawisza = new KształtKlawisza("LP");
                    nuta_nr = 7f + 7 * (oktawa_nr - 1);
                    //nazwa_krzyzykowa = "a";
                    break;
                case 10:
                    kształtKlawisza = new KształtKlawisza("CZ");
                    nuta_nr = 7.5f + 7 * (oktawa_nr - 1);
                    //nazwa_krzyzykowa = "ais";
                    break;
                case 11:
                    kształtKlawisza = new KształtKlawisza("L");
                    nuta_nr = 8f + 7 * (oktawa_nr - 1);
                    //nazwa_krzyzykowa = "h";
                    break;
            }

        }
        public Klawisz(int p_klawisz_nr)
        {
            oktawa_nr = (p_klawisz_nr + 9) / 12;
            nr_klawisza_w_oktawie = (p_klawisz_nr + 9) % 12;
            UstawWartosci(oktawa_nr, nr_klawisza_w_oktawie);
        }

        public Klawisz(int p_oktawa_nr, int p_nr_klawisza_w_oktawie)
        {
            UstawWartosci(p_oktawa_nr, p_nr_klawisza_w_oktawie);
        }

        public Nuta getNuta(string rodzaj)
        {
            Nuta nuta;            
            if (nuta_nr == (int)(nuta_nr))
            {                
                nuta=new Nuta((int)nuta_nr);
            }
            else
            {
                switch (rodzaj)
                {
                    case "bemol":
                        nuta = new Nuta((int)nuta_nr+1,"cala_nuta","bemol");break;
                    case "krzyżyk":
                        nuta = new Nuta((int)nuta_nr, "cala_nuta", "krzyzyk"); break;
                    default: throw new Exception("rodzaj="+rodzaj);
                }
            }                    
            return nuta;
        }

    }


    public class PodpisKlawisza
    {
        public string rodzaj;
        public Nuta nuta;
        public string rodzaj_podpisu_nuty;
        public int poziom;
        public string napis;
        public PodpisKlawisza(int p_poziom,string p_napis)
        {
            rodzaj = "string";
            poziom = p_poziom;
            napis = p_napis;
        }
        public PodpisKlawisza(int p_poziom, Nuta p_nuta, string p_rodzaj_podpisu_nuty)
        {
            rodzaj = "podpis_nuty";
            poziom = p_poziom;
            nuta = p_nuta;
            rodzaj_podpisu_nuty = p_rodzaj_podpisu_nuty;
        }
    }

    public class WyrysowanieKlawisza
    {
        Color kolor_tla;
        public WyrysowanieKlawisza(Color p_kolor_tla)
        {
            kolor_tla = p_kolor_tla;
        }
    }

    public class WyroznienieKlawisza
    {          
    }

    public class WyroznienieKlawiszaKolko: WyroznienieKlawisza
    {
        public float stosunek_srednicy_wybrania_do_szerokosc_klawisza_czarnego;
        public Color kolor;
        public WyroznienieKlawiszaKolko(float p_stosunek_promienia_wybrania_do_szerokosc_klawisza_czarnego, Color p_kolor)
        {
            stosunek_srednicy_wybrania_do_szerokosc_klawisza_czarnego = p_stosunek_promienia_wybrania_do_szerokosc_klawisza_czarnego;
            kolor = p_kolor;
        }
    }

    class WierszKlawiaturowy : WierszArkusza
    {
        public int nr_klawisza_od;
        public int nr_klawisza_do;
        public List<Klawisz> listaKlawiszy;
        public Klawisz pierwszyKlawisz;
        public string polozenie_pionowe;
        public int liczba_wierszy_bialych;
        public Dictionary<int, List<PodpisKlawisza>> podpisyKlawiszy = new Dictionary<int, List<PodpisKlawisza>>();
        public Dictionary<int, WyrysowanieKlawisza> wyrysowaniaKlawiszy =new Dictionary<int, WyrysowanieKlawisza>();        
        public Dictionary<int, WyroznienieKlawisza> wyroznieniaKlawiszy = new Dictionary<int, WyroznienieKlawisza>();

        void UstawWartosci(int p_nr_klawisza_od, int p_nr_klawisza_do, string p_polozenie_pionowe)
        {
            nr_klawisza_od = p_nr_klawisza_od;
            nr_klawisza_do = p_nr_klawisza_do;
            listaKlawiszy = new List<Klawisz>();
            if (nr_klawisza_od > 1)
            {
                Klawisz klawiszPoprzedzajacy = new Klawisz(nr_klawisza_od - 1);
                if (klawiszPoprzedzajacy.kształtKlawisza.rodzaj == "czarny")
                {
                    klawiszPoprzedzajacy.kształtKlawisza.polowki = "P";
                    listaKlawiszy.Add(klawiszPoprzedzajacy);
                }
            }
            for (int nr_klawisza = nr_klawisza_od; nr_klawisza <= nr_klawisza_do; nr_klawisza++)
            {
                listaKlawiszy.Add(new Klawisz(nr_klawisza));
            }
            Klawisz klawiszNastepny = new Klawisz(nr_klawisza_do + 1);
            if (klawiszNastepny.kształtKlawisza.rodzaj == "czarny")
            {
                klawiszNastepny.kształtKlawisza.polowki = "L";
                listaKlawiszy.Add(klawiszNastepny);
            }
            pierwszyKlawisz = new Klawisz(nr_klawisza_od);
            polozenie_pionowe = p_polozenie_pionowe;
            liczba_wierszy_bialych = (int)(new Klawisz(nr_klawisza_do).nuta_nr - new Klawisz(nr_klawisza_od).nuta_nr + 1);
        }

        public WierszKlawiaturowy(int p_nr_klawisza_od, int p_nr_klawisza_do, string p_polozenie_pionowe)
        {
            UstawWartosci(p_nr_klawisza_od, p_nr_klawisza_do, p_polozenie_pionowe);
        }

        public WierszKlawiaturowy(Nuta p_nuta_od, Nuta p_nuta_do, string p_polozenie_pionowe)
        {
            UstawWartosci(p_nuta_od.getKlawisz().klawisz_nr, p_nuta_do.getKlawisz().klawisz_nr, p_polozenie_pionowe);
        }
    }


    class RysunekWierszaKlawiaturowego : RysunekWiersza
    {
        public WierszKlawiaturowy wiersz_klawiaturowy;
        public int szerokosc_klawisza_bialego_a;
        public int wysokosc_klawisza_bialego_a;
        public int szerokosc_klawisza_czarnego_a;
        public int wysokosc_klawisza_czarnego_a;
        public RysunekWierszaKlawiaturowego(WierszKlawiaturowy p_wiersz_klawiaturowy,
                                            float p_interlinia_podstawowa,
                                            int p_lewy_gorny_x_a,
                                            int p_lewy_gorny_y_a,
                                            int p_szerokosc_calkowita_a,
                                            int p_wysokosc_calkowita_a
                                      )
        {
            interlinia_podstawowa = p_interlinia_podstawowa;
            wiersz_klawiaturowy = p_wiersz_klawiaturowy;
            lewy_gorny_x_a = p_lewy_gorny_x_a;
            lewy_gorny_y_a = p_lewy_gorny_y_a;

            wysokosc_calkowita_a = p_wysokosc_calkowita_a;
            szerokosc_calkowita_a = p_szerokosc_calkowita_a;


            int szerokosc_klawisza_bialego_wynikajaca_z_szerokosci_klawiatury = szerokosc_calkowita_a / wiersz_klawiaturowy.liczba_wierszy_bialych;
            int maksymalna_szerokosc_klawisza_bialego_wg_wysokosci_klawiatury = (int)(p_wysokosc_calkowita_a * Parametry.Klawiatura.stosunek_szerokosci_do_wysokosci_klawisza_bialego);
            if (szerokosc_klawisza_bialego_wynikajaca_z_szerokosci_klawiatury <= maksymalna_szerokosc_klawisza_bialego_wg_wysokosci_klawiatury)
                szerokosc_klawisza_bialego_a = szerokosc_klawisza_bialego_wynikajaca_z_szerokosci_klawiatury;
            else szerokosc_klawisza_bialego_a = maksymalna_szerokosc_klawisza_bialego_wg_wysokosci_klawiatury;

            wysokosc_klawisza_bialego_a = (int)(szerokosc_klawisza_bialego_a / Parametry.Klawiatura.stosunek_szerokosci_do_wysokosci_klawisza_bialego);

            margines_lewy_a = (szerokosc_calkowita_a - szerokosc_klawisza_bialego_a * wiersz_klawiaturowy.liczba_wierszy_bialych) / 2;
            margines_prawy_a = margines_lewy_a;
            margines_gorny_a = p_wysokosc_calkowita_a - wysokosc_klawisza_bialego_a;
            margines_dolny_a = 0;

            wysokosc_bez_marginesow_a = p_wysokosc_calkowita_a - margines_gorny_a - margines_dolny_a;
            szerokosc_bez_marginesow_a = p_szerokosc_calkowita_a - margines_lewy_a - margines_prawy_a;

            szerokosc_klawisza_czarnego_a = (int)(szerokosc_klawisza_bialego_a * Parametry.Klawiatura.stosunek_szerokosci_klawisza_czarnego_do_bialego);
            wysokosc_klawisza_czarnego_a = (int)(wysokosc_klawisza_bialego_a * Parametry.Klawiatura.stosunek_wysokosci_klawisza_czarnego_do_bialego);



        }

        public void Rysuj(Graphics g, bool czy_rysowac)
        {
            if (czy_rysowac)
            {
                // Pen pen = new Pen(Color.Red, 3);
                //g.DrawRectangle(pen, new Rectangle(lewy_gorny_x_a, lewy_gorny_y_a, szerokosc_bez_marginesow_a, wysokosc_bez_marginesow_a));


                List<Klawisz> listaKlawiszy = wiersz_klawiaturowy.listaKlawiszy;
                WyroznienieKlawisza wyroznienieKlawisza;
                List<PodpisKlawisza> podpisyKlawisza=null;
                foreach (Klawisz klawisz in listaKlawiszy)
                {                    
                    wiersz_klawiaturowy.wyroznieniaKlawiszy.TryGetValue(klawisz.klawisz_nr, out wyroznienieKlawisza);
                    wiersz_klawiaturowy.podpisyKlawiszy.TryGetValue(klawisz.klawisz_nr, out podpisyKlawisza);
                    if (klawisz.kształtKlawisza.rodzaj == "biały")
                        RysujKlawisz(g, klawisz, wyroznienieKlawisza, podpisyKlawisza);
                }
                foreach (Klawisz klawisz in listaKlawiszy)
                {
                    wiersz_klawiaturowy.wyroznieniaKlawiszy.TryGetValue(klawisz.klawisz_nr, out wyroznienieKlawisza);
                    wiersz_klawiaturowy.podpisyKlawiszy.TryGetValue(klawisz.klawisz_nr, out podpisyKlawisza);
                    if (klawisz.kształtKlawisza.rodzaj == "czarny")
                        RysujKlawisz(g, klawisz, wyroznienieKlawisza, podpisyKlawisza);                    
                }

            }
        }

        
        
        
      

        public void RysujKlawisz(Graphics g, Klawisz klawisz, WyroznienieKlawisza wyroznienieKlawisza, List<PodpisKlawisza> podpisyKlawisza)
        {
            Color kolor_klawisza;
            Color kolor_napisu;
            switch (klawisz.kształtKlawisza.rodzaj)
            {
                case "biały":
                    kolor_klawisza = Color.White;
                    kolor_napisu = Color.Black;
                    break;
                case "czarny":
                    kolor_klawisza = Color.Black;
                    kolor_napisu= Color.White;
                    break;
                default:
                    throw new Exception("Nieznany rodzaj klawisza" + klawisz.kształtKlawisza.rodzaj);
            }
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(kolor_klawisza);
            Pen pen = new Pen(Color.Black, 3);

            float x0 = (klawisz.nuta_nr - wiersz_klawiaturowy.pierwszyKlawisz.nuta_nr) * szerokosc_klawisza_bialego_a;

            int wysokosc_klawisza=0;
            int szerokosc_klawisza=0;
            int srodek_gorny_x_klawisza= (int)((0.5f + klawisz.nuta_nr - wiersz_klawiaturowy.pierwszyKlawisz.nuta_nr) * szerokosc_klawisza_bialego_a);
            int srodek_gorny_y_klawisza = 0;

            switch (klawisz.kształtKlawisza.rodzaj)
            {
                case "biały":
                    wysokosc_klawisza = wysokosc_klawisza_bialego_a;
                    szerokosc_klawisza = szerokosc_klawisza_bialego_a;
                    g.DrawRectangle(pen, new Rectangle(X_a(srodek_gorny_x_klawisza - szerokosc_klawisza / 2), Y_a(srodek_gorny_y_klawisza), szerokosc_klawisza, wysokosc_klawisza));
                    break;
                case "czarny":
                    wysokosc_klawisza = wysokosc_klawisza_czarnego_a;
                    switch (klawisz.kształtKlawisza.polowki)
                    {
                        case "LP":
                            szerokosc_klawisza = szerokosc_klawisza_czarnego_a;
                            g.FillRectangle(myBrush, new Rectangle(X_a(srodek_gorny_x_klawisza - szerokosc_klawisza / 2), Y_a(srodek_gorny_y_klawisza), szerokosc_klawisza, wysokosc_klawisza)); break;
                        case "L":
                            szerokosc_klawisza = szerokosc_klawisza_czarnego_a/2;
                            g.FillRectangle(myBrush, new Rectangle(X_a(srodek_gorny_x_klawisza - szerokosc_klawisza), Y_a(srodek_gorny_y_klawisza), szerokosc_klawisza , wysokosc_klawisza)); break;
                        case "P":
                            szerokosc_klawisza = szerokosc_klawisza_czarnego_a / 2;
                            g.FillRectangle(myBrush, new Rectangle(X_a(srodek_gorny_x_klawisza), Y_a(srodek_gorny_y_klawisza), szerokosc_klawisza, wysokosc_klawisza)); break;                            
                        default: throw new Exception("Nieznane klawisz.kształtKlawisza.polowki=" + klawisz.kształtKlawisza.polowki);
                    }
                    break;
                default: throw new Exception("Nieznany rodzaj klawisza" + klawisz.kształtKlawisza.rodzaj);

            }
            
            int wysokosc_napisu = (int)(szerokosc_klawisza_czarnego_a/3.7f);

            //wyroznienie
            if (wyroznienieKlawisza!= null)
            {               
                Type t = wyroznienieKlawisza.GetType();
                if (t.Equals(typeof(WyroznienieKlawiszaKolko)))
                {
                    WyroznienieKlawiszaKolko wyroznienieKlawiszaKolko = (WyroznienieKlawiszaKolko)wyroznienieKlawisza;
                    int srednica = (int)(szerokosc_klawisza_czarnego_a * wyroznienieKlawiszaKolko.stosunek_srednicy_wybrania_do_szerokosc_klawisza_czarnego);
                    SolidBrush brush = new SolidBrush(wyroznienieKlawiszaKolko.kolor);
                    g.FillEllipse(brush, X_a(srodek_gorny_x_klawisza - srednica/2), Y_a(srodek_gorny_y_klawisza+ wysokosc_klawisza- 1.1f*srednica), srednica,  srednica);
                }                               
            }

            if (podpisyKlawisza!=null)
            {
                foreach (PodpisKlawisza podpisKlawisza in podpisyKlawisza)
                {
                    switch (podpisKlawisza.rodzaj)
                    {
                        case "podpis_nuty":
                            Nuta nuta = new Nuta(podpisKlawisza.nuta.nr, "cala_nuta", podpisKlawisza.nuta.znak_chromatyczny);
                            nuta.kolor_podpisu = kolor_napisu;
                            nuta.kolor_tla = Color.Transparent;
                            nuta.rodzaj_podpisu = podpisKlawisza.rodzaj_podpisu_nuty;
                            Rysowanie.RysujPodpisNuty(g, nuta, wysokosc_napisu, X_a(srodek_gorny_x_klawisza), Y_a(wysokosc_klawisza-3f*interlinia_podstawowa) - (int)(1.5f*podpisKlawisza.poziom * wysokosc_napisu), true);
                            break;
                        default: throw new Exception("Nieobsluzony podpisKlawisza.rodzaj:" + podpisKlawisza.rodzaj);
                    }
                }   
               
            }

        }
    }

}
