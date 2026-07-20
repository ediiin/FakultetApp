using FakultetApp.Login;
using FakultetApp.Views.Admin;
using FakultetApp.Views.Admin.Asistenti;
using FakultetApp.Views.Admin.ProfesoriLogika;
using FakultetApp.Views.Predmeti;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views
{
    /// <summary>
    /// Interaction logic for AdminDashboardView.xaml
    /// </summary>
    public partial class AdminDashboardView : UserControl
    {
        public AdminDashboardView()
        {
            InitializeComponent();
        }

        private void Nav_Click(object sender, RoutedEventArgs e)
        {
            Button? kliknutoDugme = sender as Button;

            if (kliknutoDugme == null) return;

            txtNaslovSekcije.Text = kliknutoDugme.Content.ToString();

            switch (kliknutoDugme.Name)
            {
                case "btnStudenti":
                    PrikaznikSadrzaja.Content = App.ServiceProvider.GetRequiredService<UpravljanjeStudentimaView>();
                    break;
                case "btnProfesori":
                    PrikaznikSadrzaja.Content = App.ServiceProvider.GetRequiredService<UpravljanjeProfesorimaView>();
                    break;
                case "btnPredmeti":
                    PrikaznikSadrzaja.Content = App.ServiceProvider.GetRequiredService<UpravljanjePredmetimaView>();
                    break;
                case "btnAsistenti":
                    PrikaznikSadrzaja.Content = App.ServiceProvider.GetRequiredService<UpravljanjeAsistentimaView>();
                    break;
                case "btnNoviAdmin":
                    PrikaznikSadrzaja.Content = App.ServiceProvider.GetRequiredService<NoviAdminView>();
                    break;
                case "btnOdjava":
                    OdjavaLogika();
                    break;
            }
        }

        private void OdjavaLogika()
        {
            var upit = MessageBox.Show("Jeste li sigurni da se zelite odjaviti?",
                "Upit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(upit == MessageBoxResult.Yes)
            {
                var login = App.ServiceProvider.GetRequiredService<LoginProzor>();
                login.Show();

                Window.GetWindow(this).Close();
            }
        }
    }
}
