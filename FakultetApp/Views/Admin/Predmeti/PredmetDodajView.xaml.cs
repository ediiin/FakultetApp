using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.Predmeti
{
    public partial class PredmetDodajView : UserControl
    {
        private readonly PredmetServis _predmetServis;
        private readonly GodinaStudijaServis _godineStudijaServis;
        private readonly StudijServis _studijServis;
        private readonly ProfesorServis _profesorServis;

        private readonly AsistentServis _asistentServis;
        private readonly AsistentPredmetServis _asistentPredmetServis;

        public PredmetDodajView(PredmetServis predmetServis,
            GodinaStudijaServis godineStudijaServis,
            StudijServis studijServis,
            ProfesorServis profesorServis,
            AsistentServis asistentServis,
            AsistentPredmetServis asistentPredmetServis)
        {
            InitializeComponent();
            _predmetServis = predmetServis;
            _godineStudijaServis = godineStudijaServis;
            _studijServis = studijServis;
            _profesorServis = profesorServis;
            _asistentServis = asistentServis;
            _asistentPredmetServis = asistentPredmetServis;

            UcitajStudije();
            UcitajProfesore();
            UcitajAsistente(); 
        }

        private void UcitajStudije()
        {
            cmbStudij.ItemsSource = _studijServis.GetAll();
            cmbStudij.DisplayMemberPath = "PuniNaziv";
            if (cmbStudij.Items.Count > 0) cmbStudij.SelectedIndex = 0;
        }

        private void UcitajProfesore()
        {
            var profesori = _profesorServis.GetAll();
            cmbProfesor.ItemsSource = profesori;
            cmbProfesor.DisplayMemberPath = "ImePrezime"; 
            if (cmbProfesor.Items.Count > 0) cmbProfesor.SelectedIndex = 0;
        }

        private void UcitajAsistente()
        {
            var asistenti = _asistentServis.GetAll();
            cmbAsistent.ItemsSource = asistenti;
            cmbAsistent.DisplayMemberPath = "ImePrezime"; 
            if (cmbAsistent.Items.Count > 0) cmbAsistent.SelectedIndex = 0;
        }

        private void CmbStudij_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStudij.SelectedItem is Studij odabraniStudij)
            {
                var pripadajuceGodine = _godineStudijaServis.GetAllByStudijId(odabraniStudij.Id);
                cmbGodinaStudija.ItemsSource = pripadajuceGodine;
                cmbGodinaStudija.DisplayMemberPath = "Opis";
                cmbGodinaStudija.IsEnabled = true;

                if (pripadajuceGodine.Any())
                    cmbGodinaStudija.SelectedIndex = 0;
            }
            else
            {
                cmbGodinaStudija.ItemsSource = null;
                cmbGodinaStudija.IsEnabled = false;
            }
        }

        private void BtnSpasi_Click(object sender, RoutedEventArgs e)
        {
            if (!Validno())
                return;

            var godinaStudija = cmbGodinaStudija.SelectedItem as GodinaStudija;
            var profesor = cmbProfesor.SelectedItem as Profesor;
            var asistent = cmbAsistent.SelectedItem as Asistent;
            int.TryParse(tbEcts.Text, out int ects);

            var noviPredmet = new Predmet()
            {
                Naziv = tbNaziv.Text.Trim(),
                ECTS = ects,
                GodinaStudijaId = godinaStudija!.Id,
                ProfesorId = profesor!.Id
            };

            _predmetServis.Add(noviPredmet);

            //asistentPredet
            if (asistent != null && noviPredmet.Id > 0)
            {
                var asistentPredmet = new AsistentPredmet
                {
                    PredmetId = noviPredmet.Id,
                    AsistentId = asistent.Id
                };

                _asistentPredmetServis.Add(asistentPredmet);
            }

            MessageBox.Show($"Uspješno kreiran predmet '{noviPredmet.Naziv}' i dodijeljeni profesor i asistent!",
                "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);

            OcistiSvaPolja();
        }

        private void OcistiSvaPolja()
        {
            tbNaziv.Clear();
            tbEcts.Clear();
            if (cmbStudij.Items.Count > 0) cmbStudij.SelectedIndex = 0;
            cmbGodinaStudija.ItemsSource = null;
            cmbGodinaStudija.IsEnabled = false;
            if (cmbProfesor.Items.Count > 0) cmbProfesor.SelectedIndex = 0;
            if (cmbAsistent.Items.Count > 0) cmbAsistent.SelectedIndex = 0;
            OcistiGreske();
        }

        private bool Validno()
        {
            OcistiGreske();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(tbNaziv.Text))
            {
                lblNazivError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(tbEcts.Text) || !int.TryParse(tbEcts.Text, out int ects) || ects <= 0)
            {
                lblEctsError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (cmbStudij.SelectedItem == null)
            {
                lblStudijError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (cmbGodinaStudija.SelectedItem == null)
            {
                lblGodinaStudijaError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (cmbProfesor.SelectedItem == null)
            {
                lblProfesorError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (cmbAsistent.SelectedItem == null)
            {
                lblAsistentError.Visibility = Visibility.Visible;
                isValid = false;
            }

            return isValid;
        }

        private void OcistiGreske()
        {
            lblNazivError.Visibility = Visibility.Hidden;
            lblEctsError.Visibility = Visibility.Hidden;
            lblStudijError.Visibility = Visibility.Hidden;
            lblGodinaStudijaError.Visibility = Visibility.Hidden;
            lblProfesorError.Visibility = Visibility.Hidden;
            lblAsistentError.Visibility = Visibility.Hidden;
        }
    }
}