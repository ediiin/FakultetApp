namespace Fakultet.Core.Modeli.Forum
{
    public class Materijal
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = null!;
        public string? Opis { get; set; } // npr. "Uraditi zadatke do narednog casa"
        public string? PutanjaFajla { get; set; } // za otvaranje pdf, word...
        public string? WebLink { get; set; } //nullable link do yt videa
        public string TipMaterijala { get; set; } = null!;
        public DateTime DatumPostavljanja { get; set; }
        public int PredmetId { get; set; }
        public Predmet Predmet { get; set; } = null!;
        public int OsobaId { get; set; }
        public Osoba Osoba { get; set; } = null!; // profesor ili Asistent koji je objavio
    }
}
