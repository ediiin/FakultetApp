using Fakultet.Servisi.Bazni;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using FakultetApp;
using FakultetApp.Login;
using FakultetApp.Views;
using FakultetApp.Views.Admin;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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
            // ostali servisi ce ici ovdje
        }

        // grupisanje views/prozora
        public static void RegistrujViewove(this IServiceCollection services)
        {
            // transient stvaraju se iznova
            services.AddTransient<LoginProzor>();
            services.AddTransient<NoviAdminView>();
            services.AddTransient<AdminDashboardView>(); 
            services.AddTransient<UpravljanjeStudentimaView>(); 
            services.AddTransient<StudentiPregledView>(); 
            services.AddTransient<StudentiDodajView>(); 

            // mainwindow drzimo u memoriji zato singleton
            services.AddSingleton<MainWindow>();
        }
    }
}
