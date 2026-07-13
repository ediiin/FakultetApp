using Fakultet.Core.Modeli;
using Fakultet.Servisi.Helperi;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views
{
    /// <summary>
    /// Interaction logic for NoviAdminView.xaml
    /// </summary>
    public partial class NoviAdminView : UserControl
    {
        private SpolServis? _spolServis;
        private GradServis? _gradServis;
        private OsobaServis? _osobaServis;
        public NoviAdminView(SpolServis spolServis, GradServis gradServis, OsobaServis osobaServis)
        {
            InitializeComponent();

            _spolServis = spolServis;
            _gradServis = gradServis;
            _osobaServis = osobaServis;

            UcitajSpolove();
            UcitajGradove();
        }

        private void UcitajGradove()
        {
            cmbGrad.ItemsSource = _gradServis!.GetAll();
            cmbGrad.DisplayMemberPath = "Naziv";
            cmbGrad.SelectedIndex = 0;
        }

        private void UcitajSpolove()
        {
            cmbSpol.ItemsSource = _spolServis!.GetAll();
            cmbSpol.DisplayMemberPath = "Naziv";
            cmbSpol.SelectedIndex = 0;
        }

        private void BtnSpasi_Click(object sender, RoutedEventArgs e)
        {
            if (!Validno())
                return;

            var spol = cmbSpol.SelectedItem as Spol;
            var grad = cmbGrad.SelectedItem as Grad;
            var noviAdmin = new Osoba()
            {
                Ime = tbIme.Text,
                Prezime = tbPrezime.Text,
                KorisnickoIme = tbKorisnickoIme.Text,
                Email = tbEmail.Text,
                SpolId = spol!.Id,
                GradId = grad!.Id,
                JMBG = tbJMBG.Text,
                DatumRodjenja = dtpDatumRodjenja.SelectedDate!.Value,
                LozinkaHash = BCrypt.Net.BCrypt.HashPassword(pbLozinka.Password),
                Uloge = Uloge.Admin
            };

            _osobaServis!.Add(noviAdmin);
            MessageBox.Show("Uspješno dodan novi administrator!", "Uspjeh",
                MessageBoxButton.OK, MessageBoxImage.Exclamation);
            OcistiSvaPolja();
        }

        private void OcistiSvaPolja()
        {
            tbIme.Clear();
            tbPrezime.Clear();
            tbKorisnickoIme.Clear();
            tbEmail.Clear();
            tbJMBG.Clear();
            pbLozinka.Clear();
            dtpDatumRodjenja.SelectedDate = null;
            cmbGrad.SelectedIndex = 0;
            cmbSpol.SelectedIndex = 0;
        }

        private bool Validno()
        {
            OcistiErrorTextove();

            var greske = OsobaValidacija.ValidirajSve(
                tbIme.Text,
                tbPrezime.Text,
                tbKorisnickoIme.Text,
                tbEmail.Text,
                tbJMBG.Text,
                dtpDatumRodjenja.SelectedDate,
                pbLozinka.Password
            );

            if (greske.Count == 0)
                return true;

            if (greske.TryGetValue("Ime", out string? errIme)) { lblImeError.Text = errIme; lblImeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Prezime", out string? errPrezime)) { lblPrezimeError.Text = errPrezime; lblPrezimeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("KorisnickoIme", out string? errKorIme)) { lblKorisnickoImeError.Text = errKorIme; lblKorisnickoImeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Email", out string? errEmail)) { lblEmailError.Text = errEmail; lblEmailError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("JMBG", out string? errJmbg)) { lblJMBGError.Text = errJmbg; lblJMBGError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("DatumRodjenja", out string? errDatum)) { lblDatumRodjenjaError.Text = errDatum; lblDatumRodjenjaError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Lozinka", out string? errLozinka)) { lblLozinkaError.Text = errLozinka; lblLozinkaError.Visibility = Visibility.Visible; }

            return false;
        }

        private void OcistiErrorTextove()
        {
            lblImeError.Visibility = Visibility.Hidden;
            lblPrezimeError.Visibility = Visibility.Hidden;
            lblKorisnickoImeError.Visibility = Visibility.Hidden;
            lblEmailError.Visibility = Visibility.Hidden;
            lblJMBGError.Visibility = Visibility.Hidden;
            lblDatumRodjenjaError.Visibility = Visibility.Hidden;
            lblLozinkaError.Visibility = Visibility.Hidden;
        }
    }
}
