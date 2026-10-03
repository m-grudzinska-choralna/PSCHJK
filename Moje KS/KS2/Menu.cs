using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS2
{
    public partial class OknoGlowne
    {


        private void rozpoznawanieNutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parametry.Tryb.tryb = "rozpoznawanie";
            Parametry.Tryb.rodzaj_cwiczen = "nuty";
            if (!Parametry.ZnakiChromatyczne.co_najmniej_jeden_wybrany())
                Parametry.ZnakiChromatyczne.znaki["brak"] = true;
            if (!Parametry.WartosciRytmiczne.co_najmniej_jeden_wybrany())
                Parametry.WartosciRytmiczne.wartosci["cala_nuta"] = true;
            Zeruj();
        }

        private void rozpoznawanieSymboliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parametry.Tryb.tryb = "rozpoznawanie";
            Parametry.Tryb.rodzaj_cwiczen = "symbole";
            Zeruj();
            //panel1.AutoScrollMinSize = new Size(0, panel1.Height);
            /*
            ZakresNutDialog zakres_dlg = new ZakresNutDialog("rozpoznawanie", "symbole");

            if (zakres_dlg.ShowDialog(this) == DialogResult.OK)
            {
                Zeruj();
            }
            */
        }


        private void nutyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parametry.Tryb.tryb = "generowanie";
            Parametry.Tryb.rodzaj_cwiczen = "nuty";
            if (!Parametry.ZnakiChromatyczne.co_najmniej_jeden_wybrany())
                Parametry.ZnakiChromatyczne.znaki["brak"] = true;
            if (!Parametry.WartosciRytmiczne.co_najmniej_jeden_wybrany())
                Parametry.WartosciRytmiczne.wartosci["cala_nuta"] = true;
            Zeruj();
        }


        private void tonacjeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parametry.Tryb.tryb = "materialy";
            Parametry.Tryb.rodzaj_cwiczen = "tonacje";
            Zeruj();
        }

   
        private void edytorNutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parametry.Tryb.tryb = "edytor";
            Parametry.Tryb.rodzaj_cwiczen = "podstawowy";

            Zeruj();
        }

        private void edytorInterwałówToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parametry.Tryb.tryb = "edytor";
            Parametry.Tryb.rodzaj_cwiczen = "interwalowy";
            Zeruj();
        }


        private void interwałyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parametry.Tryb.tryb = "generowanie";
            Parametry.Tryb.rodzaj_cwiczen = "interwaly";
            if (!Parametry.ZnakiChromatyczne.co_najmniej_jeden_wybrany())
                Parametry.ZnakiChromatyczne.znaki["brak"] = true;
            if (!Parametry.WartosciRytmiczne.co_najmniej_jeden_wybrany())
                Parametry.WartosciRytmiczne.wartosci["cala_nuta"] = true;
            Zeruj();
        }

        private void rozpoznawanieInterwałówPięcioliniaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parametry.Tryb.tryb = "rozpoznawanie";
            Parametry.Tryb.rodzaj_cwiczen = "interwaly_pieciolinia";
            if (!Parametry.ZnakiChromatyczne.co_najmniej_jeden_wybrany())
                Parametry.ZnakiChromatyczne.znaki["brak"] = true;
            if (!Parametry.WartosciRytmiczne.co_najmniej_jeden_wybrany())
                Parametry.WartosciRytmiczne.wartosci["cala_nuta"] = true;
            Zeruj();
        }

        private void rozpoznawanieInterwałówKlawiaturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parametry.Tryb.tryb = "rozpoznawanie";
            Parametry.Tryb.rodzaj_cwiczen = "interwaly_klawiatura";
            if (!Parametry.ZnakiChromatyczne.co_najmniej_jeden_wybrany())
                Parametry.ZnakiChromatyczne.znaki["brak"] = true;
            if (!Parametry.WartosciRytmiczne.co_najmniej_jeden_wybrany())
                Parametry.WartosciRytmiczne.wartosci["cala_nuta"] = true;
            Zeruj();
        }


        private void rozpoznawanieKlawiszyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parametry.Tryb.tryb = "rozpoznawanie";
            Parametry.Tryb.rodzaj_cwiczen = "klawisze";
            Zeruj();
        }

    }
}
