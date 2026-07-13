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
    /// Interaction logic for StudentiDodajView.xaml
    /// </summary>
    public partial class StudentiDodajView : UserControl
    {
        private StudentServis _studentServis;
        public StudentiDodajView(StudentServis studentServis)
        {
            InitializeComponent();
            _studentServis = studentServis;
            tbIndeks.Text = _studentServis.GenerisiIndeks();
        }

        private void BtnSpasi_Click(object sender, RoutedEventArgs e)
        {
            return;
        }
    }
}
