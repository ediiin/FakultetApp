using Fakultet.Servisi.Bazni;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using Fakultet.Servisi.Setup;
using FakultetApp.Login;
using FakultetApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace FakultetApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Dodali smo "static" da mu mozemo pristupiti iz cijele aplikacije
        public static IServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                var services = new ServiceCollection();

                services.AddDbContext<FakultetAppDbContext>();
                services.RegistrujServise();
                services.RegistrujViewove();

                ServiceProvider = services.BuildServiceProvider();

                // Ovdje se dešava loading (konekcija na bazu, sejdovanje...)
                var seeder = ServiceProvider.GetRequiredService<DataSeedServis>();
                seeder.SeedujSve();

                var loginProzor = ServiceProvider.GetRequiredService<LoginProzor>();
                loginProzor.Show();
            }
            catch (Exception ex)
            {
                // Ova poruka će nam reći TAČNO šta je problem!
                MessageBox.Show($"Aplikacija je pukla na startu!\n\n" +
                                $"Greška: {ex.Message}\n\n" +
                                $"Unutrašnja greška: {ex.InnerException?.Message}\n\n" +
                                $"Lokacija: {ex.StackTrace}",
                                "DIAGNOSTIC CRASH REPORT",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                // Gasimo aplikaciju jer ne može nastaviti sa radom
                Current.Shutdown();
            }
        }
    }
}
