using Fakultet.Core.Modeli.Forum;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakultet.Servisi.IServis.Forum
{
    public class ZahtjevZaPotvrduServis: BazniServis<ZahtjevZaPotvrdu>
    {
        public ZahtjevZaPotvrduServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }

        public override List<ZahtjevZaPotvrdu> GetAll()
        {
            return _dbSet.Include(z => z.Student).ToList();
        }
    }
}
