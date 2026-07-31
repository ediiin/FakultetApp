using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakultet.Servisi.IServis.FakultetskiProcesi
{
    public class IspitServis: BazniServis<Ispit>
    {
        public IspitServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
