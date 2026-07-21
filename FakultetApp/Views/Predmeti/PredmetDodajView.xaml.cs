using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
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

namespace FakultetApp.Views.Predmeti
{
    /// <summary>
    /// Interaction logic for PredmetDodajView.xaml
    /// </summary>
    public partial class PredmetDodajView : UserControl
    {
        private readonly PredmetServis _predmetServis;
        private readonly GodinaStudijaServis _godineStudijaServis;
        private readonly StudijServis _studijServis;
        private readonly ProfesorServis _profesorServis;
        public PredmetDodajView(PredmetServis predmetServis,
            GodinaStudijaServis godineStudijaServis,
            StudijServis studijServis,
            ProfesorServis profesorServis)
        {
            InitializeComponent();
            _predmetServis = predmetServis;
            _godineStudijaServis = godineStudijaServis;
            _studijServis = studijServis;
            _profesorServis = profesorServis;

            UcitajStudije();
            UcitajProfesore();
        }

        private void UcitajStudije()
        {
            cmbStudij.ItemsSource = _studijServis.GetAll();
            cmbStudij.DisplayMemberPath = "PuniNaziv";
            cmbStudij.SelectedIndex = 0;
        }

        private void UcitajProfesore()
        {
            var profesori = _profesorServis.GetAll();
            cmbProfesor.ItemsSource = profesori;
            cmbProfesor.DisplayMemberPath = "ImePrezime";
            cmbProfesor.SelectedIndex = 0;
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
            int.TryParse(tbEcts.Text, out int ects);

            var noviPredmet = new Predmet()
            {
                Naziv = tbNaziv.Text.Trim(),
                ECTS = ects,
                GodinaStudijaId = godinaStudija!.Id,
                ProfesorId = profesor!.Id
            };

            _predmetServis.Add(noviPredmet);

            MessageBox.Show($"Uspješno kreiran predmet '{noviPredmet.Naziv}' i dodijeljen profesoru!",
                "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);

            OcistiSvaPolja();
        }

        private void OcistiSvaPolja()
        {
            tbNaziv.Clear();
            tbEcts.Clear();
            cmbStudij.SelectedIndex = -1;
            cmbGodinaStudija.ItemsSource = null;
            cmbGodinaStudija.IsEnabled = false;
            cmbProfesor.SelectedIndex = -1;
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

            return isValid;
        }

        private void OcistiGreske()
        {
            lblNazivError.Visibility = Visibility.Hidden;
            lblEctsError.Visibility = Visibility.Hidden;
            lblStudijError.Visibility = Visibility.Hidden;
            lblGodinaStudijaError.Visibility = Visibility.Hidden;
            lblProfesorError.Visibility = Visibility.Hidden;
        }
    }
}
