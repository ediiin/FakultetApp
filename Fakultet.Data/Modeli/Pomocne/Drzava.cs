using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakultet.Core.Modeli
{
    public class Drzava
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = null!;
        public string Regija { get; set; } = null!;
        public string Oznaka { get; set; } = null!;
    }
}
