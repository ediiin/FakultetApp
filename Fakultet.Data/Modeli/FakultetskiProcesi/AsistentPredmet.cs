using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

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
