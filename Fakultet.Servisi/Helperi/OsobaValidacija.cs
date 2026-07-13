using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Fakultet.Servisi.Helperi
{
    public static class OsobaValidacija
    {
        public static Dictionary<string, string> ValidirajSve(string ime, string prezime, string korisnickoIme,
                                                              string email, string jmbg, DateTime? datumRodjenja, string lozinka)
        {
            var greske = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(ime))
                greske.Add("Ime", "Polje 'Ime' obavezno!");

            if (string.IsNullOrWhiteSpace(prezime))
                greske.Add("Prezime", "Polje 'Prezime' obavezno!");

            if (string.IsNullOrWhiteSpace(korisnickoIme))
                greske.Add("KorisnickoIme", "Polje 'Korisničko Ime' obavezno!");

            if (string.IsNullOrWhiteSpace(email))
                greske.Add("Email", "Polje 'Email' obavezno!");
            else if (!Regex.IsMatch(email, @"^[a-z.0-9]{2,}@fit\.ba$"))
                greske.Add("Email", "Polje 'Email' mora biti oblika 'xx@fit.ba'!");

            if (string.IsNullOrWhiteSpace(jmbg))
                greske.Add("JMBG", "Polje 'JMBG' obavezno!");
            else if (jmbg.Length != 13)
                greske.Add("JMBG", "Polje 'JMBG' mora imati tačno 13 znakova!");

            if (datumRodjenja == null)
                greske.Add("DatumRodjenja", "Polje 'Datum Rođenja' obavezno!");
            else if (datumRodjenja > DateTime.UtcNow)
                greske.Add("DatumRodjenja", "Datum rođenja ne može biti u budućnosti!");

            if (string.IsNullOrWhiteSpace(lozinka))
            {
                greske.Add("Lozinka", "Polje 'Lozinka' obavezno!");
            }
            else
            {
                string lozinkaGreska = "";
                if (lozinka.Length < 8) lozinkaGreska += "- Minimalno 8 znakova\n";
                if (!lozinka.Any(char.IsUpper)) lozinkaGreska += "- Obavezno veliko slovo\n";
                if (!lozinka.Any(char.IsLower)) lozinkaGreska += "- Obavezno malo slovo\n";
                if (!lozinka.Any(char.IsDigit)) lozinkaGreska += "- Obavezno broj\n";
                if (!lozinka.Any(c => !char.IsLetterOrDigit(c))) lozinkaGreska += "- Obavezno specijalan znak\n";

                if (!string.IsNullOrEmpty(lozinkaGreska))
                    greske.Add("Lozinka", lozinkaGreska.TrimEnd()); 
            }

            return greske;
        }
    }
}
