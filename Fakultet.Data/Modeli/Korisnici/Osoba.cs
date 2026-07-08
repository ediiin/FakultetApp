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
        public string Ime { get; set; } = null!;
        public string Prezime { get; set; } = null!;
        public int SpolId { get; set; }
        public Spol Spol { get; set; } = null!;
        public int GradId { get; set; }
        public Grad Grad { get; set; } = null!;
        public DateTime DatumRodjenja { get; set; }
        public string Email { get; set; } = null!;
        public string JMBG { get; set; } = null!;
        public string KorisnickoIme { get; set; } = null!;
        public string LozinkaHash { get; set; } = null!;
        public Uloge Uloge { get; set; }
    }
}
