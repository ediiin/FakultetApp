using Fakultet.Core.Modeli;
using Fakultet.Servisi.Helperi;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using System.Windows.Controls;

namespace FakultetApp.Views.StudentViews
{
    public partial class StudentiLicniPodaciView : UserControl
    {
        private readonly Student _student;
        private readonly StudentPredmetServis _studentPredmetServis;

        public StudentiLicniPodaciView(StudentPredmetServis studentPredmetServis, Student student)
        {
            InitializeComponent();

            _student = student;
            _studentPredmetServis = studentPredmetServis;

            this.DataContext = _student;

            UcitajUspjeh();
        }

        private void UcitajUspjeh()
        {
            var upisaniPredmeti = _studentPredmetServis.GetByStudentId(_student.Id);

            if (upisaniPredmeti == null || !upisaniPredmeti.Any()) return;

            var ucitaneGodine = upisaniPredmeti
                .GroupBy(sp => sp.Predmet.GodinaStudija.Opis)
                .Select(g => new PrikazUspjeha
                {
                    GodinaOpis = g.Key,
                    Predmeti = g.ToList(),
                    ProsjekGodine = g.Any(p => p.Polozio && p.Ocjena > 5)
                                    ? g.Where(p => p.Polozio && p.Ocjena > 5).Average(p => (double)p.Ocjena)
                                    : 0
                })
                .OrderBy(g => GodinaStudijaHelper.OdrediBrojGodine(g.GodinaOpis))
                .ToList();

            var ukupniProsjek = upisaniPredmeti.Any(p => p.Polozio && p.Ocjena > 5)
                                ? upisaniPredmeti.Where(p => p.Polozio && p.Ocjena > 5).Average(p => (double)p.Ocjena)
                                : 0;

            karticaUspjeh.DataContext = new
            {
                UspjehPoGodinama = ucitaneGodine,
                UkupniProsjek = ukupniProsjek
            };
        }
    }

    public class PrikazUspjeha
    {
        public string GodinaOpis { get; set; }
        public double ProsjekGodine { get; set; }
        public List<StudentPredmet> Predmeti { get; set; }
    }
}