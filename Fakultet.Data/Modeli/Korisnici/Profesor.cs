namespace Fakultet.Core.Modeli
{
    public class Profesor : Osoba
    {
        public decimal Plata { get; set; }
        public Zvanje Zvanje { get; set; }
        public float Ocjena { get; set; }
        public string ImePrezime => $"{Ime} {Prezime}";
        public override string ToString()
        {
            return ImePrezime;
        }
        public string ZvanjeOpis => Zvanje.ToFriendlyString();
        public string ZvanjeImePrezime => $"({ZvanjeOpis}) {Ime} {Prezime}";
    }
}
