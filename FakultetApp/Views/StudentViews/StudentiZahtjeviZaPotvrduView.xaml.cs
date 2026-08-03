using Fakultet.Core.Modeli;
using Fakultet.Core.Modeli.Forum;
using Fakultet.Servisi.IServis.Forum;
using FakultetApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.StudentViews
{
    /// <summary>
    /// Interaction logic for StudentiZahtjeviZaPotvrduView.xaml
    /// </summary>
    public partial class StudentiZahtjeviZaPotvrduView : UserControl
    {
        private readonly ZahtjevZaPotvrduServis _zahtjevServis;
        private readonly int _trenutniStudentId;

        public StudentiZahtjeviZaPotvrduView(ZahtjevZaPotvrduServis zahtjevServis, Student trenutniStudent)
        {
            InitializeComponent();

            _zahtjevServis = zahtjevServis;
            _trenutniStudentId = trenutniStudent.Id;

            UcitajSvrhe();
            UcitajZahtjeve();
        }

        private void UcitajSvrhe()
        {
            cmbSvrha.ItemsSource = Enum.GetValues(typeof(SvrhaPotvrde));
            cmbSvrha.SelectedIndex = 0; 
        }

        private void UcitajZahtjeve()
        {
            var zahtjeviIzBaze = _zahtjevServis.GetAll();

            var mojiZahtjevi = zahtjeviIzBaze
                .Where(z => z.StudentId == _trenutniStudentId)
                .OrderByDescending(z => z.DatumPodnosenja)
                .Select(z => new ZahtjevPrikazVM(z)) 
                .ToList();

            lstZahtjevi.ItemsSource = mojiZahtjevi;
        }

        private void BtnPodnesi_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSvrha.SelectedItem == null)
            {
                MessageBox.Show("Molimo odaberite svrhu potvrde.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SvrhaPotvrde odabranaSvrha = (SvrhaPotvrde)cmbSvrha.SelectedItem;
            string napomena = txtNapomena.Text.Trim();

            var noviZahtjev = new ZahtjevZaPotvrdu
            {
                StudentId = _trenutniStudentId,
                SvrhaPotvrde = odabranaSvrha,
                StanjePotvrde = StanjePotvrde.NaCekanju,
                DatumPodnosenja = DateTime.Now,
                Napomena = string.IsNullOrEmpty(napomena) ? null : napomena
            };

            _zahtjevServis.Add(noviZahtjev);

            txtNapomena.Clear();
            cmbSvrha.SelectedIndex = 0;

            MessageBox.Show("Zahtjev uspješno podnesen!", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
            UcitajZahtjeve();
        }

        private void BtnPonisti_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int zahtjevId)
            {
                var rezultat = MessageBox.Show("Da li ste sigurni da želite poništiti ovaj zahtjev?",
                                               "Potvrda poništavanja",
                                               MessageBoxButton.YesNo,
                                               MessageBoxImage.Question);

                if (rezultat == MessageBoxResult.Yes)
                {
                    var zahtjevZaPonistiti = _zahtjevServis.GetById(zahtjevId);
                    if (zahtjevZaPonistiti != null)
                    {
                        zahtjevZaPonistiti.StanjePotvrde = StanjePotvrde.Ponistena;
                        zahtjevZaPonistiti.DatumObrade = DateTime.Now;

                        _zahtjevServis.Update(zahtjevZaPonistiti);
                        UcitajZahtjeve();
                    }
                }
            }
        }
    }
}
