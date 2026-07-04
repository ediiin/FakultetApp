using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakultet.Core.Modeli.Forum
{
    public class ChatPoruka
    {
        public int Id { get; set; }
        public int PosiljalacId { get; set; }
        public Osoba Posiljalac { get; set; }
        public int PrimalacId { get; set; }
        public Osoba Primalac { get; set; }
        public string Sadrzaj { get; set; }
        public DateTime VrijemeSlanja { get; set; }
        public bool Procitano { get; set; }
    }
}
