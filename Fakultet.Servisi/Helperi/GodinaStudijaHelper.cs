using Fakultet.Core.Modeli;

namespace Fakultet.Servisi.Helperi
{
    public static class GodinaStudijaHelper
    {
        public static int OdrediBrojGodine(string? opis)
        {
            if (string.IsNullOrWhiteSpace(opis)) return 0;

            string tekst = opis.ToLower();

            if (tekst.Contains("prv") || tekst.Contains("1")) return 1;
            if (tekst.Contains("drug") || tekst.Contains("2")) return 2;
            if (tekst.Contains("tre") || tekst.Contains("3")) return 3;
            if (tekst.Contains("cetv") || tekst.Contains("četv") || tekst.Contains("4")) return 4;
            if (tekst.Contains("pet") || tekst.Contains("5")) return 5;
            if (tekst.Contains("sest") || tekst.Contains("šest") || tekst.Contains("6")) return 6;

            return 0;
        }

        public static int GetBrojGodine(this GodinaStudija? godina)
        {
            if (godina == null) return 0;
            return OdrediBrojGodine(godina.Opis);
        }
    }
}
