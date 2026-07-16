using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakultet.Servisi.IServis.Korisnici
{
    public class ProfesorServis: BazniServis<Profesor>
    {
        ProfesorServis(FakultetAppDbContext dbContext): base(dbContext)
        { 
        }

        public List<Profesor> Filter(string imePrezime)
        {
            if (string.IsNullOrWhiteSpace(imePrezime))
                return GetAll();

            var filterText = imePrezime.ToLower().Trim();
            return _dbSet
                .Include(p => p.Spol)
                .Include(p => p.Grad)
                    .ThenInclude(g => g.Drzava)
                .Where(p => p.Uloge == Uloge.Profesor
                    && (
                        p.Ime.ToLower().Contains(filterText)
                        || p.Prezime.ToLower().Contains(filterText)
                    )
                )
                .ToList();
        }

        public override List<Profesor> GetAll()
        {
            return _dbSet
                .Include(p => p.Spol)
                .Include(p => p.Grad)
                    .ThenInclude(g => g.Drzava)
                .Where(p => p.Uloge == Uloge.Profesor)
                .ToList();
        }
    }
}
