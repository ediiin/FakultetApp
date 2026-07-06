using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakultet.Servisi.IServis.Pomocni
{
    public class GradServis: BazniServis<Grad>
    {
        public override List<Grad> GetAll()
        {
            return _dbSet.Include(g => g.Drzava)
                .AsNoTracking()
                .ToList();
        }
    }
}
