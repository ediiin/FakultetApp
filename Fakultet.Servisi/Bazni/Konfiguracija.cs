using Microsoft.Extensions.Configuration;
using System;
using System.IO; // <-- OVO TI JE FALILO ZA 'Directory'

namespace Fakultet.Servisi.Bazni
{
    public static class Konfiguracija
    {
        public static IConfiguration Crtaj { get; set; }

        static Konfiguracija()
        {
            Crtaj = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build(); 
        }

        public static string ConnectionString => Crtaj.GetConnectionString("FakultetBaza");
        public static string AdminPassword => Crtaj["AppSettings:InitialAdminPassword"];
    }
}