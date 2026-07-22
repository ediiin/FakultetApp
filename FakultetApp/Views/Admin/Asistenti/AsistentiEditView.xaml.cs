using Fakultet.Core.Modeli;
using Fakultet.Servisi.Helperi;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.Admin.Asistenti
{
    public partial class AsistentiEditView : UserControl
    {
        private Asistent _asistent;
        private readonly AsistentServis _asistentServis;
        private readonly GradServis _gradServis;
        private readonly SpolServis _spolServis;

        public AsistentiEditView(
            Asistent asistent,
            AsistentServis asistentServis,
            GradServis gradServis,
            SpolServis spolServis)
        {    
            InitializeComponent();
            _asistent = asistent;
            _asistentServis = asistentServis;
            _gradServis = gradServis;
            _spolServis = spolServis;
            
            UcitajGrad();
            UcitajSpol();
            UcitajPodatkeAsistenta();
        }

        private void UcitajGrad()
        {
            var asistentovGrad = _gradServis.GetById(_asistent.GradId);

            if (asistentovGrad != null)
            {
                cmbGrad.ItemsSource = new List<Grad> { asistentovGrad };
                cmbGrad.DisplayMemberPath = "Naziv";
                cmbGrad.SelectedIndex = 0;
            }
        }

        private void UcitajSpol()
        {
            var asistentovSpol = _spolServis.GetById(_asistent.SpolId);

            if (asistentovSpol != null)
            {
                cmbSpol.ItemsSource = new List<Spol> { asistentovSpol };
                cmbSpol.DisplayMemberPath = "Naziv";
                cmbSpol.SelectedIndex = 0;
            }
        }

        private void UcitajPodatkeAsistenta()
        {
            tbIme.Text = _asistent.Ime;
            tbPrezime.Text = _asistent.Prezime;
            tbKorisnickoIme.Text = _asistent.KorisnickoIme;
            tbEmail.Text = _asistent.Email;
            tbJMBG.Text = _asistent.JMBG;
            dtpDatumRodjenja.SelectedDate = _asistent.DatumRodjenja;
            tbPlata.Text = _asistent.Plata.ToString("0.00"); 
        }

        private void BtnSpasi_Click(object sender, RoutedEventArgs e)
        {
            if (!Validno())
                return;

            _asistent.Ime = tbIme.Text;
            _asistent.Prezime = tbPrezime.Text;
            _asistent.KorisnickoIme = tbKorisnickoIme.Text;
            _asistent.Email = tbEmail.Text;
            _asistent.Plata = decimal.Parse(tbPlata.Text);

            if (!string.IsNullOrWhiteSpace(pbLozinka.Password))
            {
                _asistent.LozinkaHash = BCrypt.Net.BCrypt.HashPassword(pbLozinka.Password);
            }

            _asistentServis.Update(_asistent);
            MessageBox.Show("Podaci asistenta su uspješno ažurirani!", "Uspjeh", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            pbLozinka.Clear();
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
                    pbLozinka.Password,
                    false
                );

            if (greske.TryGetValue("Ime", out string? errIme)) { lblImeError.Text = errIme; lblImeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Prezime", out string? errPrezime)) { lblPrezimeError.Text = errPrezime; lblPrezimeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("KorisnickoIme", out string? errKorIme)) { lblKorisnickoImeError.Text = errKorIme; lblKorisnickoImeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Email", out string? errEmail)) { lblEmailError.Text = errEmail; lblEmailError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("JMBG", out string? errJmbg)) { lblJMBGError.Text = errJmbg; lblJMBGError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("DatumRodjenja", out string? errDatum)) { lblDatumRodjenjaError.Text = errDatum; lblDatumRodjenjaError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Lozinka", out string? errLozinka)) { lblLozinkaError.Text = errLozinka; lblLozinkaError.Visibility = Visibility.Visible; }
            
            if (!decimal.TryParse(tbPlata.Text, out _))
            {
                lblPlataError.Text = "Unesite validan iznos plate (npr. 1500.50)!";
                lblPlataError.Visibility = Visibility.Visible;
                return false;
            }
            if (greske.Count == 0)
                return true;

            return false;
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