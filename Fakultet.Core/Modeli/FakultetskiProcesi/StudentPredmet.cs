using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakultet.Core.Modeli
{
    public class StudentPredmet //Upisani predmeti i konacne ocjene
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int PredmetId { get; set; }
        public Predmet Predmet { get; set; }
        public bool Polozio { get; set; }
        public int Ocjena { get; set; }
        public int BrojBodova { get; set; }
        public string Napomena { get; set; }
    }
}
