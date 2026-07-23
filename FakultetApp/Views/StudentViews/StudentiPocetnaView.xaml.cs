using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.Forum;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FakultetApp.Views.StudentViews
{
    /// <summary>
    /// Interaction logic for StudentiPocetnaView.xaml
    /// </summary>
    public partial class StudentiPocetnaView : UserControl
    {
        private readonly Student _student;
        private readonly PostServis _postServis;

        private List<PostViewModel> _sveObavijesti = new();
        private const int BrojObjavaPoStranici = 6;
        private int _trenutnaStranica = 1;
        public StudentiPocetnaView(Student student, PostServis postServis)
        {
            InitializeComponent();
            _student = student;
            _postServis = postServis;

            lblDobrodosao.Text = $"Dobrodošli, {_student.Ime}!";

            UcitajSveObavijesti();
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

        // --- LOGIKA ZA OTVARANJE DETALJA (MASTER-DETAIL) ---
        private void BtnProcitajVise_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is PostViewModel izabranaObjava)
            {
                // Popunjavanje podataka u Detail View
                txtDetaljiNaslov.Text = izabranaObjava.Naslov;
                txtDetaljiSadrzaj.Text = izabranaObjava.Sadrzaj; // Puni, neskraćeni sadržaj
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
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Naslov { get; set; } = null!;
        public string Sadrzaj { get; set; } = null!;
        public string KratkiSadrzaj { get; set; } = null!;
        public DateTime DatumObjave { get; set; }
        public string OznakaPredmeta { get; set; } = null!;
        public string AutorIme { get; set; } = null!;
    }
}