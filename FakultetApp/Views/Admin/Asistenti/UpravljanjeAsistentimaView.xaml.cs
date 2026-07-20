using Microsoft.Extensions.DependencyInjection;
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
