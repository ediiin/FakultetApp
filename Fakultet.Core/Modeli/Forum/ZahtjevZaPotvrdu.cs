using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakultet.Core.Modeli.Forum
{
    public class ZahtjevZaPotvrdu
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public StanjePotvrde StanjePotvrde { get; set; }
        public SvrhaPotvrde SvrhaPotvrde { get; set; }
        public DateTime DatumPodnosenja { get; set; }
    }
}
