using Fakultet.Core.Modeli;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;
using System;
using System.Collections.Generic;
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



        public DataSeedServis()
        {
            _spolServis = new SpolServis();
            _drzavaServis = new DrzavaServis();
            _gradServis = new GradServis();
            _studijServis = new StudijServis();
            _godinaStudijaServis = new GodinaStudijaServis();
            _osobaServis = new OsobaServis();
        }

        public void SeedujSve()
        {
            //spolovi
            if (_spolServis.GetAll().Count == 0)
            {
                _spolServis.Add(new Spol { Naziv = "Muški", Oznaka = 'M' });
                _spolServis.Add(new Spol { Naziv = "Ženski", Oznaka = 'Ž' });
                _spolServis.Add(new Spol { Naziv = "Ostalo", Oznaka = '*' });
            }


            //drzave ------------------------------------------------------------------
            if (_drzavaServis.GetAll().Count == 0)
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

            //gradovi ---------------------------------------------------------------
            if (_gradServis.GetAll().Count == 0)
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

            //smjer ---------------------------------------------------------------------
            if (_studijServis.GetAll().Count == 0)
            {
                _studijServis.Add(new Studij { Smjer = "Razvoj softvera", Zvanje = "Bachelor" });
                _studijServis.Add(new Studij { Smjer = "Razvoj softvera", Zvanje = "Master" });
                _studijServis.Add(new Studij { Smjer = "Razvoj softvera", Zvanje = "Doktorat" });
                _studijServis.Add(new Studij { Smjer = "Softverski inžinjering", Zvanje = "Bachelor" });
                _studijServis.Add(new Studij { Smjer = "Softverski inžinjering", Zvanje = "Master" });
                _studijServis.Add(new Studij { Smjer = "Softverski inžinjering", Zvanje = "Doktorat" });
            }

            if (_godinaStudijaServis.GetAll().Count == 0)
            {
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

            //admin -----------------------------------------------------------------------
            if (_osobaServis.GetAll().Any(o => o.Uloge == Uloge.Admin))
                return;

            var muskiSpol = _spolServis.GetAll().FirstOrDefault(s => s.Oznaka == 'M');
            var mostar = _gradServis.GetAll().FirstOrDefault(g => g.Naziv == "Mostar");

            if (muskiSpol == null || mostar == null) return;

            // Čitamo tajnu šifru iz appsettings.json i odmah je hashujemo
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Konfiguracija.AdminPassword);

            _osobaServis.Add(new Osoba
            {
                Ime = "Sistem",
                Prezime = "Administrator",
                Email = "admin@fit.ba",
                KorisnickoIme = "admin",
                LozinkaHash = hashedPassword,
                JMBG = "0101999170000", 
                DatumRodjenja = new System.DateTime(2003, 1, 1),
                SpolId = muskiSpol.Id,
                GradId = mostar.Id,
                Uloge = Uloge.Admin 
            });
        }
    }
}
