using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;

namespace Fakultet.Servisi.IServis.FakultetskiProcesi
{
    public class StudentIspitServis: BazniServis<StudentIspit>
    {
        public StudentIspitServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }

        public override List<StudentIspit> GetAll()
        {
            return _dbSet.Include(si => si.Student).Include(si => si.Ispit).ToList();
        }

        public int BrojIzlazakaNaPredmet(int studentId, int predmetId)
        {
            var brojIzlazaka = _dbContext.StudentiIspiti
                                         .Include(si => si.Ispit)
                                         .Count(si => si.StudentId == studentId &&
                                                      si.Ispit.PredmetId == predmetId);

            return brojIzlazaka;
        }

        public void OdjaviIspit(int studentId, int ispitId)
        {
            var prijava = _dbSet.FirstOrDefault(si => si.StudentId == studentId && si.IspitId == ispitId);

            if (prijava != null)
            {
                _dbSet.Remove(prijava);
                _dbContext.SaveChanges();
            }
        }

        public List<StudentIspit> GetPrijaveByStudent(int studentId)
        {
            return _dbSet
                .Include(si => si.Ispit)
                   .ThenInclude(i => i.Predmet) 
                .Where(si => si.StudentId == studentId)
                .ToList();
        }
    }
}
