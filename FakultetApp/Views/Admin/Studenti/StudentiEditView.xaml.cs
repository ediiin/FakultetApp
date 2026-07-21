using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            // cmbStatus.ItemsSource = Enum.GetValues(typeof(Status));
            UcitajSpol();
            UcitajGrad();
            UcitajStudij();

            // 2. TEKSTUALNA POLJA
            tbIme.Text = _student.Ime;
            tbPrezime.Text = _student.Prezime;
            tbKorisnickoIme.Text = _student.KorisnickoIme;
            tbEmail.Text = _student.Email;

            // Polja zabranjena za izmjenu
            tbJMBG.Text = _student.JMBG;
            dtpDatumRodjenja.SelectedDate = _student.DatumRodjenja;
            tbIndeks.Text = _student.Indeks;
            dtpDatumUpisa.SelectedDate = _student.DatumUpisa;

            // 3. SELEKTOVANJE VRIJEDNOSTI (Poveži ID-eve iz objekta sa ComboBox-om)
            // cmbSpol.SelectedValue = _student.SpolId;
            // cmbGrad.SelectedValue = _student.GradId;
            // cmbGodinaStudija.SelectedValue = _student.GodinaStudijaId;
            // cmbStatus.SelectedItem = _student.Status;
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
            // 1. OSNOVNA VALIDACIJA (Možeš dodati metode za prikaz lbl errora kao u dodavanju)
            if (string.IsNullOrWhiteSpace(tbIme.Text) || string.IsNullOrWhiteSpace(tbPrezime.Text))
            {
                MessageBox.Show("Ime i prezime su obavezni!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. AŽURIRANJE PODATAKA
            _student.Ime = tbIme.Text;
            _student.Prezime = tbPrezime.Text;
            _student.KorisnickoIme = tbKorisnickoIme.Text;
            _student.Email = tbEmail.Text;

            // _student.GodinaStudijaId = (int)cmbGodinaStudija.SelectedValue;
            // _student.Status = (Status)cmbStatus.SelectedItem;
            // ... (Ažuriraj ostale editabilne dropdowne)

            // 3. PROMJENA LOZINKE (samo ako je nešto upisano)
            if (!string.IsNullOrWhiteSpace(pbLozinka.Password))
            {
                // Primijeni svoju metodu za kriptovanje (npr. HashHelper)
                // _student.LozinkaHash = MojHashHelper.NapraviHash(pbLozinka.Password); 
            }

            try
            {
                // 4. SPAŠAVANJE KROZ SERVIS
                // Pretpostavljam da imaš metodu Update(id, objekat)
                // _studentServis.Update(_student.Id, _student);

                MessageBox.Show("Podaci o studentu uspješno ažurirani!", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);

                // 5. POVRATAK NAZAD
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    // Vraćanje na view za upravljanje studentima
                    // Napomena: Ukoliko UpravljanjeStudentima prima neke servise u konstruktoru, dodaj ih ovdje
                    // mainWindow.GlavniSadrzajAplikacije.Content = new UpravljanjeStudentimaView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

