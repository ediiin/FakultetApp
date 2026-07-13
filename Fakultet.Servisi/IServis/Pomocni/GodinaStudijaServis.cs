using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Fakultet.Servisi.IServis.Pomocni
{
    public class GodinaStudijaServis: BazniServis<GodinaStudija>
    {
        public GodinaStudijaServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }

        public override List<GodinaStudija> GetAll()
        {
            return _dbSet.Include(gs => gs.Studij).AsNoTracking().ToList();
        }

        public List<GodinaStudija> GetAllByStudijId(int id)
        {
            return _dbSet.Include(gs => gs.Studij).AsNoTracking()
                .Where(g => g.StudijId == id)
                .ToList();
        }
    }
}
