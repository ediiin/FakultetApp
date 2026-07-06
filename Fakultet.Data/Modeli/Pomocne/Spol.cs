using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakultet.Core.Modeli
{
    public class Spol
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = null!;
        public char Oznaka { get; set; } //M, Z, *
    }
}
