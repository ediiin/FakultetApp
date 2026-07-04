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
        public string Smjer { get; set; } //(npr.Softverski izenjering)
        public string Zvanje { get; set; } //(npr.Bachelor inzenjer elektrotehnike)
    }
}
