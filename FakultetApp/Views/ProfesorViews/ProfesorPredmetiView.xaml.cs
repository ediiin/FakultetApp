using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.ProfesorViews
{
    /// <summary>
    /// Interaction logic for ProfesorPredmetiView.xaml
    /// </summary>
    public partial class ProfesorPredmetiView : UserControl
    {
        private readonly PredmetServis _predmetServis;
        private readonly StudentPredmetServis _studentPredmetServis;
        private readonly Profesor _ulogovaniProfesor;

        public ProfesorPredmetiView(PredmetServis predmetServis,
            StudentPredmetServis studentPredmetServis,
            Profesor profesor)
        {
            InitializeComponent();

            _predmetServis = predmetServis;
            _studentPredmetServis = studentPredmetServis;
            _ulogovaniProfesor = profesor;

            UcitajPredmete();
        }

        private void UcitajPredmete()
        {
            var predmeti = _predmetServis.GetPredmetiByProfesor(_ulogovaniProfesor.Id);
            cmbMojiPredmeti.ItemsSource = predmeti;

            if (predmeti.Any())
                cmbMojiPredmeti.SelectedIndex = 0;
        }

        private void CmbMojiPredmeti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMojiPredmeti.SelectedItem is Predmet odabraniPredmet)
            {
                txtPravilaBodovanja.Text = odabraniPredmet.PravilaBodovanja;
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

        private void BtnSacuvajBodovanje_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMojiPredmeti.SelectedItem is Predmet odabraniPredmet)
            {
                string noviTekst = txtPravilaBodovanja.Text;

                _predmetServis.SacuvajPravilaBodovanja(odabraniPredmet.Id, noviTekst);

                MessageBox.Show("Podsjetnik za bodovanje je uspješno sačuvan!", "Uspjeh",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                odabraniPredmet.PravilaBodovanja = noviTekst;
            }
        }

        private void BtnUnosOcjena_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMojiPredmeti.SelectedItem is Predmet odabraniPredmet)
            {
                var prikaznik = this.Parent as ContentControl;

                if (prikaznik != null)
                {
                    prikaznik.Content = ActivatorUtilities.CreateInstance<ProfesorUnosOcjenaView>(
                        App.ServiceProvider!,
                        _ulogovaniProfesor
                    );
                }
            }
            else
            {
                MessageBox.Show("Molimo prvo odaberite predmet.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnZakaziIspit_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMojiPredmeti.SelectedItem is Predmet odabraniPredmet)
            {
                var prikaznik = this.Parent as ContentControl;

                if (prikaznik != null)
                {
                    prikaznik.Content = ActivatorUtilities.CreateInstance<ProfesorIspitView>(App.ServiceProvider!, _ulogovaniProfesor, true);
                }
            }
            else
            {
                MessageBox.Show("Molimo prvo odaberite predmet.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnUnosKonacnihOcjena_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMojiPredmeti.SelectedItem is Predmet odabraniPredmet)
            {
                var prikaznik = this.Parent as ContentControl;

                if (prikaznik != null)
                {
                    prikaznik.Content = ActivatorUtilities.CreateInstance<ProfesorUnosKonacnihOcjenaView>(
                        App.ServiceProvider!,
                        _ulogovaniProfesor
                    );
                }
            }
            else
            {
                MessageBox.Show("Molimo prvo odaberite predmet.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
