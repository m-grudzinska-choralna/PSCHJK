using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KS2
{
    public partial class OknoGlowne
    {

        //-----------------------------------STEROWANIE-----------------------------------------
        private void Wstecz_button_Click(object sender, EventArgs e)
        {
            EkranWstecz();
            UstawKontrolki();
            OdswiezPanel();
        }

        private void Dalej_button_Click(object sender, EventArgs e)
        {
            EkranDalej();
            UstawKontrolki();
            OdswiezPanel();
        }


        private void zwin_button_Click(object sender, EventArgs e)
        {
            Parametry.Ogolne.pasek_zwin = !Parametry.Ogolne.pasek_zwin;
            Zeruj(false);
        }


        private void odpowiedzi_checkBox_CheckedChanged(object sender, EventArgs e)
        {

            if (uruchomione)
            {
                Parametry.Cwiczenia.odpowiedzi = odpowiedzi_checkBox.Checked;
                Dane.czy_odpowiedz = Parametry.Cwiczenia.odpowiedzi;
                OdswiezPanel();
            }
            panel1.Focus();
        }


        //-----------------------------------KOPIOWANIE-----------------------------------------
        private void kopiowanie_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            Parametry.Ogolne.kopiowanie = kopiowanie_checkbox.Checked;
            OdswiezPanel();
        }

        private void kopiuj_button_Click(object sender, EventArgs e)
        {
            KopiujDoSchowka();
        }

        //-----------------------------------KLUCZ-----------------------------------------


        private void klucz_wiol_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (uruchomione && rb.Checked)
            {
                Parametry.ZakresNut.klucz = "wiol";
                Parametry.Przelicz();

                if (Parametry.Tryb.tryb == "edytor")
                {
                    Ekran ekran = Dane.AktualnyEkran();
                    Edytor edytor = (Edytor)(ekran.zawartosc);
                    edytor.UstawKlucz("wiol");                    
                    UstawKontrolki();
                    OdswiezPanel();
                }
                if (!Parametry.OK())
                {
                    UstawKontrolki();
                    OdswiezPanel();
                    return;
                }
                if (Parametry.Tryb.tryb != "edytor") Zeruj();
            }
        }

        private void klucz_bas_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (uruchomione && rb.Checked)
            {
                Parametry.ZakresNut.klucz = "bas";
                Parametry.Przelicz();

                if (Parametry.Tryb.tryb == "edytor")
                {
                    Ekran ekran = Dane.AktualnyEkran();
                    Edytor edytor = (Edytor)(ekran.zawartosc);
                    edytor.UstawKlucz("bas");
                    UstawKontrolki();
                    OdswiezPanel();
                }
                if (!Parametry.OK())
                {
                    UstawKontrolki();
                    OdswiezPanel();
                    return;
                }
                if (Parametry.Tryb.tryb != "edytor") Zeruj();
            }
        }


        private void wiol_od_combobox_SelectionChangeCommitted(object sender, EventArgs e)
        {

            Parametry.ZakresNut.wiolinowy_od = ((Nuta)wiol_od_combobox.SelectedItem).nr;
            Parametry.Przelicz();
            UstawCheckboxes();
            

            if (Parametry.Tryb.tryb == "edytor")
            {
                Ekran ekran = Dane.AktualnyEkran();
                Edytor edytor = (Edytor)(ekran.zawartosc);
                edytor.wiol_od = Parametry.ZakresNut.wiolinowy_od;
            }
            if (!Parametry.OK())
            {
                UstawKontrolki();
                OdswiezPanel();
                return;
            }
            if (Parametry.Tryb.tryb != "edytor") Zeruj();
            else OdswiezPanel();
        }

        private void wiol_do_combobox_SelectionChangeCommitted(object sender, EventArgs e)
        {

            Parametry.ZakresNut.wiolinowy_do = ((Nuta)wiol_do_combobox.SelectedItem).nr;
            Parametry.Przelicz();
            UstawCheckboxes();
            if (Parametry.Tryb.tryb == "edytor")
            {
                Ekran ekran = Dane.AktualnyEkran();
                Edytor edytor = (Edytor)(ekran.zawartosc);
                edytor.wiol_do = Parametry.ZakresNut.wiolinowy_do;
            }
            if (!Parametry.OK())
            {
                UstawKontrolki();
                OdswiezPanel();
                return;
            }
            if (Parametry.Tryb.tryb != "edytor") Zeruj();
            else OdswiezPanel();


        }

        private void bas_od_combobox_SelectionChangeCommitted(object sender, EventArgs e)
        {

            Parametry.ZakresNut.basowy_od = ((Nuta)bas_od_combobox.SelectedItem).nr;
            Parametry.Przelicz();
            UstawCheckboxes();

            if (Parametry.Tryb.tryb == "edytor")
            {
                Ekran ekran = Dane.AktualnyEkran();
                Edytor edytor = (Edytor)(ekran.zawartosc);
                edytor.bas_od = Parametry.ZakresNut.basowy_od;
            }
            if (!Parametry.OK())
            {
                UstawKontrolki();
                OdswiezPanel();
                return;
            }
            if (Parametry.Tryb.tryb != "edytor") Zeruj();
            else OdswiezPanel();

        }

        private void bas_do_combobox_SelectionChangeCommitted(object sender, EventArgs e)
        {

            Parametry.ZakresNut.basowy_do = ((Nuta)bas_do_combobox.SelectedItem).nr;
            Parametry.Przelicz();
            UstawCheckboxes();

            if (Parametry.Tryb.tryb == "edytor")
            {
                Ekran ekran = Dane.AktualnyEkran();
                Edytor edytor = (Edytor)(ekran.zawartosc);
                edytor.bas_do = Parametry.ZakresNut.basowy_do;
            }
            if (!Parametry.OK())
            {
                UstawKontrolki();
                OdswiezPanel();
                return;
            }
            if (Parametry.Tryb.tryb != "edytor") Zeruj();
            else OdswiezPanel();


        }

        //--------------------------------- CHECKBOXES----------------------------------------------------------------------------

        private void ZnakChromatycznyNacisniecie(string checkbox_name)
        {
            if (uruchomione)
            {
                Parametry.ZnakiChromatyczne.znaki[checkbox_name] = ((CheckBox)(znaki_chromatyczne_panel.Controls[checkbox_name])).Checked;
                Parametry.Przelicz();
                UstawKontrolki();
                if (!Parametry.OK())
                {
                    OdswiezPanel();
                    return;
                }
                if (Parametry.Tryb.tryb != "edytor") Zeruj();

            }
            panel1.Focus();

        }


        private void WartoscRytmicznaNacisniecie(string checkbox_name)
        {
            if (uruchomione)
            {
                Parametry.WartosciRytmiczne.wartosci[checkbox_name] = ((CheckBox)(wartosci_rytmiczne_panel.Controls[checkbox_name])).Checked;
                UstawCheckboxes();
                if (!Parametry.OK())
                {
                    OdswiezPanel();
                    return;
                }
                if (Parametry.Tryb.tryb != "edytor") Zeruj();
            }
            panel1.Focus();

        }

        private void PauzaCheckboxNacisniecie(string checkbox_name)
        {
            if (uruchomione)
            {
                Parametry.Pauzy.pauzy[checkbox_name] = ((CheckBox)(pauzy_panel.Controls[checkbox_name])).Checked;
                UstawCheckboxes();

                if (!Parametry.OK())
                {
                    OdswiezPanel();
                    return;
                }
                if (Parametry.Tryb.tryb != "edytor") Zeruj();
            }
            panel1.Focus();

        }

        private void InterwalButtonClick(string interwal_kod)
        {

            int poprzedni_stan = (int)(Parametry.ZaznaczanieInterwalow.zaznaczenie_interwalow[interwal_kod]);
            Parametry.ZaznaczanieInterwalow.zaznaczenie_interwalow[interwal_kod] = 1 - poprzedni_stan;
            UstawKontrolki();
            if (!Parametry.OK())
            {

                OdswiezPanel();
                return;
            }
            if (Parametry.Tryb.tryb != "edytor") Zeruj();

        }


        private void InterwalNumericUpDownNacisniecie(string interwal_kod)
        {

            foreach (NumericUpDown numericUpDown in interwaly_okreslanie_liczebnosci_panel.Controls.OfType<NumericUpDown>())
            {
                string interwalkod = (numericUpDown.Name).Replace("interwal_numericupdown_", "");
                int ile = int.Parse("0" + numericUpDown.Value);
                Parametry.LiczebnoscInterwalow.ile_interwalow_do_losowania[interwalkod] = ile;
            }


            if (!Parametry.OK())
            {
                OdswiezPanel();
                return;
            }

            EkranDalej();
            UstawKontrolki();
            OdswiezPanel();
        }

        //---------------------------EDYTOR----------------------------------------------------------------------------------------------------

        private void edytor_metrum_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Ekran ekran = Dane.AktualnyEkran();
                Edytor edytor = (Edytor)(ekran.zawartosc);
                edytor.UstawMetrum(((System.Collections.Generic.KeyValuePair<string, string>)this.edytor_metrum_comboBox.SelectedItem).Key.ToString());
                OdswiezPanel();
            }
        }


        private void edytor_pokaz_nute_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Ekran ekran = Dane.AktualnyEkran();
                Edytor edytor = (Edytor)(ekran.zawartosc);
                edytor.UstawPokazNute(edytor_pokaz_nute_checkBox.Checked);
                OdswiezPanel();
            }
        }

        private void edytor_pokaz_podpis_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Ekran ekran = Dane.AktualnyEkran();
                Edytor edytor = (Edytor)(ekran.zawartosc);
                edytor.UstawPokazPodpis(edytor_pokaz_podpis_checkBox.Checked);
                OdswiezPanel();
            }
        }


        private void edytor_pokaz_pierwsza_nute_checkBox_CheckedChanged_1(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Ekran ekran = Dane.AktualnyEkran();
                Edytor edytor = (Edytor)(ekran.zawartosc);
                edytor.UstawPokazPierwszaNute(edytor_pokaz_pierwsza_nute_checkBox.Checked);
                OdswiezPanel();
            }
        }

        private void edytor_pokaz_pierwsza_druga_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Ekran ekran = Dane.AktualnyEkran();
                Edytor edytor = (Edytor)(ekran.zawartosc);
                edytor.UstawPokazDrugaNute(edytor_pokaz_druga_nute_checkBox.Checked);
                OdswiezPanel();
            }
        }



        private void edytor_pokaz_klawiature_checkBox_CheckedChanged(object sender, EventArgs e)
        {

            if (uruchomione)
            {
                Parametry.ZakresNut.klucz = "wiol";

                if (Parametry.Tryb.tryb == "edytor")
                {
                    Ekran ekran = Dane.AktualnyEkran();
                    Edytor edytor = (Edytor)(ekran.zawartosc);
                    edytor.pokaz_klawiature = edytor_pokaz_klawiature_checkBox.Checked;
                    OdswiezPanel();
                }
                if (!Parametry.OK())
                {
                    OdswiezPanel();
                    return;
                }
                if (Parametry.Tryb.tryb != "edytor") Zeruj();
            }
        }

        private void klawiatura_pokaz_pieciolinie_checkBox_CheckedChanged(object sender, EventArgs e)
        {

            if (uruchomione)
            {
                Parametry.ZakresNut.klucz = "wiol";

                if (Parametry.Tryb.tryb == "edytor")
                {
                    Ekran ekran = Dane.AktualnyEkran();
                    Edytor edytor = (Edytor)(ekran.zawartosc);
                    edytor.pokaz_pieciolinie = edytor_pokaz_pieciolinie_checkBox.Checked;
                    OdswiezPanel();
                }
                if (!Parametry.OK())
                {
                    OdswiezPanel();
                    return;
                }
                if (Parametry.Tryb.tryb != "edytor") Zeruj();
            }
        }

        private void edytor_klawiatura_podpisy_klawiszy_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Ekran ekran = Dane.AktualnyEkran();
                Edytor edytor = (Edytor)(ekran.zawartosc);
                edytor.UstawPodpisyKlawiszy(((System.Collections.Generic.KeyValuePair<string, string>)this.edytor_klawiatura_podpisy_klawiszy_comboBox.SelectedItem).Key.ToString());
                OdswiezPanel();
            }

  
        }

        //---------------------GENEROWANIE NUTY------------------------------------------------------------------------------------------


        private void losowe_nuty_liczba_nut_wiolinowy_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Parametry.LosoweNuty.liczba_nut_wiolinowy = ((System.Collections.Generic.KeyValuePair<int, int>)(parametry_losowe_nuty_liczba_nut_wiolinowy_comboBox.SelectedItem)).Key;
                Dane.PrzejdzDoOstatniegoEkranu();
                Dane.PrzejdzDoNastepnegoEkranu();
                OdswiezPanel();
            }
        }

        private void losowe_nuty_liczba_nut_basowy_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {

            Parametry.LosoweNuty.liczba_nut_basowy = ((System.Collections.Generic.KeyValuePair<int, int>)(parametry_losowe_nuty_liczba_nut_basowy_comboBox.SelectedItem)).Key;
            Dane.PrzejdzDoOstatniegoEkranu();
            Dane.PrzejdzDoNastepnegoEkranu();
            OdswiezPanel();

        }

        private void losowe_nuty_tonacja_combobox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Tonacja tonacja = new Tonacja(parametry_losowe_nuty_tonacja_combobox.SelectedItem.ToString().Trim());
                Parametry.LosoweNuty.tonacja = tonacja;
                Dane.PrzejdzDoOstatniegoEkranu();
                Dane.PrzejdzDoNastepnegoEkranu();
                OdswiezPanel();
            }
        }


        private void losowe_nuty_pokaz_nute_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Parametry.LosoweNuty.pokaz_nute = parametry_losowe_nuty_pokaz_nute_checkBox.Checked;
                OdswiezPanel();
            }
        }

        private void losowe_nuty_pokaz_podpis_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Parametry.LosoweNuty.pokaz_podpis = parametry_losowe_nuty_pokaz_podpis_checkBox.Checked;
                OdswiezPanel();
            }
        }

  

        //---------------------GENEROWANIE INTERWALY------------------------------------------------------------------------------------------

        private void generowanie_intwerwaly_pokaz_pierwsza_nute_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Parametry.LosoweInterwaly.czy_pokazywac_pierwsza_nute = parametry_generowanie_intwerwaly_pokaz_pierwsza_nute_checkBox.Checked;
                OdswiezPanel();
            }
        }

        private void generowanie_intwerwaly_pokaz_druga_nute_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Parametry.LosoweInterwaly.czy_pokazywac_druga_nute = parametry_generowanie_intwerwaly_pokaz_druga_nute_checkBox.Checked;
                OdswiezPanel();
            }
        }

        private void generowanie_intwerwaly_pokaz_podpis_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Parametry.LosoweInterwaly.czy_podpis = parametry_generowanie_intwerwaly_pokaz_podpis_checkBox.Checked;
                OdswiezPanel();
            }
        }


        private void generowanie_interwaly_tonacje_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Tonacja tonacja = new Tonacja(parametry_generowanie_interwaly_tonacje_comboBox.SelectedItem.ToString().Trim());
            Parametry.LosoweInterwaly.tonacja = tonacja;
            Zeruj();
        }


        private void generowanie_intwerwaly_pomijaj_ident_znaki_chrom_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Parametry.LosoweInterwaly.czy_pomijac_identyczne_znaki_chromatyczne = parametry_generowanie_intwerwaly_pomijaj_ident_znaki_chrom_checkBox.Checked;
                Zeruj(true);
            }
        }

        //---------------------PARAMETRY CWICZENIA------------------------------------------------------------------------------------------

        private void kolejne_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            Parametry.ZakresPytan.czy_kolejne = parametry_cwiczenia_kolejne_checkBox.Checked;
            if (!Parametry.OK())
            {
                OdswiezPanel();
                return;
            }
            Zeruj();
        }

        private void nazwy_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Parametry.Cwiczenia.nazwy_nut = ((System.Collections.Generic.KeyValuePair<string, string>)this.parametry_cwiczenia_nazwy_comboBox.SelectedItem).Key.ToString();
                OdswiezPanel();
            }
        }


        //-----------------------------------TONACJE-----------------------------------------
        private void parametry_tonacje_lista_tonacji_zmiana_tonacji()
        {
            Tonacja tonacja_old = Parametry.Tonacje.tonacja;
            Tonacja tonacja_new = new Tonacja(parametry_materialy_tonacje_lista_tonacji_comboBox.SelectedItem.ToString().Trim());
            Parametry.Tonacje.tonacja = tonacja_new;
            if (tonacja_new.tryb != tonacja_old.tryb)
                UstawTonacjeSekcje();
            OdswiezPanel();
        }

        private void parametry_tonacje_lista_tonacji_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            parametry_tonacje_lista_tonacji_zmiana_tonacji();
        }

        private void parametry_tonacje_podpisy_nut_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Parametry.Tonacje.czy_pokazywac_podpisy_nut = parametry_materialy_tonacje_podpisy_nut_checkbox.Checked;
                OdswiezPanel();
            }
        }

        private void parametry_tonacje_tetrachordy_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Parametry.Tonacje.czy_pokazywac_terachordy = parametry_materialy_tonacje_tetrachordy_checkBox.Checked;
                OdswiezPanel();
            }
        }

        private void parametry_tonacje_dziubki_poltonowe_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Parametry.Tonacje.czy_pokazywac_dziubki_poltonowe = parametry_materialy_tonacje_dziubki_poltonowe_checkBox.Checked;
                OdswiezPanel();
            }
        }

        private void parametry_tonacje_tytuly_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Parametry.Tonacje.czy_pokazywac_tytuly = parametry_materialy_tonacje_tytuly_checkBox.Checked;
                OdswiezPanel();
            }
        }

        private void parametry_materialy_tonacje_klucz_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (uruchomione)
            {
                Parametry.Tonacje.klucz = ((System.Collections.Generic.KeyValuePair<string, string>)this.parametry_materialy_tonacje_klucz_comboBox.SelectedItem).Key.ToString();
                OdswiezPanel();
            }
        }



        private void parametry_materialy_tonacje_sekcje_checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Parametry.Tonacje.sekcje = new List<string>();

            for (int i = 0; i < parametry_materialy_tonacje_sekcje_checkedListBox.Items.Count; i++)
            {
                if ((parametry_materialy_tonacje_sekcje_checkedListBox.GetItemCheckState(i) == CheckState.Checked)
                    && (e.Index != i))
                    //Parametry.Tonacje.sekcje.Add(parametry_materialy_tonacje_sekcje_checkedListBox.Items[i].ToString());
                    Parametry.Tonacje.sekcje[parametry_materialy_tonacje_sekcje_checkedListBox.Items[i].ToString()] = true;
                else Parametry.Tonacje.sekcje[parametry_materialy_tonacje_sekcje_checkedListBox.Items[i].ToString()] = false;
            }

            if (e.NewValue == CheckState.Checked)
                Parametry.Tonacje.sekcje[parametry_materialy_tonacje_sekcje_checkedListBox.Items[e.Index].ToString()] = true;
            else Parametry.Tonacje.sekcje[parametry_materialy_tonacje_sekcje_checkedListBox.Items[e.Index].ToString()] = false;

            /*
            //foreach (object itemChecked in parametry_materialy_tonacje_sekcje_checkedListBox.CheckedItems)
            //for (int j = 0; j < Parametry.Tonacje.sekcje.Count(); j++)
            for (int i = 0; i < parametry_materialy_tonacje_sekcje_checkedListBox.Items.Count; i++)
            {
                if (parametry_materialy_tonacje_sekcje_checkedListBox.GetItemCheckState(i) == CheckState.Checked)
                    Parametry.Tonacje.sekcje.Add(parametry_materialy_tonacje_sekcje_checkedListBox.Items[i].ToString());

            }
            */

            OdswiezPanel(); //TODO - ABY UNIKNAc migotania, trzeba zrobić dwa checkboxlist -osobno dla moll i durr, wtedy w ustaw konrolki nie bedzie wywoływana ta akcja.

        }

        private void ekran_tonacja_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {

            Ekran ekran = Dane.AktualnyEkran();
            Edytor edytor = (Edytor)(ekran.zawartosc);
            Tonacja tonacja = new Tonacja(edytor_tonacja_comboBox.SelectedItem.ToString().Trim());
            edytor.UstawTonacja(tonacja);
            OdswiezPanel();

        }


    }
}
