using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;

namespace Fakultet.Servisi.IServis.Pomocni
{
    public class DrzavaServis: BazniServis<Drzava>
    {
        public DrzavaServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
