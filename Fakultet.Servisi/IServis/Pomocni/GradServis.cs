using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;

namespace Fakultet.Servisi.IServis.Pomocni
{
    public class GradServis: BazniServis<Grad>
    {
        public GradServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }
        public override List<Grad> GetAll()
        {
            return _dbSet.Include(g => g.Drzava)
                .AsNoTracking()
                .ToList();
        }
    }
}
