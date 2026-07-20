using Fakultet.Core.Modeli;
using Fakultet.Servisi.Bazni;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakultet.Servisi.IServis.Korisnici
{
    public class StudentServis: BazniServis<Student>
    {
        public StudentServis(FakultetAppDbContext dbContext): base(dbContext)
        {
        }

        public string GenerisiIndeks()
        {
            string prefiksGodine = DateTime.Now.ToString("yy");
            string pocetak = $"IB{prefiksGodine}";

            var zadnjiStudent = _dbSet
                .Where(s => s.Indeks.StartsWith(pocetak))
                .OrderByDescending(s => s.Indeks)
                .FirstOrDefault();

            if (zadnjiStudent == null)
                return $"{pocetak}0001";

            string zadnjeCifreString = zadnjiStudent.Indeks.Substring(4, 4);

            if (int.TryParse(zadnjeCifreString, out int trenutniBroj))
            {
                int noviBroj = trenutniBroj + 1;
                return $"{pocetak}{noviBroj:D4}"; // :D4 pravi od 31 -> 0031
            }

            return $"{pocetak}0001";
        }

        public List<Student> Filter(string indeksImePrezime)
        {
            if (string.IsNullOrWhiteSpace(indeksImePrezime))
                return GetAll();

            var filterText = indeksImePrezime.ToLower().Trim();

            return _dbSet
                .Include(s => s.Grad)
                    .ThenInclude(g => g.Drzava)
                .Include(s => s.Spol)
                .Include(s => s.GodinaStudija)
                .Where(s => s.Uloge == Uloge.Student
                    && (
                        s.Ime.ToLower().Contains(filterText)
                        || s.Prezime.ToLower().Contains(filterText)
                        || s.Indeks.ToLower().Contains(filterText)
                        ) && s.Aktivan == true
                )
                .ToList();
        }

        public override List<Student> GetAll()
        {
            return _dbSet
                .Include(s => s.Grad)
                    .ThenInclude(g => g.Drzava)
                .Include(s => s.Spol)
                .Include(s => s.GodinaStudija)
                .Where(s => s.Uloge == Uloge.Student && s.Aktivan == true)
                .ToList();
        }

        public void Deaktiviraj(int id)
        {
            var student = GetById(id);

            if (student != null)
            {
                student.Aktivan = false;
                _dbContext.SaveChanges();
            }
        }
    }
}
