using Fakultet.Core.Modeli;
using Fakultet.Core.Modeli.Forum;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using Fakultet.Servisi.IServis.Forum;
using Fakultet.Servisi.IServis.Pomocni;
using FakultetApp.Views.StudentViews;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Path = System.IO.Path;

namespace FakultetApp.Views.AsistentViews
{
    /// <summary>
    /// Interaction logic for AsistentObjaveMaterijaliView.xaml
    /// </summary>
    public partial class AsistentObjaveMaterijaliView : UserControl
    {
        private readonly GodinaStudijaServis _godinaServis;
        private readonly AsistentPredmetServis _asistentPredmetServis; 
        private readonly PostServis _postServis;
        private readonly MaterijalServis _materijalServis;
        private readonly int _trenutniAsistentId;
        //varijabla koja cuva originalnu putanju sa asistentovog racunara
        private string _odabraniIzvorniFajl = string.Empty;

        private int _trenutnaStranicaObjave = 1;
        private int _trenutnaStranicaMaterijali = 1;
        private const int BrojPoStranici = 6;
        private List<PostViewModel> _sveObjaveZaPredmet = new List<PostViewModel>();
        private List<MaterijalViewModel> _sviMaterijaliZaPredmet = new List<MaterijalViewModel>();

        public AsistentObjaveMaterijaliView(
            GodinaStudijaServis godinaServis,
            AsistentPredmetServis asistentPredmetServis,
            PostServis postServis,
            MaterijalServis materijalServis,
            Asistent trenutniAsistent)
        {
            InitializeComponent();

            _godinaServis = godinaServis;
            _asistentPredmetServis = asistentPredmetServis;
            _postServis = postServis;
            _materijalServis = materijalServis;
            _trenutniAsistentId = trenutniAsistent.Id;

            UcitajGodine();
        }

        private void UcitajGodine()
        {
            var godine = _godinaServis.GetAll();
            cmbGodina.ItemsSource = godine;
            cmbGodina.DisplayMemberPath = "Opis";
            cmbGodina.SelectedValuePath = "Id";

            if (godine != null && godine.Count > 0)
                cmbGodina.SelectedIndex = 0;
        }

        private void CmbGodina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbGodina.SelectedValue is int odabranaGodinaId)
            {
                var predmeti = _asistentPredmetServis.GetPredmetiByYearAndAsistent(odabranaGodinaId, _trenutniAsistentId);

                cmbPredmet.ItemsSource = predmeti;
                cmbPredmet.DisplayMemberPath = "Naziv";
                cmbPredmet.SelectedValuePath = "Id";
                cmbPredmet.IsEnabled = true;

                _sveObjaveZaPredmet.Clear();
                _sviMaterijaliZaPredmet.Clear();
                icObjave.ItemsSource = null;
                icMaterijali.ItemsSource = null;
                txtNemaObjava.Visibility = Visibility.Collapsed;
                txtNemaMaterijala.Visibility = Visibility.Collapsed;
                PanelPaginacijaObjava.Children.Clear();
                PanelPaginacijaMaterijala.Children.Clear();

                if (predmeti != null && predmeti.Count > 0)
                    cmbPredmet.SelectedIndex = 0;
            }
        }

        private void CmbPredmet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OsvjeziPrikaz();
        }

        // LOGIKA TABOVA I PRIKAZA
        private void Tab_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;
            OsvjeziPrikaz();
        }

        private void OsvjeziPrikaz()
        {
            PrikazObjava.Visibility = Visibility.Collapsed;
            PrikazMaterijala.Visibility = Visibility.Collapsed;
            UnosObjave.Visibility = Visibility.Collapsed;
            UnosMaterijala.Visibility = Visibility.Collapsed;

            if (tabObjave.IsChecked == true)
            {
                btnDodajNovo.Content = "➕ Dodaj objavu";
                PrikazObjava.Visibility = Visibility.Visible;
                UcitajObjave();
            }
            else if (tabMaterijali.IsChecked == true)
            {
                btnDodajNovo.Content = "➕ Dodaj materijal";
                PrikazMaterijala.Visibility = Visibility.Visible;
                UcitajMaterijale();
            }
        }

        private void UcitajObjave()
        {
            if (cmbPredmet.SelectedValue is int predmetId)
            {
                var objave = _postServis.GetByPredmet(predmetId);
                _sveObjaveZaPredmet = objave?.Select(p => new PostViewModel
                {
                    Naslov = p.Naslov,
                    Sadrzaj = p.Sadrzaj,
                    DatumObjave = p.DatumObjave
                }).OrderByDescending(p => p.DatumObjave).ToList() ?? new List<PostViewModel>();

                _trenutnaStranicaObjave = 1;
                PrikaziStranicuObjava();
            }
        }

        private void PrikaziStranicuObjava()
        {
            if (_sveObjaveZaPredmet == null || _sveObjaveZaPredmet.Count == 0)
            {
                icObjave.ItemsSource = null;
                txtNemaObjava.Visibility = Visibility.Visible;
                PanelPaginacijaObjava.Children.Clear();
                return;
            }

            txtNemaObjava.Visibility = Visibility.Collapsed;
            int ukupnoStranica = (int)Math.Ceiling((double)_sveObjaveZaPredmet.Count / BrojPoStranici);

            if (_trenutnaStranicaObjave > ukupnoStranica) _trenutnaStranicaObjave = ukupnoStranica;
            if (_trenutnaStranicaObjave < 1) _trenutnaStranicaObjave = 1;

            var stranicaObjave = _sveObjaveZaPredmet
                .Skip((_trenutnaStranicaObjave - 1) * BrojPoStranici)
                .Take(BrojPoStranici)
                .ToList();

            icObjave.ItemsSource = stranicaObjave;
            KreirajPaginaciju(PanelPaginacijaObjava, ukupnoStranica, _trenutnaStranicaObjave, (stranica) => {
                _trenutnaStranicaObjave = stranica;
                PrikaziStranicuObjava();
            });
        }

        private void UcitajMaterijale()
        {
            if (cmbPredmet.SelectedValue is int predmetId)
            {
                var materijali = _materijalServis.GetByPredmet(predmetId);
                _sviMaterijaliZaPredmet = materijali?.Select(m => new MaterijalViewModel
                {
                    Naslov = m.Naziv,
                    TipMaterijala = m.TipMaterijala,
                    DatumObjave = m.DatumPostavljanja,
                }).OrderByDescending(m => m.DatumObjave).ToList() ?? new List<MaterijalViewModel>();

                _trenutnaStranicaMaterijali = 1;
                PrikaziStranicuMaterijala();
            }
        }

        private void PrikaziStranicuMaterijala()
        {
            if (_sviMaterijaliZaPredmet == null || _sviMaterijaliZaPredmet.Count == 0)
            {
                icMaterijali.ItemsSource = null;
                txtNemaMaterijala.Visibility = Visibility.Visible;
                PanelPaginacijaMaterijala.Children.Clear();
                return;
            }

            txtNemaMaterijala.Visibility = Visibility.Collapsed;
            int ukupnoStranica = (int)Math.Ceiling((double)_sviMaterijaliZaPredmet.Count / BrojPoStranici);

            if (_trenutnaStranicaMaterijali > ukupnoStranica) _trenutnaStranicaMaterijali = ukupnoStranica;
            if (_trenutnaStranicaMaterijali < 1) _trenutnaStranicaMaterijali = 1;

            var stranicaMaterijala = _sviMaterijaliZaPredmet
                .Skip((_trenutnaStranicaMaterijali - 1) * BrojPoStranici)
                .Take(BrojPoStranici)
                .ToList();

            icMaterijali.ItemsSource = stranicaMaterijala;
            KreirajPaginaciju(PanelPaginacijaMaterijala, ukupnoStranica, _trenutnaStranicaMaterijali, (stranica) => {
                _trenutnaStranicaMaterijali = stranica;
                PrikaziStranicuMaterijala();
            });
        }

        private void KreirajPaginaciju(StackPanel panel, int ukupnoStranica, int trenutnaStranica, Action<int> onStranicaPromijenjena)
        {
            panel.Children.Clear();
            if (ukupnoStranica <= 1) return;

            for (int i = 1; i <= ukupnoStranica; i++)
            {
                Button btn = new Button
                {
                    Content = i.ToString(),
                    Width = 30,
                    Height = 30,
                    Margin = new Thickness(3),
                    FontWeight = FontWeights.Bold,
                    Cursor = Cursors.Hand
                };

                if (i == trenutnaStranica)
                {
                    btn.Background = (Brush)FindResource("ButtonColor");
                    btn.Foreground = Brushes.White;
                }
                else
                {
                    btn.Background = Brushes.Transparent;
                    btn.Foreground = (Brush)FindResource("PrimaryText");
                }

                int stranicaBroj = i;
                btn.Click += (s, e) => onStranicaPromijenjena(stranicaBroj);
                panel.Children.Add(btn);
            }
        }

        // FORME ZA DODAVANJE
        private void BtnDodajNovo_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPredmet.SelectedValue == null)
            {
                MessageBox.Show("Molimo prvo odaberite predmet!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PrikazObjava.Visibility = Visibility.Collapsed;
            PrikazMaterijala.Visibility = Visibility.Collapsed;

            if (tabObjave.IsChecked == true)
            {
                txtNoviNaslovObjave.Clear();
                txtNoviSadrzajObjave.Clear();
                UnosObjave.Visibility = Visibility.Visible;
            }
            else
            {
                txtNoviNaslovMaterijala.Clear();
                txtPutanjaMaterijala.Clear();
                cmbTipMaterijala.SelectedIndex = 0;
                UnosMaterijala.Visibility = Visibility.Visible;
            }
        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            OsvjeziPrikaz();
        }

        //dodavanje objave
        private void BtnSpasiObjavu_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNoviNaslovObjave.Text) || string.IsNullOrWhiteSpace(txtNoviSadrzajObjave.Text))
            {
                MessageBox.Show("Molimo popunite naslov i sadržaj objave.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool jeGlobalna = chkGlobalnaObjava.IsChecked == true;

            if (!jeGlobalna && cmbPredmet.SelectedValue == null)
            {
                MessageBox.Show("Molimo odaberite predmet ili označite da je objava globalna.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int? predmetIdZaBazu = jeGlobalna ? (int?)null : (int)cmbPredmet.SelectedValue;

            var novaObjava = new Post
            {
                Naslov = txtNoviNaslovObjave.Text,
                Sadrzaj = txtNoviSadrzajObjave.Text,
                DatumObjave = DateTime.Now,
                PredmetId = predmetIdZaBazu,
                OsobaId = _trenutniAsistentId // Sada asistent spašava objavu
            };

            _postServis.Add(novaObjava);

            string poruka = jeGlobalna ? "Globalna objava uspješno dodana!" : "Objava za predmet uspješno dodana!";
            MessageBox.Show(poruka, "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);

            txtNoviNaslovObjave.Clear();
            txtNoviSadrzajObjave.Clear();
            chkGlobalnaObjava.IsChecked = false;

            OsvjeziPrikaz();
        }

        //za materijal gde se sakriva ili prikazuje unos fajla ili link
        private void CmbTipMaterijala_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded || PanelFajl == null || PanelLink == null) return;

            string odabraniTip = (cmbTipMaterijala.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (odabraniTip == "Video")
            {
                PanelFajl.Visibility = Visibility.Collapsed;
                PanelLink.Visibility = Visibility.Visible;
            }
            else
            {
                PanelFajl.Visibility = Visibility.Visible;
                PanelLink.Visibility = Visibility.Collapsed;
            }
        }

        //odabir fajla
        private void BtnOdaberiFajl_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Odaberi materijal";

            //ogranicimo sta asistent moze dodati zavisno od tipa
            string odabraniTip = (cmbTipMaterijala.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (odabraniTip == "PDF")
                openFileDialog.Filter = "PDF fajlovi (*.pdf)|*.pdf";
            else if (odabraniTip == "Skripta (Word)")
                openFileDialog.Filter = "Word fajlovi (*.doc;*.docx)|*.doc;*.docx";
            else
                openFileDialog.Filter = "Svi fajlovi (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                _odabraniIzvorniFajl = openFileDialog.FileName; //prava putanja C:\Users\Asistent\Desktop\skripta.pdf
                txtPutanjaMaterijala.Text = Path.GetFileName(_odabraniIzvorniFajl); //samo ime fajla za vizuelni prikaz u TextBoxu
            }
        }

        //spasavanje materijala
        private void BtnSpasiMaterijal_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPredmet.SelectedValue == null)
            {
                MessageBox.Show("Molimo odaberite predmet.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNoviNaslovMaterijala.Text))
            {
                MessageBox.Show("Molimo unesite naslov materijala.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string odabraniTip = (cmbTipMaterijala.SelectedItem as ComboBoxItem)?.Content.ToString();
            string? putanjaZaBazu = null;
            string? webLinkZaBazu = null;

            if (odabraniTip == "Video")
            {
                if (string.IsNullOrWhiteSpace(txtVideoLink.Text))
                {
                    MessageBox.Show("Molimo unesite link za video.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                webLinkZaBazu = txtVideoLink.Text;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(_odabraniIzvorniFajl))
                {
                    MessageBox.Show("Molimo odaberite fajl sa računara.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    //kreiramo folder "Uploads/Materijali" unutar same aplikacije
                    string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
                    string uploadsFolder = Path.Combine(baseFolder, "Uploads", "Materijali");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    //ime fajla npr. "skripta.pdf"
                    string imeFajla = Path.GetFileName(_odabraniIzvorniFajl);

                    // dodamo jedinstveni ID (Guid) da se fajlovi ne bi prepisali 
                    // npr. ako dva asistenta uploadaju razlicite fajlove sa istim imenom "vjezba1.pdf"
                    string jedinstvenoIme = Guid.NewGuid().ToString() + "_" + imeFajla;
                    string odredisnaPutanja = Path.Combine(uploadsFolder, jedinstvenoIme);

                    File.Copy(_odabraniIzvorniFajl, odredisnaPutanja, true);
                    putanjaZaBazu = Path.Combine("Uploads", "Materijali", jedinstvenoIme);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška prilikom kopiranja fajla: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            var noviMaterijal = new Materijal
            {
                Naziv = txtNoviNaslovMaterijala.Text,
                TipMaterijala = odabraniTip,
                Opis = txtNoviOpisMaterijala.Text,
                PutanjaFajla = putanjaZaBazu,
                WebLink = webLinkZaBazu,
                DatumPostavljanja = DateTime.Now,
                PredmetId = (int)cmbPredmet.SelectedValue,
                OsobaId = _trenutniAsistentId // Sada asistent postavlja materijal
            };

            _materijalServis.Add(noviMaterijal);

            MessageBox.Show("Materijal uspješno dodan!", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);

            txtNoviNaslovMaterijala.Clear();
            txtNoviOpisMaterijala.Clear();
            txtPutanjaMaterijala.Clear();
            txtVideoLink.Clear();
            _odabraniIzvorniFajl = string.Empty;

            OsvjeziPrikaz();
        }
    }
}