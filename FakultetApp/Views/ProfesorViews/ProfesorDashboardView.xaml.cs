using Fakultet.Core.Modeli;
using FakultetApp.Login;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.ProfesorViews
{
    /// <summary>
    /// Interaction logic for ProfesorDashboardView.xaml
    /// </summary>
    public partial class ProfesorDashboardView : UserControl
    {
        private readonly Profesor _prijavljeniProfesor;

        public ProfesorDashboardView(Profesor profesor)
        {
            InitializeComponent();
            _prijavljeniProfesor = profesor;

            txtImeProfesora.Text = $"Prof. {_prijavljeniProfesor.ImePrezime}";
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

                    case nameof(btnMojiPredmeti):
                        PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<ProfesorPredmetiView>(App.ServiceProvider!, _prijavljeniProfesor);
                        break;

                    case nameof(btnObjaveMaterijali):
                        PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<ProfesorObjaveMaterijaliView>(App.ServiceProvider!, _prijavljeniProfesor);
                        break;

                    case nameof(btnIspitniRokovi):
                        PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<ProfesorIspitView>(App.ServiceProvider!, _prijavljeniProfesor);
                        break;

                    case nameof(btnChat):
                        PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<ProfesoriChatView>(App.ServiceProvider!, _prijavljeniProfesor);
                        break;

                    case nameof(btnOdjava):
                        OdjaviSe();
                        break;
                }
            }
        }

        private void UcitajPocetnu()
        {
            // PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<ProfesoriPocetnaView>(App.ServiceProvider!, _prijavljeniProfesor);
        }

        private void OdjaviSe()
        {
            var upit = MessageBox.Show("Jeste li sigurni da se želite odjaviti?",
                "Odjava", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (upit == MessageBoxResult.Yes)
            {
                var login = App.ServiceProvider!.GetRequiredService<LoginProzor>();
                login.Show();
                Window.GetWindow(this).Close();
            }
        }
    }
}
