using Fakultet.Core.Modeli;
using Fakultet.Servisi.Helperi;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.Admin.Asistenti
{
    /// <summary>
    /// Interaction logic for AsistentDodajView.xaml
    /// </summary>
    public partial class AsistentDodajView : UserControl
    {
        private readonly AsistentServis _asistentServis;
        private readonly GradServis _gradServis;
        private readonly SpolServis _spolServis;

        public AsistentDodajView(AsistentServis asistentServis, GradServis gradServis, SpolServis spolServis)
        {
            InitializeComponent();
            _asistentServis = asistentServis;
            _gradServis = gradServis;
            _spolServis = spolServis;

            UcitajSpolove();
            UcitajGradove();
        }

        private void UcitajGradove()
        {
            cmbGrad.ItemsSource = _gradServis.GetAll();
            cmbGrad.DisplayMemberPath = "Naziv";
            cmbGrad.SelectedIndex = 0;
        }

        private void UcitajSpolove()
        {
            cmbSpol.ItemsSource = _spolServis.GetAll();
            cmbSpol.DisplayMemberPath = "Naziv";
            cmbSpol.SelectedIndex = 0;
        }

        private void BtnSpasi_Click(object sender, RoutedEventArgs e)
        {
            if (!Validno())
                return;

            var grad = cmbGrad.SelectedItem as Grad;
            var spol = cmbSpol.SelectedItem as Spol;

            decimal.TryParse(tbPlata.Text, out decimal plata);

            var noviAsistent = new Asistent()
            {
                Ime = tbIme.Text,
                Prezime = tbPrezime.Text,
                KorisnickoIme = tbKorisnickoIme.Text,
                Email = tbEmail.Text,
                DatumRodjenja = dtpDatumRodjenja.SelectedDate!.Value,
                JMBG = tbJMBG.Text,
                Uloge = Uloge.Asistent,
                SpolId = spol!.Id,
                GradId = grad!.Id,
                Plata = plata,
                LozinkaHash = BCrypt.Net.BCrypt.HashPassword(pbLozinka.Password)
            };

            _asistentServis.Add(noviAsistent);
            MessageBox.Show($"Uspješno dodan asistent: {noviAsistent.Ime} {noviAsistent.Prezime}",
                "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);

            OcistiSvaPolja();
        }

        private void OcistiSvaPolja()
        {
            tbIme.Clear();
            tbPrezime.Clear();
            tbKorisnickoIme.Clear();
            tbEmail.Clear();
            tbJMBG.Clear();
            tbPlata.Clear();
            pbLozinka.Clear();
            dtpDatumRodjenja.SelectedDate = null;
            cmbGrad.SelectedIndex = 0;
            cmbSpol.SelectedIndex = 0;
        }

        private bool Validno()
        {
            OcistiGreske();

            var greske = OsobaValidacija.ValidirajSve(
                tbIme.Text,
                tbPrezime.Text,
                tbKorisnickoIme.Text,
                tbEmail.Text,
                tbJMBG.Text,
                dtpDatumRodjenja.SelectedDate,
                pbLozinka.Password
            );

            bool status = true;

            if (string.IsNullOrWhiteSpace(tbPlata.Text) || !decimal.TryParse(tbPlata.Text, out decimal plata) || plata < 0)
            {
                lblPlataError.Text = "Unesite validan iznos plate!";
                lblPlataError.Visibility = Visibility.Visible;
                status = false;
            }
            else if (!decimal.TryParse(tbPlata.Text, out decimal plataa) || plataa > 15001)
            {
                lblPlataError.Text = "Plata za asistenta ne može biti veća od 15000KM!";
                lblPlataError.Visibility = Visibility.Visible;
                status = false;
            }

            if (greske.TryGetValue("Ime", out string? errIme)) { lblImeError.Text = errIme; lblImeError.Visibility = Visibility.Visible; status = false; }
            if (greske.TryGetValue("Prezime", out string? errPrezime)) { lblPrezimeError.Text = errPrezime; lblPrezimeError.Visibility = Visibility.Visible; status = false; }
            if (greske.TryGetValue("KorisnickoIme", out string? errKorIme)) { lblKorisnickoImeError.Text = errKorIme; lblKorisnickoImeError.Visibility = Visibility.Visible; status = false; }
            if (greske.TryGetValue("Email", out string? errEmail)) { lblEmailError.Text = errEmail; lblEmailError.Visibility = Visibility.Visible; status = false; }
            if (greske.TryGetValue("JMBG", out string? errJmbg)) { lblJMBGError.Text = errJmbg; lblJMBGError.Visibility = Visibility.Visible; status = false; }
            if (greske.TryGetValue("DatumRodjenja", out string? errDatum)) { lblDatumRodjenjaError.Text = errDatum; lblDatumRodjenjaError.Visibility = Visibility.Visible; status = false; }
            if (greske.TryGetValue("Lozinka", out string? errLozinka)) { lblLozinkaError.Text = errLozinka; lblLozinkaError.Visibility = Visibility.Visible; status = false; }

            return status;
        }

        private void OcistiGreske()
        {
            lblImeError.Visibility = Visibility.Hidden;
            lblPrezimeError.Visibility = Visibility.Hidden;
            lblKorisnickoImeError.Visibility = Visibility.Hidden;
            lblEmailError.Visibility = Visibility.Hidden;
            lblJMBGError.Visibility = Visibility.Hidden;
            lblDatumRodjenjaError.Visibility = Visibility.Hidden;
            lblLozinkaError.Visibility = Visibility.Hidden;
            lblPlataError.Visibility = Visibility.Hidden;
        }
    }
}

