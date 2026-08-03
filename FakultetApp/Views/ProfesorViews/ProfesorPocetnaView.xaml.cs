using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using FakultetApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.ProfesorViews
{
    public partial class ProfesorPocetnaView : UserControl
    {
        private readonly Profesor _profesor;
        private readonly IspitServis _ispitServis;
        private readonly PredmetServis _predmetServis;
        private readonly StudentIspitServis _studentIspitServis;

        public ProfesorPocetnaView(
            Profesor profesor,
            IspitServis ispitServis,
            PredmetServis predmetServis,
            StudentIspitServis studentIspitServis)
        {
            InitializeComponent();
            _profesor = profesor;
            _ispitServis = ispitServis;
            _predmetServis = predmetServis;
            _studentIspitServis = studentIspitServis;

            UcitajPodatke();
        }

        private void UcitajPodatke()
        {
            var profesorPredmetiIds = _predmetServis.GetPredmetiByProfesor(_profesor.Id)
                                                    .Select(p => p.Id)
                                                    .ToList();

            var sviIspitiProfesora = _ispitServis.GetAll()
                                                 .Where(i => profesorPredmetiIds.Contains(i.PredmetId))
                                                 .ToList();

            var svePrijave = _studentIspitServis.GetAll();

            var neocijenjeni = sviIspitiProfesora
                .Where(i => i.DatumOdrzavanja < DateTime.Now &&
                            svePrijave.Any(p => p.IspitId == i.Id && !p.Ocjena.HasValue))
                .Select(i => new IspitPrikazDTO
                {
                    IspitId = i.Id,
                    PredmetId = i.PredmetId,
                    PredmetNaziv = i.Predmet?.Naziv ?? "N/A",
                    DatumOdrzavanja = i.DatumOdrzavanja,
                    Dodatni = i.Dodatni,
                    BrojPrijavljenih = svePrijave.Count(p => p.IspitId == i.Id)
                })
                .OrderByDescending(i => i.DatumOdrzavanja)
                .ToList();

            dgNeocijenjeniIspiti.ItemsSource = neocijenjeni;
            lblNemaNeocijenjenih.Visibility = neocijenjeni.Any() ? Visibility.Collapsed : Visibility.Visible;

            var nadolazeci = sviIspitiProfesora
                .Where(i => i.DatumOdrzavanja >= DateTime.Now)
                .Select(i => new IspitPrikazDTO
                {
                    IspitId = i.Id,
                    PredmetId = i.PredmetId,
                    PredmetNaziv = i.Predmet?.Naziv ?? "N/A",
                    DatumOdrzavanja = i.DatumOdrzavanja,
                    Dodatni = i.Dodatni,
                    BrojPrijavljenih = svePrijave.Count(p => p.IspitId == i.Id)
                })
                .OrderBy(i => i.DatumOdrzavanja)
                .ToList();

            dgNadolazeciIspiti.ItemsSource = nadolazeci;
            lblNemaNadolazecih.Visibility = nadolazeci.Any() ? Visibility.Collapsed : Visibility.Visible;
        }

        private void BtnUnesiOcjene_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var odabraniIspit = button?.DataContext as IspitPrikazDTO;

            if (odabraniIspit != null)
            {
                var prikaznik = this.Parent as ContentControl;
                if (prikaznik != null)
                {
                    prikaznik.Content = ActivatorUtilities.CreateInstance<ProfesorUnosOcjenaView>(App.ServiceProvider!, _profesor, odabraniIspit.IspitId);
                }
            }
        }
    }
}