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

        public List<Osoba> PretraziOsobe(string pretraga, int trenutniKorisnikId)
        {
            pretraga = pretraga.ToLower().Trim();

            var pretragaDb = _dbSet
                .Where(o => o.Id != trenutniKorisnikId);

            var rezultati = pretragaDb.Where(o =>
                o.Ime.ToLower().Contains(pretraga) ||
                o.Prezime.ToLower().Contains(pretraga) ||
                (o.Ime + " " + o.Prezime).ToLower().Contains(pretraga) ||
                (o is Student && ((Student)o).Indeks.ToLower().Contains(pretraga))
            )
            .Take(15) // limitira na 15 rezultata
            .ToList();

            return rezultati;
        }
    }
}

