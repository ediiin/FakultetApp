using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Fakultet.Core.Modeli.Forum
{
    public class Materijali
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string LinkDoVidea { get; set; }
        public DateTime DatumPostavljanja { get; set; }
        public int PredmetId { get; set; }
        public Predmet Predmet { get; set; }
        public int OsobaId { get; set; }
        public Osoba Osoba { get; set; } // ko je postavio
    }
}
