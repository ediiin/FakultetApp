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

        //dodaj
        private void BtnIspis_Click(object sender, RoutedEventArgs e)
        {
            return;
        }
    }
}
