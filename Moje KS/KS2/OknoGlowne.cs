using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;


namespace KS2
{
    public partial class OknoGlowne : Form
    {
        bool uruchomione = false;
        
        
        public Rysowanie rysowanie = new Rysowanie();
        //Edytor edytor = null;

        /*WERSJA Z BITMAPĄ
        Bitmap panelImage;
        */

        Arkusz panel1_arkusz = null;
        public RysunekArkusza panel1_rysunekarkusza = null;
         
        Hashtable kontrolki_do_ukrycia_przy_wstecznym = new Hashtable();


        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        private void UstawNoweOkno()
        {
            UstawKontrolki();
            rysowanie.PrzeliczWspolczynniki(panel1.Height, panel1.Width);
            OdswiezPanel();

        }


        private void Ekran_Resize(object sender, EventArgs e)
        {
            UstawNoweOkno();
        }



        int x = 0;

        private void panel1_arkusz_wygeneruj()
        {
            panel1_arkusz = null;
            Ekran ekran = null;
            if (Parametry.czy_numerowanie_ekranow())
            {
                ekran = Dane.AktualnyEkran();
                if (ekran != null)
                {
                    switch (Parametry.Tryb.tryb)
                    {
                        case "rozpoznawanie":
                            switch (Parametry.Tryb.rodzaj_cwiczen)
                            {
                                case "nuty":
                                    ZawartoscEkranu_RozpoznawanieNuty zawartoscEkranu_RozpoznawanieNuty = (ZawartoscEkranu_RozpoznawanieNuty)(ekran.zawartosc);
                                    panel1_arkusz = Arkusz.GetArkusz_rozpoznawanie_nuta(zawartoscEkranu_RozpoznawanieNuty);
                                    break;

                                case "symbole":
                                    panel1_arkusz = null;
                                    /*
                                    Symbol symbol = ((ZawartoscEkranu_RozpoznawanieSymbole)(ekran.zawartosc)).symbol;
                                    panel1_arkusz = PokazEkran_rozpoznawanie_symbole(symbol);
                                    */
                                    break;
                                case "klawisze":
                                    ZawartoscEkranu_RozpoznawanieKlawisze zawartoscEkranu_RozpoznawanieKlawisze = (ZawartoscEkranu_RozpoznawanieKlawisze)(ekran.zawartosc);
                                    panel1_arkusz = Arkusz.GetArkusz_rozpoznawanie_klawisze(zawartoscEkranu_RozpoznawanieKlawisze);
                                    break;
                                case "interwaly_pieciolinia":
                                    ZawartoscEkranu_RozpoznawanieInterwaly zawartoscEkranu_RozpoznawanieInterwaly = (ZawartoscEkranu_RozpoznawanieInterwaly)(ekran.zawartosc);
                                    panel1_arkusz = Arkusz.GetArkusz_rozpoznawanie_interwaly_pieciolinia(zawartoscEkranu_RozpoznawanieInterwaly, Dane.czy_odpowiedz);
                                    break;
                                default: throw new Exception("Nieznany rodzaj");
                            }
                            break;
                        case "generowanie":
                            switch (Parametry.Tryb.rodzaj_cwiczen)
                            {
                                case "nuty":
                                    ZawartoscEkranu_GenerowanieNuty zawartoscEkranu_GenerowanieNuty = (ZawartoscEkranu_GenerowanieNuty)(ekran.zawartosc);
                                    panel1_arkusz = Arkusz.GetArkusz_generowanie_nuty(zawartoscEkranu_GenerowanieNuty);
                                    break;
                                case "interwaly":
                                    ZawartoscEkranu_GenerowanieInterwaly zawartoscEkranu_GenerowanieInterwaly = (ZawartoscEkranu_GenerowanieInterwaly)(ekran.zawartosc);
                                    panel1_arkusz = Arkusz.GetArkusz_generowanie_interwaly(zawartoscEkranu_GenerowanieInterwaly);
                                    break;
                                default: throw new Exception("Nieprawidłowy rodzaj");

                            };
                            break;

                        case "edytor":
                            Edytor edytor = (Edytor)(ekran.zawartosc);
                            panel1_arkusz = edytor.UtworzEkran(panel1);
                            break;
                        default: throw new Exception("Nieznany tryb");
                    }
                }
            }
            else
            {
                switch (Parametry.Tryb.tryb)
                {
                    case "materialy":
                        switch (Parametry.Tryb.rodzaj_cwiczen)
                        {
                            case "tonacje":
                                Parametryzacja parametryzacja = new Parametryzacja();
                                parametryzacja.czy_pokazywac_dziubki_poltonowe = Parametry.Tonacje.czy_pokazywac_dziubki_poltonowe;
                                parametryzacja.czy_pokazywac_podpisy_nut = Parametry.Tonacje.czy_pokazywac_podpisy_nut;
                                parametryzacja.czy_pokazywac_terachordy = Parametry.Tonacje.czy_pokazywac_terachordy;
                                parametryzacja.czy_pokazywac_tytuly = Parametry.Tonacje.czy_pokazywac_tytuly;
                                parametryzacja.klucz = Parametry.Tonacje.klucz;
                                parametryzacja.sekcje = Parametry.Tonacje.sekcje;
                                panel1_arkusz = Arkusz.GetArkusz_materialy_tonacje(Parametry.Tonacje.tonacja, parametryzacja);
                                break;
                            default:
                                throw new Exception("Nieznany rodzaj");
                        };
                        break;
                }
            }

            //if (ekran == null && Parametry.czy_numerowanie_ekranow()) panel1_arkusz = null;
            float margines_gorny_i= Parametry.Panel.margines_gorny_i;
            if (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen == "rozpoznawanie_klawisze")
                margines_gorny_i = 12;

                panel1_rysunekarkusza = new RysunekArkusza(panel1_arkusz,
                                                      Rysowanie.interlinia_podstawowa,
                                                      Rysowanie.grubosc_linii_podstawowa,
                                                      Parametry.Panel.margines_lewy_a,
                                                      Parametry.Panel.margines_prawy_a,
                                                      margines_gorny_i,
                                                      Parametry.Panel.margines_dolny_i,
                                                      panel1.Width
                                                      );

        }


        bool info = true;
     
        private void panel1_Paint(object sender, PaintEventArgs e)
        {            
            panel1.Focus();
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.High;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;


            /* WERSJA Z BITMAPA
            e.Graphics.DrawImageUnscaled(panelImage, 0, 0);
            */



            //this.cofnij_button.Enabled = (Dane.nr_rundy >= 1);

            //runda_textBox.Text = (++x).ToString();
            int ile_cofnietych = Dane.nr_pokazywanego_ekranu - Dane.ekrany.Count + 1;
            if (ile_cofnietych != 0 && Parametry.czy_numerowanie_ekranow() && !Parametry.czy_kolejne())
                runda_textBox.Text = (Dane.nr_pokazywanego_ekranu - Dane.ekrany.Count + 1).ToString();
            else runda_textBox.Text = "";
            runda_textBox.Update();



            Parametry.AutoScrollPosition_Y = panel1.AutoScrollPosition.Y;
            if (!Parametry.OK()) return;
            //Graphics g = e.Graphics;

            //nadmiar
            //myGraphics = this.CreateGraphics();
            // panelImage = new Bitmap(panel1.Width, panel1.Height, myGraphics);
            //panelImage = new Bitmap(panel1.Width, panel1.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);//myGraphics);

            /*WERSJA Z BITMAPA
            panelImage = new Bitmap(panel1.Width, panel1.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);//myGraphics);                     
            Graphics g = Graphics.FromImage(panelImage);
 

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.High;
            g.CompositingQuality = CompositingQuality.HighQuality;

            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            // g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            */
            



            

            Graphics g = e.Graphics;
            Ekran ekran = null;
            if (Parametry.czy_numerowanie_ekranow())
            {
                ekran = Dane.AktualnyEkran();
            }

            if (Parametry.Tryb.rodzaj_cwiczen == "symbole")
            {
                Symbol symbol = ((ZawartoscEkranu_RozpoznawanieSymbole)(ekran.zawartosc)).symbol;
                int szerokosc = 0;
                Rysowanie.RysujSymbol(g,
                                      symbol,
                                      Color.Green,
                                      panel1.Width / 3,
                                      symbol.kod == "cala_nuta" ? panel1.Height / 40 : panel1.Height / 3,
                                      panel1.Height / 6f,
                                      ref szerokosc,
                                      true,
                                      true
                                      );
                if (Dane.czy_odpowiedz)
                    Rysowanie.RysujTekst(g,
                                          symbol.nazwa,
                                          panel1.Width / 20,
                                          panel1.Width / 40,
                                          panel1.Width / 20,
                                          Parametry.Cwiczenia.kolor_odpowiedzi1
                                          );
            }
            else
            {

                panel1_rysunekarkusza.Rysuj(panel1, g);
                if ((!Dane.czy_odpowiedz) && Parametry.Tryb.tryb + "." + Parametry.Tryb.rodzaj_cwiczen=="rozpoznawanie.klawisze")
                {
                    int szerokosc=0;
                    Nuta nuta1 = ((ZawartoscEkranu_RozpoznawanieKlawisze)(ekran.zawartosc)).nuta;
                    if (nuta1.znak_chromatyczny != "brak")
                    {
                        Rysowanie.RysujSymbol(g,
                                             new Symbol(nuta1.znak_chromatyczny),
                                             Color.Blue,
                                             panel1.Width*7/8,
                                             (int)(Rysowanie.interlinia_podstawowa * 0.5f),
                                             Rysowanie.interlinia_podstawowa * 4,
                                             ref szerokosc,
                                             true,
                                             true);
                    }
                }


               if (Dane.czy_odpowiedz)
                {
                    switch (Parametry.Tryb.tryb + "." + Parametry.Tryb.rodzaj_cwiczen)
                    {
                        case "rozpoznawanie.nuty":
                            Nuta nuta = ((ZawartoscEkranu_RozpoznawanieNuty)(ekran.zawartosc)).nuta;
                            Nuta nutax = new Nuta(nuta.nr, "cala_nuta", nuta.znak_chromatyczny);
                            switch (Parametry.Cwiczenia.nazwy_nut)
                            {
                                case "literowe": nutax.rodzaj_podpisu = "nazwa_literowa"; break;
                                case "solmizacyjne": nutax.rodzaj_podpisu = "nazwa_solmizacyjna"; break;
                            }
                            nutax.kolor_podpisu = Parametry.Cwiczenia.kolor_odpowiedzi1;
                            Rysowanie.RysujPodpisNuty(g,
                                                      nutax,
                                                      (int)Rysowanie.interlinia_podstawowa * 2,
                                                      panel1.Width - (int)Rysowanie.interlinia_podstawowa * 10,
                                                      (int)(Rysowanie.interlinia_podstawowa * 0.5f),
                                                      true);
                            break;
                        case "rozpoznawanie.klawisze":
                            Nuta nuta1 = ((ZawartoscEkranu_RozpoznawanieKlawisze)(ekran.zawartosc)).nuta;
                            Nuta nuta1x = new Nuta(nuta1.nr, "cala_nuta", nuta1.znak_chromatyczny);
                            switch (Parametry.Cwiczenia.nazwy_nut)
                            {
                                case "literowe": nuta1x.rodzaj_podpisu = "nazwa_literowa"; break;
                                case "solmizacyjne": nuta1x.rodzaj_podpisu = "nazwa_solmizacyjna"; break;
                            }
                            nuta1x.kolor_podpisu = Parametry.Cwiczenia.kolor_odpowiedzi1;
                            Rysowanie.RysujPodpisNuty(g,
                                                      nuta1x,
                                                      (int)Rysowanie.interlinia_podstawowa * 6,
                                                      panel1.Width/2,
                                                      (int)(Rysowanie.interlinia_podstawowa * 0.5f),
                                                      true);
                            break;
                        case "rozpoznawanie.interwaly_pieciolinia":
                        case "rozpoznawanie.interwaly_klawiatura":
                            Interwal interwal = (Interwal)(ekran.odpowiedz);

                            Rysowanie.RysujTekst(g,
                                         interwal.symbol,
                                         panel1.Width - (int)Rysowanie.interlinia_podstawowa * 7,
                                         (int)(Rysowanie.interlinia_podstawowa * 6),
                                         (int)Rysowanie.interlinia_podstawowa * 3,
                                         Parametry.Cwiczenia.kolor_odpowiedzi2
                                         );

                
                            break;
                            
                    }

                }
            }

            if (info)
            {
                Pen p = new Pen(Color.Black);
                System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 14);
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
                e.Graphics.DrawString("Uwaga! Niniejszy program powstał na WYŁĄCZNY użytek Marty Grudzińskiej", drawFont, drawBrush, 200, 20);
                e.Graphics.DrawString(" i osób przez nią upoważnionych.", drawFont, drawBrush, 200, 80);
                e.Graphics.DrawString(" Poznańska Szkoła Chóralna Jerzego Kurczewskiego, 2021", drawFont, drawBrush, 200, 140);
                info = false;
            }
        }


        public void OdswiezPanel()
        {
            /* WERSJA Z BITMAPĄ
            if (uruchomione) rysujPanel();
            panel1.Refresh();
            */

            /*
            switch ((tmp++) % 4)
            {
                case 0: panel1.BackColor = Color.Red; break;
                case 1: panel1.BackColor = Color.Green; break;
                case 2: panel1.BackColor = Color.Yellow; break;
                case 3: panel1.BackColor = Color.PaleGreen; break;

            }
            */
            int x = 0;

            if (uruchomione)
            {
                Parametry.AutoScrollPosition_Y = panel1.AutoScrollPosition.Y;

                panel1_arkusz_wygeneruj();
                if (panel1_arkusz != null)
                {
                    panel1_rysunekarkusza.wysokosc_rysunku_calkowita_a_oblicz();
                    panel1.AutoScrollMinSize = new Size(0, panel1_rysunekarkusza.wysokosc_rysunku_calkowita_a);
                }

                panel1.Refresh();
                if (Parametry.Ogolne.kopiowanie) KopiujDoSchowka();
            }



        }




        private void Zeruj(bool czy_zerowac_dane=true)
        {

            Dane.czy_odpowiedz = false;
            //DrawingControl.SuspendDrawing(this);
            // DrawingControl.SuspendDrawing(panel1);
            //czy_rysowac = false;
            this.SuspendLayout(); //ODKOMENTOWAC!!!
            if (czy_zerowac_dane)
            {
                Dane.nr_pokazywanego_ekranu = 0;
                Dane.ekrany = new List<Ekran>();

                if (Parametry.OK())
                {
                    Dane.ZaladujElementy();
                    if (Parametry.czy_numerowanie_ekranow())
                    {
                        Dane.ZerujListeEkranow();
                        if (!Parametry.czy_kolejne())
                           Dane.PrzejdzDoNastepnegoEkranu();
                    }
                }
            }

            UstawMenu();
            UstawKontrolki();
            
            uruchomione = true;
            //if (a != 0) return;
            glowny_groupBox.Update();

            rysowanie.PrzeliczWspolczynniki(panel1.Height, panel1.Width);

            // panel1.AutoScrollMinSize = new Size(0, panel1.Height); //ODKOMENTOWAC!!!
            //glowny_groupBox.Update();

            //   this.Update();
            // this.Update();            



            // System.Threading.Thread.Sleep(1000);

            
                OdswiezPanel();
           
            DrawingControl.ResumeDrawing(this);


  
            //DrawingControl.ResumeDrawing(panel1);
            // panel1.Refresh();
            // czy_rysowac = false;
            // 

            //this.ResumeLayout();

            //


        }


  

        private void EkranDalej()
        {
            switch (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen)
            {
                case ("materialy_tonacje"):
                    int indeks = parametry_materialy_tonacje_lista_tonacji_comboBox.SelectedIndex;
                    if (indeks < parametry_materialy_tonacje_lista_tonacji_comboBox.Items.Count - 1)
                        indeks++;
                    else indeks = 0;
                    parametry_materialy_tonacje_lista_tonacji_comboBox.SelectedIndex = indeks;
                    parametry_tonacje_lista_tonacji_zmiana_tonacji();
                    break;
                default:
                    if (Parametry.czy_numerowanie_ekranow())
                    {
                        Dane.PrzejdzDoNastepnegoEkranu();
                    };
                    break;
            }
        }

        private void EkranWstecz()
        {
            switch (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen)
            {
                case ("materialy_tonacje"):
                    int indeks = parametry_materialy_tonacje_lista_tonacji_comboBox.SelectedIndex;
                    if (indeks > 0)
                        indeks--;
                    else indeks = parametry_materialy_tonacje_lista_tonacji_comboBox.Items.Count - 1;
                    parametry_materialy_tonacje_lista_tonacji_comboBox.SelectedIndex = indeks;
                    parametry_tonacje_lista_tonacji_zmiana_tonacji();
                    break;
                default:
                    if (Parametry.czy_numerowanie_ekranow())
                    {
                        Dane.PrzejdzDoPoprzedniegoEkranu();
                    };
                    break;
            }
        }

     


        private void Ekran_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Parametry.OK()) return;
            
            if (Parametry.Tryb.tryb == "edytor")
            {
                Ekran ekran = Dane.AktualnyEkran();
                Edytor edytor = (Edytor)(ekran.zawartosc);
                edytor.NacisniecieKlawisza(this, sender, e);
                return;
            }

            if ( e.Control)
            {
               // kopiuj_button.BackColor = Color.Red;
            }

            if (e.KeyCode == Keys.Space)
            {
                EkranDalej();
                UstawKontrolki();
                OdswiezPanel();
            }
            if (e.KeyCode == Keys.Back)
            {

                EkranWstecz();
                UstawKontrolki();
                OdswiezPanel();
            }


            if (e.KeyCode == Keys.F12)
            {
                int x = 0;
                OdswiezPanel();
            }

            /*
            if (e.KeyCode == Keys.F12)
            {
                Parametry.GrafikaNut.wysokosc_podpisow_l = Parametry.GrafikaNut.wysokosc_podpisow_l - 0.5f;
                OdswiezPanel();
            }

            if (e.KeyCode == Keys.F11)
            {
                Parametry.GrafikaNut.wysokosc_podpisow_l = Parametry.GrafikaNut.wysokosc_podpisow_l + 0.2f;
                OdswiezPanel();
            }
            */

            if (e.KeyCode == Keys.F1)
            {
                Parametry.GrafikaNut.odstep_po_nucie_i= Parametry.GrafikaNut.odstep_po_nucie_i + 0.2f;
                //this.Odstep.Text = Parametry.GrafikaNut.odstep_po_nucie_i.ToString();
                OdswiezPanel();
            }

            if (e.KeyCode == Keys.F2)
            {
                Parametry.GrafikaNut.odstep_po_nucie_i = Parametry.GrafikaNut.odstep_po_nucie_i - 0.2f;
                //this.Odstep.Text = Parametry.GrafikaNut.odstep_po_nucie_i.ToString();
                OdswiezPanel();                
            }

            if (e.KeyCode == Keys.F3)
            {
                Parametry.GrafikaNut.odstep_po_kluczu_i = Parametry.GrafikaNut.odstep_po_kluczu_i + 0.2f;                
                OdswiezPanel();
            }

            if (e.KeyCode == Keys.F4)
            {
                Parametry.GrafikaNut.odstep_po_kluczu_i = Parametry.GrafikaNut.odstep_po_kluczu_i - 0.2f;
                OdswiezPanel();
            }

            if (e.KeyCode == Keys.Add)
            {
                Parametry.Panel.powiekszenie += 0.1f;
                rysowanie.PrzeliczWspolczynniki(panel1.Height, panel1.Width);
                OdswiezPanel();
            }

            if (e.KeyCode == Keys.Subtract)
            {
                Parametry.Panel.powiekszenie -= 0.1f;
                rysowanie.PrzeliczWspolczynniki(panel1.Height, panel1.Width);
                OdswiezPanel();
            }

        }



        public void KopiujDoSchowka()
        {


            Bitmap bmp = ObrobkaGrafiki.PanelToBitmap(panel1);


            /* Kopiowanie z wyświetlonego ekranu
             * zrezygnowałem z tego, bo gdy okno cześciowo wychodzi za ekran
             * to kopiuje tylko wyswetloną cześc,
             * a gdy jest przykryte przez inne okno, to kopiuje 
             * te cześć innego okna
             * 
            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);          
            Graphics myGraphics = panel1.CreateGraphics();
            Size s = panel1.Size;
            Bitmap bmp = new Bitmap(panel1.Width, panel1.Height);
            Graphics memoryGraphics = Graphics.FromImage(bmp);            
            int borderWidth = (this.Width - this.ClientSize.Width) / 2;
            int titleBarHeight = this.Height - this.ClientSize.Height -  borderWidth;
            memoryGraphics.CopyFromScreen(this.Left + panel1.Left + borderWidth , this.Top + panel1.Top + titleBarHeight, 0, 0, panel1.Size, CopyPixelOperation.SourceCopy);
            Clipboard.SetImage(bmp);
            */

            int max_x = panel1.ClientSize.Width - 1;  //bmp.Width - 1 - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
            int max_y= panel1.ClientSize.Height - 1; //bmp.Height - 1 - System.Windows.Forms.SystemInformation.;
            int x1 = max_x;
            int y1 = max_y;
            int x2 = 0;
            int y2 = 0;

            Color tlo = bmp.GetPixel(0, 0);

            for (int x = 0; x < max_x; x++)
            {
                for (int y = 0; y < max_y; y++)
                {
                    Color k = bmp.GetPixel(x, y);

                    if (k != tlo)
                    {
                        if (x < x1) x1 = x;
                        if (y < y1) y1 = y;
                        if (x > x2) x2 = x;
                        if (y > y2) y2 = y;
                    }
                }

            }
 
            int w = x2 - x1 + 1;
            int h = y2 - y1 + 1;

            if (x2 <= x1) return ;
            if (y2 <= y1) return ;

            Bitmap cuttedImage = ObrobkaGrafiki.CropBitmap(bmp, x1, y1, x2, y2);
            Clipboard.SetImage(cuttedImage);
            return ;
        }


    }




}
