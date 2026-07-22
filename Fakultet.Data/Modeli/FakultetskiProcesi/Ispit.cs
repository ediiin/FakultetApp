namespace Fakultet.Core.Modeli
{
    public class Ispit
    {
        public int Id { get; set; }
        public int PredmetId { get; set; }
        public Predmet Predmet { get; set; } = null!;
        public DateTime DatumOdrzavanja { get; set; }
        public int BrojZadataka { get; set; }
        public int MaxBrojBodova { get; set; }
    }
}
