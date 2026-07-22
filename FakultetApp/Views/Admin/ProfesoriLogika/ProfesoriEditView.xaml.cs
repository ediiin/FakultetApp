using Fakultet.Core.Modeli;
using Fakultet.Servisi.Helperi;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.Admin.ProfesoriLogika
{
    public partial class ProfesoriEditView : UserControl
    {
        private readonly Profesor _profesor;
        private readonly ProfesorServis _profesorServis; 
        private readonly GradServis _gradServis;
        private readonly SpolServis _spolServis;

        public ProfesoriEditView(Profesor profesor,
            ProfesorServis profesorServis,
            GradServis gradServis,
            SpolServis spolServis)
        {
            InitializeComponent();

            _profesor = profesor;
            _profesorServis = profesorServis;
            _gradServis = gradServis;
            _spolServis = spolServis;

            UcitajPodatkeProfesora();
        }

        private void UcitajGrad()
        {
            var profesorovGrad = _gradServis.GetById(_profesor.GradId);

            if (profesorovGrad != null)
            {
                cmbGrad.ItemsSource = new List<Grad> { profesorovGrad };
                cmbGrad.DisplayMemberPath = "ZvanjeImePrezime";
                cmbGrad.SelectedIndex = 0;
            }
        }

        private void UcitajSpol()
        {
            var profesorovSpol = _spolServis.GetById(_profesor.SpolId);

            if (profesorovSpol != null)
            {
                cmbSpol.ItemsSource = new List<Spol> { profesorovSpol };
                cmbSpol.DisplayMemberPath = "Naziv";
                cmbSpol.SelectedIndex = 0;
            }
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
            cmbZvanje.SelectedValue = _profesor.Zvanje;
        }

        private void UcitajPodatkeProfesora()
        {
            UcitajSpol();
            UcitajGrad();
            UcitajZvanje();

            tbIme.Text = _profesor.Ime;
            tbPrezime.Text = _profesor.Prezime;
            tbKorisnickoIme.Text = _profesor.KorisnickoIme;
            tbEmail.Text = _profesor.Email;

            tbJMBG.Text = _profesor.JMBG;
            dtpDatumRodjenja.SelectedDate = _profesor.DatumRodjenja;

            tbPlata.Text = _profesor.Plata.ToString("0.00");
        }

        private void BtnSacuvajIzmjene_Click(object sender, RoutedEventArgs e)
        {
            if (!Validno())
                return;

            _profesor.Ime = tbIme.Text;
            _profesor.Prezime = tbPrezime.Text;
            _profesor.KorisnickoIme = tbKorisnickoIme.Text;
            _profesor.Email = tbEmail.Text;
            _profesor.Zvanje = (Zvanje)cmbZvanje.SelectedValue;
            _profesor.Plata = decimal.Parse(tbPlata.Text);

            if (!string.IsNullOrWhiteSpace(pbLozinka.Password))
            {
                _profesor.LozinkaHash = BCrypt.Net.BCrypt.HashPassword(pbLozinka.Password);
            }

            _profesorServis.Update(_profesor);
            MessageBox.Show("Podaci o profesoru uspješno ažurirani!", "Uspjeh",
                MessageBoxButton.OK, MessageBoxImage.Information);
            pbLozinka.Clear();
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
            lblZvanjeError.Visibility = Visibility.Hidden;
            lblPlataError.Visibility = Visibility.Hidden;
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

            if (cmbZvanje.SelectedValue == null)
            {
                lblZvanjeError.Visibility = Visibility.Visible;
                return false;
            }

            if (!decimal.TryParse(tbPlata.Text, out _))
            {
                lblPlataError.Text = "Unesite validan iznos plate (npr. 1500.50)!";
                lblPlataError.Visibility = Visibility.Visible;
                return false;
            }

            if (greske.TryGetValue("Ime", out string? errIme)) { lblImeError.Text = errIme; lblImeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Prezime", out string? errPrezime)) { lblPrezimeError.Text = errPrezime; lblPrezimeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("KorisnickoIme", out string? errKorIme)) { lblKorisnickoImeError.Text = errKorIme; lblKorisnickoImeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Email", out string? errEmail)) { lblEmailError.Text = errEmail; lblEmailError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("JMBG", out string? errJmbg)) { lblJMBGError.Text = errJmbg; lblJMBGError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("DatumRodjenja", out string? errDatum)) { lblDatumRodjenjaError.Text = errDatum; lblDatumRodjenjaError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Lozinka", out string? errLozinka)) { lblLozinkaError.Text = errLozinka; lblLozinkaError.Visibility = Visibility.Visible; }

            if (greske.Count == 0)
                return true;

            return false;
        }
    }
}
