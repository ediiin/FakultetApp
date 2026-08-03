using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using System.Windows;
using System.Windows.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                if (odabraniIspit.DatumOdrzavanja <= DateTime.Now.AddHours(24))
                {
                    MessageBox.Show("Prijava ispita više nije moguća! Istekao je rok za prijavu (manje od 24h do početka ispita).",
                                    "Rok istekao", MessageBoxButton.OK, MessageBoxImage.Warning);
                    OsvjeziSveTabele();
                    return;
                }

                var dosadasnjiIzlasci = _studentIspitServis.BrojIzlazakaNaPredmet(_prijavljeniStudent.Id, odabraniIspit.PredmetId);
                int noviBrojIzlaska = dosadasnjiIzlasci + 1;
                bool jeKomisijski = noviBrojIzlaska >= 4;

                decimal cijenaIspita = 0.00m;

                if (odabraniIspit.Dodatni)
                {
                    cijenaIspita = 80.00m; 
                }
                else if (jeKomisijski)
                {
                    cijenaIspita = 50.00m; 
                }

                var novaPrijava = new StudentIspit
                {
                    StudentId = _prijavljeniStudent.Id,
                    IspitId = odabraniIspit.Id,
                    BrojIzlazaka = noviBrojIzlaska,
                    Komisijski = jeKomisijski,
                    Dodatni = odabraniIspit.Dodatni,
                    Cijena = cijenaIspita,
                    Polozio = false,
                    DatumPrijave = DateTime.Now
                };

                _studentIspitServis.Add(novaPrijava);

                string obavijest = $"Uspješno ste prijavili ispit iz predmeta: {odabraniIspit.Predmet.Naziv}.\n" +
                                    $"Ovo je vaš {noviBrojIzlaska}. izlazak.";

                if (cijenaIspita > 0)
                    obavijest += $"\nCijena prijave iznosi: {cijenaIspita:F2} KM.";

                MessageBox.Show(obavijest, "Uspješna prijava", MessageBoxButton.OK, MessageBoxImage.Information);

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
