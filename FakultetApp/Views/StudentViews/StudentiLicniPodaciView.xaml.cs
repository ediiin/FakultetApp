using Fakultet.Core.Modeli;
using System.Windows.Controls;

namespace FakultetApp.Views.StudentViews
{
    /// <summary>
    /// Interaction logic for StudentiLicniPodaciView.xaml
    /// </summary>
    public partial class StudentiLicniPodaciView : UserControl
    {
        private readonly Student _student;

        public StudentiLicniPodaciView(Student student)
        {
            InitializeComponent();
            _student = student;

            this.DataContext = _student;
        }
    }
}
