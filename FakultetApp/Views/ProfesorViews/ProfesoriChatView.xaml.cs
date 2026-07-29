using Fakultet.Core.Modeli;
using Fakultet.Core.Modeli.Forum;
using Fakultet.Servisi.IServis.Forum;
using Fakultet.Servisi.IServis.Korisnici;
using FakultetApp.ViewModels;
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

namespace FakultetApp.Views.ProfesorViews
{
    /// <summary>
    /// Interaction logic for ProfesoriChatView.xaml
    /// </summary>
    public partial class ProfesoriChatView : UserControl
    {
        private readonly ChatPorukaServis _chatPorukaServis;    
        private readonly OsobaServis _osobaServis;    
        private readonly int _trenutniProfesorId;

        private List<RazgovorPreviewViewModel> _sviRazgovori = new List<RazgovorPreviewViewModel>();
        private RazgovorPreviewViewModel? _odabraniRazgovor = null;

        public ProfesoriChatView(ChatPorukaServis chatPorukaServis
            , OsobaServis osobaServis
            , Profesor trenutniProfesor)
        {
            InitializeComponent();

            _chatPorukaServis = chatPorukaServis;
            _osobaServis = osobaServis;
            _trenutniProfesorId = trenutniProfesor.Id;

            UcitajNedavneRazgovore();
        }

        private void UcitajNedavneRazgovore()
        {
            var siroviRazgovori = _chatPorukaServis.GetNedavniRazgovoriZaOsobu(_trenutniProfesorId);

            _sviRazgovori = siroviRazgovori.Select(r => new RazgovorPreviewViewModel
            {
                SagovornikId = r.Sagovornik.Id,
                ImePrezime = $"{r.Sagovornik.Ime} {r.Sagovornik.Prezime}",
                UlogaIliIndeks = GetUlogaIliIndeksText(r.Sagovornik),
                PosljednjaPoruka = r.PosljednjaPoruka.Sadrzaj,
                VrijemePosljednjePoruke = r.PosljednjaPoruka.VrijemeSlanja,
                BrojNeprocitanih = r.BrojNeprocitanih
            }).OrderByDescending(r => r.VrijemePosljednjePoruke).ToList();

            PrikaziRazgovore(_sviRazgovori);
        }

        private void PrikaziRazgovore(List<RazgovorPreviewViewModel> razgovori)
        {
            lstRazgovori.ItemsSource = null;
            lstRazgovori.ItemsSource = razgovori;
        }

        private void TxtPretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pretraga = txtPretraga.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(pretraga))
            {
                PrikaziRazgovore(_sviRazgovori);
                return;
            }

            var filtrirani = _sviRazgovori.Where(r =>
                r.ImePrezime.ToLower().Contains(pretraga) ||
                r.UlogaIliIndeks.ToLower().Contains(pretraga)
            ).ToList();

            var noviKorisnici = _osobaServis.PretraziOsobe(pretraga, _trenutniProfesorId);
            foreach (var osoba in noviKorisnici)
            {
                if (!filtrirani.Any(f => f.SagovornikId == osoba.Id))
                {
                    filtrirani.Add(new RazgovorPreviewViewModel
                    {
                        SagovornikId = osoba.Id,
                        ImePrezime = $"{osoba.Ime} {osoba.Prezime}",
                        UlogaIliIndeks = GetUlogaIliIndeksText(osoba),
                        PosljednjaPoruka = "Započnite razgovor...",
                        VrijemePosljednjePoruke = DateTime.Now,
                        BrojNeprocitanih = 0
                    });
                }
            }

            PrikaziRazgovore(filtrirani);
        }

        private void LstRazgovori_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstRazgovori.SelectedItem is RazgovorPreviewViewModel odabrani)
            {
                _odabraniRazgovor = odabrani;
                OtvariChat(odabrani);
            }
        }

        private void OtvariChat(RazgovorPreviewViewModel sagovornik)
        {
            // prikazi aktivni chat
            PrikazPrazno.Visibility = Visibility.Collapsed;
            PrikazAktivniChat.Visibility = Visibility.Visible;

            txtZaglavljeIme.Text = sagovornik.ImePrezime;
            txtZaglavljeUloga.Text = sagovornik.UlogaIliIndeks;
            txtZaglavljeInicijali.Text = sagovornik.Inicijali;

            _chatPorukaServis.OznaciKaoProcitano(_trenutniProfesorId, sagovornik.SagovornikId);
            sagovornik.BrojNeprocitanih = 0;

            UcitajPorukeZaSagovornika(sagovornik.SagovornikId);
        }

        private void UcitajPorukeZaSagovornika(int sagovornikId)
        {
            var porukeIzBaze = _chatPorukaServis.GetPorukeIzmedju(_trenutniProfesorId, sagovornikId);

            var porukeZaPrikaz = porukeIzBaze.Select(p => new PorukaPrikazViewModel
            {
                Id = p.Id,
                PosiljalacId = p.PosiljalacId,
                Sadrzaj = p.Sadrzaj,
                VrijemeSlanja = p.VrijemeSlanja,
                Procitano = p.Procitano,
                IsMojaPoruka = (p.PosiljalacId == _trenutniProfesorId) // ako sam poslao ja == true znaci poruka ide desno
            }).OrderBy(p => p.VrijemeSlanja).ToList();

            icPoruke.ItemsSource = porukeZaPrikaz;

            // skroluj skroz na dno (najnovije poruke)
            scrollPoruke.UpdateLayout();
            scrollPoruke.ScrollToBottom();
        }

        private void BtnPosalji_Click(object sender, RoutedEventArgs e)
        {
            PosaljiPoruku();
        }

        private void TxtNovaPoruka_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PosaljiPoruku();
            }
        }

        private void PosaljiPoruku()
        {
            if (_odabraniRazgovor == null || string.IsNullOrWhiteSpace(txtNovaPoruka.Text)) return;

            string tekst = txtNovaPoruka.Text.Trim();

            var novaPoruka = new ChatPoruka
            {
                PosiljalacId = _trenutniProfesorId,
                PrimalacId = _odabraniRazgovor.SagovornikId,
                Sadrzaj = tekst,
                VrijemeSlanja = DateTime.Now,
                Procitano = false
            };

            _chatPorukaServis.Add(novaPoruka); //salje poruku

            txtNovaPoruka.Clear();
            UcitajPorukeZaSagovornika(_odabraniRazgovor.SagovornikId);

            // update poruke na lijevom panelu
            _odabraniRazgovor.PosljednjaPoruka = tekst;
            _odabraniRazgovor.VrijemePosljednjePoruke = DateTime.Now;
        }

        private string GetUlogaIliIndeksText(Osoba osoba)
        {
            if (osoba is Student s) return $"Student ({s.Indeks})";
            if (osoba is Profesor) return "Profesor";
            if (osoba is Asistent) return "Asistent";
            return "Korisnik";
        }
    }
}
