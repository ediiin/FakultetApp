using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakultet.Core.Modeli.Forum
{
    public class Post // obavjestenja na predmetima
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string Sadrzaj { get; set; }
        public DateTime DatumObjave { get; set; }
        public int PredmetId { get; set; } // ako je null onda je globalna obavijest
        public Predmet Predmet { get; set; }
        public int OsobaId { get; set; }
        public Osoba Osoba { get; set; } // ko je objavio post
    }
}
