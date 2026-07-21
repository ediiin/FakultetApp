using Fakultet.Servisi.Helperi;
using Fakultet.Servisi.IServis.Korisnici;
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
using System.Windows.Shapes;

namespace FakultetApp.Login
{
    /// <summary>
    /// Interaction logic for LoginProzor.xaml
    /// </summary>
    public partial class LoginProzor : Window
    {
        private readonly OsobaServis _osobaServis;
        int brojPokusaja = 0;
        public LoginProzor(OsobaServis osobaServis)
        {
            InitializeComponent();
            _osobaServis = osobaServis;
        }

        //async da mozemo koristi task.delay bez blokiranja aplikacije
        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string unos = txtKorisnik.Text;
            string lozinka = txtLozinka.Password;

            if(string.IsNullOrEmpty(unos) || string.IsNullOrEmpty(lozinka))
            {
                PrikaziGresku("Morate unijeti korisničko ime i lozinku!");
                return;
            }

            var prijavljenaOsoba = _osobaServis.Login(unos, lozinka);
            if (prijavljenaOsoba != null)
            {
                //ako se loginuje vracamo counter
                brojPokusaja = 0;
                MessageBox.Show($"Uspješna prijava! Dobrodošli, " +
                    $"{prijavljenaOsoba.Ime} ({prijavljenaOsoba.Uloge})"
                    , "Info"
                    , MessageBoxButton.OK
                    , MessageBoxImage.Information);

                MainWindow glavniProzor = new MainWindow(prijavljenaOsoba);
                glavniProzor.Show();

                this.Close();
            }
            else
            {
                brojPokusaja++;
                txtLozinka.Clear();

                if(brojPokusaja >= 3)
                {
                    btnLogin.IsEnabled = false;
                    PrikaziGresku("Previse neuspješnih prijava. Dugme je zaključano!!");

                    await Task.Delay(5000);

                    btnLogin.IsEnabled = true;
                    brojPokusaja = 0;
                    lblGreska.Visibility = Visibility.Collapsed;
                }
                else
                {
                    PrikaziGresku("Niste unijeli ispravne podatke!");
                }
            }
        }

        private void PrikaziGresku(string greska)
        {
            lblGreska.Text = greska;
            lblGreska.Visibility = Visibility.Visible;
        }

        private void BtnThemeToggle_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.ToggleTheme();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
