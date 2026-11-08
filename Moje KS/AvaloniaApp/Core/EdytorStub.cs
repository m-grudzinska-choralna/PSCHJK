using System;
using System.Collections.Generic;

namespace KS2
{
    // Minimal Edytor stub to satisfy references from linked Core sources during migration.
    // This is a lightweight placeholder; the full Edytor with WinForms dependencies remains in the original WinForms project.
    public class Edytor : ZawartoscEkranu
    {
        public class SystemNutowyDefinicjaStub
        {
            public string klucz;
            public Tonacja tonacja;
        }

        private SystemNutowyDefinicjaStub def = new SystemNutowyDefinicjaStub();

        public Metrum metrum;
        public int wiol_od;
        public int wiol_do;
        public int bas_od;
        public int bas_do;

        public bool pokaz_nute;
        public bool pokaz_podpis;
        public bool pokaz_pierwsza_nute;
        public bool pokaz_druga_nute;
        public bool pokaz_klawiature;
        public bool pokaz_pieciolinie;
        public string podpisy_klawiszy_rodzaj;

        public Edytor(string p_klucz, Tonacja p_tonacja, Metrum p_metrum,
                      int p_wiol_od, int p_wiol_do, int p_bas_od, int p_bas_do,
                      bool p_pokaz_nute, bool p_pokaz_podpis, bool p_pokaz_pierwsza_nute, bool p_pokaz_druga_nute,
                      bool p_pokaz_klawiature, bool p_pokaz_pieciolinie, string p_podpisy_klawiszy)
        {
            def.klucz = p_klucz;
            def.tonacja = p_tonacja;
            metrum = p_metrum;
            wiol_od = p_wiol_od;
            wiol_do = p_wiol_do;
            bas_od = p_bas_od;
            bas_do = p_bas_do;
            pokaz_nute = p_pokaz_nute;
            pokaz_podpis = p_pokaz_podpis;
            pokaz_pierwsza_nute = p_pokaz_pierwsza_nute;
            pokaz_druga_nute = p_pokaz_druga_nute;
            pokaz_klawiature = p_pokaz_klawiature;
            pokaz_pieciolinie = p_pokaz_pieciolinie;
            podpisy_klawiszy_rodzaj = p_podpisy_klawiszy;
        }

        public SystemNutowyDefinicjaStub aktualnaDefinicjaSystemuNutowego()
        {
            return def;
        }
    }
}
