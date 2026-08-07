using Fakultet.Servisi.IServis.FakultetskiProcesi;
using Fakultet.Servisi.IServis.Forum;
using Fakultet.Servisi.IServis.Pomocni;
using FakultetApp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FakultetApp.Views.StudentViews
{
    public partial class StudentiObjaveView : UserControl
    {
        private readonly GodinaStudijaServis _godinaServis;
        private readonly PredmetServis _predmetServis;
        private readonly PostServis _postServis;
        private int _trenutnaStranica = 1;
        private const int BrojObjavaPoStranici = 6;
        private List<PostViewModel> _sveObjaveZaPredmet = new List<PostViewModel>();

        public StudentiObjaveView(GodinaStudijaServis godinaServis, PredmetServis predmetServis, PostServis postServis)
        {
            InitializeComponent();

            _godinaServis = godinaServis;
            _predmetServis = predmetServis;
            _postServis = postServis;

            UcitajGodine();
            UcitajObjavePrvogPredmeta();
        }

        private void UcitajGodine()
        {
            var godine = _godinaServis.GetAll();
            cmbGodina.ItemsSource = godine;
            cmbGodina.DisplayMemberPath = "Opis";
            cmbGodina.SelectedValuePath = "Id"; 
        }

        private void UcitajObjavePrvogPredmeta()
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

                _sveObjaveZaPredmet.Clear();
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
                UcitajObjaveZaPredmet(odabraniPredmetId);
            }
        }

        private void UcitajObjaveZaPredmet(int predmetId)
        {
            var objaveBaza = _postServis.GetByPredmet(predmetId);

            if (objaveBaza == null || objaveBaza.Count == 0)
            {
                _sveObjaveZaPredmet.Clear();
                icObavijesti.ItemsSource = null;
                PanelPaginacija.Children.Clear(); 
                txtNemaObjava.Visibility = Visibility.Visible;
                return;
            }

            txtNemaObjava.Visibility = Visibility.Collapsed;

            _sveObjaveZaPredmet = objaveBaza.Select(p => new PostViewModel
            {
                Id = p.Id,
                Naslov = p.Naslov,
                Sadrzaj = p.Sadrzaj,
                KratkiSadrzaj = p.Sadrzaj.Length > 100 ? p.Sadrzaj.Substring(0, 100) + "..." : p.Sadrzaj,
                DatumObjave = p.DatumObjave,
                AutorIme = p.Osoba != null ? $"{p.Osoba.Ime} {p.Osoba.Prezime}" : "Nepoznat Autor"
            })
            .OrderByDescending(p => p.DatumObjave)
            .ToList();

            PrikaziStranicu(1);
        }

        private void PrikaziStranicu(int brojStranice)
        {
            _trenutnaStranica = brojStranice;

            var objaveZaPrikaz = _sveObjaveZaPredmet
                .Skip((_trenutnaStranica - 1) * BrojObjavaPoStranici)
                .Take(BrojObjavaPoStranici)
                .ToList();

            icObavijesti.ItemsSource = objaveZaPrikaz;

            int ukupanBrojStranica = (int)Math.Ceiling((double)_sveObjaveZaPredmet.Count / BrojObjavaPoStranici);

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
            if (sender is Button btn && btn.Tag is PostViewModel izabranaObjava)
            {
                txtDetaljiNaslov.Text = izabranaObjava.Naslov;
                txtDetaljiSadrzaj.Text = izabranaObjava.Sadrzaj;
                txtDetaljiAutor.Text = izabranaObjava.AutorIme;
                txtDetaljiDatum.Text = izabranaObjava.DatumObjave.ToString("dd.MM.yyyy. HH:mm");

                PrikazListe.Visibility = Visibility.Collapsed;
                PrikazDetalja.Visibility = Visibility.Visible;
            }
        }

        private void BtnNazad_Click(object sender, RoutedEventArgs e)
        {
            PrikazDetalja.Visibility = Visibility.Collapsed;
            PrikazListe.Visibility = Visibility.Visible;
        }
    }
}