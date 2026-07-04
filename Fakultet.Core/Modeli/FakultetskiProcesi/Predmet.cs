using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakultet.Core.Modeli
{
    public class Predmet
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int ECTS { get; set; }
        public int ProfesorId { get; set; }
        public Profesor Profesor { get; set; }
        public int GodinaStudijaId { get; set; }
        public GodinaStudija GodinaStudija { get; set; }
    }
}
