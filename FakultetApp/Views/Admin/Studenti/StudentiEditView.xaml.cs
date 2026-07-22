using Fakultet.Core.Modeli;
using Fakultet.Servisi.Helperi;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.Admin.Studenti
{
    /// <summary>
    /// Interaction logic for StudentiEditView.xaml
    /// </summary>
    public partial class StudentiEditView : UserControl
    {
        private readonly Student _student;
        private readonly StudentServis _studentServis;
        private readonly StudijServis _studijServis;
        private readonly GodinaStudijaServis _godineStudijaServis;
        private readonly GradServis _gradServis;
        private readonly SpolServis _spolServis;

        public StudentiEditView(Student student, 
            StudentServis studentServis,
            StudijServis studijServis,
            GodinaStudijaServis godineStudijaServis,
            GradServis gradServis,
            SpolServis spolServis)
        {
            InitializeComponent();

            _student = student;
            _studentServis = studentServis;
            _godineStudijaServis = godineStudijaServis;
            _studijServis = studijServis;
            _gradServis = gradServis;
            _spolServis = spolServis;

            UcitajPodatkeStudenta();
        }

        private void UcitajGrad()
        {
            var studentovGrad = _gradServis.GetById(_student.GradId);

            if (studentovGrad != null)
            {
                cmbGrad.ItemsSource = new List<Grad> { studentovGrad };
                cmbGrad.DisplayMemberPath = "Naziv";
                cmbGrad.SelectedIndex = 0;
            }
        }

        private void UcitajSpol()
        {
            var studentovSpol = _spolServis.GetById(_student.SpolId);

            if (studentovSpol != null)
            {
                cmbSpol.ItemsSource = new List<Spol> { studentovSpol };
                cmbSpol.DisplayMemberPath = "Naziv";
                cmbSpol.SelectedIndex = 0;
            }
        }

        private void UcitajStudij()
        {
            cmbStudij.ItemsSource = _studijServis.GetAll();
            cmbStudij.DisplayMemberPath = "PuniNaziv";
            cmbStudij.SelectedValuePath = "Id";
            cmbStudij.SelectedValue = _student.GodinaStudija.StudijId;
        }

        private void UcitajPodatkeStudenta()
        {
            cmbStatus.ItemsSource = Enum.GetValues(typeof(Status));
            cmbStatus.SelectedValue = _student.Status;
            UcitajSpol();
            UcitajGrad();
            UcitajStudij();

            tbIme.Text = _student.Ime;
            tbPrezime.Text = _student.Prezime;
            tbKorisnickoIme.Text = _student.KorisnickoIme;
            tbEmail.Text = _student.Email;

            tbJMBG.Text = _student.JMBG;
            dtpDatumRodjenja.SelectedDate = _student.DatumRodjenja;
            tbIndeks.Text = _student.Indeks;
            dtpDatumUpisa.SelectedDate = _student.DatumUpisa;
        }

        private void CmbStudij_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var studij = (Studij)cmbStudij.SelectedItem;
            var pripadajuceGodine = _godineStudijaServis
                .GetAllByStudijId(studij.Id);

            cmbGodinaStudija.ItemsSource = pripadajuceGodine;
            cmbGodinaStudija.DisplayMemberPath = "Opis";
            cmbGodinaStudija.SelectedValuePath = "Id";
            cmbGodinaStudija.IsEnabled = true;

            if (pripadajuceGodine.Any(g => g.Id == _student.GodinaStudijaId))
            {
                cmbGodinaStudija.SelectedValue = _student.GodinaStudijaId;
            }
            else if (pripadajuceGodine.Any())
            {
                cmbGodinaStudija.SelectedIndex = 0;
            }
        }

        private void BtnSacuvajIzmjene_Click(object sender, RoutedEventArgs e)
        {
            if (!Validno())
                return;

            _student.Ime = tbIme.Text;
            _student.Prezime = tbPrezime.Text;
            _student.KorisnickoIme = tbKorisnickoIme.Text;
            _student.Email = tbEmail.Text;
            var status = (Status)cmbStatus.SelectedItem;
            var godinaStudija = (GodinaStudija)cmbGodinaStudija.SelectedItem;
            _student.GodinaStudijaId = godinaStudija.Id;
            _student.Status = status;

            if (!string.IsNullOrWhiteSpace(pbLozinka.Password))
            {
                _student.LozinkaHash = BCrypt.Net.BCrypt.HashPassword(pbLozinka.Password);
            }

            _studentServis.Update(_student);
            MessageBox.Show("Podaci o studentu uspješno ažurirani!", "Uspjeh", 
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
            lblStudijError.Visibility = Visibility.Hidden;
            lblGodinaStudijaError.Visibility = Visibility.Hidden;
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

            if (cmbStudij.SelectedItem == null)
                lblStudijError.Visibility = Visibility.Visible;

            if (cmbGodinaStudija.SelectedItem == null)
                lblGodinaStudijaError.Visibility = Visibility.Visible;

            if (greske.TryGetValue("Ime", out string? errIme)) { lblImeError.Text = errIme; lblImeError.Visibility = Visibility.Visible; };
            if (greske.TryGetValue("Prezime", out string? errPrezime)) { lblPrezimeError.Text = errPrezime; lblPrezimeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("KorisnickoIme", out string? errKorIme)) { lblKorisnickoImeError.Text = errKorIme; lblKorisnickoImeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Email", out string? errEmail)) { lblEmailError.Text = errEmail; lblEmailError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("JMBG", out string? errJmbg)) { lblJMBGError.Text = errJmbg; lblJMBGError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("DatumRodjenja", out string? errDatum)) { lblDatumRodjenjaError.Text = errDatum; lblDatumRodjenjaError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Lozinka", out string? errLozinka)) { lblLozinkaError.Text = errLozinka; lblLozinkaError.Visibility = Visibility.Visible; }

            if (cmbStudij.SelectedItem == null
                || cmbGodinaStudija.SelectedItem == null)
                return false;
            if (greske.Count == 0)
                return true;
            return false;
        }
    }
}

