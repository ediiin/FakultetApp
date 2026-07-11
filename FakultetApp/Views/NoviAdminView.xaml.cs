using Fakultet.Core.Modeli;
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
            bool validno = true;
            OcistiErrorTextove();

            if(string.IsNullOrWhiteSpace(tbIme.Text))
            {
                lblImeError.Text = "Polje 'Ime' obavezno!";
                lblImeError.Visibility = Visibility.Visible;
                validno = false;
            }
            if (string.IsNullOrWhiteSpace(tbPrezime.Text))
            {
                lblPrezimeError.Text = "Polje 'Prezime' obavezno!";
                lblPrezimeError.Visibility = Visibility.Visible;
                validno = false;
            }
            if (string.IsNullOrWhiteSpace(tbKorisnickoIme.Text))
            {
                lblKorisnickoImeError.Text = "Polje 'Korisničko Ime' obavezno!";
                lblKorisnickoImeError.Visibility = Visibility.Visible;
                validno = false;
            }
            if (string.IsNullOrWhiteSpace(tbEmail.Text))
            {
                lblEmailError.Text = "Polje 'Email' obavezno!";
                lblEmailError.Visibility = Visibility.Visible;
                validno = false;
            }
            else if (!Regex.IsMatch(tbEmail.Text, @"^[a-z.]{2,}@fit\.ba$"))
            {
                lblEmailError.Text = "Polje 'Email' mora biti oblika 'xx@fit.ba'!";
                lblEmailError.Visibility = Visibility.Visible;
                validno = false;
            }
            if (string.IsNullOrWhiteSpace(tbJMBG.Text))
            {
                lblJMBGError.Text = "Polje 'JMBG' obavezno!";
                lblJMBGError.Visibility = Visibility.Visible;
                validno = false;
            }
            else if (tbJMBG.Text.Length != 13)
            {
                lblJMBGError.Text = "Polje 'JMBG' mora imati tačno 13 znakova!";
                lblJMBGError.Visibility = Visibility.Visible;
                validno = false;
            }
            if(dtpDatumRodjenja.SelectedDate == null)
            {
                lblDatumRodjenjaError.Text = "Polje 'Datum Rođenja' obavezno!";
                lblDatumRodjenjaError.Visibility = Visibility.Visible;
                validno = false;
            }
            else if(dtpDatumRodjenja.SelectedDate > DateTime.UtcNow)
            {
                lblDatumRodjenjaError.Text = "Datum rođenja ne može biti veći od " +
                    "trenutnog datuma!";
                lblDatumRodjenjaError.Visibility = Visibility.Visible;
                validno = false;
            }

            //lozinka
            if (string.IsNullOrWhiteSpace(pbLozinka.Password))
            {
                lblLozinkaError.Text = "Polje 'Lozinka' obavezno!";
                lblLozinkaError.Visibility = Visibility.Visible;
                validno = false;
            }
            else
            {
                string greska = "";
                if (pbLozinka.Password.Length < 8)
                    greska += "- Minimalno 8 znakova" + Environment.NewLine;
                if (!pbLozinka.Password.Any(char.IsUpper))
                    greska += "- Obavezno veliko slovo" + Environment.NewLine;
                if (!pbLozinka.Password.Any(char.IsLower))
                    greska += "- Obavezno malo slovo" + Environment.NewLine;
                if (!pbLozinka.Password.Any(char.IsDigit))
                    greska += "- Obavezno broj" + Environment.NewLine;
                if (!pbLozinka.Password.Any(c => !char.IsLetterOrDigit(c)))
                    greska += "- Obavezno specijalan znak" + Environment.NewLine;
                if (greska != "")
                {
                    lblLozinkaError.Text = greska;
                    lblLozinkaError.Visibility = Visibility.Visible;
                    validno = false;
                }
            }

            return validno;
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
