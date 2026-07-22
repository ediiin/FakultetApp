using Fakultet.Core.Modeli;
using Fakultet.Servisi.Helperi;
using FakultetApp.Views;
using System.Windows;
using System.Windows.Input;

namespace FakultetApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Osoba _korisnik;
        public MainWindow(Osoba osoba)
        {
            InitializeComponent();
            _korisnik = osoba;

            UcitajInterfacePoUlozi();
        }

        private void UcitajInterfacePoUlozi()
        {
            if(_korisnik.Uloge == Uloge.Admin)
            {
                var adminView = new AdminDashboardView();
                GlavniSadrzajAplikacije.Content = adminView;
            }
            else if (_korisnik.Uloge == Uloge.Student)
            {
                //
            }
            else if (_korisnik.Uloge == Uloge.Profesor)
            {
                //
            }
            else if (_korisnik.Uloge == Uloge.Asistent)
            {
                //
            }
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnThemeToggle_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.ToggleTheme();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}