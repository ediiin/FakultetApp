using Fakultet.Core.Modeli.Forum;
using Fakultet.Servisi.IServis.Forum;
using FakultetApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.Admin
{
    public partial class AdminZahtjeviZaPotvrduView : UserControl
    {
        private readonly ZahtjevZaPotvrduServis _zahtjevServis;

        public AdminZahtjeviZaPotvrduView(ZahtjevZaPotvrduServis zahtjevServis)
        {
            InitializeComponent();
            _zahtjevServis = zahtjevServis;
            UcitajSveZahtjeve();
        }

        private void UcitajSveZahtjeve()
        {
            var sviZahtjevi = _zahtjevServis.GetAll();

            // prvo oni na cekanju pa onda po datumu order
            var mapiraniZahtjevi = sviZahtjevi
                .OrderBy(z => z.StanjePotvrde != StanjePotvrde.NaCekanju)
                .ThenByDescending(z => z.DatumPodnosenja)
                .Select(z => new AdminZahtjevVM(z))
                .ToList();

            lstSviZahtjevi.ItemsSource = mapiraniZahtjevi;
        }

        private void BtnOdobri_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int zahtjevId)
            {
                PromijeniStatusZahtjeva(zahtjevId, StanjePotvrde.Odobrena);
            }
        }

        private void BtnOdbij_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int zahtjevId)
            {
                PromijeniStatusZahtjeva(zahtjevId, StanjePotvrde.Odbijena);
            }
        }

        private void PromijeniStatusZahtjeva(int id, StanjePotvrde noviStatus)
        {
            var zahtjev = _zahtjevServis.GetById(id);
            if (zahtjev != null)
            {
                zahtjev.StanjePotvrde = noviStatus;
                zahtjev.DatumObrade = DateTime.Now;

                _zahtjevServis.Update(zahtjev);

                UcitajSveZahtjeve();
            }
        }
    }
}