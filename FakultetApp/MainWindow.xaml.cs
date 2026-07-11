using Fakultet.Core.Modeli;
using FakultetApp.Views;
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
    }
}