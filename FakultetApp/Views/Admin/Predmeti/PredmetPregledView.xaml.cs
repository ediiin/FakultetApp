using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using FakultetApp.Views.Admin.Predmeti;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.Predmeti
{
    /// <summary>
    /// Interaction logic for PredmetPregledView.xaml
    /// </summary>
    public partial class PredmetPregledView : UserControl
    {
        private readonly PredmetServis _predmetServis;
        private readonly ProfesorServis _profesorServis;
        private readonly GodinaStudijaServis _godinaStudijaServis;
        private readonly AsistentServis _asistentServis;
        private readonly AsistentPredmetServis _asistentPredmetServis;
        private List<Predmet> _predmeti = new List<Predmet>();
        public PredmetPregledView(
            PredmetServis predmetServis,
            ProfesorServis profesorServis,
            GodinaStudijaServis godinaStudijaServis,
            AsistentServis asistentServis,
            AsistentPredmetServis asistentPredmetServis
            )
        {
            InitializeComponent();
            _predmetServis = predmetServis;
            _profesorServis = profesorServis;
            _godinaStudijaServis = godinaStudijaServis;
            _asistentServis = asistentServis;
            _asistentPredmetServis = asistentPredmetServis;

            UcitajPredmete();
        }

        private void UcitajPredmete()
        {
            _predmeti = _predmetServis.GetAll();
            dgvPredmeti.ItemsSource = _predmeti;
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tekstPretrage = txtFilter.Text;
            _predmeti = _predmetServis.Filter(tekstPretrage);
            dgvPredmeti.ItemsSource = _predmeti;
        }

        private void dgvPredmeti_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var izabraniPredmet = dgvPredmeti.SelectedItem as Predmet;
            if (izabraniPredmet != null)
            {
                var editView = new PredmetEditView(
                    izabraniPredmet,
                    _profesorServis,
                    _godinaStudijaServis,
                    _predmetServis,
                    _asistentServis,
                    _asistentPredmetServis);

                var mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow != null &&
                    mainWindow.GlavniSadrzajAplikacije.Content is AdminDashboardView dashboard &&
                    dashboard.PrikaznikSadrzaja.Content is UpravljanjePredmetimaView upravljanje)
                {
                    upravljanje.PredmetiSadrzaj.Content = editView;
                }
            }
        }
    }
}
