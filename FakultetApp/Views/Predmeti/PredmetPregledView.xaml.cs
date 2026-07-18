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

        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            var izabraniPredmet = dgvPredmeti.SelectedItem as Predmet;
            //if (izabraniPredmet != null &&
            //    MessageBox.Show($"Jeste li sigurni da želite obrisati predmet '{izabraniPredmet.Naziv}'?\n\nUpozorenje: Ovo može utjecati na ocjene i studente vezane za ovaj predmet!",
            //    "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            //{
            //    _predmetServis.Remove(izabraniPredmet.Id);
            //    MessageBox.Show("Predmet uspješno uklonjen iz sistema!", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
            //    UcitajPredmete();
            //}
        }
    }
}
