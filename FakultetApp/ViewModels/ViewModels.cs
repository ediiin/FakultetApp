using Fakultet.Core.Modeli.Forum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace FakultetApp.ViewModels
{
    // lista razgovora za pretragu
    public class RazgovorPreviewViewModel
    {
        public int SagovornikId { get; set; }
        public string ImePrezime { get; set; } = string.Empty;
        public string UlogaIliIndeks { get; set; } = string.Empty; 
        public string Inicijali => GetInicijale(ImePrezime);
        public string PosljednjaPoruka { get; set; } = string.Empty;
        public DateTime VrijemePosljednjePoruke { get; set; }
        public int BrojNeprocitanih { get; set; }

        private static string GetInicijale(string imePrezime)
        {
            if (string.IsNullOrWhiteSpace(imePrezime)) return "?";
            var dijelovi = imePrezime.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (dijelovi.Length >= 2)
                return $"{dijelovi[0][0]}{dijelovi[1][0]}".ToUpper();
            return imePrezime[0].ToString().ToUpper();
        }
    }

    // model za otvoren chat
    public class PorukaPrikazViewModel
    {
        public int Id { get; set; }
        public int PosiljalacId { get; set; }
        public string Sadrzaj { get; set; } = string.Empty;
        public DateTime VrijemeSlanja { get; set; }
        public bool Procitano { get; set; }
        public bool IsMojaPoruka { get; set; } // ako jest ide lijevo ako ne ide desno
    }

    // model za UI zahtjeva
    public class ZahtjevPrikazVM
    {
        public int Id { get; set; }
        public string SvrhaText { get; set; }
        public string Napomena { get; set; }
        public string DatumPodnosenjaText { get; set; }
        public string StatusText { get; set; }
        public string StatusBojaPozadina { get; set; }
        public string StatusBojaTekst { get; set; }
        public Visibility PrikaziDugmePonisti { get; set; }

        public ZahtjevPrikazVM(ZahtjevZaPotvrdu z)
        {
            Id = z.Id;
            SvrhaText = z.SvrhaPotvrde.ToString();
            Napomena = string.IsNullOrEmpty(z.Napomena) ? "Bez napomene" : z.Napomena;
            DatumPodnosenjaText = z.DatumPodnosenja.ToString("dd.MM.yyyy HH:mm");
            StatusText = z.StanjePotvrde.ToString();

            switch (z.StanjePotvrde)
            {
                case StanjePotvrde.NaCekanju:
                    StatusBojaPozadina = "#FFF3CD";
                    StatusBojaTekst = "#856404";
                    PrikaziDugmePonisti = Visibility.Visible;
                    break;
                case StanjePotvrde.Odobrena:
                    StatusBojaPozadina = "#D4EDDA";
                    StatusBojaTekst = "#155724";
                    PrikaziDugmePonisti = Visibility.Collapsed;
                    break;
                case StanjePotvrde.Odbijena:
                    StatusBojaPozadina = "#F8D7DA";
                    StatusBojaTekst = "#721C24";
                    PrikaziDugmePonisti = Visibility.Collapsed;
                    break;
                case StanjePotvrde.Ponistena:
                    StatusBojaPozadina = "#E2E3E5";
                    StatusBojaTekst = "#383D41";
                    PrikaziDugmePonisti = Visibility.Collapsed;
                    break;
            }
        }
    }
}
