using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.ProfesorViews
{
    /// <summary>
    /// Interaction logic for ProfesorIspitView.xaml
    /// </summary>
    public partial class ProfesorIspitView : UserControl
    {
        private readonly Profesor _profesor;
        private readonly IspitServis _ispitServis;
        private readonly PredmetServis _predmetServis;

        public ProfesorIspitView(Profesor profesor, IspitServis ispitServis, PredmetServis predmetServis)
        {
            InitializeComponent();
            _profesor = profesor;
            _ispitServis = ispitServis;
            _predmetServis = predmetServis;

            UcitajPredmete();
        }

        private void UcitajPredmete()
        {
            var profesoriPredmeti = _predmetServis.GetPredmetiByProfesor(_profesor.Id);

            cmbMojiPredmeti.ItemsSource = profesoriPredmeti;
            if (profesoriPredmeti.Any())
            {
                cmbMojiPredmeti.SelectedIndex = 0;
            }
        }

        private void BtnZakaziIspit_Click(object sender, RoutedEventArgs e)
        {
            if (!Validno())
                return;

            var odabraniPredmet = (Predmet)cmbMojiPredmeti.SelectedItem;

            DateTime odabraniDatum = dtpDatumOdrzavanja.SelectedDate!.Value;
            TimeSpan odabranoVrijeme = TimeSpan.Parse(tbVrijeme.Text);
            DateTime tacanTermin = odabraniDatum.Add(odabranoVrijeme);

            var noviIspit = new Ispit
            {
                PredmetId = odabraniPredmet.Id,
                DatumOdrzavanja = tacanTermin,
                BrojZadataka = int.Parse(tbBrojZadataka.Text),
                MaxBrojBodova = int.Parse(tbMaxBodova.Text)
            };

            _ispitServis.Add(noviIspit);

            MessageBox.Show($"Uspješno ste zakazali ispit za predmet: {odabraniPredmet.Naziv}\nTermin: {tacanTermin:dd.MM.yyyy HH:mm}",
                            "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);

            OcistiPolja();
        }

        private bool Validno()
        {
            OcistiGreske();
            bool isValid = true;

            if (cmbMojiPredmeti.SelectedItem == null)
            {
                lblPredmetError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (dtpDatumOdrzavanja.SelectedDate == null)
            {
                lblDatumError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (dtpDatumOdrzavanja.SelectedDate < DateTime.Now)
            {
                lblDatumError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (!TimeSpan.TryParse(tbVrijeme.Text, out _))
            {
                lblVrijemeError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (!int.TryParse(tbBrojZadataka.Text, out int brZadataka) || brZadataka <= 0)
            {
                lblBrojZadatakaError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (!int.TryParse(tbMaxBodova.Text, out int maxBodova) || maxBodova <= 0)
            {
                lblMaxBodovaError.Visibility = Visibility.Visible;
                isValid = false;
            }

            return isValid;
        }

        private void OcistiGreske()
        {
            lblPredmetError.Visibility = Visibility.Hidden;
            lblDatumError.Visibility = Visibility.Hidden;
            lblVrijemeError.Visibility = Visibility.Hidden;
            lblBrojZadatakaError.Visibility = Visibility.Hidden;
            lblMaxBodovaError.Visibility = Visibility.Hidden;
        }

        private void OcistiPolja()
        {
            if (cmbMojiPredmeti.Items.Count > 0)
                cmbMojiPredmeti.SelectedIndex = 0;

            dtpDatumOdrzavanja.SelectedDate = null;
            tbVrijeme.Text = "10:00";
            tbBrojZadataka.Clear();
            tbMaxBodova.Clear();
            OcistiGreske();
        }
    }
}
