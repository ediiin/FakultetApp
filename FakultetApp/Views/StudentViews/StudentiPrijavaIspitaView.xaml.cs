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

            UcitajDostupneIspite();
        }

        private void UcitajDostupneIspite()
        {
            var dostupniIspiti = _ispitServis.GetDostupniIspitiZaStudenta(_prijavljeniStudent.Id);
            dgDostupniIspiti.ItemsSource = dostupniIspiti;
        }

        private void BtnPrijaviIspit_Click(object sender, RoutedEventArgs e)
        {
            lblGreska.Visibility = Visibility.Hidden;

            if (dgDostupniIspiti.SelectedItem is Ispit odabraniIspit)
            {
                // Provjera dosadašnjih izlazaka na ovaj predmet
                var dosadasnjiIzlasci = _studentIspitServis.BrojIzlazakaNaPredmet(_prijavljeniStudent.Id, odabraniIspit.PredmetId);

                int noviBrojIzlaska = dosadasnjiIzlasci + 1;
                bool jeKomisijski = noviBrojIzlaska >= 4; // Standardno je 4. izlazak komisijski

                // Kalkulacija cijene (npr. komisijski ispit se plaća)
                decimal cijenaIspita = jeKomisijski ? 50.00m : 0.00m;

                // Kreiranje zapisa u bazi
                var novaPrijava = new StudentIspit
                {
                    StudentId = _prijavljeniStudent.Id,
                    IspitId = odabraniIspit.Id,
                    BrojIzlazaka = noviBrojIzlaska,
                    Komisijski = jeKomisijski,
                    Dodatni = false, // Zavisno od pravila fakulteta
                    Cijena = cijenaIspita,
                    Polozio = false,
                    DatumPrijave = DateTime.Now
                };

                _studentIspitServis.Add(novaPrijava);

                MessageBox.Show($"Uspješno ste prijavili ispit iz predmeta: {odabraniIspit.Predmet.Naziv}.\n" +
                                $"Ovo je vaš {noviBrojIzlaska}. izlazak.",
                                "Uspješna prijava", MessageBoxButton.OK, MessageBoxImage.Information);

                // Osvježi tabelu (ukloni tek prijavljeni ispit)
                UcitajDostupneIspite();
            }
            else
            {
                lblGreska.Visibility = Visibility.Visible;
            }
        }
    }
}
