using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using FakultetApp.Views.Admin.Studenti;
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

namespace FakultetApp.Views.Admin
{
    /// <summary>
    /// Interaction logic for StudentiPregledView.xaml
    /// </summary>
    public partial class StudentiPregledView : UserControl
    {
        private readonly StudentServis _studentServis;
        private readonly StudijServis _studijServis;
        private readonly GodinaStudijaServis _godineStudijaServis;
        private readonly GradServis _gradServis;
        private readonly SpolServis _spolServis;

        private List<Student> _studenti = new List<Student>();

        // Ubrizgavamo sve potrebne servise ovdje
        public StudentiPregledView(
            StudentServis studentServis,
            StudijServis studijServis,
            GodinaStudijaServis godineStudijaServis,
            GradServis gradServis,
            SpolServis spolServis)
        {
            InitializeComponent();

            _studentServis = studentServis;
            _studijServis = studijServis;
            _godineStudijaServis = godineStudijaServis;
            _gradServis = gradServis;
            _spolServis = spolServis;

            UcitajStudente();
        }

        private void BtnIspis_Click(object sender, RoutedEventArgs e)
        {
            var izabraniStudent = dgvStudenti.SelectedItem as Student;
            if (izabraniStudent != null &&
                MessageBox.Show($"Jeste li sigurni da želite ispisati studenta {izabraniStudent.ToString()}?"
                , "Upit", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {

                _studentServis.Deaktiviraj(izabraniStudent.Id);
                MessageBox.Show($"Student uspješno ispisan!",
                    "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                UcitajStudente();
            }
        }

        private void UcitajStudente()
        {
            _studenti = _studentServis.GetAll();
            dgvStudenti.ItemsSource = _studenti;
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string indeksImePrezime = txtFilterImePrezimeIndeks.Text.ToLower();
            _studenti = _studentServis.Filter(indeksImePrezime);
            dgvStudenti.ItemsSource = _studenti;
        }

        private void dgvStudenti_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var izabraniStudent = dgvStudenti.SelectedItem as Student;
            if (izabraniStudent != null)
            {
                var editView = new StudentiEditView(
                    izabraniStudent,
                    _studentServis,
                    _studijServis,
                    _godineStudijaServis,
                    _gradServis,
                    _spolServis);

                var mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow != null &&
                    mainWindow.GlavniSadrzajAplikacije.Content is AdminDashboardView dashboard &&
                    dashboard.PrikaznikSadrzaja.Content is UpravljanjeStudentimaView upravljanje)
                {
                    upravljanje.StudentiSadrzaj.Content = editView;
                }
            }
        }
    }
}
