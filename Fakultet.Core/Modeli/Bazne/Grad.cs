namespace Fakultet.Core.Modeli
{
    public class Grad
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int DrzavaId { get; set; }
        public Drzava Drzava { get; set; }
        public string Kanton { get; set; } 
    }
}
