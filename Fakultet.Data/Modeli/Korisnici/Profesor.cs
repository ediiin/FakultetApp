using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakultet.Core.Modeli
{
    public class Profesor : Osoba
    {
        public decimal Plata { get; set; }
        public Zvanje Zvanje { get; set; }
        public float Ocjena { get; set; }
    }
}
