namespace Fakultet.Core.Modeli
{
    public class Spol
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = null!;
        public char Oznaka { get; set; } //M, Z, *
    }
}
