using Fakultet.Core.Modeli;
using Fakultet.Core.Modeli.Forum;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;

namespace Fakultet.Servisi.IServis.Forum
{
    public class ChatPorukaServis : BazniServis<ChatPoruka>
    {
        // pomocna klasa koja grupira razgovor
        public class RazgovorInfo
        {
            public Osoba Sagovornik { get; set; }
            public ChatPoruka PosljednjaPoruka { get; set; }
            public int BrojNeprocitanih { get; set; }
        }

        public ChatPorukaServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }

        public List<RazgovorInfo> GetNedavniRazgovoriZaOsobu(int osobaId)
        {
            // sve poruke vezane za logovanog profesora
            var svePoruke = _dbContext.Set<ChatPoruka>()
                .Include(p => p.Posiljalac)
                .Include(p => p.Primalac)
                .Where(p => p.PosiljalacId == osobaId || p.PrimalacId == osobaId)
                .ToList();

            // grupisanje pod Id druge osobe
            var razgovori = svePoruke
                .GroupBy(p => p.PosiljalacId == osobaId ? p.PrimalacId : p.PosiljalacId)
                .Select(g => new RazgovorInfo
                {
                    // ako sam ja poslao druga osoba je primalac, a ako sam primio druga osoba je posiljalac
                    Sagovornik = g.First().PosiljalacId == osobaId ? g.First().Primalac : g.First().Posiljalac,

                    PosljednjaPoruka = g.OrderByDescending(p => p.VrijemeSlanja).FirstOrDefault(),

                    BrojNeprocitanih = g.Count(p => p.PrimalacId == osobaId && !p.Procitano)
                })
                .OrderByDescending(r => r.PosljednjaPoruka.VrijemeSlanja)
                .ToList();

            return razgovori;
        }

        public List<ChatPoruka> GetPorukeIzmedju(int korisnik1Id, int korisnik2Id)
        {
            return _dbContext.Set<ChatPoruka>()
                .Where(p => (p.PosiljalacId == korisnik1Id && p.PrimalacId == korisnik2Id) ||
                            (p.PosiljalacId == korisnik2Id && p.PrimalacId == korisnik1Id))
                .OrderBy(p => p.VrijemeSlanja)
                .ToList();
        }

        public void OznaciKaoProcitano(int trenutniKorisnikId, int sagovornikId)
        {
            var neprocitane = _dbContext.Set<ChatPoruka>()
                .Where(p => p.PosiljalacId == sagovornikId && p.PrimalacId == trenutniKorisnikId && !p.Procitano)
                .ToList();

            if (neprocitane.Any())
            {
                foreach (var poruka in neprocitane)
                {
                    poruka.Procitano = true;
                }
                _dbContext.SaveChanges();
            }
        }
    }
}
