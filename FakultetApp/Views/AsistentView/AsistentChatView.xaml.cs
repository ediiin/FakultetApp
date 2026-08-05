using Fakultet.Core.Modeli;
using Fakultet.Core.Modeli.Forum;
using Fakultet.Servisi.IServis.Forum;
using Fakultet.Servisi.IServis.Korisnici;
using FakultetApp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FakultetApp.Views.AsistentViews
{
    /// <summary>
    /// Interaction logic for AsistentiChatView.xaml
    /// </summary>
    public partial class AsistentChatView : UserControl
    {
        private readonly ChatPorukaServis _chatPorukaServis;
        private readonly OsobaServis _osobaServis;
        private readonly int _trenutniAsistentId;

        private List<RazgovorPreviewViewModel> _sviRazgovori = new List<RazgovorPreviewViewModel>();
        private RazgovorPreviewViewModel? _odabraniRazgovor = null;

        public AsistentChatView(ChatPorukaServis chatPorukaServis
            , OsobaServis osobaServis
            , Asistent trenutniAsistent)
        {
            InitializeComponent();

            _chatPorukaServis = chatPorukaServis;
            _osobaServis = osobaServis;
            _trenutniAsistentId = trenutniAsistent.Id;

            UcitajNedavneRazgovore();
        }

        private void UcitajNedavneRazgovore()
        {
            var siroviRazgovori = _chatPorukaServis.GetNedavniRazgovoriZaOsobu(_trenutniAsistentId);

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
            FilterRazgovor();
        }

        private void FilterRazgovor()
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

            var noviKorisnici = _osobaServis.PretraziOsobe(pretraga, _trenutniAsistentId);
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

            _chatPorukaServis.OznaciKaoProcitano(_trenutniAsistentId, sagovornik.SagovornikId);
            sagovornik.BrojNeprocitanih = 0;

            UcitajPorukeZaSagovornika(sagovornik.SagovornikId);
        }

        private void UcitajPorukeZaSagovornika(int sagovornikId)
        {
            var porukeIzBaze = _chatPorukaServis.GetPorukeIzmedju(_trenutniAsistentId, sagovornikId);

            var porukeZaPrikaz = porukeIzBaze.Select(p => new PorukaPrikazViewModel
            {
                Id = p.Id,
                PosiljalacId = p.PosiljalacId,
                Sadrzaj = p.Sadrzaj,
                VrijemeSlanja = p.VrijemeSlanja,
                Procitano = p.Procitano,
                IsMojaPoruka = (p.PosiljalacId == _trenutniAsistentId) // ako sam poslao ja == true znaci poruka ide desno
            }).OrderBy(p => p.VrijemeSlanja).ToList();

            icPoruke.ItemsSource = porukeZaPrikaz;

            // skroluj skroz na dno (najnovije poruke)
            scrollPoruke.UpdateLayout();
            scrollPoruke.ScrollToBottom();
            FilterRazgovor();
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
                PosiljalacId = _trenutniAsistentId,
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
            if (osoba is Student s) return $"Student";
            if (osoba is Profesor) return "Profesor";
            if (osoba is Asistent) return "Asistent";
            return "Korisnik";
        }
    }
}