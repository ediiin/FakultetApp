namespace Fakultet.Core.Modeli.Forum
{
    public class Post // obavjestenja na predmetima
    {
        public int Id { get; set; }
        public string Naslov { get; set; } = null!;
        public string Sadrzaj { get; set; } = null!;
        public DateTime DatumObjave { get; set; }
        public int? PredmetId { get; set; } // ako je null onda je globalna obavijest
        public Predmet Predmet { get; set; } = null!;
        public int OsobaId { get; set; }
        public Osoba Osoba { get; set; } = null!; // ko je objavio post
    }
}
