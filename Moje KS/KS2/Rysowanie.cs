using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace KS2
{


    class DrawingControl
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
           // parent.Refresh();
        }
    }

    /*
        static class DrawingControl
        {
            [DllImport("user32.dll")]
            public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

            private const int WM_SETREDRAW = 11;

            public static void SuspendDrawing(Control parent)
            {
                SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
            }

            public static void ResumeDrawing(Control parent)
            {
                SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
                parent.Refresh();
            }
        }
    */




    /*static class DrawingControl
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }
    }*/



    public class Obrazek
    {
        private Hashtable images;
        public string kod;
        public string przycisk;
        public float wsp_pow;
        public float przes_pion_wzgledem_linii;
        public int grupa;
        public int kolejnosc;
        public int wartosc_rytmiczna;
        public bool czy_zmienna_wysokosc;

        public Image getImage(Color color)
        {
            Image img = (Image)images[color];
            if (img == null)
            {
                Image img_black = (Image)((Image)(images[Color.Black])).Clone();
                img = ObrobkaGrafiki.ZmienKolor(img_black, color);
               // img = img_black;
                images.Add(color, img_black);
            }
            return img;
        }

        public Obrazek(string p_kod, string p_przycisk, string res, float p_wsp_pow,float p_przes_pion_wzgledem_linii,int p_grupa,int p_kolejnosc, int p_wartosc_rytmiczna)
        {
            kod = p_kod;
            przycisk = p_przycisk;
            wsp_pow = p_wsp_pow;
            przes_pion_wzgledem_linii = p_przes_pion_wzgledem_linii;
            grupa = p_grupa;
            kolejnosc = p_kolejnosc;
            wartosc_rytmiczna = p_wartosc_rytmiczna;

            System.Reflection.Assembly thisExe;
            thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream file = thisExe.GetManifestResourceStream(res);
            images = new Hashtable();
            images.Add(Color.Black, Image.FromStream(file));
           // img = ObrobkaGrafiki.ZmienKolor(img,Color.Red);
        }

    }




    public class RysunekArkusza
    {
        Arkusz arkusz;
        public int margines_lewy_a;
        public int margines_prawy_a;
        public int margines_gorny_a;
        public int margines_dolny_a;
        public int szerokosc_calkowita_a;
        public float interlinia_podstawowa;
        public int grubosc_linii_podstawowa;

        public int wysokosc_rysunku_calkowita_a;
        public int szerokosc_rysunku_calkowita_a;

        public RysunekArkusza(Arkusz p_arkusz,
                                     float p_interlinia_podstawowa,
                                     int p_grubosc_linii_podstawowa,
                                     int p_margines_lewy_a, int p_margines_prawy_a, float p_margines_gorny_i, float p_margines_dolny_i,
                                     int p_szerokosc_calkowita_a
                                     )
        {
            arkusz = p_arkusz;
            margines_lewy_a = p_margines_lewy_a;
            margines_prawy_a = p_margines_prawy_a;
            margines_gorny_a = (int)(p_margines_gorny_i * p_interlinia_podstawowa);//+ Parametry.AutoScrollPosition_Y;
            margines_dolny_a = (int)(p_margines_dolny_i * p_interlinia_podstawowa);
            szerokosc_calkowita_a = p_szerokosc_calkowita_a;
            interlinia_podstawowa = p_interlinia_podstawowa;
            grubosc_linii_podstawowa = p_grubosc_linii_podstawowa;
        }

        public void wysokosc_rysunku_calkowita_a_oblicz()
        {
            Rysuj(null, null, false);
        }


        public void Rysuj(Panel panel, Graphics g, bool czy_rysowac = true, bool czy_dzielic = true)
        {
            int wysokosc_a = margines_gorny_a;
            wysokosc_rysunku_calkowita_a = margines_gorny_a; //A
            szerokosc_rysunku_calkowita_a = 0;
            for (int i = 0; i < arkusz.wiersze.Count(); i++)
            {
                WierszArkusza wiersz = arkusz.wiersze[i];
                Type t = wiersz.GetType();
                if (t.Equals(typeof(WierszNutowy)))
                {
                    WierszNutowy wierszNutowy = (WierszNutowy)wiersz;
                    RysunekWierszaNutowego rysunek_systemu_nutowego =
                    new RysunekWierszaNutowego(wierszNutowy,
                                            interlinia_podstawowa, grubosc_linii_podstawowa,
                                            margines_lewy_a, wysokosc_a, szerokosc_calkowita_a,
                                            arkusz.odstep_pionowy_przed_wierszem_nutowym_i,
                                            arkusz.odstep_pionowy_po_wierszu_nutowym_i,
                                            arkusz.odstep_pionowy_przed_pierwszym_wierszem_nutowym_i
                                            );
                    rysunek_systemu_nutowego.Rysuj(g, czy_rysowac, czy_dzielic);
                    wysokosc_a += rysunek_systemu_nutowego.wysokosc_calkowita_a;
                    wysokosc_rysunku_calkowita_a += rysunek_systemu_nutowego.wysokosc_calkowita_a;
                    if (szerokosc_rysunku_calkowita_a < rysunek_systemu_nutowego.szerokosc_calkowita_a)
                        szerokosc_rysunku_calkowita_a = rysunek_systemu_nutowego.szerokosc_calkowita_a;
                }
                if (t.Equals(typeof(WierszTekstowy)))
                {
                    WierszTekstowy wierszTekstowy = (WierszTekstowy)wiersz;
                    RysunekWierszaTekstowego rysunekWierszaTekstowego =
                    new RysunekWierszaTekstowego(wierszTekstowy,
                                                 interlinia_podstawowa,
                                                 margines_lewy_a, 
                                                 wysokosc_a,
                                                 szerokosc_calkowita_a - margines_lewy_a - margines_prawy_a);
                    wysokosc_a += rysunekWierszaTekstowego.wysokosc_calkowita_a;
                    rysunekWierszaTekstowego.Rysuj(g, czy_rysowac);
                    wysokosc_rysunku_calkowita_a += rysunekWierszaTekstowego.wysokosc_calkowita_a;
                    /* TODO
                    if (szerokosc_rysunku_calkowita_a < rysunekWierszaTekstowego.szerokosc_calkowita_a)
                        szerokosc_rysunku_calkowita_a = rysunekWierszaTekstowego.szerokosc_calkowita_a;
                    */
                }
                if (t.Equals(typeof(WierszKlawiaturowy)))
                {
                    WierszKlawiaturowy wierszKlawiaturowy = (WierszKlawiaturowy)wiersz;
                    RysunekWierszaKlawiaturowego rysunekWierszaKlawiaturowego;
                    int wysokosc_klawiatury_a;
                    switch (wierszKlawiaturowy.polozenie_pionowe)
                    {
                        case "za_poprzednim":
                            wysokosc_klawiatury_a = (int)(interlinia_podstawowa * 10);
                            rysunekWierszaKlawiaturowego =
                                new RysunekWierszaKlawiaturowego(wierszKlawiaturowy,
                                                                 interlinia_podstawowa,
                                                                 margines_lewy_a,
                                                                 wysokosc_a,
                                                                 szerokosc_calkowita_a - margines_lewy_a - margines_prawy_a,
                                                                 wysokosc_klawiatury_a);
                            wysokosc_a += rysunekWierszaKlawiaturowego.wysokosc_calkowita_a;
                            rysunekWierszaKlawiaturowego.Rysuj(g, czy_rysowac);
                            wysokosc_rysunku_calkowita_a += rysunekWierszaKlawiaturowego.wysokosc_calkowita_a;
                            /* TODO
                            if (szerokosc_rysunku_calkowita_a < rysunekWierszaTekstowego.szerokosc_calkowita_a)
                                szerokosc_rysunku_calkowita_a = rysunekWierszaTekstowego.szerokosc_calkowita_a;
                            */
                            break;
                        case "caly_ekran":
                            if (czy_rysowac)
                            {

                                wysokosc_klawiatury_a = panel.Height- wysokosc_a;
                                //wysokosc_a = wysokosc_klawiatury_a / 5;
                                rysunekWierszaKlawiaturowego =
                                    new RysunekWierszaKlawiaturowego(wierszKlawiaturowy,
                                                                     interlinia_podstawowa,
                                                                     margines_lewy_a,
                                                                     wysokosc_a,
                                                                     szerokosc_calkowita_a - margines_lewy_a - margines_prawy_a,
                                                                     wysokosc_klawiatury_a);
                                wysokosc_a += rysunekWierszaKlawiaturowego.wysokosc_calkowita_a;
                                rysunekWierszaKlawiaturowego.Rysuj(g, czy_rysowac);
                                wysokosc_rysunku_calkowita_a += rysunekWierszaKlawiaturowego.wysokosc_calkowita_a;
                            }
                            break;
                        case "u_dolu":
                            if (czy_rysowac)
                            {
                                //wysokosc_klawiatury_a = (int)(panel.Height * Parametry.Edytor.klawiatura_czesc_ekranu);
                                wysokosc_klawiatury_a = panel.Height - wysokosc_a;
                                rysunekWierszaKlawiaturowego =
                                    new RysunekWierszaKlawiaturowego(wierszKlawiaturowy,
                                                                     interlinia_podstawowa,
                                                                     margines_lewy_a,
                                                                     panel.Height - wysokosc_klawiatury_a,
                                                                     szerokosc_calkowita_a - margines_lewy_a - margines_prawy_a,
                                                                     wysokosc_klawiatury_a);
                                wysokosc_a += rysunekWierszaKlawiaturowego.wysokosc_calkowita_a;
                                rysunekWierszaKlawiaturowego.Rysuj(g, czy_rysowac);
                                wysokosc_rysunku_calkowita_a += rysunekWierszaKlawiaturowego.wysokosc_calkowita_a;                             
                            }
                            break;
                        default: throw new Exception("wierszKlawiaturowy.polozenie_pionowe=" + wierszKlawiaturowy.polozenie_pionowe);
                    }
                }
            }
            //  wysokosc_rysunku_calkowita_a_oblicz();            
            /*
            if (czy_rysowac)
            {
                Pen p = new Pen(Color.Black);
                g.DrawLine(p, 1, 1, 100, wysokosc_rysunku_calkowita_a);
            }
            */
        }

    


}
    
    class RysunekWiersza
    {
        public int lewy_gorny_x_a;
        public int lewy_gorny_y_a;
        public int margines_lewy_a;
        public int margines_prawy_a;
        public int margines_gorny_a;
        public int margines_dolny_a;
        public int wysokosc_bez_marginesow_a;
        public int wysokosc_calkowita_a;
        public int szerokosc_bez_marginesow_a;
        public int szerokosc_calkowita_a;        
        public float interlinia_podstawowa;
        public int pionowe_przesuniecie_aktualnej_linii_wirtualnej_a = 0;

        public int X_a(float x_system_a)
        {
            return (int)(lewy_gorny_x_a + margines_lewy_a + x_system_a);
        }

        public int Y_a(float y_system_a)
        {
            return (int)(pionowe_przesuniecie_aktualnej_linii_wirtualnej_a+  lewy_gorny_y_a + margines_gorny_a + y_system_a+ Parametry.AutoScrollPosition_Y);
        }

    }

    class RysunekWierszaTekstowego : RysunekWiersza
    {
        WierszTekstowy wiersz_tekstowy;
        public int czcionka_rozmiar_a = 1;
        public int czcionka_rozmiar_podstawowy = 1;
        public int poczatek_a;

        Font drawFont;
        public RysunekWierszaTekstowego(WierszTekstowy p_wiersz_tekstowy,
                                        float p_interlinia_podstawowa,
                                        int p_lewy_gorny_x_a, int p_lewy_gorny_y_a,
                                        int p_szerokosc_calkowita_a
                                       )
        {
            wiersz_tekstowy = p_wiersz_tekstowy;
            interlinia_podstawowa = p_interlinia_podstawowa;
            czcionka_rozmiar_a = (int)(interlinia_podstawowa * p_wiersz_tekstowy.czcionka_rozmiar_i);
            lewy_gorny_x_a = p_lewy_gorny_x_a;
            lewy_gorny_y_a = p_lewy_gorny_y_a;
            margines_lewy_a = (int)(p_wiersz_tekstowy.margines_lewy_i * interlinia_podstawowa);
            margines_prawy_a = (int)(p_wiersz_tekstowy.margines_prawy_i * interlinia_podstawowa);
            margines_gorny_a = (int)(p_wiersz_tekstowy.margines_gorny_i * interlinia_podstawowa);
            margines_dolny_a = (int)(p_wiersz_tekstowy.margines_dolny_i * interlinia_podstawowa);


            drawFont = new Font(wiersz_tekstowy.czcionka_nazwa, czcionka_rozmiar_a * 21.3f/33);

            Size rozmiarTekstu= TextRenderer.MeasureText(wiersz_tekstowy.tekst, drawFont);

            //wiersz_tekstowy.tekst += " height="+ rozmiarTekstu.Height+" i="+ interlinia_podstawowa;
            wysokosc_bez_marginesow_a = rozmiarTekstu.Height;
            wysokosc_calkowita_a = wysokosc_bez_marginesow_a+ margines_gorny_a + margines_dolny_a;
            szerokosc_calkowita_a = p_szerokosc_calkowita_a;
            szerokosc_bez_marginesow_a = p_szerokosc_calkowita_a - margines_lewy_a - margines_prawy_a;

            switch (wiersz_tekstowy.wyrownanie)
            {
                case "left": poczatek_a = 0;break;
                case "right": poczatek_a = p_szerokosc_calkowita_a- rozmiarTekstu.Width; break;
                case "center": poczatek_a = (p_szerokosc_calkowita_a - rozmiarTekstu.Width)/2; break;
                default: throw new Exception("Nieznane wyrownanie:" + wiersz_tekstowy.wyrownanie);
            }
            
        }

        public void Rysuj(Graphics g,bool czy_rysowac)
        {
                                                
            if (czy_rysowac)
            {
                string tekst = wiersz_tekstowy.tekst;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                TextRenderer.DrawText(g,
                                       tekst,
                                       drawFont,
                                       new Point(X_a(poczatek_a),
                                                 Y_a(0)),
                                       SystemColors.ControlText, Color.White);
              //  SolidBrush drawBrush = new SolidBrush(Color.Black);
//                g.DrawString(tekst, drawFont, drawBrush, new Point(X_a(poczatek_a),Y_a(0)));
            }

            
        }
    }

    class RysunekObiektuNutowego
    {
        public ObiektNutowy obiektNutowy;
        public int poczatek_a;
        public int szerokosc_a;
        public int srodek_a;
        public float nr_linii_l;
        public int przesuniecie_pionowe_w_wirtualnej_linii_a;
        public RysunekObiektuNutowego(ObiektNutowy p_obiektNutowy, int p_poczatek_a, int p_szerokosc_a, int p_przesuniecie_pionowe_w_wirtualnej_linii_a)
        {

            obiektNutowy = p_obiektNutowy;
            poczatek_a = p_poczatek_a;
            szerokosc_a = p_szerokosc_a;
            srodek_a = p_poczatek_a + (szerokosc_a / 2);
            przesuniecie_pionowe_w_wirtualnej_linii_a = p_przesuniecie_pionowe_w_wirtualnej_linii_a;

        }


    }

    class RysunekWierszaNutowego: RysunekWiersza
    {
        WierszNutowy wierszNutowy;
        int odstep_pionowy_przed_pierwszym_wierszem_nutowym_a;
        int odstep_pionowy_przed_wierszem_nutowym_a;
        int odstep_pionowy_po_wierszu_nutowym_a;
        int grubosc_linii_podstawowa;
        int grubosc_linii_a;
        public int interlinia_system_a;        
        List<RysunekObiektuNutowego> listaRysunkowObiektowNutowych;

        //Hashtable ostatnie_znaki_chromatyczne = new Hashtable();
        AktualneZnakiChromatyczne aktualneZnakiChromatyczne = null;

        public RysunekWierszaNutowego(WierszNutowy p_system_nutowy,
                              float p_interlinia_podstawowa,int p_grubosc_linii_podstawowa,                             
                              int p_lewy_gorny_x_a, int p_lewy_gorny_y_a,
                              int p_szerokosc_calkowita_a,
                              float p_odstep_pionowy_przed_wierszem_nutowym_i,
                              float p_odstep_pionowy_po_wierszu_nutowym_i,
                              float p_odstep_pionowy_przed_pierwszym_wierszem_nutowym_i
                              )
        {
            wierszNutowy = p_system_nutowy;
            interlinia_podstawowa = p_interlinia_podstawowa;
            interlinia_system_a = (int)(interlinia_podstawowa * wierszNutowy.interlinia_wspolczynnik);
            odstep_pionowy_przed_wierszem_nutowym_a = (int)(p_odstep_pionowy_przed_wierszem_nutowym_i * interlinia_system_a);
            odstep_pionowy_po_wierszu_nutowym_a = (int)(p_odstep_pionowy_po_wierszu_nutowym_i * interlinia_system_a);
            odstep_pionowy_przed_pierwszym_wierszem_nutowym_a= (int)(p_odstep_pionowy_przed_pierwszym_wierszem_nutowym_i * interlinia_system_a);
            grubosc_linii_podstawowa = p_grubosc_linii_podstawowa;
            grubosc_linii_a = (int)(p_grubosc_linii_podstawowa * wierszNutowy.grubosc_linii_wspolczynnik);
            if (grubosc_linii_a == 0)
            {
                grubosc_linii_a = 1;
            }
            lewy_gorny_x_a = p_lewy_gorny_x_a;
            lewy_gorny_y_a = p_lewy_gorny_y_a;
            margines_lewy_a =  (int)(p_system_nutowy.margines_lewy_i* interlinia_system_a);
            margines_prawy_a = (int)(p_system_nutowy.margines_prawy_i* interlinia_system_a);
            margines_gorny_a = (int)(p_system_nutowy.margines_gorny_i* interlinia_system_a);
            margines_dolny_a = (int)(p_system_nutowy.margines_dolny_i* interlinia_system_a);
            //wysokosc_bez_marginesow_a = 0;
            // TODO!! wysokosc_bez_marginesow_a = (int)((wierszNutowy.liczba_pol_dodatkowych_gora + wierszNutowy.liczba_linii_wlasciwych - 1 + wierszNutowy.liczba_pol_dodatkowych_dol) * interlinia_system_a);            
            //wysokosc_calkowita_a = wysokosc_bez_marginesow_a + margines_gorny_a + margines_dolny_a;
            szerokosc_calkowita_a = p_szerokosc_calkowita_a;
            //szerokosc_bez_marginesow_a = p_szerokosc_calkowita_a - margines_lewy_a - margines_prawy_a;

            listaRysunkowObiektowNutowych = new List<RysunekObiektuNutowego>();
        }



        public void Rysuj(Graphics g,bool czy_rysowac, bool czy_dzielic)
        {
           
            Klucz klucz_aktualny=null;
            if (wierszNutowy.lista_obiektow_nutowych.Count > 0)
                klucz_aktualny = (Klucz)wierszNutowy.lista_obiektow_nutowych[0];
            bool faza_sprawdzania=true;
            int sprawdzany = 0;
            int rysowany = 0;
            int pozycja_rysowania = 0;
            int szerokosc_sprawdzanej_linii = (int)(wierszNutowy.odstep_przed_pierwszym_obiektem_i * interlinia_system_a);
            int szerokosc_sprawdzanego_taktu = 0;            
            int poczatek_wirtualnej_linii = 0;
            int koniec_wirtualnej_linii = -1;
            int koniec_ostatniego_pelnego_taktu = 0;
            int pozycja_konca_ostatniego_pelnego_taktu = 0;
            wysokosc_bez_marginesow_a = 0;
            Klucz klucz_poczatku_linii = klucz_aktualny;

            int nr_wirtualnej_linii=0;
      
            while (rysowany < wierszNutowy.lista_obiektow_nutowych.Count)

                {
                int aktualny = faza_sprawdzania? sprawdzany: rysowany;
                
                ObiektNutowy obiekt= wierszNutowy.lista_obiektow_nutowych[aktualny];
                

                Type t = obiekt.GetType();

                int szerokosc = 0;


                if (t.Equals(typeof(Klucz)) || (aktualny== poczatek_wirtualnej_linii))
                {

                    
                    if (t.Equals(typeof(Klucz)))
                        klucz_aktualny = (Klucz)obiekt;
                    RysujSymbol(g, new Symbol(klucz_aktualny.symbol_kod), klucz_aktualny.kolor, pozycja_rysowania, ref szerokosc, klucz_aktualny.czy_obiekt_wyswietlany, czy_rysowac && !faza_sprawdzania);
                    int szerokosc_znakow = 0;
                    int odstep1 = (int)(Parametry.GrafikaNut.odstep_po_kluczu_i * interlinia_system_a);
                    RysujZnakiPrzykluczowe(g, pozycja_rysowania + szerokosc + odstep1, ref szerokosc_znakow, klucz_aktualny.znaki_przykluczowe, klucz_aktualny, czy_rysowac && !faza_sprawdzania);
                    int odstep2 = (int)(Parametry.GrafikaNut.odstep_po_znakach_przykluczowych_i * interlinia_system_a);
                    szerokosc += odstep1 + odstep2+ szerokosc_znakow;
                    aktualneZnakiChromatyczne = new AktualneZnakiChromatyczne(klucz_aktualny);
                    if (!t.Equals(typeof(Klucz)))
                    {
                        if (faza_sprawdzania)
                            szerokosc_sprawdzanego_taktu += szerokosc;
                        else pozycja_rysowania += szerokosc;                        
                    }
                }
                if (t.Equals(typeof(Nuta)))
                {
                   
                    Nuta nuta = (Nuta)obiekt;
                    /*
                    if (!faza_sprawdzania && rysowany == 1)
                    {

                        nuta.kolor = Color.Green;
                        //MessageBox.Show(nr_linii.ToString());                        
                    }
                    if (!faza_sprawdzania && rysowany == 40)
                    {
                        
                        nuta.kolor = Color.Red;
                        //MessageBox.Show(nr_linii.ToString());                        
                    }
                    */
                    RysujNute(g, pozycja_rysowania, ref szerokosc, nuta, klucz_aktualny, wierszNutowy.wysokosc_podpisow_l, czy_rysowac && !faza_sprawdzania);                    
                    //ostatnie_znaki_chromatyczne[nuta.bazowa_nazwa_literowa]=nuta.znak_chromatyczny;
                }
                if (t.Equals(typeof(KreskaTaktowa)))
                {
                    KreskaTaktowa kreska_taktowa = (KreskaTaktowa)obiekt;
                    RysujKreskeTaktowa(g, pozycja_rysowania, ref szerokosc, kreska_taktowa, czy_rysowac && !faza_sprawdzania);
                    aktualneZnakiChromatyczne = new AktualneZnakiChromatyczne(klucz_aktualny);                    
                }

                if (t.Equals(typeof(OdstepPoziomy)))
                {
                    OdstepPoziomy odstepPoziomy = (OdstepPoziomy)obiekt;
                    szerokosc = (int)(odstepPoziomy.dlugosc_i * interlinia_system_a);
                }



                int odstep_po_a = 0;

                if (!t.Equals(typeof(OdstepPoziomy)))
                    odstep_po_a=(int)(obiekt.odstep_po_i * obiekt.wspolczynnik_odstepu_po * interlinia_system_a);

                if (!faza_sprawdzania)
                {//FAZA RYSOWANIA
                    listaRysunkowObiektowNutowych.Add(new RysunekObiektuNutowego(obiekt, pozycja_rysowania, szerokosc, pionowe_przesuniecie_aktualnej_linii_wirtualnej_a));
                    if (rysowany!=koniec_wirtualnej_linii && (!t.Equals(typeof(Klucz))))
                        szerokosc += odstep_po_a;
 
                    pozycja_rysowania += szerokosc;

                    if (rysowany == koniec_wirtualnej_linii)
                    { //RYSOWANIE-->SPRAWDZANIE
                        szerokosc_sprawdzanej_linii = (int)(wierszNutowy.odstep_przed_pierwszym_obiektem_i * interlinia_system_a);
                        faza_sprawdzania = true;
                        poczatek_wirtualnej_linii = koniec_wirtualnej_linii + 1;
                        koniec_ostatniego_pelnego_taktu = poczatek_wirtualnej_linii;
                        pozycja_konca_ostatniego_pelnego_taktu = pozycja_rysowania;
                        sprawdzany = rysowany+1;
                        klucz_poczatku_linii = klucz_aktualny;


                        for (int l = 1; l <= 5; l++)
                            RysujLinie(g, l,
                                pozycja_rysowania, czy_rysowac);
                        pionowe_przesuniecie_aktualnej_linii_wirtualnej_a += odstep_pionowy_po_wierszu_nutowym_a+4*interlinia_system_a;

                

                        nr_wirtualnej_linii++;
                    }
                    rysowany++;
                }
                else
                { //FAZA SPRAWDZANIA
                    
                    if (t.Equals(typeof(KreskaTaktowa)) || (sprawdzany == wierszNutowy.lista_obiektow_nutowych.Count - 1))
                    {
                        szerokosc_sprawdzanej_linii += szerokosc_sprawdzanego_taktu;
                        szerokosc_sprawdzanego_taktu = odstep_po_a;
                    }
                    else
                    {
                        szerokosc += odstep_po_a;
                        szerokosc_sprawdzanego_taktu += szerokosc;
                    }


                    int rozpietosc = szerokosc_calkowita_a - lewy_gorny_x_a - margines_lewy_a - margines_prawy_a-interlinia_system_a ;

                    /*
                    if (czy_rysowac)
                    {
                        Pen p = new Pen(Color.Black);
                        p.Width = 1;
                        g.DrawLine(p,
                                X_a(rozpietosc),
                                Y_a(0),
                                X_a(rozpietosc),
                                Y_a(1000));
                    }
                    */

                        if ((t.Equals(typeof(KreskaTaktowa)) && (szerokosc_sprawdzanej_linii > rozpietosc)) 
                        || (sprawdzany == wierszNutowy.lista_obiektow_nutowych.Count - 1))
                    {  //FAZA SPRAWDZANIA-->RYSOWANIE
                        if (sprawdzany == wierszNutowy.lista_obiektow_nutowych.Count - 1
                            && (koniec_ostatniego_pelnego_taktu == poczatek_wirtualnej_linii || szerokosc_sprawdzanej_linii <= rozpietosc))
                        {
                            koniec_ostatniego_pelnego_taktu = sprawdzany;
                            pozycja_konca_ostatniego_pelnego_taktu = szerokosc_sprawdzanej_linii;
                        }
                        szerokosc_sprawdzanej_linii -= odstep_po_a;
                        koniec_wirtualnej_linii = koniec_ostatniego_pelnego_taktu;
                        rysowany = poczatek_wirtualnej_linii;
                        faza_sprawdzania = false;
                        klucz_aktualny = klucz_poczatku_linii;
                        pozycja_rysowania = (int)(wierszNutowy.odstep_przed_pierwszym_obiektem_i * interlinia_system_a);
                        pionowe_przesuniecie_aktualnej_linii_wirtualnej_a += nr_wirtualnej_linii==0?odstep_pionowy_przed_pierwszym_wierszem_nutowym_a: odstep_pionowy_przed_wierszem_nutowym_a;



                    }
                    else
                    {
                        if (t.Equals(typeof(KreskaTaktowa)) || (sprawdzany == wierszNutowy.lista_obiektow_nutowych.Count - 1))
                        {
                            koniec_ostatniego_pelnego_taktu = sprawdzany;
                            pozycja_konca_ostatniego_pelnego_taktu = szerokosc_sprawdzanej_linii;
                        }
                    }
                    sprawdzany++;
                }

                
            }

            wysokosc_bez_marginesow_a = pionowe_przesuniecie_aktualnej_linii_wirtualnej_a;//+ 4 * interlinia_system_a;
            wysokosc_calkowita_a = wysokosc_bez_marginesow_a + margines_gorny_a + margines_dolny_a;

       

            foreach (ZnakDodatkowy znakDodatkowy in wierszNutowy.lista_znakow_dodatkowych)
            {
                if (czy_rysowac)
                {
                    switch (znakDodatkowy.rodzaj)
                    {
                        case "dziubek_poltonowy": RysujDziubekPoltonowy(g, klucz_aktualny, znakDodatkowy.poczatek_z, znakDodatkowy.koniec_z, czy_rysowac); break;
                        case "klamra_tetrachord": RysujKlamreKwadratowa(g, klucz_aktualny, 
                                                                           znakDodatkowy.poczatek_z, 
                                                                           znakDodatkowy.koniec_z,                                                                            
                                                                           "tetrachord", 
                                                                           czy_rysowac); break;
                    }
                }
            }



            //TODO
            // if (!czy_pierwsza_linia)


            /*
                    {
                        if (rysowany < wierszNutowy.lista_obiektow_nutowych.Count - 1)
                        {
                            for (int l = 1; l <= 5; l++)
                                RysujLinie(g, l, pozycja_rysowania, czy_rysowac);
                            nr_linii++;
                            
                            pozycja_rysowania = (int)(wierszNutowy.odstep_przed_pierwszym_obiektem_i * interlinia_system_a);
                            RysujSymbol(g, new Symbol(klucz_aktualny.symbol_kod), klucz_aktualny.kolor, pozycja_rysowania, ref szerokosc, klucz_aktualny.czy_obiekt_wyswietlany, czy_rysowac && !faza_sprawdzania);
                            int szerokosc_znakow = 0;
                            int odstep = (int)(Parametry.GrafikaNut.odstep_po_kluczu_i * interlinia_system_a);
                            RysujZnakiPrzykluczowe(g, pozycja_rysowania + szerokosc + odstep, ref szerokosc_znakow, klucz_aktualny.znaki_przykluczowe, klucz_aktualny, czy_rysowac && !faza_sprawdzania);
                            pozycja_rysowania += szerokosc + odstep + szerokosc_znakow;
                            faza_sprawdzania = true;
                            sprawdzany = rysowany;
                            szerokosc_sprawdzanej_linii = 0;
                            ile_znakow_sprawdzonych = 0;
                        }
                    }

                }
           */



            //if (!(i == wierszNutowy.lista_obiektow_nutowych.Count-1 && t.Equals(typeof(KreskaTaktowa))))



            /*

            //narysowanie pieciolinii  
            for (int l = 1; l <= 5; l++)
                RysujLinie(g, l,pozycja_rysowania,czy_rysowac);

            */

            szerokosc_calkowita_a = X_a(pozycja_rysowania);

        }

        public void RysujKlamreKwadratowa(Graphics g, Klucz klucz, int poczatek_z, int koniec_z,  string styl,bool czy_rysowac)
        {
            RysunekObiektuNutowego rysunekObiektuNutowego1 = listaRysunkowObiektowNutowych[poczatek_z];
            RysunekObiektuNutowego rysunekObiektuNutowego2 = listaRysunkowObiektowNutowych[koniec_z];
            int poczatek_a = (int)(rysunekObiektuNutowego1.poczatek_a + rysunekObiektuNutowego2.szerokosc_a/2);
            int koniec_a = (int)(rysunekObiektuNutowego2.poczatek_a + rysunekObiektuNutowego2.szerokosc_a/2);

            Nuta nuta1 = (Nuta)(rysunekObiektuNutowego1.obiektNutowy);
            Nuta nuta2 = (Nuta)(rysunekObiektuNutowego2.obiektNutowy);

            float grubosc_piora_a = 0;
            int wysokosc_a = 0;
            switch (styl)
            {
                case "tetrachord": grubosc_piora_a = 2f * grubosc_linii_a; wysokosc_a = WysokoscLinii_system_a(9); break;
            }
            wysokosc_a += -pionowe_przesuniecie_aktualnej_linii_wirtualnej_a+ rysunekObiektuNutowego1.przesuniecie_pionowe_w_wirtualnej_linii_a;

            Pen p = new Pen(Color.Black);
            p.Width = grubosc_piora_a;
            if (czy_rysowac)
            {
                SmoothingMode smoothingMode = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.Default;
                g.DrawLine(p,
                        X_a(poczatek_a),
                        Y_a(wysokosc_a),
                        X_a(koniec_a),
                        Y_a(wysokosc_a));

                g.DrawLine(p,
                   X_a(poczatek_a + grubosc_piora_a / 2),
                   Y_a(wysokosc_a),
                   X_a(poczatek_a + grubosc_piora_a / 2),
                   Y_a(wysokosc_a + interlinia_system_a * 0.75f));

                g.DrawLine(p,
                   X_a(koniec_a - grubosc_piora_a / 2),
                   Y_a(wysokosc_a),
                   X_a(koniec_a - grubosc_piora_a / 2),
                   Y_a(wysokosc_a + interlinia_system_a * 0.75f));

                g.SmoothingMode = smoothingMode;
            }
        }


        public void RysujDziubekPoltonowy(Graphics g, Klucz klucz,int poczatek_z, int koniec_z,bool czy_rysowac)
        {
            RysunekObiektuNutowego rysunekObiektuNutowego1 = listaRysunkowObiektowNutowych[poczatek_z];
            RysunekObiektuNutowego rysunekObiektuNutowego2 = listaRysunkowObiektowNutowych[koniec_z];
            int poczatek_a= (int)(rysunekObiektuNutowego1.poczatek_a+ rysunekObiektuNutowego1.szerokosc_a-interlinia_system_a/3);
            int koniec_a= (int)(rysunekObiektuNutowego2.poczatek_a+ interlinia_system_a / 3);

            Nuta nuta1 = (Nuta)(rysunekObiektuNutowego1.obiektNutowy);
            Nuta nuta2 = (Nuta)(rysunekObiektuNutowego2.obiektNutowy);

            Pen p = new Pen(Color.Black);
            p.Width = 2f * grubosc_linii_a;

            int poziom1_a = WysokoscLinii_system_a(nuta1.NumerLinii(klucz) - 1.4f);
            int poziom2_a = WysokoscLinii_system_a(nuta2.NumerLinii(klucz) - 1.4f);

            int poziom_a;
            if (poziom1_a < poziom2_a) poziom_a= poziom1_a; else poziom_a = poziom2_a;

            poziom_a += -pionowe_przesuniecie_aktualnej_linii_wirtualnej_a + rysunekObiektuNutowego1.przesuniecie_pionowe_w_wirtualnej_linii_a;
            

            int srodek_dziubka_x_z = (koniec_a + poczatek_a)/2;
            int srodek_dziubka_y_a = (int)(poziom_a + interlinia_system_a);
            if (czy_rysowac)
            {
           
                g.DrawLine(p,
                        X_a(poczatek_a),
                        Y_a(poziom_a),
                        X_a(srodek_dziubka_x_z+1),
                        Y_a(srodek_dziubka_y_a));

                g.DrawLine(p,
                            X_a(srodek_dziubka_x_z-1),
                            Y_a(srodek_dziubka_y_a),
                            X_a(koniec_a),
                            Y_a(poziom_a));
             
            }

        }

        public void RysujZnakiPrzykluczowe(Graphics g, int pozycja_a, ref int szerokosc_a, ZnakiPrzykluczowe znaki_przykluczowe, Klucz klucz,bool czy_rysowac)
        {
            szerokosc_a = 0;
            for (int i=0;i<znaki_przykluczowe.liczba_znakow;i++)
            {
                int szerokosc_znaku =0;
                string nuta_symbol="";
                string oktawa = "";
                if (znaki_przykluczowe.znak_chromatyczny=="krzyzyk")
                    nuta_symbol = ZnakiPrzykluczowe.kolejne_krzyzyki[i];
                else nuta_symbol = ZnakiPrzykluczowe.kolejne_bemole[i];

                if (klucz.kod == "wiol") oktawa = "dwukreślna"; else oktawa="mała";
                Nuta nuta= new Nuta(Nuta.NumerNuty(nuta_symbol, oktawa), "cala_nuta");
                

                float numerLinii = nuta.NumerLinii(klucz);
                if (numerLinii > 5.5 && znaki_przykluczowe.znak_chromatyczny == "krzyzyk" && klucz.kod == "wiol") numerLinii -= 3.5f;
                if (numerLinii > 5 && znaki_przykluczowe.znak_chromatyczny == "bemol" && klucz.kod == "wiol") numerLinii -= 3.5f;

                if (numerLinii > 4.5 && znaki_przykluczowe.znak_chromatyczny == "krzyzyk" && klucz.kod == "bas") numerLinii -= 3.5f;
                if (numerLinii > 4 && znaki_przykluczowe.znak_chromatyczny == "bemol" && klucz.kod == "bas") numerLinii -= 3.5f;


                if (znaki_przykluczowe.czy_obiekt_wyswietlany)
                {
                    RysujSymbol(g,
                                 new Symbol(znaki_przykluczowe.znak_chromatyczny, numerLinii),
                                 znaki_przykluczowe.kolor,
                                 pozycja_a + szerokosc_a,
                                 ref szerokosc_znaku,
                                 znaki_przykluczowe.czy_obiekt_wyswietlany,
                                 czy_rysowac
                                 );
                }
                szerokosc_a += szerokosc_znaku;
            }
        }

        public void RysujSymbol(Graphics g, Symbol symbol, Color kolor, int pozycja_a, ref int szerokosc, bool czy_symbol_wyswietlany, bool czy_rysowac)
        {
            Obrazek symbol_obrazek = (Obrazek)(Rysowanie.obrazki[symbol.kod]);
            int wysokosc = WysokoscLinii_system_a(symbol.nr_linii + symbol_obrazek.przes_pion_wzgledem_linii);
            Rysowanie.RysujSymbol(g, symbol, kolor, X_a(pozycja_a), Y_a(wysokosc), interlinia_system_a, ref szerokosc, czy_symbol_wyswietlany,czy_rysowac);
        }


        public void RysujNute(Graphics g, int pozycja_a, ref int szerokosc_a,Nuta nuta,Klucz klucz, float wysokosc_podpisu_l, bool czy_rysowac)
        {
            szerokosc_a = 0;
            int przenosnik = 0;
            float numerLinii = nuta.NumerLiniiEfektywny(klucz, ref przenosnik);
          
   
            int szerokosc_znaku_a=0;
            int szerokosc_symbolu_a = 0;
            int odstep_po_znaku_a=0;

            
            string znak_chromatyczny = nuta.znak_chromatyczny;

            string aktualnyZnakChromatyczny = (string)(aktualneZnakiChromatyczne.znaki[nuta.nr]);
            string efektywny_znak_chromatyczny = "";

            
            switch (znak_chromatyczny)
            {
                case "brak":
                    switch(aktualnyZnakChromatyczny)
                    {                        
                        case ("brak"):
                            efektywny_znak_chromatyczny = "brak";                            
                            break;
                        case ("bemol"):
                        case ("krzyzyk"):
                        case ("podwojny_krzyzyk"):
                        case ("podwojny_bemol"):
                            efektywny_znak_chromatyczny = "kasownik";                            
                            break;
                        default: throw new Exception("Nieznany znak chromatyczny");
                    }
                    break;
                case ("bemol"):
                case ("krzyzyk"):
                case ("podwojny_krzyzyk"):
                case ("podwojny_bemol"):
                    if(znak_chromatyczny==aktualnyZnakChromatyczny)
                    {
                        efektywny_znak_chromatyczny = "brak";
                    }
                    else
                    {
                        efektywny_znak_chromatyczny = znak_chromatyczny;
                    }                
                    break;
                default: throw new Exception("Nieznany znak chromatyczny");
            }
            aktualneZnakiChromatyczne.znaki[nuta.nr] = znak_chromatyczny;
     

            if (efektywny_znak_chromatyczny != "brak")
            {
                  RysujSymbol(g,
                              new Symbol(efektywny_znak_chromatyczny, numerLinii),
                              nuta.kolor,
                              pozycja_a,
                              ref szerokosc_znaku_a,
                              nuta.czy_obiekt_wyswietlany,
                              czy_rysowac
                              );
                odstep_po_znaku_a = (int)(Parametry.GrafikaNut.odstep_po_znaku_chromatycznym_i * interlinia_system_a);              
            }

            
            RysujSymbol(g,
                new Symbol(numerLinii < 3 ? nuta.wartosc_rytmiczna.obrazek1.kod: nuta.wartosc_rytmiczna.obrazek2.kod, numerLinii),
                nuta.kolor,
                pozycja_a + szerokosc_znaku_a + odstep_po_znaku_a,
                ref szerokosc_symbolu_a,
                nuta.czy_obiekt_wyswietlany,
                czy_rysowac);
            
            szerokosc_a = szerokosc_znaku_a + odstep_po_znaku_a+ szerokosc_symbolu_a;
            
            if (nuta.rodzaj_podpisu!="brak")
            {
                int podpis_x_a = X_a(pozycja_a + szerokosc_a/2 );
                int podpis_y_a = Y_a(WysokoscLinii_system_a(wysokosc_podpisu_l));

                Rysowanie.RysujPodpisNuty(g, nuta, podpis_x_a, podpis_y_a,czy_rysowac);

            }

            //dodatkowe linie
            if ((numerLinii > 5 || numerLinii <= 0) && nuta.czy_obiekt_wyswietlany)
            {
                /*
                float dlugosc = 0;
                if (nuta.wartosc_rytmiczna.nazwa == "cala_nuta") dlugosc = 2.8f;
                else dlugosc = 2.2f;
                */

                int a = 0;
                int b = 0;
                int maksymalna_linia = ((int)(numerLinii * 2)) / 2;
                if (numerLinii > 5.0f) { a = 6; b = maksymalna_linia; }
                if (numerLinii <= 0.0f) { a = maksymalna_linia; b = 0; }

                for (int i = a; i <= b; i++)
                {
                    Pen p = new Pen(Color.Black);
                    p.Width = 1f * grubosc_linii_a;
                    int wysuniecie_lewe_a = (int)(Parametry.GrafikaNut.odstep_po_znaku_chromatycznym_i * interlinia_system_a);
                    int wysuniecie_prawe_a = wysuniecie_lewe_a;
                    if (szerokosc_znaku_a > 0) wysuniecie_lewe_a = 0;                    
                    if (czy_rysowac)
                    {
                        SmoothingMode smoothingMode = g.SmoothingMode;
                        g.SmoothingMode = SmoothingMode.Default;
                        g.DrawLine(p,
                                            X_a(pozycja_a + szerokosc_znaku_a - wysuniecie_lewe_a),
                                            Y_a(WysokoscLinii_system_a(i)),
                                            X_a(pozycja_a + szerokosc_znaku_a + odstep_po_znaku_a + szerokosc_symbolu_a + wysuniecie_prawe_a),
                                            Y_a(WysokoscLinii_system_a(i)));
                        g.SmoothingMode = smoothingMode;
                    }
                }
            }
            
            if (przenosnik != 0 && nuta.czy_obiekt_wyswietlany)
            {

                Pen p = new Pen(Color.Black);
                System.Drawing.Font drawFont = new System.Drawing.Font("Arial", interlinia_system_a);
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                float x = pozycja_a - 2 * interlinia_system_a;
                float y;
                
                if (przenosnik == 1) y = Y_a(WysokoscLinii_system_a(9.5f));
                else y = Y_a(WysokoscLinii_system_a(-4.5f));

                if (czy_rysowac)
                {
                    if (przenosnik == 1)
                        g.DrawString("8", drawFont, drawBrush, X_a(x), y - 0.4f * interlinia_system_a);
                    else g.DrawString("8", drawFont, drawBrush, X_a(x), y - 1.1f * interlinia_system_a);

                    drawFont = new System.Drawing.Font("Arial Black", interlinia_system_a * 55 / 100);
                    //x += (0.9f) * interlinia_system_a;
                    //g.DrawString("va", drawFont, drawBrush, X_a(x), y );


                    p.Width = 1f * grubosc_linii_a;
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    g.DrawLine(p,
                                               X_a(x + 1.3f * interlinia_system_a),
                                               y,
                                               X_a(x + 4.8f * interlinia_system_a),
                                               y);
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    g.DrawLine(p,
                                               X_a(x + 4.4f * interlinia_system_a),
                                               y,
                                               X_a(x + 4.8f * interlinia_system_a),
                                               y);
                    g.DrawLine(p,
                                               X_a(x + 4.8f * interlinia_system_a),
                                               y,
                                               X_a(x + 4.8f * interlinia_system_a),
                                               y + 0.3f * interlinia_system_a * przenosnik);
                }
            }
            

        }

       


        

        public void RysujLinie(Graphics g, int nr_linii, int koniec_a,bool czy_rysowac)
        {
            Pen p = new Pen(Color.Black);
            p.Width = grubosc_linii_a;



            int poczatek_system_a = 0;
            int koniec_system_a = koniec_a;
            int wysokosc_system_a = WysokoscLinii_system_a(nr_linii);

            if (czy_rysowac)
            {
                SmoothingMode smoothingMode = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.Default;
                g.DrawLine(p, X_a(poczatek_system_a),
                          Y_a(wysokosc_system_a),
                          X_a(koniec_system_a),
                          Y_a(wysokosc_system_a));
                g.SmoothingMode = smoothingMode;
            }
        }

        public int WysokoscLinii_system_a(float nr_linii)
        {
           
            return wierszNutowy.margines_gorny_i +
                    (int)((5 - nr_linii) * interlinia_system_a);
        }

        public void RysujKreskeTaktowa(Graphics g, int pozycja_a, ref int szerokosc,KreskaTaktowa kreska_taktowa,bool czy_rysowac)
        {
            SmoothingMode smoothingMode=0;
            if (czy_rysowac)
            {
                smoothingMode = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.Default;
            }
            Pen p = new Pen(kreska_taktowa.kolor);
     

            switch (kreska_taktowa.kod)
            {
                case "zwykła":
                    int grubosc1_a = 2 * grubosc_linii_a;
                    p.Width = grubosc1_a;
                    if (kreska_taktowa.czy_obiekt_wyswietlany)
                    {
                        if (czy_rysowac)
                        g.DrawLine(p, X_a(pozycja_a + grubosc1_a / 2),
                                      Y_a(WysokoscLinii_system_a(1)),
                                      X_a(pozycja_a + grubosc1_a / 2),
                                      Y_a(WysokoscLinii_system_a(5)));
                    }
                    szerokosc = grubosc1_a/2;
                    break;

                case "koniec":
                  
                    grubosc1_a = 2 * grubosc_linii_a;
                    int grubosc2_a = 6 * grubosc_linii_a;
                    int odstep = 2 * grubosc_linii_a;
                    p.Width = grubosc1_a;
                    if (kreska_taktowa.czy_obiekt_wyswietlany)
                    {
                        if (czy_rysowac)
                            g.DrawLine(p, X_a(pozycja_a + grubosc1_a / 2),
                                Y_a(WysokoscLinii_system_a(1)),
                                X_a(pozycja_a + grubosc1_a / 2),
                                Y_a(WysokoscLinii_system_a(5)));
                    }
                    p.Width = grubosc2_a;
                    if (kreska_taktowa.czy_obiekt_wyswietlany)
                    {
                        if (czy_rysowac)
                            g.DrawLine(p, X_a(pozycja_a + grubosc1_a + odstep + grubosc2_a / 2),
                                Y_a(WysokoscLinii_system_a(1)),
                                X_a(pozycja_a + grubosc1_a + odstep + grubosc2_a / 2),
                                Y_a(WysokoscLinii_system_a(5)));
                    }
                    szerokosc = grubosc1_a + odstep + grubosc2_a -1;
                    break;
            }
            if (czy_rysowac)
            {
                g.SmoothingMode = smoothingMode;
            }
            //ostatnie_znaki_chromatyczne = new Hashtable();

        }

    }


    public class Rysowanie
    {        
        public static Hashtable obrazki;
        public static int interlinia_podstawowa;
        public static int grubosc_linii_podstawowa;

  
        public Rysowanie()
        {
            obrazki = new Hashtable();
            obrazki.Add("klucz_g", new Obrazek("klucz_g","klucz_g","KS2.Resources.klucz__g.png", 8f, 7.3f, 0, 1, 0));
            obrazki.Add("klucz_f", new Obrazek("klucz_f","klucz_f","KS2.Resources.klucz_f.png", 3.05f, 5f, 0, 2, 0));
            obrazki.Add("klucz_c", new Obrazek("klucz_c","klucz_c", "KS2.Resources.klucz_c.png", 1, 0.5f, 0, 3, 0));

             obrazki.Add("cala_nuta", new Obrazek("cala_nuta","cala_nuta","KS2.Resources.cala_nuta.png",5.08f,4.44f,2,1,1));
            //obrazki.Add("cala_nuta", new Obrazek("cala_nuta", "cala_nuta", "KS2.Resources.cala_nuta.png", 4.80f, 3.85f, 2, 1, 1));
            obrazki.Add("polnuta_w_gore", new Obrazek("polnuta_w_gore","polnuta","KS2.Resources.polnuta_w_gore.png", 3.7f, 3.2f,2,2,2 ));
            obrazki.Add("cwiercnuta_w_gore", new Obrazek("cwiercnuta_w_gore","cwiercnuta","KS2.Resources.cwiercnuta_w_gore.png", 3.7f, 3.2f,2,3,4 ));
            obrazki.Add("osemka_w_gore", new Obrazek("osemka_w_gore","osemka","KS2.Resources.osemka_w_gore.png", 3.7f, 3.2f,2,4,8 ));
            obrazki.Add("szesnastka_w_gore", new Obrazek("szesnastka_w_gore","szesnastka","KS2.Resources.szesnastka_w_gore.png", 3.7f, 3.2f,2,5,16 ));

            obrazki.Add("polnuta_w_dol", new Obrazek("polnuta_w_dol",null,"KS2.Resources.polnuta_w_dol.png", 3.7f, 0.5f,0,0,2 ));
            obrazki.Add("cwiercnuta_w_dol", new Obrazek("cwiercnuta_w_dol",null,"KS2.Resources.cwiercnuta_w_dol.png", 3.7f, 0.5f,0,0,4 ));
            obrazki.Add("osemka_w_dol", new Obrazek("osemka_w_dol",null,"KS2.Resources.osemka_w_dol.png", 3.7f, 0.5f,0,0,8 ));
            obrazki.Add("szesnastka_w_dol", new Obrazek("szesnastka_w_dol",null,"KS2.Resources.szesnastka_w_dol.png", 3.7f, 0.5f,0,0,16 ));

            obrazki.Add("bemol", new Obrazek("bemol","bemol","KS2.Resources.bemol.png", 2.4f, 1.8f,3,1,0));
            obrazki.Add("krzyzyk", new Obrazek("krzyzyk","krzyzyk","KS2.Resources.krzyzyk.png", 2.4f, 1.2f,3,2,0 ));            
            obrazki.Add("podwojny_bemol", new Obrazek("podwojny_bemol","podwojny_bemol","KS2.Resources.podwojny_bemol.png", 2.4f, 1.8f,3,3,0 ));
            obrazki.Add("podwojny_krzyzyk", new Obrazek("podwojny_krzyzyk","podwojny_krzyzyk","KS2.Resources.podwojny_krzyzyk.png", 1, 0.5f,3,4,0 ));
            obrazki.Add("kasownik", new Obrazek("kasownik","kasownik", "KS2.Resources.kasownik.png", 2.4f, 1.2f, 3, 5, 0));

            obrazki.Add("pauza_calonutowa_z_linia", new Obrazek("pauza_calonutowa_z_linia","calonutowa","KS2.Resources.pauza_calonutowa_z_linia.png", 0.75f, 4.0f,4,1,1));
            obrazki.Add("pauza_polnutowa_z_linia", new Obrazek("pauza_polnutowa_z_linia","polnutowa","KS2.Resources.pauza_polnutowa_z_linia.png", 0.75f, 3.75f,4,2,2));

            obrazki.Add("pauza_calonutowa", new Obrazek("pauza_calonutowa",null,"KS2.Resources.pauza_calonutowa.png", 0.75f, 4.0f,0,0,1));
            obrazki.Add("pauza_polnutowa", new Obrazek("pauza_polnutowa",null,"KS2.Resources.pauza_polnutowa.png", 0.75f, 3.75f,0,0,2));            

            obrazki.Add("pauza_cwiercnutowa", new Obrazek("pauza_cwiercnutowa","cwiercnutowa","KS2.Resources.pauza_cwiercnutowa.png", 2.9f, 4.6f,4,3,4));
            obrazki.Add("pauza_osemkowa", new Obrazek("pauza_osemkowa","osemkowa","KS2.Resources.pauza_osemkowa.png", 1.8f, 3.8f,4,4,8));
            obrazki.Add("pauza_szesnastkowa", new Obrazek("pauza_szesnastkowa","szesnastkowa","KS2.Resources.pauza_szesnastkowa.png", 2.8f, 3.77f,4,5,8));        
        }


        public void PrzeliczWspolczynniki(int FormHeight, 
                                          int FormWidth
                                         )
        {
            

            interlinia_podstawowa = 100;
            grubosc_linii_podstawowa = (int)(Parametry.Panel.wspolczynnik_grubosci_linii * interlinia_podstawowa);

            Arkusz arkusz = new Arkusz();            
            WierszArkusza wierszArkusza=null;
            WierszNutowy wierszNutowy = new WierszNutowy();
            WierszKlawiaturowy wierszKlawiaturowy = null;
            string oktawa_wiol = "mała";
            Tonacja tonacja = new Tonacja("ais-moll");
            Interwal[] schemat = Tonacja.schemat_gamy_mollowej_melodycznej;
            List<Nuta> ciagDzwiekow = tonacja.PodajCiagDzwiekow(tonacja.tonika_nazwa_literowa, oktawa_wiol, schemat, "cala_nuta");            
            Nuta nuta = new Nuta(0, "cala_nuta");

            List<Nuta> ciagDzwiekow13 = new List<Nuta>();
            for (int i = 0; i < 13; i++) ciagDzwiekow13.Add(nuta);

            switch (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen)
            {
                case ("rozpoznawanie_nuty"):                    
                    wierszNutowy.Dodaj(new Klucz("bas", tonacja));
                    wierszNutowy.Dodaj(nuta);
                    wierszNutowy.Dodaj(nuta);
                    wierszArkusza = wierszNutowy;
                    break;
                case ("rozpoznawanie_symbole"):
                    wierszNutowy.Dodaj(new Klucz("bas", tonacja));
                    wierszArkusza = wierszNutowy;
                    break;
                case ("rozpoznawanie_klawisze"):
                    /*
                    int nuty_od;
                    int nuty_do;
                    string klucz= Parametry.ZakresNut.klucz;
                    if (klucz == "wiol")
                    {
                        nuty_od = Parametry.ZakresNut.wiolinowy_od;
                        nuty_do = Parametry.ZakresNut.wiolinowy_do;
                    }
                    else
                    {
                        nuty_od = Parametry.ZakresNut.basowy_od;
                        nuty_do = Parametry.ZakresNut.basowy_do;
                    }
                    
                    wierszArkusza = new WierszKlawiaturowy(new Nuta(nuty_od), new Nuta(nuty_do), "caly_ekran");
                    */
                    wierszNutowy.Dodaj(new Klucz("bas", tonacja));
                    wierszNutowy.Dodaj(ciagDzwiekow, "brak");
                    wierszArkusza = wierszNutowy;
                    break;
                case ("generowanie_nuty"):
                    wierszNutowy.Dodaj(new Klucz("bas", tonacja));
                    wierszNutowy.Dodaj(ciagDzwiekow, "brak");
                    wierszArkusza = wierszNutowy;
                    break;
                case ("generowanie_interwaly"):
                    wierszNutowy.Dodaj(new Klucz("bas", tonacja));
                    wierszNutowy.Dodaj(ciagDzwiekow, "brak");
                    wierszArkusza = wierszNutowy;
                    break;
                case ("materialy_tonacje"):
                    wierszNutowy.Dodaj(new Klucz("bas", tonacja));
                    wierszNutowy.Dodaj(ciagDzwiekow, "brak");
                    wierszArkusza = wierszNutowy;
                    break;
                case ("edytor_podstawowy"):                   
                    wierszNutowy.Dodaj(new Klucz("bas", tonacja));
                    wierszNutowy.Dodaj(ciagDzwiekow13, "brak");
                    wierszArkusza = wierszNutowy;
                    break;
                case ("edytor_interwalowy"):                    
                    wierszNutowy.Dodaj(new Klucz("bas", tonacja));
                    wierszNutowy.Dodaj(ciagDzwiekow13, "brak");
                    wierszArkusza = wierszNutowy;
                    break;
                case ("rozpoznawanie_interwaly_pieciolinia"):
                    wierszNutowy.Dodaj(new Klucz("bas", tonacja));
                    wierszNutowy.Dodaj(nuta);
                    wierszNutowy.Dodaj(nuta);
                    wierszArkusza = wierszNutowy;
                    break;
                case ("rozpoznawanie_interwaly_klawiatura"):
                    wierszNutowy.Dodaj(new Klucz("bas", tonacja));
                    wierszNutowy.Dodaj(nuta);
                    wierszNutowy.Dodaj(nuta);
                    wierszArkusza = wierszNutowy;
                    break;

                default: throw new Exception("Nieznany tryb+rodzaj");

            }

            
            arkusz.DodajWiersz(wierszArkusza);
            arkusz.UstawWysokosciPodpisuIOdlegloscMiedzyWierszami();

            RysunekArkusza rysunekArkusza = new RysunekArkusza(  arkusz,
                                                                 interlinia_podstawowa,
                                                                 grubosc_linii_podstawowa,
                                                                 Parametry.Panel.margines_lewy_a,
                                                                 Parametry.Panel.margines_prawy_a,
                                                                 Parametry.Panel.margines_gorny_i,
                                                                 Parametry.Panel.margines_dolny_i,
                                                                 FormWidth
                                                             );
            
            rysunekArkusza.Rysuj(null,null,false);

            
     
            float interlinia_podstawowa1 = Parametry.Panel.powiekszenie *  interlinia_podstawowa * (FormWidth+0.0f)/rysunekArkusza.szerokosc_rysunku_calkowita_a ;
            
            float interlinia_podstawowa2= Parametry.Panel.powiekszenie * interlinia_podstawowa* (FormHeight + 0.0f)/rysunekArkusza.wysokosc_rysunku_calkowita_a;

            interlinia_podstawowa = (int)Math.Min(interlinia_podstawowa1, interlinia_podstawowa2);

            if (interlinia_podstawowa < 6) interlinia_podstawowa = 6;

            

            //Parametry.Panel.powiekszenie * FormHeight /
            //                   (Parametry.panel_szerokosc_wspolczynnik() * 8);

            /*
            interlinia_podstawowa = Parametry.Panel.powiekszenie*
                                      FormHeight / (Parametry.Panel.margines_gorny_i 
                                                  + Parametry.UkladArkusza.liczba_systemow * (4+Parametry.UkladArkusza.liczba_pol_dodatkowych_gora+ Parametry.UkladArkusza.liczba_pol_dodatkowych_dol)
                                                  + Parametry.Panel.margines_dolny_i);
             */
            //if (interlinia < Parametry.Panel.minimalna_interlinia) interlinia = Parametry.Panel.minimalna_interlinia;
            grubosc_linii_podstawowa = (int)(Parametry.Panel.wspolczynnik_grubosci_linii * interlinia_podstawowa);

            
        }





        public static void RysujSymbol(Graphics g, Symbol symbol, Color color, int x_a, int y_a, float interlinia_a, ref int szerokosc, bool czy_symbol_wyswietlany,bool czy_rysowac)
        {

            //System.Threading.Thread.Sleep(100);

            // Obrazek symbol_obrazek = (Obrazek)(Rysowanie.obrazki[symbol.kod]);  
            Image img= ((Obrazek)(Rysowanie.obrazki[symbol.kod])).getImage(color);
            Obrazek symbol_obrazek = (Obrazek)(Rysowanie.obrazki[symbol.kod]);

            int width = img.Width;
            int height = img.Height;

            float powiekszenie = ((float)interlinia_a * symbol_obrazek.wsp_pow) / (height);

            RectangleF destinationRect = new RectangleF(
                x_a,
                y_a,
                powiekszenie * width,
                (float)interlinia_a * symbol_obrazek.wsp_pow //powiekszenie * height                
                );

            RectangleF sourceRect = new RectangleF(0, 0, width, height);

            if (czy_symbol_wyswietlany)
            {
                if (czy_rysowac)
                g.DrawImage(
                    img,
                    destinationRect,
                    sourceRect,
                    GraphicsUnit.Pixel);
            }
            szerokosc = (int)(powiekszenie * width);
        }

        public static void RysujPodpisNuty(Graphics g, Nuta nuta, int x_a, int y_a, bool czy_rysowac)
        {
            int wysokosc_a = (int)(nuta.rozmiar_podpisu_i * interlinia_podstawowa);
            RysujPodpisNuty(g, nuta, wysokosc_a, x_a, y_a, czy_rysowac);
        }

        public static void RysujPodpisNuty(Graphics g, Nuta nuta, int wysokosc_a, int x_a, int y_a,bool czy_rysowac)
        {            

            
            Font drawFont1 = new Font(nuta.font_podpis, wysokosc_a, FontStyle.Bold);
            Font drawFont2 = new Font(nuta.font_podpis, wysokosc_a * 0.65f, FontStyle.Bold);

            if (nuta.oktawa_nr <= 1 && (nuta.rodzaj_podpisu== "nazwa_literowa" || nuta.rodzaj_podpisu == "nazwa_solmizacyjna"))
            {
                drawFont1 = new Font(nuta.font_podpis, wysokosc_a,  drawFont1.Style | FontStyle.Underline);
            }

           string nazwa_literowa = "";
            if (nuta.oktawa_nr > 2)
                nazwa_literowa = nuta.nazwa_literowa;
            else nazwa_literowa = nuta.nazwa_literowa.ToUpper();

            string tekst1;
            string tekst2;
            switch (nuta.rodzaj_podpisu)
            {
                case "inny": tekst1 = nuta.podpis_inny; tekst2 = ""; break;
                case "nazwa_literowa":
                    tekst1 = nazwa_literowa; tekst2 = "";
                    if (nuta.oktawa_nr >= 4)
                    {
                        tekst2 = Convert.ToString(nuta.oktawa_nr - 3);
                    }
                    break;
                case "nazwa_solmizacyjna":
                    tekst1 = nuta.nazwa_solmizacyjna; tekst2 = "";
                    break;
                default: throw new Exception("Nieznany rodzaj podpisu");

            }



            float dlugosc1 = TextRenderer.MeasureText(tekst1, drawFont1).Width - wysokosc_a * 0.72f;
            float dlugosc2 = TextRenderer.MeasureText(tekst2, drawFont2).Width;

            float dlugosc_podpisu = dlugosc1 + dlugosc2 * 0.75f;


            if (czy_rysowac)
            {
                TextRenderer.DrawText(g,
                                        tekst1,
                                        drawFont1,
                                        new Point(x_a - (int)(dlugosc_podpisu / 2),
                                                 y_a),
                                        nuta.kolor_podpisu,
                                        nuta.kolor_tla);

                TextRenderer.DrawText(g,
                                        tekst2,
                                        drawFont2,
                                        new Point(x_a - (int)(dlugosc_podpisu / 2) + (int)dlugosc1 + (int)(wysokosc_a * 0.2f),
                                                  y_a),
                                        nuta.kolor_podpisu,
                                        nuta.kolor_tla);
            }
            

            if (nuta.oktawa_nr == 0)
            {
                string spacje = new String(' ', tekst1.Length);
                if (czy_rysowac)
                {
                    TextRenderer.DrawText(g,
                                    spacje,
                                    drawFont1,
                                    new Point(x_a - (int)(dlugosc_podpisu / 2),
                                              y_a + wysokosc_a / 4),
                                    nuta.kolor_podpisu);
                }
            }
        }
        /*

        public static string PodajNazweObrazka(string wartosc_rytmiczna, float linia)
        {
            string obrazek_nazwa = "";


            switch (wartosc_rytmiczna)
            {
                case "cala_nuta":
                    obrazek_nazwa = "cala_nuta"; break;
                case "polnuta":
                    obrazek_nazwa = linia < 3 ? "polnuta_w_gore" : "polnuta_w_dol"; break;
                case "cwiercnuta":
                    obrazek_nazwa = linia < 3 ? "cwiercnuta_w_gore" : "cwiercnuta_w_dol"; break;
                case "osemka":
                    obrazek_nazwa = linia < 3 ? "osemka_w_gore" : "osemka_w_dol"; break;
                case "szesnastka":
                    obrazek_nazwa = linia < 3 ? "szesnastka_w_gore" : "szesnastka_w_dol"; break;
            }
            return obrazek_nazwa;
        }
        */

        public static void RysujTekst(Graphics g, String tekst, int x_a, int y_a, float wysokosc,Color kolor)
        {

    
            

            Pen p = new Pen(Color.Black);
            System.Drawing.Font drawFont = null;

            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(kolor);
  
            drawFont = new System.Drawing.Font("Arial", wysokosc);
            g.DrawString(tekst, drawFont, drawBrush, x_a, y_a);

        }


        /*  

       

      



        public void RysujSchody(Graphics g, int poczatek, int koniec)
        {
            Pen p = new Pen(Color.Black);
            p.Width = grubosc_linii;



            float powiekszenie = interlinia;
            //float stopien_szerokosc = 2f * powiekszenie;
            float stopien_szerokosc = 1f * powiekszenie;
            float stopien_wysokosc = 0.6f * powiekszenie;

            float x0 = width-10*powiekszenie;
            float y0 = height-10*powiekszenie;
            float x1 = x0 + stopien_szerokosc;
            float y1 = y0;




            for (int i=poczatek;i<=koniec;i++)
            {                
                g.DrawLine(p, x0 - grubosc_linii / 2f, y0, x1 + grubosc_linii / 2f, y1);
                Nuta nuta = new Nuta(i, "cala_nuta");

                System.Drawing.Font drawFont = new System.Drawing.Font("Arial", powiekszenie * 0.5f);
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                g.DrawString(nuta.nazwa_literowa, drawFont, drawBrush, (x0+x1)/2- powiekszenie * 0.3f, y0- powiekszenie*0.7f);


                x0 = x1;
                y0 = y1;
                float wys;
                if (nuta.odleglosc_do_nastepnego == 1)
                    wys = stopien_wysokosc;
                else wys = stopien_wysokosc*0.6f;
                y1 = y1 - wys;
                if (i != koniec)
                {
                    g.DrawLine(p, x0, y0, x1, y1);
                    y0 = y1;
                    x1 = x1 + stopien_szerokosc;
                }

            }

        }*/

    }




}
