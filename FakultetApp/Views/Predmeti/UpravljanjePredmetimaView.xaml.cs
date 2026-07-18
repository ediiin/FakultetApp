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

namespace FakultetApp.Views.Predmeti
{
    /// <summary>
    /// Interaction logic for UpravljanjePredmetimaView.xaml
    /// </summary>
    public partial class UpravljanjePredmetimaView : UserControl
    {
        public UpravljanjePredmetimaView()
        {
            InitializeComponent();
            PredmetiSadrzaj.Content = App.ServiceProvider.GetRequiredService<PredmetPregledView>();
        }

        private void PredmetNav_Click(object sender, RoutedEventArgs e)
        {
            Button? dugme = sender as Button;
            if (dugme == null) return;

            if (dugme.Name == "btnPrikaziSve")
            {
                PredmetiSadrzaj.Content = App.ServiceProvider.GetRequiredService<PredmetPregledView>();
            }
            else if (dugme.Name == "btnDodajPredmet")
            {
                PredmetiSadrzaj.Content = App.ServiceProvider.GetRequiredService<PredmetDodajView>();
            }
        }
    }
}
