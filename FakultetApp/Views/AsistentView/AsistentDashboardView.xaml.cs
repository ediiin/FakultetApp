using Fakultet.Core.Modeli;
using FakultetApp.Login;
using FakultetApp.Views.AsistentView;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.AsistentViews
{
    public partial class AsistentDashboardView : UserControl
    {
        private readonly Asistent _prijavljeniAsistent;

        public AsistentDashboardView(Asistent asistent)
        {
            InitializeComponent();
            _prijavljeniAsistent = asistent;

            txtImeAsistenta.Text = $"Asst. {_prijavljeniAsistent.ImePrezime}";
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

                    case nameof(btnPredmeti):
                        PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<AsistentPredmetiView>(App.ServiceProvider!, _prijavljeniAsistent);
                        break;

                    case nameof(btnObjaveMaterijali):
                        PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<AsistentObjaveMaterijaliView>(App.ServiceProvider!, _prijavljeniAsistent);
                        break;

                    case nameof(btnChat):
                        PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<AsistentChatView>(App.ServiceProvider!, _prijavljeniAsistent);
                        break;

                    case nameof(btnOdjava):
                        OdjaviSe();
                        break;
                }
            }
        }

        private void UcitajPocetnu()
        {
            PrikaznikSadrzaja.Content = ActivatorUtilities.CreateInstance<AsistentPocetnaView>(App.ServiceProvider!, _prijavljeniAsistent);
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