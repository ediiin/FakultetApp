using Fakultet.Servisi.IServis.FakultetskiProcesi;
using Fakultet.Servisi.IServis.Forum;
using Fakultet.Servisi.IServis.Pomocni;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FakultetApp.Views.StudentViews
{
    /// <summary>
    /// Interaction logic for StudentiDokumentiView.xaml
    /// </summary>
    public partial class StudentiDokumentiView : UserControl
    {
        private readonly GodinaStudijaServis _godinaServis;
        private readonly PredmetServis _predmetServis;
        private readonly MaterijalServis _materijalServis; 

        private int _trenutnaStranica = 1;
        private const int BrojObjavaPoStranici = 6;
        private List<MaterijalViewModel> _sviMaterijaliZaPredmet = new List<MaterijalViewModel>();

        private MaterijalViewModel? _trenutniMaterijal;

        public StudentiDokumentiView(
            GodinaStudijaServis godinaServis, 
            PredmetServis predmetServis, 
            MaterijalServis materijalServis)
        {
            InitializeComponent();

            _godinaServis = godinaServis;
            _predmetServis = predmetServis;
            _materijalServis = materijalServis;

            UcitajGodine();
            UcitajMaterijalePrvogPredmeta();
        }

        private void UcitajGodine()
        {
            var godine = _godinaServis.GetAll();
            cmbGodina.ItemsSource = godine;
            cmbGodina.DisplayMemberPath = "Opis";
            cmbGodina.SelectedValuePath = "Id";
        }

        private void UcitajMaterijalePrvogPredmeta()
        {
            var godina = _godinaServis.GetAll().FirstOrDefault();
            if (godina == null) return;

            cmbGodina.SelectedValue = godina.Id;
        }

        private void CmbGodina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbGodina.SelectedValue is int odabranaGodinaId)
            {
                var predmeti = _predmetServis.GetByYear(odabranaGodinaId);

                cmbPredmet.ItemsSource = predmeti;
                cmbPredmet.DisplayMemberPath = "Naziv";
                cmbPredmet.SelectedValuePath = "Id";
                cmbPredmet.IsEnabled = true;

                _sviMaterijaliZaPredmet.Clear();
                icObavijesti.ItemsSource = null;
                txtNemaObjava.Visibility = Visibility.Collapsed;
                PanelPaginacija.Children.Clear();

                if (predmeti != null && predmeti.Count > 0)
                {
                    cmbPredmet.SelectedIndex = 0;
                }
            }
        }

        private void CmbPredmet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPredmet.SelectedValue is int odabraniPredmetId)
            {
                UcitajMaterijaleZaPredmet(odabraniPredmetId);
            }
        }

        private void UcitajMaterijaleZaPredmet(int predmetId)
        {
            var materijaliBaza = _materijalServis.GetByPredmet(predmetId);

            if (materijaliBaza == null || materijaliBaza.Count == 0)
            {
                _sviMaterijaliZaPredmet.Clear();
                icObavijesti.ItemsSource = null;
                PanelPaginacija.Children.Clear();
                txtNemaObjava.Visibility = Visibility.Visible;
                return;
            }

            txtNemaObjava.Visibility = Visibility.Collapsed;

            _sviMaterijaliZaPredmet = materijaliBaza.Select(m => new MaterijalViewModel
            {
                Id = m.Id,
                Naslov = m.Naziv,
                Sadrzaj = m.Opis ?? "Nema dodatnog opisa.",
                KratkiSadrzaj = string.IsNullOrEmpty(m.Opis) ? "Nema opisa." : (m.Opis.Length > 100 ? m.Opis.Substring(0, 100) + "..." : m.Opis),
                TipMaterijala = m.TipMaterijala,
                PutanjaFajla = m.PutanjaFajla,
                WebLink = m.WebLink,
                DatumObjave = m.DatumPostavljanja,
                AutorIme = m.Osoba != null ? $"{m.Osoba.Ime} {m.Osoba.Prezime}" : "Nepoznat Autor"
            })
            .OrderByDescending(m => m.DatumObjave)
            .ToList();

            PrikaziStranicu(1);
        }

        private void PrikaziStranicu(int brojStranice)
        {
            _trenutnaStranica = brojStranice;

            var materijaliZaPrikaz = _sviMaterijaliZaPredmet
                .Skip((_trenutnaStranica - 1) * BrojObjavaPoStranici)
                .Take(BrojObjavaPoStranici)
                .ToList();

            icObavijesti.ItemsSource = materijaliZaPrikaz;

            int ukupanBrojStranica = (int)Math.Ceiling((double)_sviMaterijaliZaPredmet.Count / BrojObjavaPoStranici);
            GenerisiPaginaciju(ukupanBrojStranica);
        }

        private void GenerisiPaginaciju(int ukupanBrojStranica)
        {
            PanelPaginacija.Children.Clear();
            if (ukupanBrojStranica <= 1) return;

            for (int i = 1; i <= ukupanBrojStranica; i++)
            {
                int brojStr = i;
                Button btnStranica = new Button
                {
                    Content = brojStr.ToString(),
                    Width = 35,
                    Height = 35,
                    Margin = new Thickness(5, 0, 5, 0),
                    Cursor = Cursors.Hand,
                    Background = _trenutnaStranica == brojStr ? (Brush)FindResource("AccentColor") : Brushes.Transparent,
                    Foreground = _trenutnaStranica == brojStr ? Brushes.White : (Brush)FindResource("PrimaryText"),
                    BorderThickness = new Thickness(1),
                    BorderBrush = (Brush)FindResource("BorderColor")
                };

                btnStranica.Click += (s, e) => PrikaziStranicu(brojStr);
                PanelPaginacija.Children.Add(btnStranica);
            }
        }

        private void BtnProcitajVise_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is MaterijalViewModel izabraniMaterijal)
            {
                _trenutniMaterijal = izabraniMaterijal; // cuvamo za "Otvori" dugme

                txtDetaljiNaslov.Text = izabraniMaterijal.Naslov;
                txtDetaljiSadrzaj.Text = izabraniMaterijal.Sadrzaj;
                txtDetaljiAutor.Text = izabraniMaterijal.AutorIme;
                txtDetaljiTip.Text = izabraniMaterijal.TipMaterijala;
                txtDetaljiDatum.Text = izabraniMaterijal.DatumObjave.ToString("dd.MM.yyyy. HH:mm");

                // ako materijal nema ni fajl ni link sakrijemo dugme
                btnOtvoriMaterijal.Visibility = (!string.IsNullOrEmpty(izabraniMaterijal.WebLink) 
                                                || !string.IsNullOrEmpty(izabraniMaterijal.PutanjaFajla))
                                                ? Visibility.Visible : Visibility.Collapsed;

                PrikazListe.Visibility = Visibility.Collapsed;
                PrikazDetalja.Visibility = Visibility.Visible;
            }
        }

        private void BtnNazad_Click(object sender, RoutedEventArgs e)
        {
            PrikazDetalja.Visibility = Visibility.Collapsed;
            PrikazListe.Visibility = Visibility.Visible;
        }

        private void BtnOtvoriMaterijal_Click(object sender, RoutedEventArgs e)
        {
            if (_trenutniMaterijal == null) return;

            try
            {
                if (!string.IsNullOrEmpty(_trenutniMaterijal.WebLink))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = _trenutniMaterijal.WebLink,
                        UseShellExecute = true // U .NET Core / .NET 5+ ovo mora biti true da bi otvorilo u defaultnom browseru
                    });
                }
                else if (!string.IsNullOrEmpty(_trenutniMaterijal.PutanjaFajla))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = _trenutniMaterijal.PutanjaFajla,
                        UseShellExecute = true // Otvara defaultni PDF reader (Adobe, Chrome...)
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom otvaranja materijala: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class MaterijalViewModel
    {
        public int Id { get; set; }
        public string Naslov { get; set; } = null!;
        public string Sadrzaj { get; set; } = null!;
        public string KratkiSadrzaj { get; set; } = null!;
        public string TipMaterijala { get; set; } = null!;
        public string? PutanjaFajla { get; set; }
        public string? WebLink { get; set; }
        public DateTime DatumObjave { get; set; }
        public string AutorIme { get; set; } = null!;

        public string Ikona => TipMaterijala == "PDF" ? "📕" : TipMaterijala == "Video" ? "▶️" : "📄";
    }
}

