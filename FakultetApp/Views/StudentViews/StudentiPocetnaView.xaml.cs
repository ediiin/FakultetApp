using Fakultet.Core.Modeli;
using Fakultet.Servisi.Helperi;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using Fakultet.Servisi.IServis.Forum;
using FakultetApp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

namespace FakultetApp.Views.StudentViews
{
    /// <summary>
    /// Interaction logic for StudentiPocetnaView.xaml
    /// </summary>
    public partial class StudentiPocetnaView : UserControl
    {
        private readonly Student _student;
        private readonly PostServis _postServis;
        private readonly StudentPredmetServis _studentPredmetServis;
        private readonly MaterijalServis _materijalServis;

        private List<PostViewModel> _sveObavijesti = new();
        private const int BrojObjavaPoStranici = 6;
        private int _trenutnaStranica = 1;
        public StudentiPocetnaView(Student student, 
            PostServis postServis, 
            StudentPredmetServis studentPredmetServis, 
            MaterijalServis materijalServis)
        {
            InitializeComponent();
            _student = student;
            _postServis = postServis;
            _studentPredmetServis = studentPredmetServis;
            _materijalServis = materijalServis;

            lblDobrodosao.Text = $"Dobrodošli, {_student.Ime}!";

            UcitajSveObavijesti();
            UcitajUspjeh();
            UcitajMaterijale();
        }

        private void Dokument_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is DokumentViewModel odabraniMaterijal)
            {
                // Ako su i putanja i link prazni u objektu, javi poruku
                if (string.IsNullOrEmpty(odabraniMaterijal.WebLink) && string.IsNullOrEmpty(odabraniMaterijal.PutanjaFajla))
                {
                    MessageBox.Show("Dokument nema definisanu putanju do fajla niti web link u bazi!", "Informacija", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    if (!string.IsNullOrEmpty(odabraniMaterijal.WebLink))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = odabraniMaterijal.WebLink,
                            UseShellExecute = true
                        });
                    }
                    else if (!string.IsNullOrEmpty(odabraniMaterijal.PutanjaFajla))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = odabraniMaterijal.PutanjaFajla,
                            UseShellExecute = true
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška prilikom otvaranja materijala: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UcitajUspjeh()
        {
            var upisaniPredmeti = _studentPredmetServis.GetByStudentId(_student.Id);

            if (upisaniPredmeti == null || !upisaniPredmeti.Any(p => p.Polozio && p.Ocjena > 5))
            {
                karticaUspjeh.Visibility = Visibility.Collapsed;
                return;
            }

            var polozeniPredmeti = upisaniPredmeti.Where(p => p.Polozio && p.Ocjena > 5).ToList();

            var ucitaneGodine = polozeniPredmeti
                .GroupBy(sp => sp.Predmet.GodinaStudija.Opis)
                .Select(g => new GodinaUspjehDTO
                {
                    GodinaOpis = g.Key,
                    ProsjekGodine = g.Average(p => (double)p.Ocjena),
                    Predmeti = g.ToList()
                })
                .OrderBy(g => GodinaStudijaHelper.OdrediBrojGodine(g.GodinaOpis))
                .ToList();

            var ukupniProsjek = polozeniPredmeti.Average(p => (double)p.Ocjena);

            var uspjehData = new UspjehStudentaDTO
            {
                UspjehPoGodinama = ucitaneGodine,
                UkupniProsjek = ukupniProsjek
            };

            karticaUspjeh.DataContext = uspjehData;
        }

        private void UcitajMaterijale()
        {
            var zadnjiMaterijali = _materijalServis.GetMaterijaliByStudent(_student.Id)
                                                 .OrderByDescending(m => m.DatumPostavljanja)
                                                 .Take(5)
                                                 .Select(m => new DokumentViewModel
                                                 {
                                                     Naziv = m.Naziv,
                                                     Datum = m.DatumPostavljanja,
                                                     PutanjaFajla = m.PutanjaFajla,
                                                     WebLink = m.WebLink,
                                                 })
                                                 .ToList();

            icDokumenti.ItemsSource = zadnjiMaterijali;

            if (!zadnjiMaterijali.Any())
                icDokumenti.Visibility = Visibility.Collapsed;
        }

        private void UcitajSveObavijesti()
        {
            var obavijestiIzBaze = _postServis.GetAll();

            _sveObavijesti = obavijestiIzBaze.Select(p => new PostViewModel
            {
                Id = p.Id,
                Naslov = p.Naslov,
                Sadrzaj = p.Sadrzaj,
                KratkiSadrzaj = p.Sadrzaj.Length > 100 ? p.Sadrzaj.Substring(0, 100) + "..." : p.Sadrzaj,
                DatumObjave = p.DatumObjave,
                OznakaPredmeta = p.Predmet != null ? p.Predmet.Naziv.ToUpper() : "OPĆA OBAVIJEST",
                AutorIme = p.Osoba != null ? $"{p.Osoba.Ime} {p.Osoba.Prezime}" : "Nepoznat Autor"
            })
            .OrderByDescending(p => p.DatumObjave)
            .ToList();

            PrikaziStranicu(1);
        }

        private void PrikaziStranicu(int brojStranice)
        {
            _trenutnaStranica = brojStranice;

            // Korištenje LINQ metoda Skip i Take za filtriranje podataka za trenutnu stranicu
            var obavijestiZaPrikaz = _sveObavijesti
                .Skip((_trenutnaStranica - 1) * BrojObjavaPoStranici)
                .Take(BrojObjavaPoStranici)
                .ToList();

            icObavijesti.ItemsSource = obavijestiZaPrikaz;

            GenerisiDugmadZaPaginaciju();
        }

        private void GenerisiDugmadZaPaginaciju()
        {
            PanelPaginacija.Children.Clear();

            // Math.Ceiling zaokruzuje broj stranica na vise (npr. 15 objava / 7 = 2.14 -> 3 stranice)
            int ukupanBrojStranica = (int)Math.Ceiling((double)_sveObavijesti.Count / BrojObjavaPoStranici);

            // Ako imamo samo 1 stranicu, ne treba nam paginacija
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
                    Cursor = System.Windows.Input.Cursors.Hand,
                    // Stilizacija aktivnog i neaktivnog dugmeta
                    Background = _trenutnaStranica == brojStr ? (Brush)FindResource("AccentColor") : Brushes.Transparent,
                    Foreground = _trenutnaStranica == brojStr ? Brushes.White : (Brush)FindResource("PrimaryText"),
                    BorderThickness = new Thickness(1),
                    BorderBrush = (Brush)FindResource("BorderColor")
                };

                btnStranica.Click += (s, e) => PrikaziStranicu(brojStr);

                PanelPaginacija.Children.Add(btnStranica);
            }
        }

        // logika otvaranja posta
        private void BtnProcitajVise_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is PostViewModel izabranaObjava)
            {
                // Popunjavanje podataka u Detail View
                txtDetaljiNaslov.Text = izabranaObjava.Naslov;
                txtDetaljiSadrzaj.Text = izabranaObjava.Sadrzaj;
                txtDetaljiOznaka.Text = izabranaObjava.OznakaPredmeta;
                txtDetaljiAutor.Text = izabranaObjava.AutorIme;
                txtDetaljiDatum.Text = izabranaObjava.DatumObjave.ToString("dd.MM.yyyy. HH:mm");

                // Sakrij listu, prikaži detalje
                PrikazListe.Visibility = Visibility.Collapsed;
                PrikazDetalja.Visibility = Visibility.Visible;
            }
        }

        private void BtnNazad_Click(object sender, RoutedEventArgs e)
        {
            // Vrati se na listu objava
            PrikazDetalja.Visibility = Visibility.Collapsed;
            PrikazListe.Visibility = Visibility.Visible;
        }
    }
}