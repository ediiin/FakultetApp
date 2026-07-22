namespace Fakultet.Core.Modeli.Forum
{
    public class ZahtjevZaPotvrdu
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public StanjePotvrde StanjePotvrde { get; set; }
        public SvrhaPotvrde SvrhaPotvrde { get; set; }
        public DateTime DatumPodnosenja { get; set; }
    }
}
