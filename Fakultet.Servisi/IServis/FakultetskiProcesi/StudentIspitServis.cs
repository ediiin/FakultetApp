using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakultet.Servisi.IServis.FakultetskiProcesi
{
    public class StudentIspitServis: BazniServis<StudentIspit>
    {
        public StudentIspitServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }

        public int BrojIzlazakaNaPredmet(int studentId, int predmetId)
        {
            var brojIzlazaka = _dbContext.StudentiIspiti
                                         .Include(si => si.Ispit)
                                         .Count(si => si.StudentId == studentId &&
                                                      si.Ispit.PredmetId == predmetId);

            return brojIzlazaka;
        }
    }
}
