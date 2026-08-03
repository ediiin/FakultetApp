using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.StudentViews
{
    /// <summary>
    /// Interaction logic for StudentiPrijavaIspitaView.xaml
    /// </summary>
    public partial class StudentiPrijavaIspitaView : UserControl
    {
        private readonly Student _prijavljeniStudent;
        private readonly IspitServis _ispitServis;
        private readonly StudentIspitServis _studentIspitServis;

        public StudentiPrijavaIspitaView(Student student, IspitServis ispitServis, StudentIspitServis studentIspitServis)
        {
            InitializeComponent();
            _prijavljeniStudent = student;
            _ispitServis = ispitServis;
            _studentIspitServis = studentIspitServis;

            OsvjeziSveTabele();
        }

        private void OsvjeziSveTabele()
        {
            UcitajDostupneIspite();
            UcitajPrijavljeneIspite();
        }

        private void UcitajDostupneIspite()
        {
            var dostupniIspiti = _ispitServis.GetDostupniIspitiZaStudenta(_prijavljeniStudent.Id);
            dgDostupniIspiti.ItemsSource = dostupniIspiti;
        }

        private void UcitajPrijavljeneIspite()
        {
            //samo ispiti koji nisu odrzani
            var aktivnePrijave = _studentIspitServis.GetPrijaveByStudent(_prijavljeniStudent.Id)
                                                    .Where(p => p.Ispit.DatumOdrzavanja > DateTime.Now)
                                                    .ToList();

            dgPrijavljeniIspiti.ItemsSource = aktivnePrijave;
        }

        private void BtnPrijavi_Click(object sender, RoutedEventArgs e)
        {
            // ispit iz kliknutog reda
            var button = sender as Button;
            var odabraniIspit = button?.DataContext as Ispit;

            if (odabraniIspit != null)
            {
                var dosadasnjiIzlasci = _studentIspitServis.BrojIzlazakaNaPredmet(_prijavljeniStudent.Id, odabraniIspit.PredmetId);
                int noviBrojIzlaska = dosadasnjiIzlasci + 1;
                bool jeKomisijski = noviBrojIzlaska >= 4;

                decimal cijenaIspita = jeKomisijski ? 50.00m : 0.00m;

                var novaPrijava = new StudentIspit
                {
                    StudentId = _prijavljeniStudent.Id,
                    IspitId = odabraniIspit.Id,
                    BrojIzlazaka = noviBrojIzlaska,
                    Komisijski = jeKomisijski,
                    Dodatni = false,
                    Cijena = cijenaIspita,
                    Polozio = false,
                    DatumPrijave = DateTime.Now
                };

                _studentIspitServis.Add(novaPrijava);

                MessageBox.Show($"Uspješno ste prijavili ispit iz predmeta: {odabraniIspit.Predmet.Naziv}.\n" +
                                $"Ovo je vaš {noviBrojIzlaska}. izlazak.",
                                "Uspješna prijava", MessageBoxButton.OK, MessageBoxImage.Information);

                OsvjeziSveTabele();
            }
        }

        private void BtnOdjavi_Click(object sender, RoutedEventArgs e)
        {
            // prijava ispita iz tog reda odabranog
            var button = sender as Button;
            var prijavaZaOdjavu = button?.DataContext as StudentIspit;

            if (prijavaZaOdjavu != null)
            {
                // zabrana odjave 24h pred ispit
                if (prijavaZaOdjavu.Ispit.DatumOdrzavanja < DateTime.Now.AddDays(1))
                {
                    MessageBox.Show("Nije moguće odjaviti ispit jer je do održavanja ostalo manje od 24 sata.",
                                    "Greška pri odjavi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var upit = MessageBox.Show($"Jeste li sigurni da želite odjaviti ispit iz predmeta: {prijavaZaOdjavu.Ispit.Predmet.Naziv}?",
                                           "Potvrda odjave", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (upit == MessageBoxResult.Yes)
                {
                    _studentIspitServis.OdjaviIspit(prijavaZaOdjavu.StudentId, prijavaZaOdjavu.IspitId);

                    MessageBox.Show("Ispit uspješno odjavljen.", "Odjava", MessageBoxButton.OK, MessageBoxImage.Information);
                    OsvjeziSveTabele(); 
                }
            }
        }
    }
}
