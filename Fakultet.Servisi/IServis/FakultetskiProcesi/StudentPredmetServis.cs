using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakultet.Servisi.IServis.FakultetskiProcesi
{
    public class StudentPredmetServis : BazniServis<StudentPredmet>
    {
        public StudentPredmetServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }

        public List<StudentPredmet> GetStudentiByPredmet(int predmetId, string filterTekst = "")
        {
            var query = _dbSet
                .Include(sp => sp.Student)
                .Where(sp => sp.PredmetId == predmetId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterTekst))
            {
                filterTekst = filterTekst.ToLower().Trim();
                query = query.Where(sp =>
                    sp.Student.Ime.ToLower().Contains(filterTekst) ||
                    sp.Student.Prezime.ToLower().Contains(filterTekst) ||
                    sp.Student.Indeks.ToLower().Contains(filterTekst));
            }

            return query.ToList();
        }
    }
}
