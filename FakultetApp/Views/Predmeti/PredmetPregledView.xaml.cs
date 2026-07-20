using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
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
    /// Interaction logic for PredmetPregledView.xaml
    /// </summary>
    public partial class PredmetPregledView : UserControl
    {
        private readonly PredmetServis _predmetServis;
        private List<Predmet> _predmeti = new List<Predmet>();
        public PredmetPregledView(PredmetServis predmetServis)
        {
            InitializeComponent();
            _predmetServis = predmetServis;
            UcitajPredmete();
        }

        private void UcitajPredmete()
        {
            _predmeti = _predmetServis.GetAll();
            dgvPredmeti.ItemsSource = _predmeti;
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tekstPretrage = txtFilter.Text;
            _predmeti = _predmetServis.Filter(tekstPretrage);
            dgvPredmeti.ItemsSource = _predmeti;
        }
    }
}
