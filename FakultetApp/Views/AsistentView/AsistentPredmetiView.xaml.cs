using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using System.Windows.Controls;

namespace FakultetApp.Views.AsistentViews
{
    public partial class AsistentPredmetiView : UserControl
    {
        private readonly AsistentPredmetServis _asistentPredmetServis;
        private readonly StudentPredmetServis _studentPredmetServis;
        private readonly Asistent _ulogovaniAsistent;

        public AsistentPredmetiView(AsistentPredmetServis asistentPredmetServis,
            StudentPredmetServis studentPredmetServis,
            Asistent asistent)
        {
            InitializeComponent();

            _asistentPredmetServis = asistentPredmetServis;
            _studentPredmetServis = studentPredmetServis;
            _ulogovaniAsistent = asistent;

            UcitajPredmete();
        }

        private void UcitajPredmete()
        {
            var predmeti = _asistentPredmetServis.GetPredmetiByAsistent(_ulogovaniAsistent.Id);
            cmbMojiPredmeti.ItemsSource = predmeti;

            if (predmeti.Any())
                cmbMojiPredmeti.SelectedIndex = 0;
        }

        private void CmbMojiPredmeti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMojiPredmeti.SelectedItem is Predmet odabraniPredmet)
            {
                txtPretragaStudenata.Text = "";

                var sviUpisaniStudenti = _studentPredmetServis.GetStudentiByPredmet(odabraniPredmet.Id, "");
                txtBrojUpisanih.Text = sviUpisaniStudenti.Count.ToString();

                var polozeni = sviUpisaniStudenti.Where(x => x.Polozio && x.Ocjena.HasValue).ToList();
                if (polozeni.Any())
                {
                    double prosjek = polozeni.Average(x => x.Ocjena!.Value);
                    txtProsjecnaOcjena.Text = Math.Round(prosjek, 2).ToString();

                    double prolaznost = (double)polozeni.Count / sviUpisaniStudenti.Count * 100;
                    txtProlaznost.Text = $"{Math.Round(prolaznost, 1)}%";
                }
                else
                {
                    txtProsjecnaOcjena.Text = "-";
                    txtProlaznost.Text = "0%";
                }

                UcitajStudente();
            }
        }

        private void TxtPretragaStudenata_TextChanged(object sender, TextChangedEventArgs e)
        {
            UcitajStudente();
        }

        private void UcitajStudente()
        {
            if (cmbMojiPredmeti.SelectedItem is Predmet odabraniPredmet)
            {
                string filter = txtPretragaStudenata.Text;

                if (filter.Contains("Pretraga")) filter = "";

                var studentiLista = _studentPredmetServis.GetStudentiByPredmet(odabraniPredmet.Id, filter);
                lstStudenti.ItemsSource = studentiLista;
            }
        }
    }
}