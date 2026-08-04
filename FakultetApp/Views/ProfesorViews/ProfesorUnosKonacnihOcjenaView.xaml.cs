using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using FakultetApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FakultetApp.Views.ProfesorViews
{
    public partial class ProfesorUnosKonacnihOcjenaView : UserControl
    {
        private readonly Profesor _profesor;
        private readonly Predmet? _pocetniPredmet;

        private readonly PredmetServis _predmetServis;
        private readonly StudentPredmetServis _studentPredmetServis;
        private readonly StudentIspitServis _studentIspitServis;

        public ProfesorUnosKonacnihOcjenaView(
            PredmetServis predmetServis,
            StudentPredmetServis studentPredmetServis,
            StudentIspitServis studentIspitServis,
            Profesor profesor,
            Predmet? pocetniPredmet = null)
        {
            InitializeComponent();

            _predmetServis = predmetServis;
            _studentPredmetServis = studentPredmetServis;
            _studentIspitServis = studentIspitServis;
            _profesor = profesor;
            _pocetniPredmet = pocetniPredmet;

            UcitajPredmete();
        }

        private void UcitajPredmete()
        {
            var predmeti = _predmetServis.GetPredmetiByProfesor(_profesor.Id);
            cmbPredmeti.ItemsSource = predmeti;

            if (_pocetniPredmet != null && predmeti.Any(p => p.Id == _pocetniPredmet.Id))
            {
                cmbPredmeti.SelectedValue = _pocetniPredmet.Id;
            }
            else if (predmeti.Any())
            {
                cmbPredmeti.SelectedIndex = 0;
            }
        }

        private void CmbPredmeti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPredmeti.SelectedValue is int odabraniPredmetId)
            {
                UcitajStudente(odabraniPredmetId);
            }
        }

        private void UcitajStudente(int predmetId)
        {
            var prijavePredmeta = _studentPredmetServis.GetAll().Where(x => x.PredmetId == predmetId).ToList();

            var sviIspitiZaPredmet = _studentIspitServis.GetAll()
                                        .Where(i => i.Ispit != null && i.Ispit.PredmetId == predmetId && i.Ocjena.HasValue)
                                        .ToList();

            var prikazLista = new List<KonacnaOcjenaDTO>();

            foreach (var prijava in prijavePredmeta)
            {
                var ispitiStudenta = sviIspitiZaPredmet
                    .Where(i => i.StudentId == prijava.StudentId && i.Ocjena > 5)
                    .ToList();

                string detalji = ispitiStudenta.Any()
                    ? string.Join(", ", ispitiStudenta.Select(i => $"{i.Ispit.DatumOdrzavanja:dd.MM} (Ocj: {i.Ocjena})"))
                    : "Nema položenih ispita";

                double prosjek = 0;
                int predlozena = 5;

                if (ispitiStudenta.Any())
                {
                    prosjek = ispitiStudenta.Average(i => i.Ocjena!.Value);
                    predlozena = (int)Math.Round(prosjek, MidpointRounding.AwayFromZero);
                    if (predlozena < 5) predlozena = 5;
                    if (predlozena > 10) predlozena = 10;
                }

                int odabranaZaPrikaz = (prijava.Ocjena.HasValue && prijava.Ocjena.Value >= 5)
                                        ? prijava.Ocjena.Value
                                        : predlozena;

                string boja;
                string opis;

                if (!prijava.Ocjena.HasValue || prijava.Ocjena.Value == 0)
                {
                    // nije uneseno -> crvena
                    boja = "#dc3545";
                    opis = "Nije unesena konačna ocjena";
                }
                else if (prijava.Ocjena.Value > 5)
                {
                    // uneseno i polozio -> zelena
                    boja = "#28a745";
                    opis = $"Položio - Ocjena: {prijava.Ocjena.Value}";
                }
                else
                {
                    // uneseno i nije polozio (ocjena 5) -> zuta
                    boja = "#ffc107";
                    opis = "Nije položio (Ocjena 5)";
                }

                prikazLista.Add(new KonacnaOcjenaDTO
                {
                    Prijava = prijava,
                    ImePrezime = prijava.Student != null ? $"{prijava.Student.Ime} {prijava.Student.Prezime}" : "",
                    Indeks = prijava.Student?.Indeks ?? "",
                    DetaljiIspita = detalji,
                    PredlozenaOcjena = predlozena,
                    OdabranaOcjena = odabranaZaPrikaz,
                    StatusBoja = boja,
                    StatusOpis = opis
                });
            }

            // neocjenjeni na vrh liste 0 a ocjenjeni na dno 1
            prikazLista = prikazLista
                .OrderBy(x => x.Prijava.Ocjena.HasValue ? 1 : 0)
                .ThenBy(x => x.ImePrezime)
                .ToList();

            dgKonacneOcjene.ItemsSource = prikazLista;

            if (prikazLista.Any())
            {
                dgKonacneOcjene.Visibility = Visibility.Visible;
                lblNemaStudenata.Visibility = Visibility.Collapsed;
            }
            else
            {
                dgKonacneOcjene.Visibility = Visibility.Collapsed;
                lblNemaStudenata.Visibility = Visibility.Visible;
            }
        }

        private void BtnSacuvajKonacnuOcjenu_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var red = button?.DataContext as KonacnaOcjenaDTO;

            if (red != null)
            {
                var prijavaZaUpdate = red.Prijava;

                prijavaZaUpdate.Ocjena = red.OdabranaOcjena;
                prijavaZaUpdate.Polozio = red.OdabranaOcjena > 5;

                _studentPredmetServis.Update(prijavaZaUpdate);

                MessageBox.Show($"Konačna ocjena {red.OdabranaOcjena} za studenta {red.ImePrezime} je sačuvana.",
                                "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);

                if (cmbPredmeti.SelectedValue is int predmetId)
                {
                    UcitajStudente(predmetId);
                }
            }
        }

        private void BtnNazad_Click(object sender, RoutedEventArgs e)
        {
            var prikaznik = NadjiRoditeljskiKontejner<ContentControl>(this);
            if (prikaznik != null)
            {
                prikaznik.Content = ActivatorUtilities.CreateInstance<ProfesorPredmetiView>(App.ServiceProvider!, _profesor);
            }
        }

        private T? NadjiRoditeljskiKontejner<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            if (parentObject is T parent && parentObject is not UserControl) return parent;
            return NadjiRoditeljskiKontejner<T>(parentObject);
        }
    }
}