using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using Fakultet.Servisi.Helperi;
using Microsoft.EntityFrameworkCore;

namespace Fakultet.Servisi.IServis.FakultetskiProcesi
{
    public class IspitServis: BazniServis<Ispit>
    {
        public IspitServis(FakultetAppDbContext dbContext) : base(dbContext)
        {
        }

        public List<Ispit> GetDostupniIspitiZaStudenta(int studentId)
        {
            var student = _dbContext.Studenti
                                    .Include(s => s.GodinaStudija)
                                    .FirstOrDefault(s => s.Id == studentId);

            if (student == null) return new List<Ispit>();

            int studentBrojGodine = student.GodinaStudija.GetBrojGodine();

            var sveGodineZaStudij = _dbContext.GodineStudija
                                              .Where(g => g.StudijId == student.GodinaStudija.StudijId)
                                              .ToList();

            // filter za trenutnu god i sve prethodne
            var dozvoljeniGodinaIds = sveGodineZaStudij
                .Where(g => g.GetBrojGodine() <= studentBrojGodine && g.GetBrojGodine() > 0)
                .Select(g => g.Id)
                .ToList();

            var polozeniPredmetiIds = _dbContext.StudentiPredmeti
                                                .Where(sp => sp.StudentId == studentId && sp.Polozio)
                                                .Select(sp => sp.PredmetId)
                                                .ToList();

            var prijavljeniIspitiIds = _dbContext.StudentiIspiti
                                                 .Where(si => si.StudentId == studentId)
                                                 .Select(si => si.IspitId)
                                                 .ToList();

            DateTime rokZaPrijavu = DateTime.Now.AddHours(24);

            var dostupniIspiti = _dbContext.Ispiti
                .Include(i => i.Predmet)
                .Where(i =>
                    // ne moze prijava manje od 24h pred ispit
                    i.DatumOdrzavanja > rokZaPrijavu &&
                    // nije vec prijavio taj termin
                    !prijavljeniIspitiIds.Contains(i.Id) &&
                    // nije vec polozio taj predmet
                    !polozeniPredmetiIds.Contains(i.PredmetId) &&
                    // predmet je sa trenutne ili prethodne godine
                    dozvoljeniGodinaIds.Contains(i.Predmet.GodinaStudijaId)
                )
                .ToList();

            return dostupniIspiti;
        }
    }
}
