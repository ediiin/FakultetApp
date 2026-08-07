using Fakultet.Core.Modeli;
using Fakultet.Core.Modeli.Forum;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;

namespace Fakultet.Servisi.IServis.Forum
{
    public class MaterijalServis: BazniServis<Materijal>
    {
        public MaterijalServis(FakultetAppDbContext dbContext) : base(dbContext)
        { 
        }

        public override List<Materijal> GetAll()
        {
            return _dbSet.Include(d => d.Predmet)
                    .Include(d => d.Osoba)
                    .ToList();
        }

        public List<Materijal> GetByPredmet(int predmetId)
        {
            return _dbSet.Include(d => d.Predmet)
                    .Include(d => d.Osoba)
                    .Where(d => d.PredmetId == predmetId)
                    .ToList();
        }

        public List<Materijal> GetMaterijaliByStudent(int studentId)
        {
            var materijali = _dbContext.Set<Materijal>()
                .Include(m => m.Predmet) 
                .Include(m => m.Osoba)  
                .Where(m => _dbContext.Set<StudentPredmet>()
                                      .Any(sp => sp.StudentId == studentId && sp.PredmetId == m.PredmetId))
                .ToList();

            return materijali;
        }
    }
}
