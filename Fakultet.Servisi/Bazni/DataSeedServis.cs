using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace Fakultet.Servisi.Bazni
{
    public class DataSeedServis
    {
        private readonly SpolServis _spolServis;
        private readonly DrzavaServis _drzavaServis;
        private readonly GradServis _gradServis; 
        private readonly StudijServis _studijServis; 
        private readonly GodinaStudijaServis _godinaStudijaServis;
        private readonly OsobaServis _osobaServis;
        private readonly ProfesorServis _profesorServis;
        private readonly StudentServis _studentServis;

        public DataSeedServis(SpolServis spolServis,
            DrzavaServis drzavaServis,
            GradServis gradServis,
            StudijServis studijServis,
            GodinaStudijaServis godinaStudijaServis,
            OsobaServis osobaServis,
            ProfesorServis profesorServis,
            StudentServis studentServis)
        {
            _spolServis = spolServis;
            _drzavaServis = drzavaServis;
            _gradServis = gradServis;
            _studijServis = studijServis;
            _godinaStudijaServis = godinaStudijaServis;
            _osobaServis = osobaServis;
            _studentServis = studentServis;
            _profesorServis = profesorServis;
        }

        private void KreirajAdmina()
        {
            var muskiSpol = _spolServis.GetAll()
                .FirstOrDefault(s => s.Oznaka == 'M');

            var mostar = _gradServis.GetAll()
                .FirstOrDefault(g => g.Naziv == "Mostar");

            if (muskiSpol == null || mostar == null)
                return;


            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(
                Konfiguracija.AdminPassword
            );


            _osobaServis.Add(new Osoba
            {
                Ime = "Sistem",
                Prezime = "Administrator",
                Email = "admin@fit.ba",
                KorisnickoIme = "admin",
                LozinkaHash = hashedPassword,
                JMBG = "0101999170000",
                DatumRodjenja = new DateTime(2003, 1, 1),
                SpolId = muskiSpol.Id,
                GradId = mostar.Id,
                Uloge = Uloge.Admin
            });
        }

        public void SeedujSve()
        {
            //spolovi        
            var spoloviPostoje = _spolServis.GetAll().Any();
            if(!spoloviPostoje)
            {
                KreirajSpolove();
            }

            //drzave ------------------------------------------------------------------
            var drzavePostoje = _drzavaServis.GetAll().Any();
            if (!drzavePostoje)
            {
                KreirajDrzave();
            }

            //gradovi ---------------------------------------------------------------
            var gradoviPostoje = _gradServis.GetAll().Any();
            if (!gradoviPostoje)
            {
                KreirajGradove();
            }

            //smjer ---------------------------------------------------------------------
            var studijPostoje = _studijServis.GetAll().Any();
            if (!studijPostoje)
            {
                KreirajStudije();
            }

            //godine studija ---------------------------------------------------------------------
            var godStudPostoje = _godinaStudijaServis.GetAll().Any();
            if (!godStudPostoje)
            {
                KreirajGodineStudija();
            }

            //admin -----------------------------------------------------------------------
            var adminPostoji = _osobaServis.GetAll()
                .Any(o => o.KorisnickoIme == "admin");

            if (!adminPostoji)
            {
                KreirajAdmina();
            }

            //student -----------------------------------------------------------------------
            var studentPostoji = _studentServis.GetAll().Any(s => s.KorisnickoIme == "student1");

            if (!studentPostoji)
            {
                KreirajStudenta();
            }

            //profesor -----------------------------------------------------------------------
            var profesorPostoji = _profesorServis.GetAll().Any(p => p.KorisnickoIme == "profesor1");

            if (!profesorPostoji)
            {
                KreirajProfesora();
            }
        }

        private void KreirajGodineStudija()
        {
            var razvojB = _studijServis.GetAll()
                    .FirstOrDefault(s => s.Smjer == "Razvoj softvera"
                        && s.Zvanje == "Bachelor");
            if (razvojB != null)
            {
                _godinaStudijaServis.Add(new GodinaStudija
                {
                    Opis = "Prva godina - RS",
                    StudijId = razvojB.Id,
                });
                _godinaStudijaServis.Add(new GodinaStudija
                {
                    Opis = "Druga godina - RS",
                    StudijId = razvojB.Id,
                });
                _godinaStudijaServis.Add(new GodinaStudija
                {
                    Opis = "Treća godina - RS",
                    StudijId = razvojB.Id,
                });
            }

            var inzinjeringB = _studijServis.GetAll()
                .FirstOrDefault(s => s.Smjer == "Softverski inžinjering"
                    && s.Zvanje == "Bachelor");
            if (inzinjeringB != null)
            {
                _godinaStudijaServis.Add(new GodinaStudija
                {
                    Opis = "Prva godina - SI",
                    StudijId = inzinjeringB.Id,
                });
                _godinaStudijaServis.Add(new GodinaStudija
                {
                    Opis = "Druga godina - SI",
                    StudijId = inzinjeringB.Id,
                });
                _godinaStudijaServis.Add(new GodinaStudija
                {
                    Opis = "Treća godina - SI",
                    StudijId = inzinjeringB.Id,
                });
                _godinaStudijaServis.Add(new GodinaStudija
                {
                    Opis = "Četvrta godina - SI",
                    StudijId = inzinjeringB.Id,
                });
            }

            var inzinjeringM = _studijServis.GetAll()
                .FirstOrDefault(s => s.Smjer == "Softverski inžinjering"
                    && s.Zvanje == "Master");
            if (inzinjeringM != null)
            {
                _godinaStudijaServis.Add(new GodinaStudija
                {
                    Opis = "Peta godina - SI",
                    StudijId = inzinjeringM.Id,
                });
            }

            var inzinjeringD = _studijServis.GetAll()
                .FirstOrDefault(s => s.Smjer == "Softverski inžinjering"
                    && s.Zvanje == "Doktorat");
            if (inzinjeringD != null)
            {
                _godinaStudijaServis.Add(new GodinaStudija
                {
                    Opis = "Šesta godina - SI",
                    StudijId = inzinjeringD.Id,
                });
            }
        }

        private void KreirajStudije()
        {
            _studijServis.Add(new Studij { Smjer = "Razvoj softvera", Zvanje = "Bachelor" });
            _studijServis.Add(new Studij { Smjer = "Razvoj softvera", Zvanje = "Master" });
            _studijServis.Add(new Studij { Smjer = "Razvoj softvera", Zvanje = "Doktorat" });
            _studijServis.Add(new Studij { Smjer = "Softverski inžinjering", Zvanje = "Bachelor" });
            _studijServis.Add(new Studij { Smjer = "Softverski inžinjering", Zvanje = "Master" });
            _studijServis.Add(new Studij { Smjer = "Softverski inžinjering", Zvanje = "Doktorat" });
        }

        private void KreirajGradove()
        {
            var bih = _drzavaServis.GetAll().FirstOrDefault(d => d.Oznaka == "BiH");
            if (bih != null)
            {
                _gradServis.Add(new Grad
                {
                    Naziv = "Sarajevo",
                    DrzavaId = bih.Id,
                    Kanton = "SK"
                });
                _gradServis.Add(new Grad
                {
                    Naziv = "Mostar",
                    DrzavaId = bih.Id,
                    Kanton = "HNK"
                });
                _gradServis.Add(new Grad
                {
                    Naziv = "Fojnica",
                    DrzavaId = bih.Id,
                    Kanton = "SBK"
                });
                _gradServis.Add(new Grad
                {
                    Naziv = "Hadžići",
                    DrzavaId = bih.Id,
                    Kanton = "SK"
                });
                _gradServis.Add(new Grad
                {
                    Naziv = "Kiseljak",
                    DrzavaId = bih.Id,
                    Kanton = "SBK"
                });
            }

            var hrv = _drzavaServis.GetAll().FirstOrDefault(d => d.Oznaka == "Hr");
            if (hrv != null)
            {
                _gradServis.Add(new Grad
                {
                    Naziv = "Zagreb",
                    DrzavaId = hrv.Id,
                    Kanton = ""
                });
                _gradServis.Add(new Grad
                {
                    Naziv = "Split",
                    DrzavaId = hrv.Id,
                    Kanton = ""
                });
                _gradServis.Add(new Grad
                {
                    Naziv = "Makarska",
                    DrzavaId = hrv.Id,
                    Kanton = ""
                });
                _gradServis.Add(new Grad
                {
                    Naziv = "Dubrovnik",
                    DrzavaId = hrv.Id,
                    Kanton = ""
                });
            }

            var srb = _drzavaServis.GetAll().FirstOrDefault(d => d.Oznaka == "Srb");
            if (srb != null)
            {
                _gradServis.Add(new Grad
                {
                    Naziv = "Beograd",
                    DrzavaId = srb.Id,
                    Kanton = ""
                });
                _gradServis.Add(new Grad
                {
                    Naziv = "Novi Sad",
                    DrzavaId = srb.Id,
                    Kanton = ""
                });
                _gradServis.Add(new Grad
                {
                    Naziv = "Niš",
                    DrzavaId = srb.Id,
                    Kanton = ""
                });
            }

            var esp = _drzavaServis.GetAll().FirstOrDefault(d => d.Oznaka == "ESP");
            if (esp != null)
            {
                _gradServis.Add(new Grad
                {
                    Naziv = "Barcelona",
                    DrzavaId = esp.Id,
                    Kanton = ""
                });
                _gradServis.Add(new Grad
                {
                    Naziv = "Madrid",
                    DrzavaId = esp.Id,
                    Kanton = ""
                });
                _gradServis.Add(new Grad
                {
                    Naziv = "Bilbao",
                    DrzavaId = esp.Id,
                    Kanton = ""
                });
            }
        }

        private void KreirajDrzave()
        {
            _drzavaServis.Add(new Drzava
            {
                Naziv = "Bosna i Hercegovina",
                Oznaka = "BiH",
                Regija = "Balkan"
            });
            _drzavaServis.Add(new Drzava
            {
                Naziv = "Hrvatska",
                Oznaka = "Hr",
                Regija = "Balkan"
            });
            _drzavaServis.Add(new Drzava
            {
                Naziv = "Srbija",
                Oznaka = "Srb",
                Regija = "Balkan"
            });
            _drzavaServis.Add(new Drzava
            {
                Naziv = "Njemacka",
                Oznaka = "Ger",
                Regija = "Centralna Europa"
            });
            _drzavaServis.Add(new Drzava
            {
                Naziv = "Ujedinjeno Kraljevstvo",
                Oznaka = "UK",
                Regija = "Zapadna Europa"
            });
            _drzavaServis.Add(new Drzava
            {
                Naziv = "Španija",
                Oznaka = "ESP",
                Regija = "Zapadna Europa"
            });
        }

        private void KreirajSpolove()
        {
            _spolServis.Add(new Spol { Naziv = "Muški", Oznaka = 'M' });
            _spolServis.Add(new Spol { Naziv = "Ženski", Oznaka = 'Ž' });
            _spolServis.Add(new Spol { Naziv = "Ostalo", Oznaka = '*' });
        }

        private void KreirajProfesora()
        {
            var muskiSpol = _spolServis.GetAll()
                .FirstOrDefault(s => s.Oznaka == 'M');

            var mostar = _gradServis.GetAll()
                .FirstOrDefault(g => g.Naziv == "Mostar");

            if (muskiSpol == null || mostar == null)
                return;


            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(
                Konfiguracija.Profesor1Password
            );
            string hashedPassword2 = BCrypt.Net.BCrypt.HashPassword(
               Konfiguracija.Profesor2Password
           );

            _profesorServis.Add(new Profesor
            {
                Ime = "profesor",
                Prezime = "prvi",
                Email = "profesor1@fit.ba",
                KorisnickoIme = "profesor1",
                LozinkaHash = hashedPassword,
                JMBG = "0101999170001",
                DatumRodjenja = new DateTime(2003, 1, 1),
                SpolId = muskiSpol.Id,
                GradId = mostar.Id,
                Uloge = Uloge.Profesor,
                Ocjena = 10,
                Plata = 3000,
                Zvanje = Zvanje.RedovniProfesor
            });
            _profesorServis.Add(new Profesor
            {
                Ime = "profesor",
                Prezime = "drugi",
                Email = "profesor2@fit.ba",
                KorisnickoIme = "profesor2",
                LozinkaHash = hashedPassword2,
                JMBG = "6701999170001",
                DatumRodjenja = new DateTime(2003, 1, 1),
                SpolId = muskiSpol.Id,
                GradId = mostar.Id,
                Uloge = Uloge.Profesor,
                Ocjena = 10,
                Plata = 3000,
                Zvanje = Zvanje.RedovniProfesor
            });
        }

        private void KreirajStudenta()
        {
            var muskiSpol = _spolServis.GetAll()
                .FirstOrDefault(s => s.Oznaka == 'M');

            var mostar = _gradServis.GetAll()
                .FirstOrDefault(g => g.Naziv == "Mostar");

            var godStudija = _godinaStudijaServis.GetAll()
                .FirstOrDefault(gs => gs.Opis == "Prva godina - SI");

            if (muskiSpol == null || mostar == null || godStudija == null)
                return;


            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(
                Konfiguracija.Student1Password
            );
            string hashedPassword2 = BCrypt.Net.BCrypt.HashPassword(
               Konfiguracija.Student2Password
           );

            _studentServis.Add(new Student
            {
                Ime = "student",
                Prezime = "prvi",
                Email = "student1@fit.ba",
                KorisnickoIme = "student1",
                LozinkaHash = hashedPassword,
                JMBG = "0101999170000",
                DatumRodjenja = new DateTime(2003, 1, 1),
                SpolId = muskiSpol.Id,
                GradId = mostar.Id,
                Uloge = Uloge.Student,
                DatumUpisa = DateTime.Now,
                GodinaStudijaId = godStudija.Id,
                Indeks = _studentServis.GenerisiIndeks(),
                Status = Status.Samofinancirajuci,
                ZavrsioFakultet = false,
            });

            _studentServis.Add(new Student
            {
                Ime = "student",
                Prezime = "drugi",
                Email = "student2@fit.ba",
                KorisnickoIme = "student2",
                LozinkaHash = hashedPassword2,
                JMBG = "0101999170110",
                DatumRodjenja = new DateTime(2003, 1, 1),
                SpolId = muskiSpol.Id,
                GradId = mostar.Id,
                Uloge = Uloge.Student,
                DatumUpisa = DateTime.Now,
                GodinaStudijaId = godStudija.Id,
                Indeks = _studentServis.GenerisiIndeks(),
                Status = Status.Samofinancirajuci,
                ZavrsioFakultet = false,
            });
        }
    }
}
