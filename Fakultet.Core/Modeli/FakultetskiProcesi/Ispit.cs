using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakultet.Core.Modeli
{
    public class Ispit
    {
        public int Id { get; set; }
        public int PredmetId { get; set; }
        public Predmet Predmet { get; set; }
        public DateTime DatumOdrzavanja { get; set; }
        public int BrojZadataka { get; set; }
        public int MaxBrojBodova { get; set; }
    }
}
