using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS2
{
   public  class ZawartoscEkranu
    {
    }

    public class ZawartoscEkranu_GenerowanieNuty:ZawartoscEkranu
    {
        public List<Nuta> nuty_wiolinowy;
        public List<Nuta> nuty_basowy;
        public Tonacja tonacja;
    }

    public class ZawartoscEkranu_RozpoznawanieNuty : ZawartoscEkranu
    {
        public Nuta nuta;
    }

    public class ZawartoscEkranu_RozpoznawanieKlawisze : ZawartoscEkranu
    {
        public Nuta nuta;
        public Klawisz klawisz_od;
        public Klawisz klawisz_do;
    }

    public class ZawartoscEkranu_RozpoznawanieSymbole: ZawartoscEkranu
    {
        public Symbol symbol;
    }

    public class ZawartoscEkranu_RozpoznawanieInterwaly : ZawartoscEkranu
    {
        public List<Nuta> dwie_nuty;
        public string klucz;
        public Tonacja tonacja;
    }

    public class ZawartoscEkranu_GenerowanieInterwaly : ZawartoscEkranu
    {
        public List<Nuta> nuty;
        public string klucz;
        public Tonacja tonacja;
    }
}
