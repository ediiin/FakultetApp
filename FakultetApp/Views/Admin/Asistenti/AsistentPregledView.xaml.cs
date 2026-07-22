using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FakultetApp.Views.Admin.Asistenti
{
    /// <summary>
    /// Interaction logic for AsistentPregledView.xaml
    /// </summary>
    public partial class AsistentPregledView : UserControl
    {
        private readonly AsistentServis _asistentServis;
        private readonly GradServis _gradServis;
        private readonly SpolServis _spolServis;
        private List<Asistent> _asistenti = new List<Asistent>();
        public AsistentPregledView(
            AsistentServis asistentServis,
            GradServis gradServis,
            SpolServis spolServis)
        {
            InitializeComponent();
            _asistentServis = asistentServis;
            _gradServis = gradServis;
            _spolServis = spolServis;

            UcitajAsistente();
        }
        private void UcitajAsistente()
        {
            _asistenti = _asistentServis.GetAll();
            dgvAsistenti.ItemsSource = _asistenti;
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tekstPretrage = txtFilter.Text;
            _asistenti = _asistentServis.Filter(tekstPretrage);
            dgvAsistenti.ItemsSource = _asistenti;
        }

        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            var izabraniAsistent = dgvAsistenti.SelectedItem as Asistent;
            if (izabraniAsistent != null &&
                MessageBox.Show($"Jeste li sigurni da želite ukloniti asistenta {izabraniAsistent.ImePrezime} iz sistema?"
                , "Upit", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                _asistentServis.Deaktiviraj(izabraniAsistent.Id);
                MessageBox.Show("Asistent uspješno uklonjen!", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
                UcitajAsistente();
            }
        }

        private void dgvAsistenti_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var izabraniAsistent = dgvAsistenti.SelectedItem as Asistent;
            if (izabraniAsistent != null)
            {
                var editView = new AsistentiEditView(
                    izabraniAsistent,
                    _asistentServis,
                    _gradServis,
                    _spolServis
                    );

                var mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow != null &&
                    mainWindow.GlavniSadrzajAplikacije.Content is AdminDashboardView dashboard &&
                    dashboard.PrikaznikSadrzaja.Content is UpravljanjeAsistentimaView upravljanje)
                {
                    upravljanje.AsistentiSadrzaj.Content = editView;
                }
            }
        }
    }
}
