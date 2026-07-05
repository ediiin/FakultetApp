using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakultet.Core.Modeli
{
    public class StudentIspit   //Prijave ispita i rezultati
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int IspitId { get; set; }
        public Ispit Ispit { get; set; }
        public int BrojIzlazaka { get; set; }
        public bool Komisijski { get; set; }
        public bool Dodatni { get; set; }
        public decimal Cijena { get; set; }
        public int? Ocjena { get; set; }
        public bool Polozio { get; set; }
        public DateTime DatumPrijave { get; set; }
    }
}
