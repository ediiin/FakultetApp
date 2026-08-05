using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakultet.Servisi.IServis.FakultetskiProcesi
{
    public class AsistentPredmetServis: BazniServis<AsistentPredmet>
    {
        public AsistentPredmetServis(FakultetAppDbContext dbContext): base(dbContext)
        {
        }

        public AsistentPredmet GetByPredmetId(int predmetId)
        {
            return _dbContext.AsistentiPredmeti.FirstOrDefault(ap => ap.PredmetId == predmetId);
        }

        public List<Predmet> GetPredmetiByYearAndAsistent(int godinaStudijaId, int asistentId)
        {
            return _dbSet 
                .Where(ap => ap.AsistentId == asistentId && ap.Predmet.GodinaStudijaId == godinaStudijaId)
                .Include(ap => ap.Predmet)
                    .ThenInclude(p => p.Profesor)
                .Include(ap => ap.Predmet)
                    .ThenInclude(p => p.GodinaStudija)
                        .ThenInclude(gs => gs.Studij)
                .Select(ap => ap.Predmet)
                .ToList();
        }

        public void Remove(AsistentPredmet asistentPredmet)
        {
            _dbContext.AsistentiPredmeti.Remove(asistentPredmet);
            _dbContext.SaveChanges();
        }
    }
}
