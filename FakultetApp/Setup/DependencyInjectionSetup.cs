using Fakultet.Servisi.Bazni;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using FakultetApp;
using FakultetApp.Login;
using FakultetApp.Views;
using FakultetApp.Views.Admin;
using FakultetApp.Views.Admin.Asistenti;
using FakultetApp.Views.Admin.ProfesoriLogika;
using FakultetApp.Views.Admin.Studenti;
using FakultetApp.Views.Predmeti;
using Microsoft.Extensions.DependencyInjection;

namespace Fakultet.Servisi.Setup
{
    //static ne mozes pravit objekat pristupas direktno preko imena klase
    public static class DependencyInjectionSetup
    {
        // grupisanje servisa
        public static void RegistrujServise(this IServiceCollection services)
        {
            services.AddTransient<GradServis>();
            services.AddTransient<SpolServis>();
            services.AddTransient<OsobaServis>();
            services.AddTransient<DrzavaServis>();
            services.AddTransient<StudijServis>();
            services.AddTransient<GodinaStudijaServis>();
            services.AddTransient<DataSeedServis>();
            services.AddTransient<StudentServis>();
            services.AddTransient<ProfesorServis>();
            services.AddTransient<PredmetServis>();
            services.AddTransient<AsistentServis>();
            // ostali servisi ce ici ovdje
        }

        // grupisanje views/prozora
        public static void RegistrujViewove(this IServiceCollection services)
        {
            // transient stvaraju se iznova
            //login
            services.AddTransient<LoginProzor>();
            //admin - admin
            services.AddTransient<NoviAdminView>();
            services.AddTransient<AdminDashboardView>(); 
            //admin - studenti
            services.AddTransient<UpravljanjeStudentimaView>(); 
            services.AddTransient<StudentiPregledView>(); 
            services.AddTransient<StudentiDodajView>(); 
            services.AddTransient<StudentiEditView>();
            //admin - profesori
            services.AddTransient<ProfesoriDodajView>(); 
            services.AddTransient<ProfesoriPregledView>(); 
            services.AddTransient<UpravljanjeProfesorimaView>();
            //admin - predmeti
            services.AddTransient<UpravljanjePredmetimaView>(); 
            services.AddTransient<PredmetPregledView>(); 
            services.AddTransient<PredmetDodajView>();
            //admin - asistenti
            services.AddTransient<UpravljanjeAsistentimaView>();
            services.AddTransient<AsistentPregledView>();
            services.AddTransient<AsistentDodajView>();
            services.AddTransient<AsistentiEditView>();

            // mainwindow drzimo u memoriji zato singleton
            services.AddSingleton<MainWindow>();
        }
    }
}
