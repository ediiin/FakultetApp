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

        public static string ConnectionString => Crtaj.GetConnectionString("FakultetBaza")!;
        public static string AdminPassword => Crtaj["AppSettings:InitialAdminPassword"]!;
        public static string Student1Password => Crtaj["AppSettings:InitialStudent1Password"]!;
        public static string Student2Password => Crtaj["AppSettings:InitialStudent2Password"]!;
        public static string Profesor1Password => Crtaj["AppSettings:InitialProfesor1Password"]!;
        public static string Profesor2Password => Crtaj["AppSettings:InitialProfesor2Password"]!;
        public static string Asistent1Password => Crtaj["AppSettings:InitialAsistent1Password"]!;
        public static string Asistent2Password => Crtaj["AppSettings:InitialAsistent2Password"]!;
    }
}