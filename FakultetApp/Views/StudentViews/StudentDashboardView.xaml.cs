using Fakultet.Core.Modeli;
using FakultetApp.Login;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.StudentViews
{
    /// <summary>
    /// Interaction logic for StudentDashboardView.xaml
    /// </summary>
    public partial class StudentDashboardView : UserControl
    {
        private readonly Student _prijavljeniStudent;

        public StudentDashboardView(Student student)
        {
            InitializeComponent();
            _prijavljeniStudent = student;

            UcitajPocetnu();
        }

        private void Nav_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                switch (element.Name)
                {
                    case nameof(btnPocetna):
                        UcitajPocetnu();
                        break;
                    case nameof(btnObjave):
                        // PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<StudentiObjaveView>(App.ServiceProvider!, _prijavljeniStudent);
                        break;
                    case nameof(btnDokumenti):
                        // PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<StudentiDokumentiView>(App.ServiceProvider!, _prijavljeniStudent);
                        break;
                    case nameof(btnPrijavaIspita):
                        // PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<StudentiPrijavaIspitaView>(App.ServiceProvider!, _prijavljeniStudent);
                        break;
                    case nameof(btnZahtjevPotvrda):
                        // PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<StudentiZahtjevPotvrdaView>(App.ServiceProvider!, _prijavljeniStudent);
                        break;
                    case nameof(btnChat):
                        // PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<StudentiChatView>(App.ServiceProvider!, _prijavljeniStudent);
                        break;
                    case nameof(btnLicniPodaci):
                        // PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<StudentiLicniPodaciView>(App.ServiceProvider!, _prijavljeniStudent);
                        break;
                    case nameof(btnOdjava):
                        OdjaviSe();
                        break;
                }
            }
        }

        private void UcitajPocetnu()
        {
            PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<StudentiPocetnaView>(App.ServiceProvider!, _prijavljeniStudent);
        }

        private void OdjaviSe()
        {
            var upit = MessageBox.Show("Jeste li sigurni da se želite odjaviti?",
                "Upit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (upit == MessageBoxResult.Yes)
            {
                var login = App.ServiceProvider!.GetRequiredService<LoginProzor>();
                login.Show();

                Window.GetWindow(this).Close();
            }
        }
    }
}