using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.Admin
{
    /// <summary>
    /// Interaction logic for UpravljanjeStudentimaView.xaml
    /// </summary>
    public partial class UpravljanjeStudentimaView : UserControl
    {
        public UpravljanjeStudentimaView()
        {
            InitializeComponent();
            StudentiSadrzaj.Content = App.ServiceProvider.GetRequiredService<StudentiPregledView>();
        }

        private void StudentNav_Click(object sender, RoutedEventArgs e)
        {
            Button? dugme = sender as Button;
            if (dugme == null) return;

            if (dugme.Name == "btnPrikažiSve")
            {
                StudentiSadrzaj.Content = App.ServiceProvider.GetRequiredService<StudentiPregledView>();
            }
            else if (dugme.Name == "btnDodajStudenta")
            {
                StudentiSadrzaj.Content = App.ServiceProvider.GetRequiredService<StudentiDodajView>();
                return;
            }
        }
    }
}
