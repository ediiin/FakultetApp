using Fakultet.Core.Modeli.Forum;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;

namespace Fakultet.Servisi.IServis.Forum
{
    public class PostServis : BazniServis<Post>
    {
        public PostServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }

        public override List<Post> GetAll()
        {
            return _dbSet
                .Include(p => p.Predmet)
                .Include(p => p.Osoba)
                    .ThenInclude(o => o.Grad)
                .ToList();
        }
    }
}
