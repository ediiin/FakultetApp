using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;

namespace Fakultet.Servisi.IServis.Korisnici
{
    public class AsistentServis : BazniServis<Asistent>
    {
        public AsistentServis(FakultetAppDbContext dbContext): base(dbContext)
        {
        }

        public override List<Asistent> GetAll()
        {
            return _dbSet
                .Include(a => a.Grad).ThenInclude(g => g.Drzava)
                .Include(a => a.Spol)
                .Where(a => a.Uloge == Uloge.Asistent && a.Aktivan == true)
                .ToList();
        }

        public List<Asistent> Filter(string pretraga)
        {
            if (string.IsNullOrWhiteSpace(pretraga))
                return GetAll();

            var filterText = pretraga.ToLower().Trim();

            return _dbSet
                .Include(a => a.Grad).ThenInclude(g => g.Drzava)
                .Include(a => a.Spol)
                .Where(a => a.Uloge == Uloge.Asistent
                    && (
                        a.Ime.ToLower().Contains(filterText)
                        || a.Prezime.ToLower().Contains(filterText)
                    ) && a.Aktivan == true
                )
                .ToList();
        }

        public void Deaktiviraj(int id)
        {
            var asistent = GetById(id);

            if (asistent != null)
            {
                asistent.Aktivan = false;
                _dbContext.SaveChanges();
            }
        }
    }
}
