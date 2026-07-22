using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;

namespace Fakultet.Servisi.IServis.Pomocni
{
    public class StudijServis: BazniServis<Studij>
    {
        public StudijServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
