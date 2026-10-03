using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS2
{
    public class Symbol
    {
        public string kod;
        public string nazwa;
        public string nazwa_obrazka;

        public float nr_linii=0;

        public Symbol(string p_kod, float p_nr_linii)
        {
            kod = p_kod;
            nazwa_obrazka = p_kod;
            switch (p_kod)
            {
                case "cala_nuta": nazwa = "cała nuta";break;
                case "polnuta_w_gore": nazwa = "półnuta"; break;
                case "cwiercnuta_w_gore": nazwa = "ćwierćnuta"; break;
                case "osemka_w_gore": nazwa = "ósemka"; break;
                case "szesnastka_w_gore": nazwa = "szesnastka"; break;
                case "polnuta_w_dol": nazwa = "półnuta"; break;
                case "cwiercnuta_w_dol": nazwa = "ćwierćnuta"; break;
                case "osemka_w_dol": nazwa = "ósemka"; break;
                case "szesnastka_w_dol": nazwa = "szesnastka"; break;
                case "bemol": nazwa = "bemol"; break;
                case "krzyzyk": nazwa = "krzyżyk"; break;
                case "kasownik": nazwa = "kasownik"; break;
                case "podwojny_bemol": nazwa = "podwójny bemol"; break;
                case "podwojny_krzyzyk": nazwa = "podwójny krzyżyk"; break;
           
                default: throw new Exception("Nie znany symbol o kodzie "+p_kod);
            }
            nr_linii = p_nr_linii;
        }

        public Symbol(string p_kod)
        {
            kod = p_kod;
            nazwa_obrazka = p_kod;
            switch (p_kod)
            {                      
                case "pauza_calonutowa": nazwa = "pauza całonutowa"; break;
                case "pauza_polnutowa": nazwa = "pauza półnutowa"; break;
                case "pauza_calonutowa_z_linia": nazwa = "pauza całonutowa"; break;
                case "pauza_polnutowa_z_linia": nazwa = "pauza półnutowa"; break;
                case "pauza_cwiercnutowa": nazwa = "pauza ćwierćnutowa"; break;
                case "pauza_osemkowa": nazwa = "pauza ósemkowa"; break;
                case "pauza_szesnastkowa": nazwa = "pauza szesnastkowa"; break;
                case "klucz_g": nazwa = "klucz basowy"; break;
                case "klucz_f": nazwa = "cała wiolinowy"; break;
                case "bemol": nazwa = "bemol"; break;
                case "krzyzyk": nazwa = "krzyżyk"; break;
                default: throw new Exception("Nie znany symbol o kodzie" + p_kod);
            }
            nr_linii = 0;
        }
    }
}

