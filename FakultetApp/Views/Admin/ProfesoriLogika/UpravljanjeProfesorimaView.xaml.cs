using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.Admin.ProfesoriLogika
{
    /// <summary>
    /// Interaction logic for UpravljanjeProfesorimaView.xaml
    /// </summary>
    public partial class UpravljanjeProfesorimaView : UserControl
    {
        public UpravljanjeProfesorimaView()
        {
            InitializeComponent();
            ProfesoriSadrzaj.Content = App.ServiceProvider.GetRequiredService<ProfesoriPregledView>();
        }

        private void ProfesorNav_Click(object sender, RoutedEventArgs e)
        {
            Button? dugme = sender as Button;
            if (dugme == null) return;

            if (dugme.Name == "btnPrikaziSve")
            {
                ProfesoriSadrzaj.Content = App.ServiceProvider.GetRequiredService<ProfesoriPregledView>();
            }
            else if (dugme.Name == "btnDodajProfesora")
            {
                ProfesoriSadrzaj.Content = App.ServiceProvider.GetRequiredService<ProfesoriDodajView>();
            }
        }
    }
}
