namespace Fakultet.Core.Modeli
{
    public class AsistentPredmet
    {
        public int AsistentId { get; set; }
        public Asistent Asistent { get; set; } = null!;
        public int PredmetId { get; set; }
        public Predmet Predmet { get; set; } = null!;
    }
}
