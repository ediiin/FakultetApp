using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakultet.Servisi.IServis.Korisnici
{
    public class AsistentServis : BazniServis<Asistent>
    {
        public AsistentServis(FakultetAppDbContext dbContext): base(dbContext)
        {
        }
    }
}
