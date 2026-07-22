namespace Fakultet.Core.Modeli
{
    public class GodinaStudija
    {
        public int Id { get; set; }
        public string Opis { get; set; } = null!; //(npr. "Prva godina - Prvi put upisan")
        public int StudijId { get; set; }
        public Studij Studij { get; set; } = null!;
    }
}
