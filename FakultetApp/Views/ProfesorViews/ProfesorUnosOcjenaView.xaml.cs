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
    public partial class ProfesorUnosOcjenaView : UserControl
    {
        private readonly Profesor _profesor;
        private readonly PredmetServis _predmetServis;
        private readonly IspitServis _ispitServis;
        private readonly StudentIspitServis _studentIspitServis;
        private readonly int _pocetniIspitId;

        public ProfesorUnosOcjenaView(
            PredmetServis predmetServis,
            IspitServis ispitServis,
            StudentIspitServis studentIspitServis,
            Profesor profesor,
            int ispitId = 0)
        {
            InitializeComponent();
            _profesor = profesor;
            _predmetServis = predmetServis;
            _ispitServis = ispitServis;
            _studentIspitServis = studentIspitServis;
            _pocetniIspitId = ispitId;

            UcitajPredmete();
        }

        private void UcitajPredmete()
        {
            var predmeti = _predmetServis.GetPredmetiByProfesor(_profesor.Id);
            cmbPredmeti.ItemsSource = predmeti;

            // ako je prozor otvoren sa konkretnim ispitom
            if (_pocetniIspitId > 0)
            {
                var targetIspit = _ispitServis.GetIspitPoId(_pocetniIspitId);
                if (targetIspit != null)
                {
                    cmbPredmeti.SelectedValue = targetIspit.PredmetId;
                    return;
                }
            }

            if (predmeti.Any())
            {
                cmbPredmeti.SelectedIndex = 0;
            }
        }

        private void CmbPredmeti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPredmeti.SelectedValue is int odabraniPredmetId)
            {
                UcitajIspiteZaPredmet(odabraniPredmetId);
            }
            else
            {
                cmbIspiti.ItemsSource = null;
                dgPrijave.ItemsSource = null;
            }
        }

        private void UcitajIspiteZaPredmet(int predmetId)
        {
            var ispiti = _ispitServis.GetAllByPredmet(predmetId);

            var svePrijave = _studentIspitServis.GetAll();

            var cmbLista = new List<IspitCmbDTO>();
            foreach (var ispit in ispiti)
            {
                // brojac neocjenjenih studenata na tom ispitu
                int neocijenjeniCount = svePrijave.Count(p => p.IspitId == ispit.Id && !p.Ocjena.HasValue);

                string tip = ispit.Dodatni ? "Dodatni" : "Redovni";
                string statusNeocijenjeni = neocijenjeniCount > 0 ? $" ({neocijenjeniCount} neocijenjeno)" : " (Završeno)";

                cmbLista.Add(new IspitCmbDTO
                {
                    IspitId = ispit.Id,
                    PrikazTekst = $"{ispit.DatumOdrzavanja:dd.MM.yyyy} - {tip} rok{statusNeocijenjeni}"
                });
            }

            cmbIspiti.ItemsSource = cmbLista;

            // ako ima pocetni definisan preko id njega izaberi
            if (_pocetniIspitId > 0 && cmbLista.Any(i => i.IspitId == _pocetniIspitId))
            {
                cmbIspiti.SelectedValue = _pocetniIspitId;
            }
            else if (cmbLista.Any())
            {
                var prviSaNeocijenjenim = cmbLista.FirstOrDefault(i => i.PrikazTekst.Contains("neocijenjeno"));
                if (prviSaNeocijenjenim != null)
                    cmbIspiti.SelectedValue = prviSaNeocijenjenim.IspitId;
                else
                    cmbIspiti.SelectedIndex = 0;
            }
            else
            {
                dgPrijave.Visibility = Visibility.Collapsed;
                lblNemaPrijava.Visibility = Visibility.Visible;
            }
        }

        private void CmbIspiti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbIspiti.SelectedValue is int odabraniIspitId)
            {
                UcitajPrijaveZaIspit(odabraniIspitId);
            }
            else
            {
                dgPrijave.ItemsSource = null;
                dgPrijave.Visibility = Visibility.Collapsed;
                lblNemaPrijava.Visibility = Visibility.Visible;
            }
        }

        private void UcitajPrijaveZaIspit(int ispitId)
        {
            var prijave = _studentIspitServis.GetAll()
                                             .Where(p => p.IspitId == ispitId)
                                             .ToList();

            var prikazLista = prijave.Select(p => new PrijavaZaOcjenuDTO(p)).ToList();

            dgPrijave.ItemsSource = prikazLista;

            if (prikazLista.Any())
            {
                dgPrijave.Visibility = Visibility.Visible;
                lblNemaPrijava.Visibility = Visibility.Collapsed;
            }
            else
            {
                dgPrijave.Visibility = Visibility.Collapsed;
                lblNemaPrijava.Visibility = Visibility.Visible;
            }
        }

        private void BtnSacuvajOcjenu_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var red = button?.DataContext as PrijavaZaOcjenuDTO;

            if (red != null)
            {
                var prijavaZaUpdate = red.Prijava;
                prijavaZaUpdate.Ocjena = red.OdabranaOcjena;
                prijavaZaUpdate.Polozio = red.OdabranaOcjena > 5;

                _studentIspitServis.Update(prijavaZaUpdate);

                dgPrijave.Items.Refresh();

                MessageBox.Show($"Uspješno sačuvana ocjena {red.OdabranaOcjena} za studenta {red.ImePrezime}.",
                                "Ocjena upisana", MessageBoxButton.OK, MessageBoxImage.Information);

                // osvjezavamo brojac
                if (cmbPredmeti.SelectedValue is int predmetId)
                {
                    int trenutniIspitId = red.Prijava.IspitId;
                    UcitajIspiteZaPredmet(predmetId);
                    cmbIspiti.SelectedValue = trenutniIspitId;
                }
            }
        }

        private void BtnNazad_Click(object sender, RoutedEventArgs e)
        {
            var prikaznik = NadjiRoditeljskiKontejner<ContentControl>(this);

            if (prikaznik != null)
            {
                if (_pocetniIspitId > 0)
                {
                    prikaznik.Content = ActivatorUtilities.CreateInstance<ProfesorPocetnaView>(App.ServiceProvider!, _profesor);
                }
                else
                {
                    prikaznik.Content = ActivatorUtilities.CreateInstance<ProfesorPredmetiView>(App.ServiceProvider!, _profesor);
                }
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