using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakultet.Core.Modeli
{
    public class Student : Osoba
    {
        public string Indeks { get; set; }
        public DateTime DatumUpisa { get; set; }
        public int GodinaStudijaId { get; set; }
        public GodinaStudija GodinaStudija { get; set; }
        public bool ZavrsioFakultet { get; set; }
        public Status Status { get; set; }
        public override string ToString()
        {
            return $"({Indeks}) - {Ime} {Prezime}";
        }
    }
}
