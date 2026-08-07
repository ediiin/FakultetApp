using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using System.Windows.Controls;

namespace FakultetApp.Views.AsistentView
{
    public partial class AsistentPocetnaView : UserControl
    {
        private readonly AsistentPredmetServis _asistentPredmetServis;
        private readonly StudentPredmetServis _studentPredmetServis;
        private readonly Asistent _ulogovaniAsistent;

        public AsistentPocetnaView(AsistentPredmetServis apServis, StudentPredmetServis spServis, Asistent asistent)
        {
            InitializeComponent();

            _asistentPredmetServis = apServis;
            _studentPredmetServis = spServis;
            _ulogovaniAsistent = asistent;

            UcitajPodatke();
        }

        private void UcitajPodatke()
        {
            txtPozdrav.Text = $"Dobro došli, {_ulogovaniAsistent.Ime} {_ulogovaniAsistent.Prezime}!";
            txtDatum.Text = $"Danas je {DateTime.Now:dd.MM.yyyy.}";

            var predmeti = _asistentPredmetServis.GetPredmetiByAsistent(_ulogovaniAsistent.Id);
            txtBrojPredmeta.Text = predmeti.Count.ToString();

            int ukupanBrojStudenata = 0;
            int ukupnoPolozenih = 0;

            foreach (var predmet in predmeti)
            {
                var studentiNaPredmetu = _studentPredmetServis.GetStudentiByPredmet(predmet.Id, "");
                ukupanBrojStudenata += studentiNaPredmetu.Count;
                ukupnoPolozenih += studentiNaPredmetu.Count(s => s.Polozio);
            }

            txtBrojStudenata.Text = ukupanBrojStudenata.ToString();

            if (ukupanBrojStudenata > 0)
            {
                double prolaznost = (double)ukupnoPolozenih / ukupanBrojStudenata * 100;
                txtProlaznost.Text = $"{Math.Round(prolaznost, 1)}%";
            }
        }
    }
}