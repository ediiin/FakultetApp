using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakultet.Core.Modeli
{
    public class Studij
    {
        public int Id { get; set; }
        public string Smjer { get; set; } = null!; //(npr.Softverski izenjering)
        public string Zvanje { get; set; } = null!; //(npr.Bachelor inzenjer elektrotehnike)
        public string PuniNaziv => $"{Smjer} ({Zvanje})";
    }
}
