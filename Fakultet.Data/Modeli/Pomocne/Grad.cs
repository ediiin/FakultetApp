namespace Fakultet.Core.Modeli
{
    public class Grad
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = null!;
        public int DrzavaId { get; set; }
        public Drzava Drzava { get; set; } = null!;
        public string Kanton { get; set; } = null!; 
    }
}
