using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakultet.Core.Modeli
{
    public class Osoba
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public int SpolId { get; set; }
        public Spol Spol { get; set; }
        public int GradId { get; set; }
        public Grad Grad { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string Email { get; set; }
        public string JMBG { get; set; }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public Uloge Uloge { get; set; }
    }
}
