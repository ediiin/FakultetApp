namespace Fakultet.Core.Modeli.Forum
{
    public class Materijal
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = null!;
        public string LinkDoVidea { get; set; } = null!;
        public DateTime DatumPostavljanja { get; set; }
        public int PredmetId { get; set; }
        public Predmet Predmet { get; set; } = null!;
        public int OsobaId { get; set; }
        public Osoba Osoba { get; set; } = null!;// ko je postavio
    }
}
