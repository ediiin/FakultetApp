using Fakultet.Core.Modeli;
using Fakultet.Core.Modeli.Forum;
using System.Windows;
using System.Windows.Media;

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
        private readonly ZahtjevZaPotvrdu _zahtjev;

        public ZahtjevPrikazVM(ZahtjevZaPotvrdu zahtjev)
        {
            _zahtjev = zahtjev;
        }

        public int Id => _zahtjev.Id;
        public string Napomena => string.IsNullOrWhiteSpace(_zahtjev.Napomena) ? "Nema napomene" : _zahtjev.Napomena;
        public string DatumPodnosenjaText => _zahtjev.DatumPodnosenja.ToString("dd.MM.yyyy HH:mm");

        public Visibility PrikaziDugmePonisti =>
            _zahtjev.StanjePotvrde == StanjePotvrde.NaCekanju ? Visibility.Visible : Visibility.Collapsed;

        public string StatusText => _zahtjev.StanjePotvrde switch
        {
            StanjePotvrde.NaCekanju => "Na čekanju",
            StanjePotvrde.Odobrena => "Odobrena",
            StanjePotvrde.Odbijena => "Odbijena",
            StanjePotvrde.Ponistena => "Poništena",
            _ => "Nepoznato"
        };

        public string SvrhaText => _zahtjev.SvrhaPotvrde switch
        {
            SvrhaPotvrde.Stipendija => "Stipendija",
            SvrhaPotvrde.Alimentacija => "Alimentacija",
            SvrhaPotvrde.Penzija => "Penzija",
            SvrhaPotvrde.SmjestajUDom => "Smještaj u studentski dom",
            SvrhaPotvrde.Viza => "Viza",
            SvrhaPotvrde.Ostalo => "Ostalo",
            _ => "Ostalo"
        };

        public Brush StatusBojaPozadina => _zahtjev.StanjePotvrde switch
        {
            StanjePotvrde.NaCekanju => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF3CD")), // zuckasta
            StanjePotvrde.Odobrena => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D4EDDA")), // zelenkasta
            StanjePotvrde.Odbijena => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F8D7DA")), // crvenkasta
            StanjePotvrde.Ponistena => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E3E5")), // sivkasta
            _ => Brushes.Transparent
        };

        public Brush StatusBojaTekst => _zahtjev.StanjePotvrde switch
        {
            StanjePotvrde.NaCekanju => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#856404")), // tamno zuta
            StanjePotvrde.Odobrena => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#155724")), // tamno zelena
            StanjePotvrde.Odbijena => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#721C24")), // tamno crvena
            StanjePotvrde.Ponistena => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#383D41")), // tamno siva
            _ => Brushes.Black
        };
    }

    public class AdminZahtjevVM
    {
        private readonly ZahtjevZaPotvrdu _zahtjev;

        public AdminZahtjevVM(ZahtjevZaPotvrdu zahtjev)
        {
            _zahtjev = zahtjev;
        }

        public int Id => _zahtjev.Id;

        public string StudentInfo => _zahtjev.Student != null ?
            $"{_zahtjev.Student.Ime} {_zahtjev.Student.Prezime} ({_zahtjev.Student.Indeks})" : "Nepoznat student";

        public string Napomena => string.IsNullOrWhiteSpace(_zahtjev.Napomena) ? "-" : _zahtjev.Napomena;
        public string DatumPodnosenjaText => _zahtjev.DatumPodnosenja.ToString("dd.MM.yyyy HH:mm");

        public Visibility PrikaziAkcije =>
            _zahtjev.StanjePotvrde == StanjePotvrde.NaCekanju ? Visibility.Visible : Visibility.Collapsed;

        public string StatusText => _zahtjev.StanjePotvrde switch
        {
            StanjePotvrde.NaCekanju => "Na čekanju",
            StanjePotvrde.Odobrena => "Odobrena",
            StanjePotvrde.Odbijena => "Odbijena",
            StanjePotvrde.Ponistena => "Poništena",
            _ => "Nepoznato"
        };

        public string SvrhaText => _zahtjev.SvrhaPotvrde switch
        {
            SvrhaPotvrde.Stipendija => "Stipendija",
            SvrhaPotvrde.Alimentacija => "Alimentacija",
            SvrhaPotvrde.Penzija => "Penzija",
            SvrhaPotvrde.SmjestajUDom => "Smještaj u studentski dom",
            SvrhaPotvrde.Viza => "Viza",
            SvrhaPotvrde.Ostalo => "Ostalo",
            _ => "Ostalo"
        };

        public Brush StatusBojaPozadina => _zahtjev.StanjePotvrde switch
        {
            StanjePotvrde.NaCekanju => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF3CD")),
            StanjePotvrde.Odobrena => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D4EDDA")),
            StanjePotvrde.Odbijena => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F8D7DA")),
            StanjePotvrde.Ponistena => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E3E5")),
            _ => Brushes.Transparent
        };

        public Brush StatusBojaTekst => _zahtjev.StanjePotvrde switch
        {
            StanjePotvrde.NaCekanju => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#856404")),
            StanjePotvrde.Odobrena => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#155724")),
            StanjePotvrde.Odbijena => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#721C24")),
            StanjePotvrde.Ponistena => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#383D41")),
            _ => Brushes.Black
        };
    }

    // za priakz ispita
    public class IspitPrikazDTO
    {
        public int IspitId { get; set; }
        public int PredmetId { get; set; }
        public string PredmetNaziv { get; set; } = string.Empty;
        public DateTime DatumOdrzavanja { get; set; }
        public int BrojPrijavljenih { get; set; }
        public bool Dodatni { get; set; }
        public string TipRoka => Dodatni ? "Dodatni rok" : "Redovni rok";
    }

    // DTO model za ljepši prikaz ispita u ComboBox-u
    public class IspitCmbDTO
    {
        public int IspitId { get; set; }
        public string PrikazTekst { get; set; } = string.Empty;
    }

    // DTO model za jedan red u DataGrid tabeli za unos ocjene
    public class PrijavaZaOcjenuDTO
    {
        public StudentIspit Prijava { get; set; }
        public string ImePrezime => $"{Prijava.Student.Ime} {Prijava.Student.Prezime}";
        public string Indeks => Prijava.Student.Indeks;
        public int BrojIzlazaka => Prijava.BrojIzlazaka;
        public List<int> DostupneOcjene { get; } = new List<int> { 5, 6, 7, 8, 9, 10 };
        public int OdabranaOcjena { get; set; }
        public bool JeLiOcjenjen => Prijava.Ocjena.HasValue;
        public string StatusTekst => JeLiOcjenjen ? $"Ocjenjeno ({Prijava.Ocjena})" : "Čeka ocjenu";
        public string StatusBoja => JeLiOcjenjen ? "#28a745" : "#fd7e14";
        public string StatusPozadina => JeLiOcjenjen ? "#2028a745" : "#20fd7e14";

        public PrijavaZaOcjenuDTO(StudentIspit prijava)
        {
            Prijava = prijava;
            OdabranaOcjena = prijava.Ocjena ?? 5;
        }
    }

    public class KonacnaOcjenaDTO
    {
        public StudentPredmet Prijava { get; set; } = null!;
        public string ImePrezime { get; set; } = string.Empty;
        public string Indeks { get; set; } = string.Empty;
        public string DetaljiIspita { get; set; } = string.Empty;
        public int PredlozenaOcjena { get; set; }
        public int OdabranaOcjena { get; set; }
        public string StatusBoja { get; set; } = "#dc3545"; 
        public string StatusOpis { get; set; } = string.Empty;
        public List<int> DostupneOcjene { get; set; } = new() { 5, 6, 7, 8, 9, 10 };
    }

    public class GodinaUspjehDTO
    {
        public string GodinaOpis { get; set; } // Npr. "Prva godina - SI"
        public double ProsjekGodine { get; set; }
        public List<StudentPredmet> Predmeti { get; set; }
    }

    public class UspjehStudentaDTO
    {
        public List<GodinaUspjehDTO> UspjehPoGodinama { get; set; } = new();
        public double UkupniProsjek { get; set; }
    }
}
