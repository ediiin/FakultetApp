using Fakultet.Core.Modeli;
using Fakultet.Servisi.Helperi;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.Admin.ProfesoriLogika
{
    /// <summary>
    /// Interaction logic for ProfesoriDodajView.xaml
    /// </summary>
    public partial class ProfesoriDodajView : UserControl
    {
        private readonly ProfesorServis _profesorServis;
        private readonly GradServis _gradServis;
        private readonly SpolServis _spolServis;
        public ProfesoriDodajView(ProfesorServis profesorServis, GradServis gradServis, SpolServis spolServis)
        {
            InitializeComponent();
            _profesorServis = profesorServis;
            _gradServis = gradServis;
            _spolServis = spolServis;

            UcitajSpolove();
            UcitajGradove();
            UcitajZvanje();
        }

        private void UcitajZvanje()
        {
            cmbZvanje.ItemsSource = Enum.GetValues(typeof(Zvanje))
                            .Cast<Zvanje>()
                            .Select(z => new
                            {
                                Vrijednost = z,
                                Prikaz = z.ToFriendlyString()
                            })
                            .ToList();
            cmbZvanje.DisplayMemberPath = "Prikaz";
            cmbZvanje.SelectedValuePath = "Vrijednost";
            cmbZvanje.SelectedIndex = 0;
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
            decimal plata = int.Parse(tbPlata.Text);

            var noviProfesor = new Profesor()
            {
                Ime = tbIme.Text,
                Prezime = tbPrezime.Text,
                Email = tbEmail.Text,
                KorisnickoIme = tbKorisnickoIme.Text,
                DatumRodjenja = dtpDatumRodjenja.SelectedDate!.Value,
                JMBG = tbJMBG.Text,
                Uloge = Uloge.Profesor,
                SpolId = spol!.Id,
                GradId = grad!.Id,
                Plata = plata,
                Zvanje = (Zvanje)cmbZvanje.SelectedItem,
                Ocjena = 0.0f,
                LozinkaHash = BCrypt.Net.BCrypt.HashPassword(pbLozinka.Password)
            };
            _profesorServis.Add(noviProfesor);
            MessageBox.Show($"Uspješno dodan profesor: {noviProfesor.ImePrezime}!", "Uspjeh"
                , MessageBoxButton.OK, MessageBoxImage.Exclamation);

            OcitiSvaPolja();
        }

        private void OcitiSvaPolja()
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
            cmbZvanje.SelectedIndex = 0;
        }

        private bool Validno()
        {
            OcistiSveErrore();

            var greske = OsobaValidacija.ValidirajSve(
                tbIme.Text, tbPrezime.Text, tbKorisnickoIme.Text, tbEmail.Text, tbJMBG.Text
                , dtpDatumRodjenja.SelectedDate, pbLozinka.Password);

            if (greske.TryGetValue("Ime", out string? errIme)) { lblImeError.Text = errIme; lblImeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Prezime", out string? errPrezime)) { lblPrezimeError.Text = errPrezime; lblPrezimeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("KorisnickoIme", out string? errKorIme)) { lblKorisnickoImeError.Text = errKorIme; lblKorisnickoImeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Email", out string? errEmail)) { lblEmailError.Text = errEmail; lblEmailError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("JMBG", out string? errJmbg)) { lblJMBGError.Text = errJmbg; lblJMBGError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("DatumRodjenja", out string? errDatum)) { lblDatumRodjenjaError.Text = errDatum; lblDatumRodjenjaError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Lozinka", out string? errLozinka)) { lblLozinkaError.Text = errLozinka; lblLozinkaError.Visibility = Visibility.Visible; }

            if (string.IsNullOrWhiteSpace(tbPlata.Text))
            {
                lblPlataError.Text = "Polje 'Plata' obavezno!";
                lblPlataError.Visibility = Visibility.Visible;
                return false;
            }
            else if(!(decimal.TryParse(tbPlata.Text, out decimal plata) && plata > 0))
            {
                lblPlataError.Text = "Plata mora biti pozitivan broj!";
                lblPlataError.Visibility = Visibility.Visible;
                return false;
            }
            else if(!(decimal.TryParse(tbPlata.Text, out decimal plataa) && plataa < 20001))
            {
                lblPlataError.Text = "Nije dozvoljeno postavljanje plate preko 20000KM!";
                lblPlataError.Visibility = Visibility.Visible;
                return false;
            }

            if (greske.Count == 0)
                return true;
            return false;
        }

        private void OcistiSveErrore()
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
