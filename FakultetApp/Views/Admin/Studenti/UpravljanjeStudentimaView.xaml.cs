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
