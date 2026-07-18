using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakultet.Servisi.IServis.FakultetskiProcesi
{
    public class PredmetServis: BazniServis<Predmet>
    {
        public PredmetServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }   

        public override List<Predmet> GetAll()
        {
            return _dbSet
                .Include(p => p.Profesor)
                .Include(p => p.GodinaStudija)
                    .ThenInclude(gs => gs.Studij)
                .ToList();
        }

        public List<Predmet> Filter(string nazivIliProfesor)
        {
            if (string.IsNullOrWhiteSpace(nazivIliProfesor))
                return GetAll();

            var filterText = nazivIliProfesor.ToLower().Trim();

            return _dbSet
                .Include(p => p.Profesor)
                .Include(p => p.GodinaStudija)
                .Where(p => p.Naziv.ToLower().Contains(filterText)
                       || p.Profesor.Ime.ToLower().Contains(filterText)
                       || p.Profesor.Prezime.ToLower().Contains(filterText)) 
                .ToList();
        }
    }
}
