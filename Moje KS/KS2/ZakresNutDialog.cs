
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KS2
{
    public partial class ZakresNutDialog : Form
    {
        string tryb;
        string rodzaj;

        public ZakresNutDialog(string p_tryb, string p_rodzaj)
        {
            tryb = p_tryb;
            rodzaj = p_rodzaj;
            InitializeComponent();

        }

        private void ZakresNutDialog_Load(object sender, EventArgs e)
        {
            this.Text = "Zakres ćwiczeń: " + Parametry.rodzaj_nazwa_wyswietlana(tryb,rodzaj);

            var dict = new Dictionary<string, string>();
            dict = new Dictionary<string, string>();
            dict.Add("wiol", "wiolinowy");
            dict.Add("bas", "basowy");
            this.klucz_combobox.DataSource = new BindingSource(dict, null);
            this.klucz_combobox.DisplayMember = "Value";
            this.klucz_combobox.ValueMember = "Key";
            this.klucz_combobox.SelectedValue = Parametry.ZakresNut.klucz;


            Nuta[] lista_wiolinowy = new Nuta[Parametry.Konfiguracja.wiolinowy_max - Parametry.Konfiguracja.wiolinowy_min + 1];
            for (int i = Parametry.Konfiguracja.wiolinowy_min; i <= Parametry.Konfiguracja.wiolinowy_max; i++)
                lista_wiolinowy[i - Parametry.Konfiguracja.wiolinowy_min] = new Nuta(i, "cala_nuta");

            this.wiolinowy_od.DataSource = new BindingSource(lista_wiolinowy, null);
            this.wiolinowy_od.DisplayMember = "pelna_nazwa";
            this.wiolinowy_od.ValueMember = "nr";
            this.wiolinowy_od.SelectedValue = Parametry.ZakresNut.wiolinowy_od;

            this.wiolinowy_do.DataSource = new BindingSource(lista_wiolinowy, null);
            this.wiolinowy_do.DisplayMember = "pelna_nazwa";
            this.wiolinowy_do.ValueMember = "nr";
            this.wiolinowy_do.SelectedValue = Parametry.ZakresNut.wiolinowy_do;

            Nuta[] lista_basowy = new Nuta[Parametry.Konfiguracja.basowy_max - Parametry.Konfiguracja.basowy_min + 1];
            for (int i = Parametry.Konfiguracja.basowy_min; i <= Parametry.Konfiguracja.basowy_max; i++)
                lista_basowy[i - Parametry.Konfiguracja.basowy_min] = new Nuta(i, "cala_nuta");

            this.basowy_od.DataSource = new BindingSource(lista_basowy, null);
            this.basowy_od.DisplayMember = "pelna_nazwa";
            this.basowy_od.ValueMember = "nr";
            this.basowy_od.SelectedValue = Parametry.ZakresNut.basowy_od;

            this.basowy_do.DataSource = new BindingSource(lista_basowy, null);
            this.basowy_do.DisplayMember = "pelna_nazwa";
            this.basowy_do.ValueMember = "nr";
            this.basowy_do.SelectedValue = Parametry.ZakresNut.basowy_do;

            this.wiolinowy_alert_label.Text = "";
            this.basowy_alert_label.Text = "";
            this.znaki_chromatyczne_alert_label.Text = "";
            this.wartosci_rytmiczne_alert_label.Text = "";

            
            this.brak_checkBox.Checked = Parametry.ZnakiChromatyczne.znaki_chromatyczne_brak;
            this.bemol_checkBox.Checked = Parametry.ZnakiChromatyczne.bemol;
            this.krzyzyk_checkBox.Checked = Parametry.ZnakiChromatyczne.krzyzyk;            
            this.podwojny_krzyzyk_checkBox.Checked = Parametry.ZnakiChromatyczne.podwojny_krzyzyk;
            this.podwojny_bemol_checkBox.Checked = Parametry.ZnakiChromatyczne.podwojny_bemol;
            this.kasownik_checkBox.Checked = Parametry.ZnakiChromatyczne.kasownik;

            this.cala_nuta_checkBox.Checked = Parametry.WartosciRytmiczne.cala_nuta;
            this.polnuta_checkBox.Checked = Parametry.WartosciRytmiczne.polnuta;
            this.cwiercnuta_checkBox.Checked = Parametry.WartosciRytmiczne.cwiercnuta;
            this.osemka_checkBox.Checked = Parametry.WartosciRytmiczne.osemka;
            this.szesnastka_checkBox.Checked = Parametry.WartosciRytmiczne.szesnastka;


            this.pauza_calonutowa_checkBox.Checked = Parametry.Pauzy.calonutowa;
            this.pauza_polnutowa_checkBox.Checked = Parametry.Pauzy.polnutowa;
            this.pauza_cwiercnutowa_checkBox.Checked = Parametry.Pauzy.cwiercnutowa;
            this.pauza_osemkowa_checkBox.Checked = Parametry.Pauzy.osemkowa;
            this.pauza_szesnastkowa_checkBox.Checked = Parametry.Pauzy.szesnastkowa;

     

        }

        private void OK_Click(object sender, EventArgs e)
        {
            Parametry.Tryb.tryb = tryb;
            Parametry.Tryb.rodzaj_cwiczen = rodzaj;
            Parametry.ZakresNut.wiolinowy_od = ((Nuta)this.wiolinowy_od.SelectedItem).nr;
            Parametry.ZakresNut.wiolinowy_do = ((Nuta)this.wiolinowy_do.SelectedItem).nr;
            Parametry.ZakresNut.basowy_od = ((Nuta)this.basowy_od.SelectedItem).nr;
            Parametry.ZakresNut.basowy_do = ((Nuta)this.basowy_do.SelectedItem).nr;
            Parametry.ZakresNut.klucz = ((System.Collections.Generic.KeyValuePair<string, string>)this.klucz_combobox.SelectedItem).Key.ToString();


            if (rodzaj == "nuty")
            {
                Parametry.ZnakiChromatyczne.znaki_chromatyczne_brak = brak_checkBox.Checked;
            }
            if (rodzaj == "nuty" || rodzaj == "symbole")
            {
                
                Parametry.ZnakiChromatyczne.bemol = bemol_checkBox.Checked;
                Parametry.ZnakiChromatyczne.krzyzyk = krzyzyk_checkBox.Checked;
                Parametry.ZnakiChromatyczne.podwojny_krzyzyk = podwojny_krzyzyk_checkBox.Checked;
                Parametry.ZnakiChromatyczne.podwojny_bemol = podwojny_bemol_checkBox.Checked;
                Parametry.ZnakiChromatyczne.kasownik = kasownik_checkBox.Checked;
            }
            if (rodzaj == "symbole")
            {
                Parametry.ZnakiChromatyczne.kasownik = kasownik_checkBox.Checked;
            }

            Parametry.WartosciRytmiczne.cala_nuta = cala_nuta_checkBox.Checked;
            Parametry.WartosciRytmiczne.polnuta = polnuta_checkBox.Checked;
            Parametry.WartosciRytmiczne.cwiercnuta = cwiercnuta_checkBox.Checked;
            Parametry.WartosciRytmiczne.osemka = osemka_checkBox.Checked;
            Parametry.WartosciRytmiczne.szesnastka = szesnastka_checkBox.Checked;

            if (rodzaj == "symbole")
            {
                Parametry.Pauzy.calonutowa = pauza_calonutowa_checkBox.Checked;
                Parametry.Pauzy.polnutowa = pauza_polnutowa_checkBox.Checked;
                Parametry.Pauzy.cwiercnutowa = pauza_cwiercnutowa_checkBox.Checked;
                Parametry.Pauzy.osemkowa = pauza_osemkowa_checkBox.Checked;
                Parametry.Pauzy.szesnastkowa = pauza_szesnastkowa_checkBox.Checked;
            }
        }

        private void anuluj_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void UstawKontrolki()
        {

            string klucz = ((System.Collections.Generic.KeyValuePair<string, string>)this.klucz_combobox.SelectedItem).Key.ToString();
            bool ok_enabled = true;


            this.zakres_wiolinowy_groupBox.Enabled = (klucz == "wiol" || klucz == "wiolbas");
            this.zakres_basowy_groupBox.Enabled = (klucz == "bas" || klucz == "wiolbas");


            switch (rodzaj)
                {
                    case "nuty":
                        klucz_groupBox.Visible = true;
                        znaki_chromatycze_groupBox.Visible = true;
                        wartosci_rytmiczne_groupBox.Visible = true;
                        pauzy_groupBox.Visible = false;
                        brak_checkBox.Visible = true;
                        kasownik_checkBox.Visible = false;
                        break;
                    case "symbole":
                        klucz_groupBox.Visible = false;
                        znaki_chromatycze_groupBox.Visible = true;
                        wartosci_rytmiczne_groupBox.Visible = true;
                        pauzy_groupBox.Visible = true;
                        brak_checkBox.Visible = false;
                        kasownik_checkBox.Visible = true;
                        break;
                }

                switch (rodzaj)
                {
                    case "nuty":
                        if (this.wiolinowy_od.SelectedItem != null && this.wiolinowy_do.SelectedItem != null
                         && ((Nuta)this.wiolinowy_od.SelectedItem).nr > ((Nuta)this.wiolinowy_do.SelectedItem).nr - 1
                         && (klucz == "wiol" || klucz == "wiolbas")
                         )
                        {

                            wiolinowy_alert_label.Text = "wartość 'do' powinna być wyższa niż 'od' co najmniej o sekundę";
                            ok_enabled = false;
                        }
                        else wiolinowy_alert_label.Text = "";

                        if (this.basowy_od.SelectedItem != null && this.basowy_do.SelectedItem != null
                        && ((Nuta)this.basowy_od.SelectedItem).nr > ((Nuta)this.basowy_do.SelectedItem).nr - 1
                        && (klucz == "bas" || klucz == "wiolbas"))
                        {

                            basowy_alert_label.Text = "wartość 'do' powinna być wyższa niż 'od' co najmniej o sekundę";
                            ok_enabled = false;
                        }
                        else basowy_alert_label.Text = "";

                        if (!this.brak_checkBox.Checked
                             && !this.bemol_checkBox.Checked
                             && !this.krzyzyk_checkBox.Checked
                             && !this.podwojny_bemol_checkBox.Checked
                             && !this.podwojny_krzyzyk_checkBox.Checked
                             )
                        {
                            this.znaki_chromatyczne_alert_label.Text = "co najmniej jedna pozycja musi być zaznaczona";
                            ok_enabled = false;
                        }
                        else this.znaki_chromatyczne_alert_label.Text = "";

                        if (!this.cala_nuta_checkBox.Checked
                             && !this.polnuta_checkBox.Checked
                             && !this.cwiercnuta_checkBox.Checked
                             && !this.osemka_checkBox.Checked
                             && !this.szesnastka_checkBox.Checked)
                        {
                            this.wartosci_rytmiczne_alert_label.Text = "co najmniej jedna pozycja musi być zaznaczona";
                            ok_enabled = false;
                        }
                        else this.wartosci_rytmiczne_alert_label.Text = "";
                        symbole_ustawione_alert_label.Text = "";
                        break;

                        case "symbole":
                            wiolinowy_alert_label.Text = "";
                            basowy_alert_label.Text = "";
                            wartosci_rytmiczne_alert_label.Text = "";
                            znaki_chromatyczne_alert_label.Text = "";

                            bool czy_ustawione = false;
                            foreach (Control C in wartosci_rytmiczne_groupBox.Controls)
                            {
                                if (C.GetType() == typeof(System.Windows.Forms.CheckBox))
                                {
                                if (((System.Windows.Forms.CheckBox)C).Checked)
                                    czy_ustawione = true;
                                }
                            }
                            foreach (Control C in znaki_chromatycze_groupBox.Controls)
                            {
                                if (C.GetType() == typeof(System.Windows.Forms.CheckBox) && C.Name!="brak_checkBox")
                                {
                                    if (((System.Windows.Forms.CheckBox)C).Checked)
                                        czy_ustawione = true;
                                }
                            }
                            foreach (Control C in pauzy_groupBox.Controls)
                            {
                                if (C.GetType() == typeof(System.Windows.Forms.CheckBox))
                                {
                                    if (((System.Windows.Forms.CheckBox)C).Checked)
                                        czy_ustawione = true;
                                }
                            }

                        if (!czy_ustawione)
                        {
                            symbole_ustawione_alert_label.Text = "Co najmniej jedna pozycja musi być zaznaczona";
                            ok_enabled = false;
                        }
                        else symbole_ustawione_alert_label.Text = "";
                        break;

                }

                OK.Enabled = ok_enabled;

            
        }
 
        private void klucz_SelectedIndexChanged(object sender, EventArgs e)
        {

            UstawKontrolki();
        }

        private void wiolinowy_do_SelectedIndexChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void basowy_od_SelectedIndexChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void basowy_do_SelectedIndexChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void znaki_brak_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void bemol_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void krzyzyk_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

  
        private void cala_nuta_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void polnuta_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void cwiercnuta_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void osemka_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void szesnastka_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }


        private void zakres_nut_dolny_SelectedIndexChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

     

        private void ZakresNutDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void pod_krzyzyk_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void rodzaj_cwiczen_combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void znaki_chromatyczne_alert_label_Click(object sender, EventArgs e)
        {

        }

        private void wartosci_rytmiczne_groupBox_Enter(object sender, EventArgs e)
        {

        }

        private void pauza_calonutowa_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void pauza_polnutowa_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void pauza_cwiercnutowa_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void pauza_osemkowa_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void pauza_szesnastkowa_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            UstawKontrolki();
        }

        private void zakres_basowy_groupBox_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void klucz_label_Click(object sender, EventArgs e)
        {

        }

        private void zakres_wiolinowy_groupBox_Enter(object sender, EventArgs e)
        {

        }
    }
}

