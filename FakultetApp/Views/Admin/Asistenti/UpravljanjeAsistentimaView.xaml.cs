using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.Admin.Asistenti
{
    /// <summary>
    /// Interaction logic for UpravljanjeAsistentimaView.xaml
    /// </summary>
    public partial class UpravljanjeAsistentimaView : UserControl
    {
        public UpravljanjeAsistentimaView()
        {
            InitializeComponent();
            AsistentiSadrzaj.Content = App.ServiceProvider.GetRequiredService<AsistentPregledView>();
        }

        private void AsistentNav_Click(object sender, RoutedEventArgs e)
        {
            Button? dugme = sender as Button;
            if (dugme == null) return;

            if (dugme.Name == "btnPrikaziSve")
            {
                AsistentiSadrzaj.Content = App.ServiceProvider.GetRequiredService<AsistentPregledView>();
            }
            else if (dugme.Name == "btnDodajAsistenta")
            {
                AsistentiSadrzaj.Content = App.ServiceProvider.GetRequiredService<AsistentDodajView>();
            }
        }
    }
}
