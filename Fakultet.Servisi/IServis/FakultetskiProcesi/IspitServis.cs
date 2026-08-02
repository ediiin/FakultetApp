using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;
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

        public List<Ispit> GetDostupniIspitiZaStudenta(int studentId)
        {
            var prijavljeniIspitiIds = _dbContext.StudentiIspiti
                                                 .Where(si => si.StudentId == studentId)
                                                 .Select(si => si.IspitId)
                                                 .ToList();

            var dostupniIspiti = _dbContext.Ispiti
                                           .Include(i => i.Predmet) 
                                           .Where(i => i.DatumOdrzavanja > DateTime.Now.AddDays(1) &&
                                                       !prijavljeniIspitiIds.Contains(i.Id))
                                           .ToList();

            return dostupniIspiti;
        }
    }
}
