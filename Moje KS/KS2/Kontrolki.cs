using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KS2
{
    public partial class OknoGlowne
    {

        private void glowny_groupBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.High;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
        }


        public OknoGlowne()
        {
            InitializeComponent();

            SetDoubleBuffered(this);
            foreach (Control control in this.Controls) SetDoubleBuffered(control);
            foreach (Control control in glowny_groupBox.Controls)
            {
                SetDoubleBuffered(control);
                // foreach (Control control2 in control.Controls)
                //    SetDoubleBuffered(control2);
            }

            SetDoubleBuffered(interwaly_okreslanie_liczebnosci_panel);




            List<Tonacja> lista_tonacji = new List<Tonacja>();


            for (int i = Tonacja.lista_tonacji_durowych_bemolowych.Count() - 1; i >= 0; i--)
                lista_tonacji.Add(new Tonacja(Tonacja.lista_tonacji_durowych_bemolowych[i]));
            for (int i = 0; i < Tonacja.lista_tonacji_durowych_bez_znakow.Count(); i++)
                lista_tonacji.Add(new Tonacja(Tonacja.lista_tonacji_durowych_bez_znakow[i]));
            for (int i = 0; i < Tonacja.lista_tonacji_durowych_krzyzykowych.Count(); i++)
                lista_tonacji.Add(new Tonacja(Tonacja.lista_tonacji_durowych_krzyzykowych[i]));
            for (int i = Tonacja.lista_tonacji_mollowych_bemolowych.Count() - 1; i >= 0; i--)
                lista_tonacji.Add(new Tonacja(Tonacja.lista_tonacji_mollowych_bemolowych[i]));
            for (int i = 0; i < Tonacja.lista_tonacji_mollowych_bez_znakow.Count(); i++)
                lista_tonacji.Add(new Tonacja(Tonacja.lista_tonacji_mollowych_bez_znakow[i]));
            for (int i = 0; i < Tonacja.lista_tonacji_mollowych_krzyzykowych.Count(); i++)
                lista_tonacji.Add(new Tonacja(Tonacja.lista_tonacji_mollowych_krzyzykowych[i]));



            panel1.BackColor = Parametry.Panel.tlo;
            kopiowanie_checkbox.Checked = Parametry.Ogolne.kopiowanie;

            //#############################DO UKRYCIA #####################################################################

            kontrolki_do_ukrycia_przy_wstecznym.Add("parametry_losowe_nuty_Zakres_button", 1);
            kontrolki_do_ukrycia_przy_wstecznym.Add("losowe_nuty_liczba_nut_groupBox", 1);
            kontrolki_do_ukrycia_przy_wstecznym.Add("losowe_nuty_tonacja_label", 1);
            kontrolki_do_ukrycia_przy_wstecznym.Add("losowe_nuty_tonacja_combobox", 1);
            kontrolki_do_ukrycia_przy_wstecznym.Add("generowanie_intwerwaly_pomijaj_ident_znaki_chrom_checkBox", 1);
            kontrolki_do_ukrycia_przy_wstecznym.Add("kolejne_checkBox", 1);

            //###########################ZNAKI CHROMATYCZNE#################################################

            int przycisk_rozmiar = 30;

            List<string> znaki_chromatyczne_lista = new List<string>();

            znaki_chromatyczne_lista.Add("brak");

            foreach (string nazwa_obrazka in Rysowanie.obrazki.Keys)
            {
                Obrazek obrazek = (Obrazek)(Rysowanie.obrazki[nazwa_obrazka]);
                if (obrazek.grupa == 3 && nazwa_obrazka != "kasownik")
                {
                    znaki_chromatyczne_lista.Add(nazwa_obrazka);
                }
            }

            foreach (string nazwa_obrazka in znaki_chromatyczne_lista)

            {

                CheckBox checkBox = new CheckBox();
                checkBox.Appearance = Appearance.Button;

                // button.Text = nazwa_obrazka;
                checkBox.Width = przycisk_rozmiar;
                checkBox.Height = przycisk_rozmiar;
                int kolejnosc;
                if (nazwa_obrazka != "brak")
                {
                    Obrazek obrazek = (Obrazek)(Rysowanie.obrazki[nazwa_obrazka]);
                    checkBox.Name = obrazek.przycisk;
                    checkBox.BackgroundImage = (Image)obrazek.getImage(Color.Black);
                    checkBox.BackgroundImageLayout = ImageLayout.Zoom;
                    kolejnosc = obrazek.kolejnosc;
                }
                else
                {
                    checkBox.Name = "brak";
                    checkBox.Text = "";
                    kolejnosc = 0;
                }
                checkBox.Checked = (bool)(Parametry.ZnakiChromatyczne.znaki[nazwa_obrazka]);


                checkBox.Location = new Point(160 + kolejnosc * (przycisk_rozmiar + 5), 0);
                checkBox.Click += (s, e) => { ZnakChromatycznyNacisniecie(checkBox.Name); };
                znaki_chromatyczne_panel.Controls.Add(checkBox);

            }
            znaki_chromatyczne_panel.Height = przycisk_rozmiar + 5;

            Label label = new Label();
            label.Text = "";
            label.Location = new Point(5, przycisk_rozmiar / 2);
            label.ForeColor = Color.Red;
            label.Name = "znaki_chromatyczne_alert_label";
            label.Width = 1000;
            znaki_chromatyczne_panel.Controls.Add(label);


            //##########################WARTOSCI RYTMICZNE###################################################
            int ile = 0;
            foreach (Obrazek obrazek in Rysowanie.obrazki.Values)
            {
                if (obrazek.grupa == 2)
                {
                    ile++;
                    CheckBox checkBox = new CheckBox();
                    checkBox.Appearance = Appearance.Button;
                    checkBox.Name = obrazek.przycisk;
                    // button.Text = nazwa_obrazka;
                    checkBox.Width = przycisk_rozmiar;
                    checkBox.Height = przycisk_rozmiar;

                    checkBox.BackgroundImage = (Image)obrazek.getImage(Color.Black);
                    checkBox.BackgroundImageLayout = ImageLayout.Zoom;
                    int kolejnosc = obrazek.kolejnosc;

                    checkBox.Checked = (bool)(Parametry.WartosciRytmiczne.wartosci[obrazek.przycisk]);

                    checkBox.Location = new Point(160 + (kolejnosc - 1) * (przycisk_rozmiar + 5), 0);
                    checkBox.Click += (s, e) => { WartoscRytmicznaNacisniecie(checkBox.Name); };
                    wartosci_rytmiczne_panel.Controls.Add(checkBox);
                }
            }
            wartosci_rytmiczne_panel.Height = przycisk_rozmiar + 5;

            label = new Label();
            label.Text = "";
            label.Location = new Point(5, przycisk_rozmiar / 2);
            label.ForeColor = Color.Red;
            label.Name = "wartosci_rytmiczne_alert_label";
            label.Width = 1000;
            wartosci_rytmiczne_panel.Controls.Add(label);

            //############################PAUZY########################################################################

            foreach (Obrazek obrazek in Rysowanie.obrazki.Values)
            {
                if (obrazek.grupa == 4)
                {
                    CheckBox checkBox = new CheckBox();
                    checkBox.Appearance = Appearance.Button;

                    checkBox.Name = obrazek.przycisk;
                    checkBox.Width = przycisk_rozmiar;
                    checkBox.Height = przycisk_rozmiar;
                    checkBox.BackgroundImage = (Image)obrazek.getImage(Color.Black);
                    checkBox.BackgroundImageLayout = ImageLayout.Zoom;
                    int kolejnosc = obrazek.kolejnosc;

                    checkBox.Checked = (bool)(Parametry.Pauzy.pauzy[obrazek.przycisk]);


                    checkBox.Location = new Point(160 + (kolejnosc - 1) * (przycisk_rozmiar + 5), 0);
                    checkBox.Click += (s, e) => { PauzaCheckboxNacisniecie(checkBox.Name); };
                    pauzy_panel.Controls.Add(checkBox);
                }
            }
            pauzy_panel.Height = przycisk_rozmiar + 5;



            //######################## CWICZENIA ########################################################################


            var dict = new Dictionary<string, string>();
            dict.Add("literowe", "nazwy literowe");
            dict.Add("solmizacyjne", "nazwy solmizacyjne");
            parametry_cwiczenia_nazwy_comboBox.DataSource = new BindingSource(dict, null);
            parametry_cwiczenia_nazwy_comboBox.DisplayMember = "Value";
            parametry_cwiczenia_nazwy_comboBox.ValueMember = "Key";
            parametry_cwiczenia_nazwy_comboBox.SelectedValue = Parametry.Cwiczenia.nazwy_nut;

            //#################### PARAMETRY LOSOWE NUTY#########################################################################


            var dict2 = new Dictionary<int, int>();
            for (int i = 0; i <= Parametry.LosoweNuty.maksymalna_liczba_nut_w_systemie; i++)
                dict2.Add(i, i);
            //int temp = Parametry.LosoweNuty.liczba_nut_w__systemie;
            parametry_losowe_nuty_liczba_nut_wiolinowy_comboBox.DataSource = new BindingSource(dict2, null);
            parametry_losowe_nuty_liczba_nut_wiolinowy_comboBox.DisplayMember = "Value";
            parametry_losowe_nuty_liczba_nut_wiolinowy_comboBox.ValueMember = "Key";
            //Parametry.LosoweNuty.liczba_nut_w__systemie=temp;
            parametry_losowe_nuty_liczba_nut_wiolinowy_comboBox.SelectedValue = Parametry.LosoweNuty.liczba_nut_wiolinowy;


            var dict6 = new Dictionary<int, int>();
            for (int i = 0; i <= Parametry.LosoweNuty.maksymalna_liczba_nut_w_systemie; i++)
                dict6.Add(i, i);
            //int temp = Parametry.LosoweNuty.liczba_nut_w__systemie;
            parametry_losowe_nuty_liczba_nut_basowy_comboBox.DataSource = new BindingSource(dict6, null);
            parametry_losowe_nuty_liczba_nut_basowy_comboBox.DisplayMember = "Value";
            parametry_losowe_nuty_liczba_nut_basowy_comboBox.ValueMember = "Key";
            //Parametry.LosoweNuty.liczba_nut_w__systemie=temp;
            parametry_losowe_nuty_liczba_nut_basowy_comboBox.SelectedValue = Parametry.LosoweNuty.liczba_nut_basowy;


            parametry_losowe_nuty_pokaz_nute_checkBox.Checked = Parametry.LosoweNuty.pokaz_nute;
            parametry_losowe_nuty_pokaz_podpis_checkBox.Checked = Parametry.LosoweNuty.pokaz_podpis;

            foreach (Tonacja tonacja in lista_tonacji)
            {                
                parametry_losowe_nuty_tonacja_combobox.Items.Add(tonacja);
            }            
            parametry_losowe_nuty_tonacja_combobox.SelectedIndex = parametry_losowe_nuty_tonacja_combobox.FindStringExact(Parametry.LosoweNuty.tonacja.ToString());


            //#################### PARAMETRY GENEROWANIE INTERWALY ##################################################################
            parametry_generowanie_intwerwaly_pokaz_pierwsza_nute_checkBox.Checked = Parametry.LosoweInterwaly.czy_pokazywac_pierwsza_nute;
            parametry_generowanie_intwerwaly_pokaz_druga_nute_checkBox.Checked = Parametry.LosoweInterwaly.czy_pokazywac_druga_nute;
            parametry_generowanie_intwerwaly_pokaz_podpis_checkBox.Checked = Parametry.LosoweInterwaly.czy_podpis;
            parametry_generowanie_intwerwaly_pomijaj_ident_znaki_chrom_checkBox.Checked = Parametry.LosoweInterwaly.czy_pomijac_identyczne_znaki_chromatyczne;
            foreach (Tonacja tonacja in lista_tonacji)
            {
                parametry_generowanie_interwaly_tonacje_comboBox.Items.Add(tonacja);
            }
            parametry_generowanie_interwaly_tonacje_comboBox.SelectedIndex = parametry_losowe_nuty_tonacja_combobox.FindStringExact(Parametry.LosoweInterwaly.tonacja.ToString());

            //############### PARAMETRY TONACJE ##################################################################

            parametry_materialy_tonacje_tetrachordy_checkBox.Checked = Parametry.Tonacje.czy_pokazywac_terachordy;
            parametry_materialy_tonacje_dziubki_poltonowe_checkBox.Checked = Parametry.Tonacje.czy_pokazywac_dziubki_poltonowe;
            parametry_materialy_tonacje_podpisy_nut_checkbox.Checked = Parametry.Tonacje.czy_pokazywac_podpisy_nut;
            parametry_materialy_tonacje_tytuly_checkBox.Checked = Parametry.Tonacje.czy_pokazywac_tytuly;


            foreach (Tonacja tonacja in lista_tonacji)
            {
                parametry_materialy_tonacje_lista_tonacji_comboBox.Items.Add(tonacja);                
                
            }
            parametry_materialy_tonacje_lista_tonacji_comboBox.SelectedIndex = parametry_materialy_tonacje_lista_tonacji_comboBox.FindStringExact(Parametry.Tonacje.tonacja.ToString());
                        
            var dict3 = new Dictionary<string, string>();
            dict3 = new Dictionary<string, string>();
            dict3.Add("wiol", "wiolinowy");
            dict3.Add("bas", "basowy");
            dict3.Add("wiolbas", "wiolinowy i basowy");
            this.parametry_materialy_tonacje_klucz_comboBox.DataSource = new BindingSource(dict3, null);
            this.parametry_materialy_tonacje_klucz_comboBox.DisplayMember = "Value";
            this.parametry_materialy_tonacje_klucz_comboBox.ValueMember = "Key";
            this.parametry_materialy_tonacje_klucz_comboBox.SelectedValue = Parametry.Tonacje.klucz;


            //################# KLUCZ ##################################

            wiolinowy_alert_label.Text = "";
            basowy_alert_label.Text = "";

            if (Parametry.Edytor.init_klucz == "wiol")
            {
                klucz_wiol_radioButton.Checked = true;
                klucz_bas_radioButton.Checked = false;
            }
            else
            {
                klucz_wiol_radioButton.Checked = false;
                klucz_bas_radioButton.Checked = true;
            }



            Nuta[] lista_wiolinowy = new Nuta[Parametry.Konfiguracja.wiolinowy_max - Parametry.Konfiguracja.wiolinowy_min + 1];
            for (int i = Parametry.Konfiguracja.wiolinowy_min; i <= Parametry.Konfiguracja.wiolinowy_max; i++)
                lista_wiolinowy[i - Parametry.Konfiguracja.wiolinowy_min] = new Nuta(i, "cala_nuta");

            wiol_od_combobox.DataSource = new BindingSource(lista_wiolinowy, null);
            wiol_od_combobox.DisplayMember = "pelna_nazwa";
            wiol_od_combobox.ValueMember = "nr";
            wiol_od_combobox.SelectedValue = Parametry.ZakresNut.wiolinowy_od;

            wiol_do_combobox.DataSource = new BindingSource(lista_wiolinowy, null);
            wiol_do_combobox.DisplayMember = "pelna_nazwa";
            wiol_do_combobox.ValueMember = "nr";
            wiol_do_combobox.SelectedValue = Parametry.ZakresNut.wiolinowy_do;

            Nuta[] lista_basowy = new Nuta[Parametry.Konfiguracja.basowy_max - Parametry.Konfiguracja.basowy_min + 1];
            for (int i = Parametry.Konfiguracja.basowy_min; i <= Parametry.Konfiguracja.basowy_max; i++)
                lista_basowy[i - Parametry.Konfiguracja.basowy_min] = new Nuta(i, "cala_nuta");

            bas_od_combobox.DataSource = new BindingSource(lista_basowy, null);
            bas_od_combobox.DisplayMember = "pelna_nazwa";
            bas_od_combobox.ValueMember = "nr";
            bas_od_combobox.SelectedValue = Parametry.ZakresNut.basowy_od;

            bas_do_combobox.DataSource = new BindingSource(lista_basowy, null);
            bas_do_combobox.DisplayMember = "pelna_nazwa";
            bas_do_combobox.ValueMember = "nr";
            bas_do_combobox.SelectedValue = Parametry.ZakresNut.basowy_do;



            //--------------------EDYTOR-------------
            /*
            int przycisk_rozmiar = 35;
            foreach ( string nazwa_obrazka in Rysowanie.obrazki.Keys)
            {                

                Obrazek obrazek =(Obrazek) (Rysowanie.obrazki[nazwa_obrazka]);
                if (obrazek.grupa !=0)
                {
                    Button button = new Button();
                    button.Name = nazwa_obrazka;
                    // button.Text = nazwa_obrazka;
                    button.Width = 35;
                    button.Height = 35;
                    button.Location = new Point(300+(obrazek.kolejnosc-1) * przycisk_rozmiar, (obrazek.grupa-2) * przycisk_rozmiar);
                    button.BackgroundImage = obrazek.img;
                    button.BackgroundImageLayout = ImageLayout.Zoom;

                    button.Click += (s, e) => { edytor.obrazek_nacisniecie(button.Name); };
                    parametry_edytor_panel.Controls.Add(button);
                }                
            }            
            parametry_edytor_panel.Height = przycisk_rozmiar*3;
            */
            //--------------------------------------



            var dict4 = new Dictionary<string, string>();
            for (int i = 0; i < Metrum.lista_metrum.Count(); i++)
            {
                dict4.Add(Metrum.lista_metrum[i], Metrum.lista_metrum[i]);
            }
            edytor_metrum_comboBox.DataSource = new BindingSource(dict4, null);
            edytor_metrum_comboBox.DisplayMember = "Value";
            edytor_metrum_comboBox.ValueMember = "Key";
            edytor_metrum_comboBox.SelectedValue = Parametry.Edytor.init_metrum.kod;


            foreach (Tonacja tonacja in lista_tonacji)
                edytor_tonacja_comboBox.Items.Add(tonacja);
            edytor_tonacja_comboBox.SelectedIndex = edytor_tonacja_comboBox.FindStringExact((Parametry.Edytor.init_tonacja).ToString());


            edytor_pokaz_nute_checkBox.Checked = Parametry.Edytor.init_pokaz_nute;
            edytor_pokaz_podpis_checkBox.Checked = Parametry.Edytor.init_pokaz_podpis;
            edytor_pokaz_pierwsza_nute_checkBox.Checked = Parametry.Edytor.init_pokaz_pierwsza_nute;
            edytor_pokaz_druga_nute_checkBox.Checked = Parametry.Edytor.init_pokaz_druga_nute;


           

            //edytor=new Edytor(Parametry.Tonacje.klucz,Parametry.Tonacje.tonacja);


            var dict5 = new Dictionary<string, string>();
            dict5.Add("brak", "brak");
            dict5.Add("krzyzykowe", "krzyżykowe");
            dict5.Add("bemolowe", "bemolowe");
            dict5.Add("krzyzykowe i bemolowe", "krzyżykowe i bemolowe");
            edytor_klawiatura_podpisy_klawiszy_comboBox.DataSource = new BindingSource(dict5, null);
            edytor_klawiatura_podpisy_klawiszy_comboBox.DisplayMember = "Value";
            edytor_klawiatura_podpisy_klawiszy_comboBox.ValueMember = "Key";
            edytor_klawiatura_podpisy_klawiszy_comboBox.SelectedValue = Parametry.Edytor.init_podpisy_klawiszy;

            // ############################ INTERWALY OKRESLANIE LICZEBNOSCI ######################################################
            int max_wysokosc = 0;
            int wysokosc = 24;
            foreach (Interwal interwal in Interwal.interwaly)
            {
                {

                    Label lbl = new Label();
                    string lbl_nazwa = "interwal_lbl" + interwal.kod;
                    lbl.Name = lbl_nazwa;
                    lbl.Text = interwal.symbol;
                    interwaly_okreslanie_liczebnosci_panel.Controls.Add(lbl);
                    interwaly_okreslanie_liczebnosci_panel.Controls[lbl_nazwa].Width = 25;
                    lbl.TextAlign = ContentAlignment.TopRight;
                    int y = 5 + wysokosc * (interwal.liczba_stopni - 1);
                    lbl.Location = new Point(5 + (interwal.lp * 110), y + 2);

                    NumericUpDown numeric = new NumericUpDown();
                    string txt_nazwa = "interwal_numericupdown_" + interwal.kod;
                    numeric.Text = "";// interwal.lp.ToString();
                    numeric.Name = txt_nazwa;
                    numeric.ValueChanged += (s, e) => { InterwalNumericUpDownNacisniecie(interwal.kod); };
                    numeric.ReadOnly = true;

                    interwaly_okreslanie_liczebnosci_panel.Controls.Add(numeric);
                    numeric.Width = 40;
                    numeric.Location = new Point(30 + (interwal.lp * 110), y);


                    Label lbl_zakres = new Label();
                    lbl_zakres.Name = "interwal_lbl_zakres" + interwal.kod;
                    lbl_zakres.Text = "()";
                    interwaly_okreslanie_liczebnosci_panel.Controls.Add(lbl_zakres);
                    lbl_zakres.Location = new Point(70 + (interwal.lp * 110), y + 2);
                    lbl_zakres.Width = 25;
                    max_wysokosc = Math.Max(max_wysokosc, y);

                }
            }
            interwaly_okreslanie_liczebnosci_panel.Height = max_wysokosc + wysokosc;

            // ######################### INTERWALY ZAZNACZANIE #############################################################
            max_wysokosc = 0;
            wysokosc = 24;
            foreach (Interwal interwal in Interwal.interwaly)
            {
                {

                    Button btn = new Button();
                    string btn_nazwa = "interwal_btn" + interwal.kod;
                    btn.Name = btn_nazwa;
                    btn.Text = interwal.symbol;
                    interwaly_zaznaczanie_panel.Controls.Add(btn);
                    interwaly_zaznaczanie_panel.Controls[btn_nazwa].Width = 95;
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    int y = 5 + wysokosc * (interwal.liczba_stopni - 1);
                    btn.Location = new Point(5 + (interwal.lp * 110), y + 2);
                    btn.Click += (s, e) => { InterwalButtonClick(interwal.kod); };
                    max_wysokosc = Math.Max(max_wysokosc, y);

                }
            }
            interwaly_zaznaczanie_panel.Height = max_wysokosc + wysokosc;

            label = new Label();
            label.Text = "XXXXX";
            label.Location = new Point(5 + (1 * 110), 6);
            label.ForeColor = Color.Red;
            label.Name = "interwaly_zaznaczanie_alert_label";
            label.Width = 1000;
            interwaly_zaznaczanie_panel.Controls.Add(label);

            //################################ ZEROWANIE INICJALNE ########################################

            if (Parametry.Ogolne.kopiowanie) KopiujDoSchowka();
            Zeruj();


        }


        private void UstawKontrolki()
        {
            panel1.Focus();
            UstawKontrolkiKlucza();
            
            zwin_button.Text = Parametry.Ogolne.pasek_zwin == true ? "Rozwiń" : "Zwiń";



           
            //################### PANEL PARAMETROW #######################################################################################

            string panel_parametry_nazwa = "";
            switch (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen)
            {
                case ("rozpoznawanie_nuty"):
                    panel_parametry_nazwa = "parametry_cwiczenia_panel";
                    break;
                case ("rozpoznawanie_symbole"):
                    panel_parametry_nazwa = "parametry_cwiczenia_panel";
                    break;
                case ("rozpoznawanie_klawisze"):
                    panel_parametry_nazwa = "parametry_cwiczenia_panel";
                    break;
                case ("generowanie_nuty"):
                    panel_parametry_nazwa = "parametry_losowe_nuty_panel";
                    break;
                case ("generowanie_interwaly"):
                    panel_parametry_nazwa = "parametry_generowanie_interwaly_panel";
                    break;
                case ("materialy_tonacje"):
                    panel_parametry_nazwa = "parametry_materialy_tonacje_panel";
                    break;
                case ("edytor_podstawowy"):
                    panel_parametry_nazwa = "parametry_edytor_panel";
                    break;
                case ("edytor_interwalowy"):
                    panel_parametry_nazwa = "parametry_edytor_panel";
                    break;
                case ("rozpoznawanie_interwaly_pieciolinia"):
                case ("rozpoznawanie_interwaly_klawiatura"):
                    panel_parametry_nazwa = "parametry_cwiczenia_panel";
                    break;

                default: throw new Exception("Nieznany tryb+rodzaj");

            }

            //odkrycie panelu parametrow, a ukrycie innych
            foreach (Control panel in glowny_groupBox.Controls.OfType<Panel>())
            {
                if (panel.Name.StartsWith("parametry"))
                {
                    panel.Location = parametry_cwiczenia_panel.Location;
                    if (panel.Name == panel_parametry_nazwa)
                    {
                        panel.Visible = true;
                    }
                    else
                    {
                        panel.Visible = false;
                    }
                }
            }

            //##################### INNE PANELE ########################################
            Hashtable panele_visible = new Hashtable();
            panele_visible[klucz_panel] = Parametry.ZakresNut.czy_parametryzuje() && (!Parametry.Ogolne.pasek_zwin);
            panele_visible[wartosci_rytmiczne_panel] = Parametry.WartosciRytmiczne.czy_parametryzuje() && (!Parametry.Ogolne.pasek_zwin); ;
            panele_visible[znaki_chromatyczne_panel] = Parametry.ZnakiChromatyczne.czy_parametryzuje() && (!Parametry.Ogolne.pasek_zwin); ;
            panele_visible[pauzy_panel] = Parametry.Pauzy.czy_parametryzuje() && (!Parametry.Ogolne.pasek_zwin); ;
            panele_visible[interwaly_okreslanie_liczebnosci_panel] = Parametry.LiczebnoscInterwalow.czy_parametryzuje() && (!Parametry.Ogolne.pasek_zwin); ;
            panele_visible[interwaly_zaznaczanie_panel] = Parametry.ZaznaczanieInterwalow.czy_parametryzuje() && (!Parametry.Ogolne.pasek_zwin); ;
            //panele_visible[klawiatura_panel] = Parametry.Klawiatura.czy_parametryzuje() && (!Parametry.Ogolne.pasek_zwin); ;
            panele_visible[rundy_panel] = true;//Parametry.Rundy.czy_parametryzuje();


            foreach (Panel panel in panele_visible.Keys)
                panel.Visible = (bool)panele_visible[panel];


            //#############  ROZMIESZCZENIE PASKOW NARZEDZI ##################################################

            int paski_narzedzi_x = 5;
            int paski_narzedzi_y = glowny_groupBox.Controls[panel_parametry_nazwa].Location.Y
                                         + glowny_groupBox.Controls[panel_parametry_nazwa].Height;

            //KLUCZ
            int przyrost_wysokosci1 = 0;
            if ((bool)panele_visible[klucz_panel])
            {
                klucz_panel.Location = new Point(paski_narzedzi_x, paski_narzedzi_y + przyrost_wysokosci1);
                przyrost_wysokosci1 += klucz_panel.Height;
            }

            //ZNAKI CHROMATYCZNE
            int przyrost_wysokosci2 = 10;
            if ((bool)panele_visible[znaki_chromatyczne_panel])
            {
                znaki_chromatyczne_panel.Location = new Point(paski_narzedzi_x + klucz_panel.Width + 5, paski_narzedzi_y + przyrost_wysokosci2);
                przyrost_wysokosci2 += znaki_chromatyczne_panel.Height;
            }

            //WARTOSCI RYTMICZNE
            if ((bool)panele_visible[wartosci_rytmiczne_panel])
            {
                wartosci_rytmiczne_panel.Location = new Point(paski_narzedzi_x + klucz_panel.Width + 5, paski_narzedzi_y + przyrost_wysokosci2);
                przyrost_wysokosci2 += wartosci_rytmiczne_panel.Height;
            }

            paski_narzedzi_y += Math.Max(przyrost_wysokosci1, przyrost_wysokosci2);

            //PAUZY
            if ((bool)panele_visible[pauzy_panel])
            {
                pauzy_panel.Location = new Point(paski_narzedzi_x + klucz_panel.Width + 5, paski_narzedzi_y);
                paski_narzedzi_y += pauzy_panel.Height;
            }

            //PANEL INTERWAŁY DLA EDYTORA
            if ((bool)panele_visible[interwaly_okreslanie_liczebnosci_panel])
            {

                interwaly_okreslanie_liczebnosci_panel.Location = new Point(paski_narzedzi_x, paski_narzedzi_y);
                paski_narzedzi_y += interwaly_okreslanie_liczebnosci_panel.Height;
            }


            //PANEL INTERWAŁY DLA ROZPOZNAWANIA
            if ((bool)panele_visible[interwaly_zaznaczanie_panel])
            {

                interwaly_zaznaczanie_panel.Location = new Point(paski_narzedzi_x, paski_narzedzi_y);
                paski_narzedzi_y += interwaly_zaznaczanie_panel.Height;
            }


            /*
            //PANEL KLAWIATURA
            if ((bool)panele_visible[klawiatura_panel])
            {

                klawiatura_panel.Location = new Point(paski_narzedzi_x, paski_narzedzi_y);
                paski_narzedzi_y += klawiatura_panel.Height;
            }
            */

            glowny_groupBox.Height = paski_narzedzi_y + 5; //this.glowny_groupBox.Controls[panel_parametry_nazwa].Height + this.glowny_groupBox.Controls[panel_parametry_nazwa].Location.Y + 5;

            //########################PANEL ARKUSZA#########################################################################

            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;

            panel1.Width = this.Width - 25;
            panel1.Width = this.Width - 25;
            panel1.Height = this.Height - this.glowny_groupBox.Height - this.menuStrip1.Height - titleHeight - 10;

            //Ponizsza linijka powoduje przeskok
            panel1.Location = new Point(0, this.glowny_groupBox.Height + this.menuStrip1.Height);


            //################# POJEDYNCZE KONTROLKI #####################################
            UstawPojedynczeKontrolki();            
            UstawCheckboxes();
            UstawTonacjeSekcje();
        }

        void UstawPojedynczeKontrolki()
        {

            if (Parametry.Tryb.rodzaj_cwiczen == "symbole")
                parametry_cwiczenia_nazwy_comboBox.Visible = false;
            else parametry_cwiczenia_nazwy_comboBox.Visible = true;

            if (Parametry.Tryb.tryb + "." + Parametry.Tryb.rodzaj_cwiczen == "rozpoznawanie.interwaly_pieciolinia" 
                || Parametry.Tryb.tryb + "." + Parametry.Tryb.rodzaj_cwiczen == "rozpoznawanie.interwaly_klawiatura")
                parametry_cwiczenia_kolejne_checkBox.Visible = false;
            else parametry_cwiczenia_kolejne_checkBox.Visible = true;


            rodzaj_label.Text = Parametry.rodzaj_nazwa_wyswietlana(Parametry.Tryb.tryb, Parametry.Tryb.rodzaj_cwiczen);


            if (Parametry.Tryb.tryb == "edytor")
            {
                Ekran ekran = Dane.AktualnyEkran();
                if (ekran != null)
                {
                    Edytor edytor = (Edytor)(ekran.zawartosc);
                    SystemNutowyDefinicja aktualnaDefinicjaSystemuNutowego = edytor.aktualnaDefinicjaSystemuNutowego();

                    wiol_od_combobox.SelectedValue = edytor.wiol_od;
                    wiol_do_combobox.SelectedValue = edytor.wiol_do;
                    bas_od_combobox.SelectedValue = edytor.bas_od;
                    bas_do_combobox.SelectedValue = edytor.bas_do;


                    edytor_pokaz_nute_checkBox.Checked = edytor.pokaz_nute;
                    edytor_pokaz_podpis_checkBox.Checked = edytor.pokaz_podpis;
                    edytor_pokaz_pierwsza_nute_checkBox.Checked = edytor.pokaz_pierwsza_nute;
                    edytor_pokaz_druga_nute_checkBox.Checked = edytor.pokaz_druga_nute;
                    edytor_tonacja_comboBox.SelectedIndex = edytor_tonacja_comboBox.FindStringExact((aktualnaDefinicjaSystemuNutowego.tonacja).ToString());
                    edytor_metrum_comboBox.SelectedValue = edytor.metrum.kod;
                    edytor_pokaz_klawiature_checkBox.Checked = edytor.pokaz_klawiature;
                    edytor_pokaz_pieciolinie_checkBox.Checked = edytor.pokaz_pieciolinie;

                    if (aktualnaDefinicjaSystemuNutowego.klucz == "wiol")
                    {
                        klucz_wiol_radioButton.Checked = true;
                        klucz_bas_radioButton.Checked = false;
                    }
                    else
                    {
                        klucz_wiol_radioButton.Checked = false;
                        klucz_bas_radioButton.Checked = true;
                    }

                    if (Parametry.Tryb.rodzaj_cwiczen == "interwalowy")
                    {
                        edytor_pokaz_pierwsza_nute_checkBox.Visible = true;
                        edytor_pokaz_druga_nute_checkBox.Visible = true;
                        edytor_pokaz_nute_checkBox.Visible = false;
                        edytor_metrum_comboBox.Visible = false;
                        metrum_label.Visible = false;
                        edytor_pokaz_klawiature_checkBox.Visible = true;
                    }
                    if (Parametry.Tryb.rodzaj_cwiczen == "podstawowy")
                    {
                        edytor_pokaz_pierwsza_nute_checkBox.Visible = false;
                        edytor_pokaz_druga_nute_checkBox.Visible = false;
                        edytor_pokaz_nute_checkBox.Visible = true;
                        edytor_metrum_comboBox.Visible = true;
                        metrum_label.Visible = true;
                        edytor_pokaz_klawiature_checkBox.Visible = true;
                    }

                }


            }

   
            if (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen == "rozpoznawanie_symbole")
                znaki_chromatyczne_panel.Controls["brak"].Visible = false;
            else znaki_chromatyczne_panel.Controls["brak"].Visible = true;


            if (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen == "generowanie_interwaly")
            {

                foreach (Interwal interwal in Interwal.interwaly)
                {

                    Label lbl_zakres = (Label)(interwaly_okreslanie_liczebnosci_panel.Controls["interwal_lbl_zakres" + interwal.kod]);
                    int ile_wszystkich = (int)(Parametry.Interwaly.ile_wszystkich_interwalow[interwal.kod]);
                    int ile_do_wylosowania = (int)(Parametry.LiczebnoscInterwalow.ile_interwalow_do_losowania[interwal.kod]);
                    lbl_zakres.Text = "(" + ile_wszystkich.ToString() + ")";
                    NumericUpDown numericUpDown = (NumericUpDown)(interwaly_okreslanie_liczebnosci_panel.Controls["interwal_numericupdown_" + interwal.kod]);
                    if (ile_do_wylosowania > ile_wszystkich)
                        numericUpDown.BackColor = Color.Red;
                    else
                    {
                        if (ile_do_wylosowania == 0)
                            numericUpDown.BackColor = Color.LightGreen;
                        else numericUpDown.BackColor = Color.Yellow;
                    }

                    if (ile_do_wylosowania == 0) numericUpDown.Text = "";

                }
            }

            if (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen == "rozpoznawanie_interwaly_pieciolinia"
                || Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen == "rozpoznawanie_interwaly_klawiatura")
            {

                foreach (Interwal interwal in Interwal.interwaly)
                {

                    //GeneratorElementow generator = new GeneratorElementow("interwal", Parametry.ZakresNut.klucz, interwal.kod);                                        
                    //int ile_wszystkich = generator.wszystkie_mozliwe_elementy.Count(); //(int)(Parametry.LiczebnoscInterwalow.ile_wszystkich_interwalow[interwal.kod]);                    
                    Button btn = (Button)(interwaly_zaznaczanie_panel.Controls["interwal_btn" + interwal.kod]);
                    int ile_wszystkich = (int)(Parametry.Interwaly.ile_wszystkich_interwalow[interwal.kod]);
                    int czy_zaznaczony = (int)(Parametry.ZaznaczanieInterwalow.zaznaczenie_interwalow[interwal.kod]);
                    btn.Text = interwal.symbol + "  (" + ile_wszystkich.ToString() + ")";
                    if (ile_wszystkich == 0 && czy_zaznaczony == 1)
                        btn.BackColor = Color.Red;
                    else
                    {
                        if (czy_zaznaczony == 0)
                            btn.BackColor = Color.LightGreen;
                        else btn.BackColor = Color.Yellow;

                        if (ile_wszystkich > 0) btn.Visible = true;
                        else btn.Visible = false;
                    }



                }
            }
        }



        private void UstawCheckboxes()
        {
            panel1.Focus();
            //####ALERTY ###################################################
            Hashtable alerty = new Hashtable();
            Parametry.OK(ref alerty);
            wiolinowy_alert_label.Text = (string)alerty["klucz_wiolinowy"];
            basowy_alert_label.Text = (string)alerty["klucz_basowy"];
            wartosci_rytmiczne_panel.Controls["wartosci_rytmiczne_alert_label"].Text = (string)alerty["wartosci_rytmiczne"];
            znaki_chromatyczne_panel.Controls["znaki_chromatyczne_alert_label"].Text = (string)alerty["znaki_chromatyczne"];
            interwaly_zaznaczanie_panel.Controls["interwaly_zaznaczanie_alert_label"].Text = (string)alerty["interwaly_zaznaczanie"];
            odpowiedzi_checkBox.Checked = Parametry.Cwiczenia.odpowiedzi;



            foreach (string znak in Parametry.ZnakiChromatyczne.znaki.Keys)
            {
                CheckBox checkBox = (CheckBox)(znaki_chromatyczne_panel.Controls[znak]);
                checkBox.Checked = (bool)(Parametry.ZnakiChromatyczne.znaki[znak]);
                if (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen == "rozpoznawanie_klawisze" && znak != "brak" && znak != "krzyzyk" && znak != "bemol")
                    checkBox.Visible = false;
                else checkBox.Visible = true;

            }
            foreach (string wartosc in Parametry.WartosciRytmiczne.wartosci.Keys)
            {
                ((CheckBox)(wartosci_rytmiczne_panel.Controls[wartosc])).Checked = (bool)(Parametry.WartosciRytmiczne.wartosci[wartosc]);
            }
            foreach (string pauza in Parametry.Pauzy.pauzy.Keys)
            {
                ((CheckBox)(pauzy_panel.Controls[pauza])).Checked = (bool)(Parametry.Pauzy.pauzy[pauza]);
            }
        }


        private void UstawKontrolkiKlucza()
        {


            bool wiol = false;
            bool bas = false;

            if (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen == "generowanie_nuty")
            {
                klucz_panel.Controls["klucz_groupbox"].Visible = false;
                wiol = true;
                bas = true;
            }
            else
            {
                klucz_panel.Controls["klucz_groupbox"].Visible = true;
                wiol = klucz_wiol_radioButton.Checked;
                bas = klucz_bas_radioButton.Checked;
            }
            klucz_panel.Controls["klucz_groupbox"].Visible = true;

            wiol_od_combobox.Visible = wiol;
            wiol_do_combobox.Visible = wiol;
            wiol_od_label.Visible = wiol;
            wiol_do_label.Visible = wiol;
            bas_od_combobox.Visible = bas;
            bas_do_combobox.Visible = bas;
            bas_od_label.Visible = bas;
            bas_do_label.Visible = bas;
        }

        private void UstawTonacjeSekcje()
        {
            parametry_materialy_tonacje_sekcje_checkedListBox.Items.Clear();
            parametry_materialy_tonacje_sekcje_checkedListBox.ItemCheck -= parametry_materialy_tonacje_sekcje_checkedListBox_ItemCheck;

            switch (Parametry.Tonacje.tonacja.tryb)
            {
                case ("moll"):
                    parametry_materialy_tonacje_sekcje_checkedListBox.Items.Add("znaki przykluczowe",
                     (bool)(Parametry.Tonacje.sekcje["znaki przykluczowe"]) == true ? CheckState.Checked : CheckState.Unchecked);
                    parametry_materialy_tonacje_sekcje_checkedListBox.Items.Add("odmiana naturalna",
                         (bool)(Parametry.Tonacje.sekcje["odmiana naturalna"]) == true ? CheckState.Checked : CheckState.Unchecked);
                    parametry_materialy_tonacje_sekcje_checkedListBox.Items.Add("odmiana harmoniczna",
                        (bool)(Parametry.Tonacje.sekcje["odmiana harmoniczna"]) == true ? CheckState.Checked : CheckState.Unchecked);
                    parametry_materialy_tonacje_sekcje_checkedListBox.Items.Add("odmiana dorycka",
                        (bool)(Parametry.Tonacje.sekcje["odmiana dorycka"]) == true ? CheckState.Checked : CheckState.Unchecked);
                    parametry_materialy_tonacje_sekcje_checkedListBox.Items.Add("odmiana melodyczna",
                        (bool)(Parametry.Tonacje.sekcje["odmiana melodyczna"]) == true ? CheckState.Checked : CheckState.Unchecked);
                    break;
                case ("dur"):
                    parametry_materialy_tonacje_sekcje_checkedListBox.Items.Add("znaki przykluczowe",
                       (bool)(Parametry.Tonacje.sekcje["znaki przykluczowe"]) == true ? CheckState.Checked : CheckState.Unchecked);
                    parametry_materialy_tonacje_sekcje_checkedListBox.Items.Add("odmiana naturalna",
                         (bool)(Parametry.Tonacje.sekcje["odmiana naturalna"]) == true ? CheckState.Checked : CheckState.Unchecked);
                    parametry_materialy_tonacje_sekcje_checkedListBox.Items.Add("odmiana harmoniczna",
                        (bool)(Parametry.Tonacje.sekcje["odmiana harmoniczna"]) == true ? CheckState.Checked : CheckState.Unchecked);

                    break;
                default: throw new Exception("nieznany tryb tonacji");
            };
            parametry_materialy_tonacje_sekcje_checkedListBox.ItemCheck += parametry_materialy_tonacje_sekcje_checkedListBox_ItemCheck;
        }
    }

}
