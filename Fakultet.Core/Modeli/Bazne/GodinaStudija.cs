using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakultet.Core.Modeli
{
    public class GodinaStudija
    {
        public int Id { get; set; }
        public string Opis { get; set; } //(npr. "Prva godina - Prvi put upisan")
        public int StudijId { get; set; }
        public Studij Studij { get; set; }
    }
}
