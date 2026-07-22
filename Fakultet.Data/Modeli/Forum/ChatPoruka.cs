namespace Fakultet.Core.Modeli.Forum
{
    public class ChatPoruka
    {
        public int Id { get; set; }
        public int PosiljalacId { get; set; }
        public Osoba Posiljalac { get; set; } = null!;
        public int PrimalacId { get; set; }
        public Osoba Primalac { get; set; } = null!;
        public string Sadrzaj { get; set; } = null!;
        public DateTime VrijemeSlanja { get; set; }
        public bool Procitano { get; set; }
    }
}
