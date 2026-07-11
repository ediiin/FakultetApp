using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakultet.Servisi.IServis.Pomocni
{
    public class StudijServis: BazniServis<Studij>
    {
        public StudijServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
