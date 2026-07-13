using Fakultet.Core.Modeli;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FakultetApp.Views.Admin
{
    /// <summary>
    /// Interaction logic for StudentiPregledView.xaml
    /// </summary>
    public partial class StudentiPregledView : UserControl
    {
        private readonly StudentServis _studentServis;
        private List<Student> _studenti = new List<Student>();
        public StudentiPregledView(StudentServis studentServis)
        {
            InitializeComponent();
            _studentServis = studentServis;
            UcitajStudente();
        }

        private void UcitajStudente()
        {
            _studenti = _studentServis.GetAll();
            dgvStudenti.ItemsSource = _studenti;
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string indeksImePrezime = txtFilterImePrezimeIndeks.Text.ToLower();
            _studenti = _studentServis.Filter(indeksImePrezime);
            dgvStudenti.ItemsSource = _studenti;
        }

        private void BtnIspis_Click(object sender, RoutedEventArgs e)
        {
            var izabraniStudent = dgvStudenti.SelectedItem as Student;
            if (izabraniStudent != null && 
                MessageBox.Show($"Jeste li sigurni da želite ispisati studenta {izabraniStudent.ToString()}?"
                , "Upit", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {

                _studentServis.Remove(izabraniStudent.Id);
                MessageBox.Show($"Student uspješno ispisan!",
                    "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                UcitajStudente();
            }
        }
    }
}
