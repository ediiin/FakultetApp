using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using System.Windows.Controls;

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
