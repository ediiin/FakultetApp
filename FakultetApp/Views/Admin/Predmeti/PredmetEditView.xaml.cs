using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using System.Windows;
using System.Windows.Controls;

namespace FakultetApp.Views.Admin.Predmeti
{
    /// <summary>
    /// Interaction logic for PredmetEditView.xaml
    /// </summary>
    public partial class PredmetEditView : UserControl
    {
        private readonly Predmet _predmet;
        private readonly ProfesorServis _profesorServis;
        private readonly GodinaStudijaServis _godinaStudijaServis;
        private readonly PredmetServis _predmetServis;
        public PredmetEditView(
            Predmet predmet, 
            ProfesorServis profesorServis,
            GodinaStudijaServis godinaStudijaServis,
            PredmetServis predmetServis)
        {
            InitializeComponent();

            _predmet = predmet;
            _profesorServis = profesorServis;
            _godinaStudijaServis = godinaStudijaServis;
            _predmetServis = predmetServis;

            UcitajProfesora();
            UcitajGodinuStudija();
            UcitajPodatkePredmeta();
        }

        private void UcitajPodatkePredmeta()
        {
            tbNaziv.Text = _predmet.Naziv;
            tbECTS.Text = _predmet.ECTS.ToString();
        }

        private void UcitajGodinuStudija()
        {
            cmbGodinaStudija.ItemsSource = _godinaStudijaServis.GetAll();
            cmbGodinaStudija.DisplayMemberPath = "Opis";
            cmbGodinaStudija.SelectedValuePath = "Id";
            cmbGodinaStudija.SelectedValue = _predmet.GodinaStudijaId;
        }

        private void UcitajProfesora()
        {
            cmbProfesor.ItemsSource = _profesorServis.GetAll();
            cmbProfesor.DisplayMemberPath = "ZvanjeImePrezime";
            cmbProfesor.SelectedValuePath = "Id";
            cmbProfesor.SelectedValue = _predmet.ProfesorId;
        }

        private void BtnSacuvajIzmjene_Click(object sender, RoutedEventArgs e)
        {
            OcistiSveErrore();

            if (!Validno())
                return;

            _predmet.Naziv = tbNaziv.Text.Trim();
            _predmet.ECTS = int.Parse(tbECTS.Text.Trim());
            _predmet.ProfesorId = (int)cmbProfesor.SelectedValue;
            _predmet.GodinaStudijaId = (int)cmbGodinaStudija.SelectedValue;

            _predmetServis.Update(_predmet);
            MessageBox.Show("Uspješno sačuvane izmjene predmeta!", "Uspjeh", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool Validno()
        {
            bool validno = true;
            if (string.IsNullOrWhiteSpace(tbNaziv.Text))
            {
                lblNazivError.Text = "Polje 'naziv' obavezno!";
                lblNazivError.Visibility = Visibility.Visible;
                validno = false;
            }
            if (string.IsNullOrWhiteSpace(tbECTS.Text))
            {
                lblECTSError.Text = "Polje 'ECTS' obavezno!";
                lblECTSError.Visibility = Visibility.Visible;
                validno = false;
            }
            if (!(int.TryParse(tbECTS.Text, out int broj) && broj > 0))
            {
                lblECTSError.Text = "Polje 'ECTS' mora biti pozitivan cijeli broj!";
                lblECTSError.Visibility = Visibility.Visible;
                validno = false;
            }

            return validno;
        }

        private void OcistiSveErrore()
        {
            lblNazivError.Visibility = Visibility.Hidden;
            lblProfesorError.Visibility = Visibility.Hidden;
            lblGodinaStudijaError.Visibility = Visibility.Hidden;
            lblECTSError.Visibility = Visibility.Hidden;
        }
    }
}
