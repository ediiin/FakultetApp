using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FakultetApp.Views.Admin.ProfesoriLogika
{
    /// <summary>
    /// Interaction logic for ProfesoriPregledView.xaml
    /// </summary>
    public partial class ProfesoriPregledView : UserControl
    {
        private readonly ProfesorServis _profesorServis;
        private readonly GradServis _gradServis;
        private readonly SpolServis _spolServis;
        private List<Profesor> _profesori = new List<Profesor>();
        public ProfesoriPregledView(
            ProfesorServis profesorServis,
            GradServis gradServis,
            SpolServis spolServis)
        { 
            InitializeComponent();
            _profesorServis = profesorServis;
            _gradServis = gradServis;
            _spolServis = spolServis;
            UcitajProfesore();
        }

        private void UcitajProfesore()
        {
            _profesori = _profesorServis.GetAll();
            dgvProfesori.ItemsSource = _profesori;
        }

        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            var izabraniProfesor = dgvProfesori.SelectedItem as Profesor;
            if(izabraniProfesor != null &&
                MessageBox.Show($"Jeste li sigurni da želite raskinuti ugovor sa {izabraniProfesor.ImePrezime}?"
                    , "Upit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _profesorServis.Deaktiviraj(izabraniProfesor.Id); 
                MessageBox.Show("Uspješno raskinut ugovor", "Uspjeh", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                UcitajProfesore();
            }
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tekstPretrage = txtFilter.Text;
            _profesori = _profesorServis.Filter(tekstPretrage);
            dgvProfesori.ItemsSource = _profesori;
        }

        private void dgvProfesori_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var izabraniprofesor = dgvProfesori.SelectedItem as Profesor;
            if (izabraniprofesor != null)
            {
                var editView = new ProfesoriEditView(
                    izabraniprofesor,
                    _profesorServis,
                    _gradServis,
                    _spolServis
                    );

                var mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow != null &&
                    mainWindow.GlavniSadrzajAplikacije.Content is AdminDashboardView dashboard &&
                    dashboard.PrikaznikSadrzaja.Content is UpravljanjeProfesorimaView upravljanje)
                {
                    upravljanje.ProfesoriSadrzaj.Content = editView;
                }
            }
        }
    }
}
