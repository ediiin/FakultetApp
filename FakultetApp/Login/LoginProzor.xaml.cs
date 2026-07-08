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
        private readonly OsobaServis _osobaServis = new OsobaServis();
        public LoginProzor()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
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
                PrikaziGresku("Niste unijeli ispravno ime ili lozinku!");
                txtLozinka.Clear();
            }
        }

        private void PrikaziGresku(string greska)
        {
            lblGreska.Text = greska;
            lblGreska.Visibility = Visibility.Visible;
        }
    }
}
