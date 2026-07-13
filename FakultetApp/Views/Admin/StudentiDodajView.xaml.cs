using Fakultet.Core.Modeli;
using Fakultet.Servisi.Helperi;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.Admin
{
    /// <summary>
    /// Interaction logic for StudentiDodajView.xaml
    /// </summary>
    public partial class StudentiDodajView : UserControl
    {
        private readonly StudentServis _studentServis;
        private readonly GodinaStudijaServis _godineStudijaServis;
        private readonly StudijServis _studijServis;
        private readonly GradServis _gradServis;
        private readonly SpolServis _spolServis;
        public StudentiDodajView(StudentServis studentServis,
            GodinaStudijaServis godineStudijaServis,
            StudijServis studijServis,
            GradServis gradServis,
            SpolServis spolServis)
        {
            InitializeComponent();
            _studentServis = studentServis;
            _godineStudijaServis = godineStudijaServis;
            _studijServis = studijServis;
            _gradServis = gradServis;
            _spolServis = spolServis;

            UcitajSpolove();
            UcitajGradove();
            UcitajStudij();

            cmbStatus.ItemsSource = Enum.GetValues(typeof(Status));
            cmbStatus.SelectedIndex = 0;
            cmbStudij.SelectedIndex = 0;
            cmbSpol.SelectedIndex = 0;
            cmbGrad.SelectedIndex = 0;

            tbIndeks.Text = _studentServis.GenerisiIndeks();
            dtpDatumUpisa.SelectedDate = DateTime.Now;
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

        private void UcitajStudij()
        {
            cmbStudij.ItemsSource = _studijServis.GetAll();
            cmbStudij.DisplayMemberPath = "PuniNaziv";
        }

        private void BtnSpasi_Click(object sender, RoutedEventArgs e)
        {
            if(!Validno())
                return;

            var grad = cmbGrad.SelectedItem as Grad;
            var spol = cmbSpol.SelectedItem as Spol;
            var godinaStudija = cmbGodinaStudija.SelectedItem as GodinaStudija;
            var noviStudent = new Student()
            {
                Ime = tbIme.Text,
                Prezime = tbPrezime.Text,
                KorisnickoIme = tbKorisnickoIme.Text,
                Email = tbEmail.Text,
                DatumRodjenja = dtpDatumRodjenja.SelectedDate!.Value,
                DatumUpisa = dtpDatumUpisa.SelectedDate!.Value,
                JMBG = tbJMBG.Text,
                ZavrsioFakultet = false,
                Uloge = Uloge.Student,
                SpolId = spol!.Id,
                GradId = grad!.Id,
                GodinaStudijaId = godinaStudija!.Id,
                Indeks = tbIndeks.Text,
                LozinkaHash = BCrypt.Net.BCrypt.HashPassword(pbLozinka.Password),
                Status = (Status)cmbStatus.SelectedItem
            };
            _studentServis.Add(noviStudent);
            MessageBox.Show($"Uspješno dodan student {noviStudent.ToString()}",
                "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
            cmbStatus.SelectedIndex = 0;
            cmbStudij.SelectedIndex = 0;
            dtpDatumUpisa.SelectedDate = DateTime.Now;
            cmbGodinaStudija.ItemsSource = null;
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

            if (cmbStudij.SelectedItem == null)
                lblStudijError.Visibility = Visibility.Visible;

            if (cmbGodinaStudija.SelectedItem == null)
                lblGodinaStudijaError.Visibility = Visibility.Visible;

            if (greske.Count == 0)
                return true;

            if (greske.TryGetValue("Ime", out string? errIme)) { lblImeError.Text = errIme; lblImeError.Visibility = Visibility.Visible; };
            if (greske.TryGetValue("Prezime", out string? errPrezime)) { lblPrezimeError.Text = errPrezime; lblPrezimeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("KorisnickoIme", out string? errKorIme)) { lblKorisnickoImeError.Text = errKorIme; lblKorisnickoImeError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Email", out string? errEmail)) { lblEmailError.Text = errEmail; lblEmailError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("JMBG", out string? errJmbg)) { lblJMBGError.Text = errJmbg; lblJMBGError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("DatumRodjenja", out string? errDatum)) { lblDatumRodjenjaError.Text = errDatum; lblDatumRodjenjaError.Visibility = Visibility.Visible; }
            if (greske.TryGetValue("Lozinka", out string? errLozinka)) { lblLozinkaError.Text = errLozinka; lblLozinkaError.Visibility = Visibility.Visible; }

            if(cmbStudij.SelectedItem == null
                || cmbGodinaStudija.SelectedItem == null)
            return false;
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
            lblStudijError.Visibility = Visibility.Hidden;
            lblGodinaStudijaError.Visibility = Visibility.Hidden;
        }

        private void CmbStudij_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStudij.SelectedItem is Studij odabraniStudij)
            {
                var pripadajuceGodine = _godineStudijaServis
                    .GetAllByStudijId(odabraniStudij.Id);
                                        
                cmbGodinaStudija.ItemsSource = pripadajuceGodine;
                cmbGodinaStudija.DisplayMemberPath = "Opis"; 

                cmbGodinaStudija.IsEnabled = true;

                //automatski izaberemo prvu ali se moze promjenuti naravno
                if (pripadajuceGodine.Any())
                    cmbGodinaStudija.SelectedIndex = 0;
            }
            else
            {
                cmbGodinaStudija.ItemsSource = null;
                cmbGodinaStudija.IsEnabled = false;
            }
        }
    }
}
