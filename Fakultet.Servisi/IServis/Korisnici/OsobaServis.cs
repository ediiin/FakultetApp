using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;

namespace Fakultet.Servisi.IServis.Korisnici
{
    public class OsobaServis: BazniServis<Osoba>
    {
        public OsobaServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }

        public Osoba? Login(string korisnickoImeIliEmail, string unesenaLozinka)
        {
            var osoba = _dbSet
                .FirstOrDefault(o => o.KorisnickoIme == korisnickoImeIliEmail || o.Email == korisnickoImeIliEmail);

            if (osoba == null)
                return null;

            bool ispravnaLozinka = BCrypt.Net.BCrypt.Verify(unesenaLozinka, osoba.LozinkaHash);

            if (ispravnaLozinka)
            {
                return osoba; //uspjesan login
            }

            return null; //neuspjesna login vraca null umjesto objekta
        }
    }
}

