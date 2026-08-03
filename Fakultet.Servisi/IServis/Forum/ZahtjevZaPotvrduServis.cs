using Fakultet.Core.Modeli.Forum;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;

namespace Fakultet.Servisi.IServis.Forum
{
    public class ZahtjevZaPotvrduServis: BazniServis<ZahtjevZaPotvrdu>
    {
        public ZahtjevZaPotvrduServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }

        public override List<ZahtjevZaPotvrdu> GetAll()
        {
            return _dbSet.Include(z => z.Student).ToList();
        }
    }
}
