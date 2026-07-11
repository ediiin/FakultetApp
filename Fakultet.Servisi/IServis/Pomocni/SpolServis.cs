using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakultet.Servisi.IServis.Pomocni
{
    public class SpolServis: BazniServis<Spol>
    {
        public SpolServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
