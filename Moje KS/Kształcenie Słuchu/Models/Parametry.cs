namespace KsztalcenieSluchu.Models;

public static class Parametry
{
    public static class Ograniczenie
    {
        /*
        public static string edycjaProgramu = "uczniowie";
        */

        public static string edycjaProgramu = "uczniowie";

        public static int max_rok = 2060;
        public static int max_miesiac = 2;

        public static bool CzyProgramWygasl()
        {
            if (edycjaProgramu == "")
                return false;

            var now = System.DateTime.Now;
            if (now.Year > max_rok) return true;
            return now.Year == max_rok && now.Month > max_miesiac;
        }
    }
}
