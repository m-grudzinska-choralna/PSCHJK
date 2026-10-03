using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KS2
{
    public class Element
    {
        public int nr_elementu;
        public string rodzaj_elementu;
        public Object element;
        public Object odpowiedz;
        public int ile_razy_zadawane = 0;
        public int ostatni_raz_wylosowany_w_losowaniu_nr = -99999;


        public Element(string p_rodzaj_elementu, int p_nr_elementu, Object p_element, Object p_odpowiedz)
        {
            rodzaj_elementu = p_rodzaj_elementu;
            nr_elementu = p_nr_elementu;
            element = p_element;
            odpowiedz = p_odpowiedz;

        }
    }

    public class GeneratorElementow
    {

        public List<Element> wszystkie_mozliwe_elementy = null;
        public List<Element> wylosowane_elementy = new List<Element>();
        private Random rand = new Random();
        private string rodzaj_elementow;

        public int nuty_od;
        public int nuty_do;
        public string klucz;

        public void UstawPodstawoweWartosci(string p_rodzaj_elementow)
        {
            wszystkie_mozliwe_elementy = new List<Element>();
            wylosowane_elementy = new List<Element>();
            rodzaj_elementow = p_rodzaj_elementow;
        }

        public GeneratorElementow(Hashtable generatoryInterwalow)
        {
            UstawPodstawoweWartosci("typ_interwalu");
            int nr_elementu = 0;

            int maks_wielkosc = 0;

            foreach (string typ_interwalu in generatoryInterwalow.Keys)
            {
                int wielkosc = ((GeneratorElementow)generatoryInterwalow[typ_interwalu]).wszystkie_mozliwe_elementy.Count();
                maks_wielkosc = Math.Max(maks_wielkosc, wielkosc);
            }

            foreach (string typ_interwalu in generatoryInterwalow.Keys)
            {

                int ile = ((GeneratorElementow)generatoryInterwalow[typ_interwalu]).wszystkie_mozliwe_elementy.Count();

                float wsp = 1;
                if (ile <= maks_wielkosc * 0.4f) wsp = 1.6f;

                for (int i = 0; i < wsp * ile; i++)
                {
                    wszystkie_mozliwe_elementy.Add(new Element(rodzaj_elementow, nr_elementu++, typ_interwalu, typ_interwalu));
                }
            }

            int x = 1;
        }

        public GeneratorElementow(string p_rodzaj_cwiczen, string p_klucz, string p_interwal_kod = null)
        {
            UstawPodstawoweWartosci(p_rodzaj_cwiczen);
            int nr_elementu = 0;

            klucz = p_klucz;
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


            string[] znaki_chromatyczne = new string[20];
            int ile_znakow_chromatycznych = 0;

            foreach (string znak in Parametry.ZnakiChromatyczne.znaki.Keys)
            {
                if ((bool)(Parametry.ZnakiChromatyczne.znaki[znak]))
                    znaki_chromatyczne[ile_znakow_chromatycznych++] = znak;
            }

            Symbol symbol;
            switch (rodzaj_elementow)
            {
                case "symbol":
                    foreach (string wartosc in Parametry.WartosciRytmiczne.getEtykiety())
                    {
                        if ((bool)(Parametry.WartosciRytmiczne.wartosci[wartosc]))
                        {
                            if (wartosc == "cala_nuta")
                            {
                                symbol = new Symbol(wartosc, 3);
                                wszystkie_mozliwe_elementy.Add(new Element(rodzaj_elementow, nr_elementu++, symbol, symbol.nazwa));
                            }
                            else
                            {
                                symbol = new Symbol(wartosc + "_w_gore", 3);
                                wszystkie_mozliwe_elementy.Add(new Element(rodzaj_elementow, nr_elementu++, symbol, symbol.nazwa));
                                symbol = new Symbol(wartosc + "_w_dol", 3);
                                wszystkie_mozliwe_elementy.Add(new Element(rodzaj_elementow, nr_elementu++, symbol, symbol.nazwa));
                            }
                        }
                    }

                    foreach (string pauza in Parametry.Pauzy.getEtykiety())
                    {

                        if ((bool)Parametry.Pauzy.pauzy[pauza])
                        {
                            string kod = "pauza_" + pauza;
                            if (pauza == "calonutowa" || pauza == "polnutowa") kod += "_z_linia";
                            symbol = new Symbol(kod);
                            wszystkie_mozliwe_elementy.Add(new Element(rodzaj_elementow, nr_elementu++, symbol, symbol.nazwa));
                        }
                    }


                    foreach (string znak in Parametry.ZnakiChromatyczne.getEtykiety())
                    {
                        if (znak != "brak" && (bool)Parametry.ZnakiChromatyczne.znaki[znak])
                        {
                            symbol = new Symbol(znak, 3);
                            wszystkie_mozliwe_elementy.Add(new Element(rodzaj_elementow, nr_elementu++, symbol, symbol.nazwa));
                        }
                    }

                    break;

                case "nuta":

                    for (int znak_chrom = 0; znak_chrom < ile_znakow_chromatycznych; znak_chrom++)
                    {
                        for (int nr_nuty = nuty_od; nr_nuty <= nuty_do; nr_nuty++)
                        {
                            Nuta nuta1 = new Nuta(nr_nuty, "cala_nuta", znaki_chromatyczne[znak_chrom]);
                            wszystkie_mozliwe_elementy.Add(new Element(rodzaj_elementow, nr_elementu++, nuta1, nuta1));
                        }
                    }

                    break;

                case "interwal":
                    Interwal interwal = Interwal.getInterwal(p_interwal_kod, p_interwal_kod == "1_0" ? 0 : 1);
                    for (int nr_nuty = nuty_od; nr_nuty <= nuty_do - interwal.liczba_stopni + 1; nr_nuty++)
                    {
                        for (int znak_chrom = 0; znak_chrom < ile_znakow_chromatycznych; znak_chrom++)
                        {
                            Nuta nuta1 = new Nuta(nr_nuty, "cala_nuta", znaki_chromatyczne[znak_chrom]);

                            Nuta nuta2 = nuta1.PrzesunOInterwal(interwal, true);
                            if ((nuta2 != null && (bool)(Parametry.ZnakiChromatyczne.znaki[nuta2.znak_chromatyczny]))
                                && (!Parametry.LosoweInterwaly.czy_pomijac_identyczne_znaki_chromatyczne || nuta1.znak_chromatyczny != nuta2.znak_chromatyczny)
                                )

                            {
                                List<Nuta> dwie_nuty = new List<Nuta>() { nuta1, nuta2 };
                                wszystkie_mozliwe_elementy.Add(new Element(rodzaj_elementow, nr_elementu++, dwie_nuty, interwal));
                            }
                        }
                    }
                    break;
                case "klawisz":
                    List<Nuta> lista_nut = new List<Nuta>();
                    for (int znak_chrom = 0; znak_chrom < ile_znakow_chromatycznych; znak_chrom++)
                    {
                        for (int nr_nuty = nuty_od; nr_nuty <= nuty_do; nr_nuty++)
                        {
                            Nuta nuta1 = new Nuta(nr_nuty, "cala_nuta", znaki_chromatyczne[znak_chrom]);

                            if ((znaki_chromatyczne[znak_chrom] == "brak")
                                || (znaki_chromatyczne[znak_chrom] == "bemol" && nr_nuty > nuty_od)
                                || (znaki_chromatyczne[znak_chrom] == "krzyzyk" && nr_nuty < nuty_do))
                            {
                                //wszystkie_mozliwe_elementy.Add(new Element(rodzaj_elementow, nr_elementu++, nuta1, nuta1));
                                lista_nut.Add(nuta1);
                            }
                        }
                    }
                    //posortowanie
                    int nuta_min_lp;
                    while (lista_nut.Count()>0)
                    {
                        nuta_min_lp = 0;
                        for (int i=1;i< lista_nut.Count();i++)
                        {
                            if ((lista_nut[i].getKlawisz().klawisz_nr< lista_nut[nuta_min_lp].getKlawisz().klawisz_nr)
                               || (lista_nut[i].getKlawisz().klawisz_nr == lista_nut[nuta_min_lp].getKlawisz().klawisz_nr && lista_nut[i].nr< lista_nut[nuta_min_lp].nr))
                            {
                                nuta_min_lp = i;
                            }
                        }
                        wszystkie_mozliwe_elementy.Add(new Element(rodzaj_elementow, nr_elementu++, lista_nut[nuta_min_lp], lista_nut[nuta_min_lp]));
                        lista_nut.RemoveAt(nuta_min_lp);
                    }


                    break;
                default: throw new Exception("Nieobsłużony rodzaj elementów:" + rodzaj_elementow);
            }


        }

        public WartoscRytmiczna LosowaWartoscRytmiczna()
        {
            List<WartoscRytmiczna> wartosci_rytmiczne = new List<WartoscRytmiczna>();
            foreach (string wartosc in Parametry.WartosciRytmiczne.wartosci.Keys)
            {
                if ((bool)(Parametry.WartosciRytmiczne.wartosci[wartosc]))
                    wartosci_rytmiczne.Add((WartoscRytmiczna)(WartoscRytmiczna.wartosci[wartosc]));
            }

            return wartosci_rytmiczne[rand.Next(0, wartosci_rytmiczne.Count())];
        }

        public Element WylosujKolejnyElement(Element p_elementDoZachowaniaOdNiegoOdleglosci = null)
        {
            Element wylosowanyElement = null;

            int min_ile_razy_zadawane = 999999;
            for (int i = 0; i < wszystkie_mozliwe_elementy.Count(); i++)
            {
                if (wszystkie_mozliwe_elementy[i].ile_razy_zadawane < min_ile_razy_zadawane)
                    min_ile_razy_zadawane = wszystkie_mozliwe_elementy[i].ile_razy_zadawane;
            }

            List<int> numery_elementow_do_losowania = new List<int>();

            int nr_poprzednego_elementu = -1;
            if (wylosowane_elementy.Count() > 0) nr_poprzednego_elementu = wylosowane_elementy.Last().nr_elementu;

            for (int i = 0; i < wszystkie_mozliwe_elementy.Count(); i++)
            {
                if (wszystkie_mozliwe_elementy[i].ile_razy_zadawane == min_ile_razy_zadawane
                    && wylosowane_elementy.Count() - wszystkie_mozliwe_elementy[i].ostatni_raz_wylosowany_w_losowaniu_nr >= wszystkie_mozliwe_elementy.Count() / 2
                    && i != nr_poprzednego_elementu
                    )
                    numery_elementow_do_losowania.Add(i);
            }

            List<int> numery_odleglych_elementow = new List<int>();



            if (wylosowane_elementy.Count() > 0 || p_elementDoZachowaniaOdNiegoOdleglosci != null)
            {
                Element elementDoZachowaniaOdNiegoOdleglosci;
                if (p_elementDoZachowaniaOdNiegoOdleglosci is null)
                    elementDoZachowaniaOdNiegoOdleglosci = wylosowane_elementy.Last();
                else elementDoZachowaniaOdNiegoOdleglosci = p_elementDoZachowaniaOdNiegoOdleglosci;

                float maksymalna_odleglosc = 0;
                foreach (int i in numery_elementow_do_losowania)
                {
                    float odl = odleglosc(wszystkie_mozliwe_elementy[i], elementDoZachowaniaOdNiegoOdleglosci);
                    if (odl > maksymalna_odleglosc)
                        maksymalna_odleglosc = odl;
                }

                foreach (int i in numery_elementow_do_losowania)
                {
                    if (odleglosc(wszystkie_mozliwe_elementy[i], elementDoZachowaniaOdNiegoOdleglosci) > maksymalna_odleglosc / 2)
                        numery_odleglych_elementow.Add(i);
                }
            }

            if (numery_odleglych_elementow.Count > 1)
                wylosowanyElement = wszystkie_mozliwe_elementy[numery_odleglych_elementow[rand.Next(0, numery_odleglych_elementow.Count)]];
            else
            {
                if (numery_elementow_do_losowania.Count >= 1)
                    wylosowanyElement = wszystkie_mozliwe_elementy[numery_elementow_do_losowania[rand.Next(0, numery_elementow_do_losowania.Count)]];
                else wylosowanyElement = wszystkie_mozliwe_elementy[rand.Next(0, wszystkie_mozliwe_elementy.Count())];
            }


            wylosowanyElement.ile_razy_zadawane++;
            wylosowanyElement.ostatni_raz_wylosowany_w_losowaniu_nr = wylosowane_elementy.Count() + 1;
            wylosowane_elementy.Add(wylosowanyElement);

            switch (rodzaj_elementow)
            {
                case "nuta":
                    ((Nuta)(wylosowanyElement.element)).wartosc_rytmiczna = LosowaWartoscRytmiczna();
                    break;
                case "interwal":
                    List<Nuta> dwie_nuty = (List<Nuta>)(wylosowanyElement.element);
                    dwie_nuty[0].wartosc_rytmiczna = LosowaWartoscRytmiczna();
                    dwie_nuty[1].wartosc_rytmiczna = LosowaWartoscRytmiczna();
                    if (rand.Next(2) == 1)
                    {

                        dwie_nuty.Add(dwie_nuty[0]);
                        dwie_nuty.RemoveAt(0);
                    }
                    break;

            }







            /* for (int j = 1; j <= Parametry.LosoweNuty.liczba_systemow; j++)
             {
                 WierszNutowy system_nutowy = new WierszNutowy();
                 system_nutowy.Dodaj(new Klucz(Dane.klucz, C_dur));

                 List<int> numery_wylosowanych_nut = new List<int>();
                 Random random = new Random();
                 int t = random.Next(100);
                 for (int i = 1; i <= Parametry.LosoweNuty.liczba_nut_w_systemie; i++)
                 {

                     bool znaleziona = false;
                     Nuta nuta = new Nuta(1, "cala_nuta");
                     int iteracje = 0;
                     while (!znaleziona && iteracje < 1000)
                     {
                         Dane.PrzejdzDoNowegoPytaniaOdpowiedzi(false);
                         nuta = Dane.aktualne_pytanie.nuta1;
                         znaleziona = true;
                         if (numery_wylosowanych_nut.Contains(nuta.nr) && numery_wylosowanych_nut.Count > 0)
                             znaleziona = false;
                         iteracje++;
                     }

                     numery_wylosowanych_nut.Add(nuta.nr);

                     if (Parametry.LosoweNuty.pokaz_podpis)
                         nuta.rodzaj_podpisu = "nazwa_literowa";
                     else nuta.rodzaj_podpisu = "brak";
                     nuta.czy_obiekt_wyswietlany = Parametry.LosoweNuty.pokaz_nute;
                     system_nutowy.Dodaj(nuta);

                     if (i != Parametry.LosoweNuty.liczba_nut_w_systemie)
                         system_nutowy.Dodaj(new KreskaTaktowa("zwykła"));
                     else system_nutowy.Dodaj(new KreskaTaktowa("koniec"));
                 }
                 arkusz.DodajWiersz(system_nutowy);
             }*/



            return wylosowanyElement;
        }

        public float odleglosc(Element pyt1, Element pyt2)
        {
            float odleglosc = 0;
            switch (rodzaj_elementow)
            {
                case "nuta":
                    odleglosc = Math.Abs(((Nuta)pyt2.element).nr - ((Nuta)pyt1.element).nr);
                    break;
                case "symbol":
                    odleglosc = 0;
                    break;
                case "klawisz":
                    odleglosc = Math.Abs(((Nuta)pyt2.element).nr - ((Nuta)pyt1.element).nr);
                    break;
                case "interwal":
                    int a1 = Math.Min(((List<Nuta>)pyt1.element)[0].nr, ((List<Nuta>)pyt1.element)[1].nr);
                    int b1 = Math.Max(((List<Nuta>)pyt1.element)[0].nr, ((List<Nuta>)pyt1.element)[1].nr);
                    int a2 = Math.Min(((List<Nuta>)pyt2.element)[0].nr, ((List<Nuta>)pyt2.element)[1].nr);
                    int b2 = Math.Max(((List<Nuta>)pyt2.element)[0].nr, ((List<Nuta>)pyt2.element)[1].nr);
                    if (a1 == a2 || b1 == b2) odleglosc = 0; else odleglosc = 1;
                    break;
                case "typ_interwalu":
                    odleglosc = 0;
                    break;
                default: throw new Exception("nieprawidlowy rodzaj_elementow:" + rodzaj_elementow);
            }
            return Math.Abs(odleglosc);
        }
    }


    public class Ekran
    {
        public string tryb;
        public string rodzaj_cwiczen;
        public Element element;
        public ZawartoscEkranu zawartosc;
        public Object odpowiedz;
        public Ekran(string p_tryb, string p_rodzaj_cwiczen, Element p_element, Ekran poprzedniEkran)
        {
            tryb = p_tryb;
            rodzaj_cwiczen = p_rodzaj_cwiczen;
            element = p_element;
            GeneratorElementow generatorElementow;
            switch (tryb)
            {
                case "rozpoznawanie":
                    switch (rodzaj_cwiczen)
                    {
                        case "nuty":
                            generatorElementow = (GeneratorElementow)(Dane.generatoryElementow[0]);
                            if (element == null)
                                element = generatorElementow.WylosujKolejnyElement();
                            zawartosc = new ZawartoscEkranu_RozpoznawanieNuty() { nuta = (Nuta)element.element };
                            odpowiedz = element.odpowiedz;
                            break;
                        case "symbole":
                            generatorElementow = (GeneratorElementow)(Dane.generatoryElementow[0]);
                            if (element == null)
                                element = generatorElementow.WylosujKolejnyElement();
                            zawartosc = new ZawartoscEkranu_RozpoznawanieSymbole() { symbol = (Symbol)(element.element) };
                            odpowiedz = element.odpowiedz;
                            break;
                        case "klawisze":
                            generatorElementow = (GeneratorElementow)(Dane.generatoryElementow[0]);
                            if (element == null)
                                element = generatorElementow.WylosujKolejnyElement();
                            Nuta nuta_od = new Nuta(generatorElementow.nuty_od);
                            Nuta nuta_do = new Nuta(generatorElementow.nuty_do);
                            zawartosc = new ZawartoscEkranu_RozpoznawanieKlawisze()
                            {
                                nuta = (Nuta)element.element,
                                klawisz_od = nuta_od.getKlawisz(),
                                klawisz_do = nuta_do.getKlawisz()
                            };
                            odpowiedz = element.odpowiedz;
                            break;
                        case "interwaly_pieciolinia":
                        case "interwaly_klawiatura":
                            if (element == null)
                            {
                                GeneratorElementow generatorTypowInterwalow = (GeneratorElementow)(Dane.generatoryElementow["typy_interwalow"]);
                                string typ_interwalu = (string)(generatorTypowInterwalow.WylosujKolejnyElement().element);

                                Element elementDoZachowaniaOdNiegoOdleglosci = null;
                                if (poprzedniEkran != null)
                                    elementDoZachowaniaOdNiegoOdleglosci = poprzedniEkran.element;
                                element = ((GeneratorElementow)(Dane.generatoryElementow[typ_interwalu])).WylosujKolejnyElement(elementDoZachowaniaOdNiegoOdleglosci);
                            }
                            zawartosc = new ZawartoscEkranu_RozpoznawanieInterwaly()
                            {
                                dwie_nuty = (List<Nuta>)(element.element),
                                klucz = Parametry.ZakresNut.klucz,
                                tonacja = new Tonacja("C-dur")
                            };
                            odpowiedz = element.odpowiedz;
                            break;
                        default: throw new Exception("Nieprawidłowy rodzaj ćwiczeń");
                    };
                    break;
                case "generowanie":
                    switch (p_rodzaj_cwiczen)
                    {
                        case "nuty":
                            List<Nuta> nuty_wiolinowy = new List<Nuta>();
                            List<Nuta> nuty_basowy = new List<Nuta>();

                            for (int i = 0; i < Parametry.LosoweNuty.liczba_nut_wiolinowy; i++)
                                nuty_wiolinowy.Add((Nuta)(((GeneratorElementow)(Dane.generatoryElementow["wiol"])).WylosujKolejnyElement().element));

                            for (int i = 0; i < Parametry.LosoweNuty.liczba_nut_basowy; i++)
                                nuty_basowy.Add((Nuta)(((GeneratorElementow)(Dane.generatoryElementow["bas"])).WylosujKolejnyElement().element));

                            zawartosc = new ZawartoscEkranu_GenerowanieNuty() { nuty_wiolinowy = nuty_wiolinowy, nuty_basowy = nuty_basowy, tonacja = Parametry.LosoweNuty.tonacja };
                            break;
                        case "interwaly":
                            List<string> wartosci_rytmiczne_do_wylosowania = new List<string>();

                            foreach (string wartosc in Parametry.WartosciRytmiczne.getEtykiety())
                            {
                                if ((bool)Parametry.WartosciRytmiczne.wartosci[wartosc])
                                    wartosci_rytmiczne_do_wylosowania.Add(wartosc);
                            }


                            List<Nuta> nuty = new List<Nuta>();
                            Tonacja tonacja = Parametry.LosoweInterwaly.tonacja;
                            string klucz = Parametry.ZakresNut.klucz;

                            List<string> interwaly_do_wylosowania_kolejne = new List<string>();
                            if (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen == "generowanie_interwaly")
                            {
                                foreach (string interwal_kod in Parametry.LiczebnoscInterwalow.ile_interwalow_do_losowania.Keys)
                                {
                                    if ((int)(Parametry.Interwaly.ile_wszystkich_interwalow[interwal_kod]) > 0)
                                    {
                                        int ile = (int)(Parametry.LiczebnoscInterwalow.ile_interwalow_do_losowania[interwal_kod]);
                                        for (int i = 0; i < ile; i++)
                                            interwaly_do_wylosowania_kolejne.Add(interwal_kod);
                                    }

                                }
                            }

                            Random r = new Random();
                            List<string> interwaly_do_wylosowania_losowo = new List<string>();
                            while (interwaly_do_wylosowania_kolejne.Count() > 0)
                            {
                                int los = r.Next(interwaly_do_wylosowania_kolejne.Count());
                                interwaly_do_wylosowania_losowo.Add(interwaly_do_wylosowania_kolejne[los]);
                                interwaly_do_wylosowania_kolejne.RemoveAt(los);
                            }

                            foreach (string interwal_kod in interwaly_do_wylosowania_losowo)
                            {

                                generatorElementow = (GeneratorElementow)(Dane.generatoryElementow[interwal_kod]);
                                List<Nuta> dwie_nuty = (List<Nuta>)(generatorElementow.WylosujKolejnyElement().element);
                                /*
                                List<Nuta> dwie_nuty2 = null;                                
                                if (r.Next(2) == 0)
                                {
                                    dwie_nuty2 = dwie_nuty;
                                }
                                else
                                {
                                    dwie_nuty2 = new List<Nuta>();
                                    dwie_nuty2.Add(dwie_nuty[1]);
                                    dwie_nuty2.Add(dwie_nuty[0]);

                                }
                                dwie_nuty2[0].wartosc_rytmiczna = WartoscRytmiczna.wartosc(wartosci_rytmiczne_do_wylosowania[r.Next(wartosci_rytmiczne_do_wylosowania.Count)]);
                                dwie_nuty2[1].wartosc_rytmiczna = WartoscRytmiczna.wartosc(wartosci_rytmiczne_do_wylosowania[r.Next(wartosci_rytmiczne_do_wylosowania.Count)]);                                
                                nuty.AddRange(dwie_nuty2);
                                */
                                nuty.AddRange(dwie_nuty);
                            }


                            zawartosc = new ZawartoscEkranu_GenerowanieInterwaly() { nuty = nuty, tonacja = tonacja, klucz = klucz };
                            break;
                        default: throw new Exception("nieprawidlowy rodzaj cwiczen");
                    }
                    break;
                case "edytor":
                    Edytor edytor = null;
                    if (poprzedniEkran == null)
                    {
                        edytor = new Edytor(Parametry.Edytor.init_klucz,
                                            Parametry.Edytor.init_tonacja,
                                            (rodzaj_cwiczen == "interwalowy") ? new Metrum("2/1") : Parametry.Edytor.init_metrum,
                                            Parametry.ZakresNut.wiolinowy_od,
                                            Parametry.ZakresNut.wiolinowy_do,
                                            Parametry.ZakresNut.basowy_od,
                                            Parametry.ZakresNut.basowy_do,
                                            Parametry.Edytor.init_pokaz_nute,
                                            Parametry.Edytor.init_pokaz_podpis,
                                            Parametry.Edytor.init_pokaz_pierwsza_nute,
                                            Parametry.Edytor.init_pokaz_druga_nute,
                                            Parametry.Edytor.init_pokaz_klawiature,
                                            Parametry.Edytor.init_pokaz_pieciolinie,
                                            Parametry.Edytor.init_podpisy_klawiszy
                                            );
                    }
                    else
                    {
                        Edytor poprzedni_edytor = (Edytor)(poprzedniEkran.zawartosc);
                        edytor = new Edytor(poprzedni_edytor.aktualnaDefinicjaSystemuNutowego().klucz,
                                            poprzedni_edytor.aktualnaDefinicjaSystemuNutowego().tonacja,
                                            poprzedni_edytor.metrum,
                                            poprzedni_edytor.wiol_od,
                                            poprzedni_edytor.wiol_do,
                                            poprzedni_edytor.bas_od,
                                            poprzedni_edytor.bas_do,
                                            poprzedni_edytor.pokaz_nute,
                                            poprzedni_edytor.pokaz_podpis,
                                            poprzedni_edytor.pokaz_pierwsza_nute,
                                            poprzedni_edytor.pokaz_druga_nute,
                                            poprzedni_edytor.pokaz_klawiature,
                                            poprzedni_edytor.pokaz_pieciolinie,
                                            poprzedni_edytor.podpisy_klawiszy_rodzaj
                                          );
                    }
                    zawartosc = edytor;
                    break;
                default: throw new Exception("Nieznany tryb: "+ tryb);

            }
        }
        private static Arkusz Arkusz_generowanie_nuta()
        {
            Arkusz arkusz = new Arkusz();

            Tonacja C_dur = new Tonacja("C-dur");

            for (int j = 1; j <= Parametry.LosoweNuty.liczba_systemow; j++)
            {
                WierszNutowy system_nutowy = new WierszNutowy();
                system_nutowy.Dodaj(new Klucz(Parametry.ZakresNut.klucz, C_dur));

                for (int i = 1; i <= Parametry.LosoweNuty.liczba_nut_wiolinowy; i++)
                {

                    Nuta nuta = (Nuta)(((GeneratorElementow)(Dane.generatoryElementow[0])).WylosujKolejnyElement().element);

                    if (Parametry.LosoweNuty.pokaz_podpis)
                        nuta.rodzaj_podpisu = "nazwa_literowa";
                    else nuta.rodzaj_podpisu = "brak";
                    nuta.czy_obiekt_wyswietlany = Parametry.LosoweNuty.pokaz_nute;
                    system_nutowy.Dodaj(nuta);

                    if (i != Parametry.LosoweNuty.liczba_nut_wiolinowy)
                        system_nutowy.Dodaj(new KreskaTaktowa("zwykła"));
                    else system_nutowy.Dodaj(new KreskaTaktowa("koniec"));
                }
                arkusz.DodajWiersz(system_nutowy);
            }

            return arkusz;
        }


    }



    public static class Dane
    {
        public static bool czy_odpowiedz = false;
        public static int nr_pokazywanego_ekranu;

        public static List<Ekran> ekrany = new List<Ekran>();

        public static Hashtable generatoryElementow = new Hashtable();

        public static Ekran AktualnyEkran()
        {
            Ekran ekran = null;
            if (ekrany.Count > 0)
                ekran = ekrany[nr_pokazywanego_ekranu];
            return ekran;

        }




        public static void PrzejdzDoPoprzedniegoEkranu()
        {
            if (Parametry.czy_kolejne() || nr_pokazywanego_ekranu > 0)
            {


                if (Parametry.czy_kolejne())
                {
                    bool czy_cofnij = false;
                    if (Parametry.Cwiczenia.odpowiedzi && nr_pokazywanego_ekranu != -1)
                    {
                        if (Dane.czy_odpowiedz) czy_cofnij = true;
                        Dane.czy_odpowiedz = !Dane.czy_odpowiedz;
                    }
                    else czy_cofnij = true;

                    if (czy_cofnij)
                    {
                        if (nr_pokazywanego_ekranu > 0)
                            nr_pokazywanego_ekranu--;
                        else nr_pokazywanego_ekranu = ekrany.Count() - 1;
                    }
                }
                else
                {
                    if (nr_pokazywanego_ekranu >= 1)
                    {
                        nr_pokazywanego_ekranu--;
                        if (Parametry.Cwiczenia.odpowiedzi)
                        {
                            Dane.czy_odpowiedz = true;
                        }
                    }

                }
            }
        }

        public static void PrzejdzDoOstatniegoEkranu()
        {
            if (ekrany.Count > 0)
                nr_pokazywanego_ekranu = ekrany.Count() - 1;
        }
        public static void PrzejdzDoNastepnegoEkranu()
        {

            bool nowe_ekran = false;
            if (ekrany.Count > 0 && Parametry.czy_odpowiedzi() && nr_pokazywanego_ekranu != -1)
            {
                if (Dane.czy_odpowiedz) nowe_ekran = true;
                Dane.czy_odpowiedz = !Dane.czy_odpowiedz;
            }
            else nowe_ekran = true;

            if (nowe_ekran)
            {
                if (Parametry.czy_kolejne())
                {
                    if (nr_pokazywanego_ekranu < ekrany.Count() - 1)
                        nr_pokazywanego_ekranu++;
                    else nr_pokazywanego_ekranu = 0;
                }
                else
                {
                    if (nr_pokazywanego_ekranu < ekrany.Count() - 1)
                    {
                        nr_pokazywanego_ekranu++;
                        if (nr_pokazywanego_ekranu == ekrany.Count() - 1 || !Parametry.Cwiczenia.odpowiedzi)
                            Dane.czy_odpowiedz = false;
                        else Dane.czy_odpowiedz = true;
                    }
                    else
                    {

                        Ekran ekran = null;
                        if (ekrany.Count() > 0) ekran = ekrany.Last();
                        ekrany.Add(new Ekran(Parametry.Tryb.tryb, Parametry.Tryb.rodzaj_cwiczen, null, ekran));
                        nr_pokazywanego_ekranu = ekrany.Count() - 1;
                    }
                }

            }
        }


        public static void ZerujListeEkranow()
        {
            if (Parametry.czy_numerowanie_ekranow())
            {
                if (Parametry.czy_kolejne())
                    nr_pokazywanego_ekranu = 0;
                else
                {
                    ekrany = new List<Ekran>();
                    //PrzejdzDoNastepnegoEkranu();
                    nr_pokazywanego_ekranu = 0;
                }
            }
        }


        public static void ZaladujElementy()
        {
            generatoryElementow = new Hashtable();

            GeneratorElementow generator;
            switch (Parametry.Tryb.tryb + "_" + Parametry.Tryb.rodzaj_cwiczen)
            {
                case ("rozpoznawanie_nuty"):
                    generatoryElementow.Add(0, new GeneratorElementow("nuta", Parametry.ZakresNut.klucz));
                    break;
                case ("rozpoznawanie_symbole"):
                    generatoryElementow.Add(0, new GeneratorElementow("symbol", null, null));
                    break;
                case ("rozpoznawanie_klawisze"):
                    generatoryElementow.Add(0, new GeneratorElementow("klawisz", Parametry.ZakresNut.klucz));
                    break;
                case ("generowanie_nuty"):
                    generatoryElementow.Add("wiol", new GeneratorElementow("nuta", "wiol"));
                    generatoryElementow.Add("bas", new GeneratorElementow("nuta", "bas"));
                    break;
                case ("generowanie_interwaly"):
                    foreach (Interwal interwal in Interwal.interwaly)
                    {

                        generator = new GeneratorElementow("interwal", Parametry.ZakresNut.klucz, interwal.kod);
                        generatoryElementow.Add(interwal.kod, generator);
                        Parametry.Interwaly.ile_wszystkich_interwalow[interwal.kod] = generator.wszystkie_mozliwe_elementy.Count();
                    }
                    break;

                case ("rozpoznawanie_interwaly_pieciolinia"):
                case ("rozpoznawanie_interwaly_klawiatura"):
                    foreach (Interwal interwal in Interwal.interwaly)
                    {
                        int czy_zaznaczony = (int)(Parametry.ZaznaczanieInterwalow.zaznaczenie_interwalow[interwal.kod]);
                        if (czy_zaznaczony == 1)
                        {
                            generator = new GeneratorElementow("interwal", Parametry.ZakresNut.klucz, interwal.kod);
                            generatoryElementow.Add(interwal.kod, generator);
                            Parametry.Interwaly.ile_wszystkich_interwalow[interwal.kod] = generator.wszystkie_mozliwe_elementy.Count();
                        }
                    }
                    generator = new GeneratorElementow(generatoryElementow);
                    generatoryElementow.Add("typy_interwalow", generator);
                    break;
                case ("materialy_tonacje"): break;
                case ("edytor_podstawowy"): break;
                case ("edytor_interwalowy"): break;
                default: throw new Exception("Nieznany tryb+rodzaj");
            }


            //ekrany = new List<Ekran>();

            if (Parametry.czy_kolejne())
               // Parametry.ZakresPytan.czy_kolejne && Parametry.Tryb.tryb == "rozpoznawanie" && (Parametry.Tryb.rodzaj_cwiczen == "nuty" || Parametry.Tryb.rodzaj_cwiczen == "symbole" || Parametry.Tryb.rodzaj_cwiczen == "klawisze")
               
            {
                GeneratorElementow generatorElementow = (GeneratorElementow)generatoryElementow[0];
                foreach (Element element in generatorElementow.wszystkie_mozliwe_elementy)
                {
                    if (Parametry.Tryb.rodzaj_cwiczen == "nuty")
                    {
                        ((Nuta)((element.element))).wartosc_rytmiczna = generatorElementow.LosowaWartoscRytmiczna();
                    }
                    ekrany.Add(new Ekran(Parametry.Tryb.tryb, Parametry.Tryb.rodzaj_cwiczen, element, null));
                }
            }
        }


        /*

        public static class CwiczenieNuty
        {
            public static Nuta nuta = null;
            public static Nuta poprzednia_nuta = null;

            public static void PrzejdzDoNuty(Nuta p_nuta)
            {
                CwiczenieNuty.poprzednia_nuta = CwiczenieNuty.nuta;
                CwiczenieNuty.nuta = p_nuta;
            }

            public static void ZerujNute()
            {
                switch (Parametry.ZakresNut.klucz)
                {
                    case "wiol": Dane.klucz = "wiol"; break;
                    case "bas": Dane.klucz = "bas"; break;
                    case "wiolbas":
                        if (rand.Next(0, 2) == 0) Dane.klucz = "wiol";
                        else Dane.klucz = "bas";
                        break;
                }
                Nuta nuta;

                if (Parametry.ZakresPytan.czy_kolejne)
                {
                    if (Dane.klucz == "wiol")
                        nuta = new Nuta(Parametry.ZakresNut.wiolinowy_od, LosowaWartoscRytmiczna(), LosowyZnakChromatyczny());
                    else nuta = new Nuta(Parametry.ZakresNut.basowy_od, LosowaWartoscRytmiczna(), LosowyZnakChromatyczny());
                }
                else nuta = PodajLosowaNuteInnaNiz(Dane.CwiczenieNuty.nuta);

                PrzejdzDoNuty(nuta);
            }
        }        
        */
        /*

        public static class CwiczenieSymbole
        {
            static public int nr_poprzedniego_symbolu = -1;
            static public int nr_symbolu;
            static public Symbol[] symbole=new Symbol[100];
            static public int liczba_symboli = 0;

            public static void PrzejdzDoSymbolu(int p_nr_symbolu)
            {
                nr_poprzedniego_symbolu = nr_symbolu;
                nr_symbolu = p_nr_symbolu;
            }
            public static void ZerujSymbol()
            {
                nr_symbolu = 0;
            }


            public static void ZaladujListeSymboli()
            {
                liczba_symboli = 0;
                if (Parametry.WartosciRytmiczne.cala_nuta)
                    symbole[liczba_symboli++] = new Symbol("cala_nuta",3);
                if (Parametry.WartosciRytmiczne.polnuta)
                {
                    symbole[liczba_symboli++] = new Symbol("polnuta_w_gore", 3);
                    symbole[liczba_symboli++] = new Symbol("polnuta_w_dol", 3);
                }
                if (Parametry.WartosciRytmiczne.cwiercnuta)
                {
                    symbole[liczba_symboli++] = new Symbol("cwiercnuta_w_gore", 3);
                    symbole[liczba_symboli++] = new Symbol("cwiercnuta_w_dol", 3);
                }
                if (Parametry.WartosciRytmiczne.osemka)
                {
                    symbole[liczba_symboli++] = new Symbol("osemka_w_gore", 3);
                    symbole[liczba_symboli++] = new Symbol("osemka_w_dol", 3);
                }
                if (Parametry.WartosciRytmiczne.szesnastka)
                {
                    symbole[liczba_symboli++] = new Symbol("szesnastka_w_gore", 3);
                    symbole[liczba_symboli++] = new Symbol("szesnastka_w_dol", 3);
                }
                if (Parametry.Pauzy.calonutowa)
                    symbole[liczba_symboli++] = new Symbol("pauza_calonutowa");
                if (Parametry.Pauzy.polnutowa)
                    symbole[liczba_symboli++] = new Symbol("pauza_polnutowa");
                if (Parametry.Pauzy.cwiercnutowa)
                    symbole[liczba_symboli++] = new Symbol("pauza_cwiercnutowa");
                if (Parametry.Pauzy.osemkowa)
                    symbole[liczba_symboli++] = new Symbol("pauza_osemkowa");
                if (Parametry.Pauzy.szesnastkowa)
                    symbole[liczba_symboli++] = new Symbol("pauza_szesnastkowa");

                if (Parametry.ZnakiChromatyczne.bemol)
                    symbole[liczba_symboli++] = new Symbol("bemol", 3);
                if (Parametry.ZnakiChromatyczne.krzyzyk)
                    symbole[liczba_symboli++] = new Symbol("krzyzyk", 3);
                if (Parametry.ZnakiChromatyczne.podwojny_bemol)
                    symbole[liczba_symboli++] = new Symbol("podwojny_bemol", 3);
                if (Parametry.ZnakiChromatyczne.podwojny_krzyzyk)
                    symbole[liczba_symboli++] = new Symbol("podwojny_krzyzyk", 3);
                if (Parametry.ZnakiChromatyczne.kasownik)
                    symbole[liczba_symboli++] = new Symbol("kasownik", 3);

            }
        }

        public static void PrzejdzDoKolejnegoPytaniaOdpowiedzi()
        {
            bool nowe_pytanie = false;
            if (Parametry.Pokazywanie.odpowiedzi)
            {
                if (Dane.czy_odpowiedz) nowe_pytanie = true;
                Dane.czy_odpowiedz = !Dane.czy_odpowiedz;
            }
            else nowe_pytanie = true; ;

            if (nowe_pytanie)
            {
                switch (Parametry.ZakresPytan.rodzaj_cwiczen)
                {
                    case "nuty":
                        if (Parametry.ZakresPytan.czy_kolejne)
                            CwiczenieNuty.PrzejdzDoNuty(PodajKolejnaNuta(CwiczenieNuty.nuta));
                        else CwiczenieNuty.PrzejdzDoNuty(PodajLosowaNuteInnaNiz(CwiczenieNuty.nuta));
                        break;
                    case "symbole":
                        if (Parametry.ZakresPytan.czy_kolejne)
                            CwiczenieSymbole.PrzejdzDoSymbolu(PodajKolejnySymbol(CwiczenieSymbole.nr_symbolu));
                        else CwiczenieSymbole.PrzejdzDoSymbolu(PodajLosowySymbolInnyNiz(CwiczenieSymbole.nr_symbolu));
                        break;

                }
            }
        }



        public static void ZerujPytanie()
        {
            switch (Parametry.ZakresPytan.rodzaj_cwiczen)
            {
                case ("nuty"):
                    CwiczenieNuty.ZerujNute();                    
                    break;
                case ("symbole"):
                    CwiczenieSymbole.ZerujSymbol();
                    break;
            }
            Dane.czy_odpowiedz = false;
        }


        public static void PrzejdzDoPoprzedniegoPytania()
        {
            switch (Parametry.ZakresPytan.rodzaj_cwiczen)
            {
                case ("nuty"):
                    CwiczenieNuty.nuta = CwiczenieNuty.poprzednia_nuta;
                    break;
                case ("symbole"):
                    CwiczenieSymbole.nr_symbolu = CwiczenieSymbole.nr_poprzedniego_symbolu;
                    break;
            }
            if (Parametry.Pokazywanie.odpowiedzi)
            {
                Dane.czy_odpowiedz = true;
            }
        }


        public static Nuta PodajKolejnaNuta(Nuta p_nuta)
        {

            if ((klucz == "wiol" && (p_nuta.nr + kolejne_kierunek < Parametry.ZakresNut.wiolinowy_od || p_nuta.nr + kolejne_kierunek > Parametry.ZakresNut.wiolinowy_do))
                || (klucz == "bas" && (p_nuta.nr + kolejne_kierunek < Parametry.ZakresNut.basowy_od || p_nuta.nr + kolejne_kierunek > Parametry.ZakresNut.basowy_do)))

            {
                kolejne_kierunek = -kolejne_kierunek;
            }

            return new Nuta(p_nuta.nr + kolejne_kierunek, LosowaWartoscRytmiczna(), LosowyZnakChromatyczny());
        }


        public static Nuta PodajLosowaNuteInnaNiz(Nuta p_nuta)
        {
            if (Parametry.ZakresNut.klucz == "wiolbas")
            {
                if (Dane.klucz == "wiol") Dane.klucz = "bas";
                else Dane.klucz = "wiol";
            }
            else Dane.klucz = Parametry.ZakresNut.klucz;

            int nr_nuty_old = -1;
            if (CwiczenieNuty.nuta != null) nr_nuty_old = p_nuta.nr;

            int los_od = 0;
            int los_do = 0;

            if (Dane.klucz == "wiol")
            {
                los_od = Parametry.ZakresNut.wiolinowy_od;
                los_do = Parametry.ZakresNut.wiolinowy_do;
            }
            else
            {
                los_od = Parametry.ZakresNut.basowy_od;
                los_do = Parametry.ZakresNut.basowy_do;
            }

            int nr_nuty_new = 0;
            while ((nr_nuty_new = rand.Next(los_od, los_do + 1)) == nr_nuty_old) ;
            return new Nuta(nr_nuty_new, LosowaWartoscRytmiczna(), LosowyZnakChromatyczny());
        }

        public static int PodajLosowySymbolInnyNiz(int p_numer_symbolu)
        {
            return rand.Next(0, CwiczenieSymbole.liczba_symboli);

        }

        public static int PodajKolejnySymbol(int p_numer_symbolu)
        {
            int wynik;
            if (p_numer_symbolu + 1 >= CwiczenieSymbole.liczba_symboli)
                wynik = 0;
            else wynik = p_numer_symbolu + 1;
            return wynik;
        }



        public static int liczba_nut = 0;
        public static string klucz = null;

        public static bool czy_odpowiedz = false;
        public static int kolejne_kierunek = 1;        



        public static string LosowyZnakChromatyczny()
        {
            string[] znaki_do_losowania = new string[10];
            int ile_do_losowania=0;

            if(Parametry.ZnakiChromatyczne.znaki_chromatyczne_brak) znaki_do_losowania[ile_do_losowania++] = "brak";
            if(Parametry.ZnakiChromatyczne.bemol) znaki_do_losowania[ile_do_losowania++] = "bemol";
            if(Parametry.ZnakiChromatyczne.krzyzyk) znaki_do_losowania[ile_do_losowania++] = "krzyzyk";
            if (Parametry.ZnakiChromatyczne.podwojny_bemol) znaki_do_losowania[ile_do_losowania++] = "podwojny_bemol";
            if (Parametry.ZnakiChromatyczne.podwojny_krzyzyk) znaki_do_losowania[ile_do_losowania++] = "podwojny_krzyzyk";

            return znaki_do_losowania[rand.Next(0, ile_do_losowania)];
        }

*/
    }
}
