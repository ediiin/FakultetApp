using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;

namespace Fakultet.Servisi.IServis.Pomocni
{
    public class SpolServis: BazniServis<Spol>
    {
        public SpolServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
