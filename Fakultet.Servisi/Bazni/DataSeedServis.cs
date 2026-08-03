using Fakultet.Core.Modeli;
using Fakultet.Core.Modeli.Forum;
using Fakultet.Servisi.IServis.FakultetskiProcesi;
using Fakultet.Servisi.IServis.Forum;
using Fakultet.Servisi.IServis.Korisnici;
using Fakultet.Servisi.IServis.Pomocni;

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
        private readonly AsistentServis _asistentServis;
        private readonly PostServis _postServis;
        private readonly PredmetServis _predmetServis;
        private readonly MaterijalServis _materijalServis;
        private readonly ChatPorukaServis _chatPorukaServis;
        private readonly ZahtjevZaPotvrduServis _zahtjevZaPotvrduServis;
        private readonly StudentPredmetServis _studentPredmetServis;
        private readonly IspitServis _ispitServis;
        private readonly StudentIspitServis _studentIspitServis;

        public DataSeedServis(SpolServis spolServis,
            DrzavaServis drzavaServis,
            GradServis gradServis,
            StudijServis studijServis,
            GodinaStudijaServis godinaStudijaServis,
            OsobaServis osobaServis,
            ProfesorServis profesorServis,
            StudentServis studentServis,
            AsistentServis asistentServis,
            PostServis postServis,
            PredmetServis predmetServis,
            MaterijalServis materijalServis,
            ChatPorukaServis chatPorukaServis,
            ZahtjevZaPotvrduServis zahtjevZaPotvrduServis,
            StudentPredmetServis studentPredmetServis,
            IspitServis ispitServis,
            StudentIspitServis studentIspitServis)
        {
            _spolServis = spolServis;
            _drzavaServis = drzavaServis;
            _gradServis = gradServis;
            _studijServis = studijServis;
            _godinaStudijaServis = godinaStudijaServis;
            _osobaServis = osobaServis;
            _studentServis = studentServis;
            _profesorServis = profesorServis;
            _asistentServis = asistentServis;
            _postServis = postServis;
            _predmetServis = predmetServis;
            _materijalServis = materijalServis;
            _chatPorukaServis = chatPorukaServis;
            _zahtjevZaPotvrduServis = zahtjevZaPotvrduServis;
            _studentPredmetServis = studentPredmetServis;
            _ispitServis = ispitServis;
            _studentIspitServis = studentIspitServis;
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

            //asistent -----------------------------------------------------------------------
            var asistentPostoji = _asistentServis.GetAll().Any(p => p.KorisnickoIme == "asistent1");

            if (!asistentPostoji)
            {
                KreirajAsistenta();
            }

            //predmet ------------------------------------------------------------------------
            var predmetPostoji = _predmetServis.GetAll().Any();

            if (!predmetPostoji)
            {
                KreirajPredmete();
            }

            //post ---------------------------------------------------------------------------
            var postPostoji = _postServis.GetAll().Any();

            if (!postPostoji)
            {
                KreirajPostove();
            }

            //materijal -----------------------------------------------------------------------
            var materijalPostoji = _materijalServis.GetAll().Any();

            if (!materijalPostoji)
            {
                KreirajMaterijale();
            }

            // chat poruke -----------------------------------------------------------------------
            var chatPostoji = _chatPorukaServis.GetAll().Any();
            if (!chatPostoji)
            {
                KreirajChatPoruke();
            }

            //potvrde ----------------------------------------------------------------------------
            var zahtjeviPostoje = _zahtjevZaPotvrduServis.GetAll().Any();
            if (!zahtjeviPostoje)
            {
                KreirajZahtjeveZaPotvrde();
            }

            //studentPredmet ----------------------------------------------------------------------------
            var studentPredmetPostoji = _studentPredmetServis.GetAll().Any();
            if (!studentPredmetPostoji)
            {
                GenerisiStudentPredmetVeze();
            }

            //ispiti ------------------------------------------------------------------------------------
            var ispitiPostoje = _ispitServis.GetAll().Any();
            if (!ispitiPostoje)
            {
                GenerisiIspite();
            }

            //prijave ispita (StudentIspit) -------------------------------------------------------------
            var prijavePostoje = _studentIspitServis.GetAll().Any();
            if (!prijavePostoje)
            {
                GenerisiPrijaveIspita();
            }
        }

        public void GenerisiIspite()
        {
            var sviPredmeti = _predmetServis.GetAll();

            foreach (var predmet in sviPredmeti)
            {
                var redovniIspit = new Ispit
                {
                    PredmetId = predmet.Id,
                    DatumOdrzavanja = DateTime.Now.AddDays(10).AddHours(10),
                    BrojZadataka = 5,
                    MaxBrojBodova = 100,
                    Dodatni = false
                };
                _ispitServis.Add(redovniIspit);

                var dodatniIspit = new Ispit
                {
                    PredmetId = predmet.Id,
                    DatumOdrzavanja = DateTime.Now.AddDays(25).AddHours(12),
                    BrojZadataka = 3,
                    MaxBrojBodova = 100,
                    Dodatni = true
                };
                _ispitServis.Add(dodatniIspit);
            }
        }

        public void GenerisiPrijaveIspita()
        {
            var sviStudenti = _studentServis.GetAll();
            var sviIspiti = _ispitServis.GetAll();
            var sveVezeStudentPredmet = _studentPredmetServis.GetAll();

            foreach (var student in sviStudenti)
            {
                var predmetiKojeSlusaIds = sveVezeStudentPredmet
                    .Where(sp => sp.StudentId == student.Id && !sp.Polozio)
                    .Select(sp => sp.PredmetId)
                    .ToList();

                var dostupniIspiti = sviIspiti
                    .Where(i => predmetiKojeSlusaIds.Contains(i.PredmetId) && !i.Dodatni)
                    .ToList();

                foreach (var ispit in dostupniIspiti)
                {
                    bool vecPrijavio = _studentIspitServis.GetAll()
                        .Any(si => si.StudentId == student.Id && si.IspitId == ispit.Id);

                    if (!vecPrijavio)
                    {
                        var novaPrijava = new StudentIspit
                        {
                            StudentId = student.Id,
                            IspitId = ispit.Id,
                            BrojIzlazaka = 1,
                            Komisijski = false,
                            Dodatni = ispit.Dodatni,
                            Cijena = ispit.Dodatni ? 80.00m : 0.00m,
                            Polozio = false,
                            DatumPrijave = DateTime.Now.AddDays(-2)
                        };

                        _studentIspitServis.Add(novaPrijava);
                    }
                }
            }
        }

        public void GenerisiStudentPredmetVeze()
        {
            var studenti = _studentServis.GetAll();
            var sviPredmeti = _predmetServis.GetAll();

            int dodanoVeza = 0;

            foreach (var student in studenti)
            {
                var predmetiZaNjegovuGodinu = sviPredmeti
                    .Where(p => p.GodinaStudijaId == student.GodinaStudijaId)
                    .ToList();

                foreach (var predmet in predmetiZaNjegovuGodinu)
                {
                    bool vecSlusa = _studentPredmetServis.GetAll().Any(sp => sp.StudentId == student.Id && sp.PredmetId == predmet.Id);

                    if (!vecSlusa)
                    {
                        var novaVeza = new StudentPredmet
                        {
                            StudentId = student.Id,
                            PredmetId = predmet.Id,
                            Polozio = false,
                        };

                        _studentPredmetServis.Add(novaVeza);
                        dodanoVeza++;
                    }
                }
            }
        }

        private void KreirajZahtjeveZaPotvrde()
        {
            var student = _studentServis.GetAll().FirstOrDefault();

            if (student == null) return;

            var zahtjevi = new[]
            {
                (
                    Svrha: SvrhaPotvrde.Stipendija,
                    Stanje: StanjePotvrde.Odobrena,
                    Napomena: "Za prijavu na opštinsku stipendiju",
                    DatumPodnosenja: new DateTime(2025, 10, 15, 9, 30, 0),
                    DatumObrade: new DateTime(2025, 10, 16, 11, 0, 0)
                ),
                (
                    Svrha: SvrhaPotvrde.Penzija,
                    Stanje: StanjePotvrde.Odbijena,
                    Napomena: "Porodična penzija",
                    DatumPodnosenja: new DateTime(2026, 2, 5, 14, 15, 0),
                    DatumObrade: new DateTime(2026, 2, 6, 9, 10, 0)
                ),
                (
                    Svrha: SvrhaPotvrde.Ostalo,
                    Stanje: StanjePotvrde.Ponistena,
                    Napomena: "Potvrda za studentsku polikliniku",
                    DatumPodnosenja: new DateTime(2026, 4, 10, 8, 0, 0),
                    DatumObrade: new DateTime(2026, 4, 10, 10, 0, 0) 
                ),
                (
                    Svrha: SvrhaPotvrde.Alimentacija,
                    Stanje: StanjePotvrde.NaCekanju,
                    Napomena: "Za redovan sudski postupak",
                    DatumPodnosenja: DateTime.Now.AddHours(-3), 
                    DatumObrade: (DateTime?)null
                ),
                (
                    Svrha: SvrhaPotvrde.SmjestajUDom,
                    Stanje: StanjePotvrde.NaCekanju,
                    Napomena: "Konkurs za studentski dom Nedžarići",
                    DatumPodnosenja: DateTime.Now.AddMinutes(-45),
                    DatumObrade: (DateTime?)null
                )
            };

            foreach (var z in zahtjevi)
            {
                _zahtjevZaPotvrduServis.Add(new ZahtjevZaPotvrdu
                {
                    StudentId = student.Id,
                    SvrhaPotvrde = z.Svrha,
                    StanjePotvrde = z.Stanje,
                    Napomena = z.Napomena,
                    DatumPodnosenja = z.DatumPodnosenja,
                    DatumObrade = z.DatumObrade,
                });
            }
        }

        private void KreirajChatPoruke()
        {
            var profesor = _osobaServis.GetAll().FirstOrDefault(o => o.Ime == "profesor");

            var studenti = _osobaServis.GetAll().ToList();
            var student1 = studenti.FirstOrDefault();
            var student2 = studenti.Skip(1).FirstOrDefault();

            if (profesor == null || student1 == null || student2 == null)
                return; 

            DateTime vrijemePoruke = new DateTime(2026, 7, 28, 14, 0, 0); 

            // =======================================================================
            // RAZGOVOR 1: 10 poruka (Sve pročitano)
            // =======================================================================
            var razgovor1 = new[]
            {
                (Posiljalac: student1, Tekst: "Poštovani profesore, da li mi možete pojasniti treći zadatak iz zadaće?"),
                (Posiljalac: profesor, Tekst: "Pozdrav. Koji tačno dio trećeg zadatka Vam nije jasan?"),
                (Posiljalac: student1, Tekst: "Nije mi jasno kako da pravilno povežem bazu podataka koristeći Entity Framework."),
                (Posiljalac: profesor, Tekst: "Provjerite da li ste ispravno podesili DbContext klasu i connection string u appsettings.json."),
                (Posiljalac: student1, Tekst: "Connection string je tu, ali mi prilikom pokretanja javlja grešku 'Invalid object name'."),
                (Posiljalac: profesor, Tekst: "To obično znači da Vam nedostaju tabele u bazi. Jeste li pokrenuli migracije?"),
                (Posiljalac: student1, Tekst: "Zaboravio sam ukucati 'Update-Database'. Pokušat ću sada."),
                (Posiljalac: student1, Tekst: "Evo prošlo je, sada sve radi kako treba. Hvala Vam puno!"),
                (Posiljalac: profesor, Tekst: "Odlično. Obratite pažnju i na relacije između tabela za sljedeću zadaću."),
                (Posiljalac: student1, Tekst: "Hoću, pregledat ću materijale koje ste postavili jučer.")
            };

            foreach (var poruka in razgovor1)
            {
                _chatPorukaServis.Add(new ChatPoruka
                {
                    PosiljalacId = poruka.Posiljalac.Id,
                    PrimalacId = poruka.Posiljalac.Id == profesor.Id ? student1.Id : profesor.Id,
                    Sadrzaj = poruka.Tekst,
                    VrijemeSlanja = vrijemePoruke,
                    Procitano = true
                });
                vrijemePoruke = vrijemePoruke.AddMinutes(5); // svaka sljedeca poruka stize 5 minuta kasnije
            }

            vrijemePoruke = new DateTime(2026, 7, 29, 9, 0, 0);

            var razgovor2 = new[]
            {
                (Posiljalac: student2, Tekst: "Poštovani, kada planirate objaviti rezultate ispita?"),
                (Posiljalac: profesor, Tekst: "Pozdrav, rezultati će biti objavljeni večeras najkasnije do 20h."),
                (Posiljalac: student2, Tekst: "U redu, hvala Vam. Da li će biti organizovan uvid u radove?"),
                (Posiljalac: profesor, Tekst: "Da, uvid će biti sutra u 12:00h u kabinetu."),
                (Posiljalac: student2, Tekst: "Može li se uvid obaviti online? Nisam u mogućnosti doći lično na fakultet sutra."),
                (Posiljalac: profesor, Tekst: "Nažalost, pravila fakulteta nalažu da se uvid obavlja isključivo uživo na fakultetu."),
                (Posiljalac: student2, Tekst: "Razumijem. Da li mogu poslati kolegu sa indeksom da samo pogleda gdje sam pogriješio?"),
                (Posiljalac: profesor, Tekst: "Ne, uvidu morate prisustvovati lično. Bodovi se ne mogu korigovati preko posrednika."),
                (Posiljalac: student2, Tekst: "Da li onda postoji opcija da dođem neki drugi dan ove sedmice?"),
                (Posiljalac: profesor, Tekst: "Mogu Vas primiti u petak u 14:00h u svom kabinetu, da li Vam taj termin odgovara?"),
                (Posiljalac: student2, Tekst: "Petak u 14h mi savršeno odgovara, hvala Vam puno na razumijevanju!"),
                (Posiljalac: profesor, Tekst: "Nema na čemu. Molim Vas samo da mi se javite mailom u petak ujutro da potvrdite dolazak."),
                (Posiljalac: student2, Tekst: "Dogovoreno, poslat ću Vam podsjetnik na mail ujutro."),
                (Posiljalac: student2, Tekst: "Samo još jedno kratko pitanje u vezi samog bodovanja, ako nije problem."),
                (Posiljalac: profesor, Tekst: "Slobodno pitajte."),
                (Posiljalac: student2, Tekst: "Da li parcijalni bodovi iz seminarskog rada vrijede i za popravni rok u septembru?"),
                (Posiljalac: profesor, Tekst: "Da, bodovi iz seminarskog rada i prve parcijale se prenose na sve rokove do kraja akademske godine."),
                (Posiljalac: student2, Tekst: "Odlično, onda mi iz drugog dijela treba samo još 15 bodova za prolaz."),
                (Posiljalac: student2, Tekst: "Fokusirat ću se na zadnja 3 poglavlja iz skripte."),
                (Posiljalac: student2, Tekst: "Hvala Vam još jednom profesore na izdvojenom vremenu, vidimo se u petak!")
            };

            for (int i = 0; i < razgovor2.Length; i++)
            {
                var poruka = razgovor2[i];

                //da zadnje 2 poruke budu neprocitane
                bool jeProcitano = i < razgovor2.Length - 2;

                _chatPorukaServis.Add(new ChatPoruka
                {
                    PosiljalacId = poruka.Posiljalac.Id,
                    PrimalacId = poruka.Posiljalac.Id == profesor.Id ? student2.Id : profesor.Id,
                    Sadrzaj = poruka.Tekst,
                    VrijemeSlanja = vrijemePoruke,
                    Procitano = jeProcitano
                });
                vrijemePoruke = vrijemePoruke.AddMinutes(3);
            }
        }

        private void KreirajMaterijale()
        {
            var osoba = _osobaServis.GetAll().FirstOrDefault(o => o.Ime == "profesor");
            var predmet = _predmetServis.GetAll().FirstOrDefault(p => p.Naziv == "Predmet I");

            var pdf = new Materijal
            {
                Naziv = "Skripta za I parcijalni",
                Opis = "Ova skripta obuhvata sve lekcije od 1. do 5. sedmice. Obavezno pročitati poglavlje 3 pred ispit.",
                TipMaterijala = "PDF",
                PutanjaFajla = @"TestniMaterijali\primjer.pdf",
                WebLink = null,
                DatumPostavljanja = new DateTime(2026, 3, 1, 10, 0, 0),
                PredmetId = predmet.Id, 
                OsobaId = osoba.Id  
            };
            var video = new Materijal
            {
                Naziv = "Predavanje: Uvod u C#",
                Opis = "Snimak predavanja održanog preko Teams platforme.",
                TipMaterijala = "Video",
                PutanjaFajla = null,
                WebLink = "https://www.youtube.com/watch?v=dQw4w9WgXcQ&list=RDdQw4w9WgXcQ&start_radio=1",
                DatumPostavljanja = new DateTime(2026, 3, 5, 12, 30, 0),
                PredmetId = predmet.Id,
                OsobaId = osoba.Id
            };
            var link = new Materijal
            {
                Naziv = "Microsoft Dokumentacija za LINQ",
                Opis = "Zvanična dokumentacija. Trebat će vam za izradu projektnog zadatka.",
                TipMaterijala = "Link",
                PutanjaFajla = null,
                WebLink = "https://learn.microsoft.com/en-us/dotnet/csharp/linq/",
                DatumPostavljanja = new DateTime(2026, 3, 10, 8, 15, 0),
                PredmetId = predmet.Id,
                OsobaId = osoba.Id
            };

            _materijalServis.Add(pdf);
            _materijalServis.Add(video);
            _materijalServis.Add(link);
        }

        private void KreirajPredmete()
        {
            var godinaStudija = _godinaStudijaServis.GetAll()
                .FirstOrDefault(gs => gs.Opis == "Prva godina - SI");
            var profesor = _profesorServis.GetAll().FirstOrDefault();

            if (profesor == null || godinaStudija == null)
                return;

            _predmetServis.Add(new Predmet()
            {
                ECTS = 60,
                GodinaStudijaId = godinaStudija.Id,
                Naziv = "Predmet I",
                ProfesorId = profesor.Id
            });
            _predmetServis.Add(new Predmet()
            {
                ECTS = 60,
                GodinaStudijaId = godinaStudija.Id,
                Naziv = "Predmet I",
                ProfesorId = profesor.Id
            });
        }

        private void KreirajPostove()
        {
            var osoba = _osobaServis.GetAll().FirstOrDefault(o => o.Ime == "profesor");
            var predmet = _predmetServis.GetAll().FirstOrDefault(p => p.Naziv == "Predmet I");

            if (osoba == null || predmet == null)
                return;

            _postServis.Add(new Post()
            {
                DatumObjave = DateTime.Now,
                Naslov = "Dobrodošli na FakultetApp forum!",
                Sadrzaj = "Ova stranica će nam služiti za čitanje obavijesti od fakultetske uprave! \n" +
                "Postovi poput objava vezanih za predmete, rezultate ispita i slično!",
                PredmetId = null,
                OsobaId = osoba.Id
            });

            _postServis.Add(new Post()
            {
                DatumObjave = DateTime.Now,
                Naslov = "Ispit iz Predmet I",
                Sadrzaj = "Ispit iz predmeta Predmet I se pomjera na 26.09.2026. Potrebno je nositi indeks i uplatnicu" +
                "u slučaju polaganja komisijskog ispita!",
                PredmetId = predmet.Id,
                OsobaId = osoba.Id
            });

            _postServis.Add(new Post()
            {
                DatumObjave = DateTime.Now,
                Naslov = "Informacije za godišnji odmor",
                Sadrzaj = "Godišnji odmor počinje od 17.07.2026 i trajati će do 25.08.2026!",
                PredmetId = null,
                OsobaId = osoba.Id
            });

            _postServis.Add(new Post()
            {
                DatumObjave = DateTime.Now,
                Naslov = "Ovjera ljetnog semestra",
                Sadrzaj = "Ovjera ljetnog semestra će se vršiti od 01.09.2026. do 10.09.2026. " +
               "Studenti su obavezni dostaviti indeks i uredno popunjene prijavne obrasce.",
                PredmetId = null,
                OsobaId = osoba.Id
            });

            _postServis.Add(new Post()
            {
                DatumObjave = DateTime.Now,
                Naslov = "Rezultati ispita iz Predmet I",
                Sadrzaj = "Rezultati prvog ispitnog roka iz predmeta Predmet I objavljeni su na studentskom portalu. " +
                           "Uvid u radove će se održati u ponedjeljak od 10:00 do 12:00 sati.",
                PredmetId = predmet.Id,
                OsobaId = osoba.Id
            });

            _postServis.Add(new Post()
            {
                DatumObjave = DateTime.Now,
                Naslov = "Obavijest o radu studentske službe",
                Sadrzaj = "Studentska služba će u petak raditi skraćeno od 08:00 do 12:00 sati " +
                           "zbog planiranog održavanja informacionog sistema.",
                PredmetId = null,
                OsobaId = osoba.Id
            });

            _postServis.Add(new Post()
            {
                DatumObjave = DateTime.Now,
                Naslov = "Laboratorijske vježbe iz Predmet I",
                Sadrzaj = "Laboratorijske vježbe iz predmeta Predmet I počinju naredne sedmice. " +
                           "Raspored grupa dostupan je na oglasnoj ploči i studentskom portalu.",
                PredmetId = predmet.Id,
                OsobaId = osoba.Id
            });

            _postServis.Add(new Post()
            {
                DatumObjave = DateTime.Now,
                Naslov = "Poziv na studentsku radionicu",
                Sadrzaj = "Pozivamo sve zainteresovane studente da prisustvuju radionici o razvoju desktop aplikacija " +
                           "u C# i WPF-u. Radionica će se održati u amfiteatru A1 u četvrtak sa početkom u 14:00 sati.",
                PredmetId = null,
                OsobaId = osoba.Id
            });
        }

        private void KreirajAsistenta()
        {
            var muskiSpol = _spolServis.GetAll()
                .FirstOrDefault(s => s.Oznaka == 'M');

            var mostar = _gradServis.GetAll()
                .FirstOrDefault(g => g.Naziv == "Mostar");

            if (muskiSpol == null || mostar == null)
                return;


            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(
                Konfiguracija.Asistent1Password
            );
            string hashedPassword2 = BCrypt.Net.BCrypt.HashPassword(
               Konfiguracija.Asistent2Password
           );

            _asistentServis.Add(new Asistent
            {
                Ime = "Asistent",
                Prezime = "Prvi",
                Email = "asistent1@fit.ba",
                KorisnickoIme = "asistent1",
                LozinkaHash = hashedPassword,
                JMBG = "0101999170301",
                DatumRodjenja = new DateTime(2003, 1, 1),
                SpolId = muskiSpol.Id,
                GradId = mostar.Id,
                Uloge = Uloge.Asistent,
                Plata = 3000,
            });
            _asistentServis.Add(new Asistent
            {
                Ime = "Asistent",
                Prezime = "Drugi",
                Email = "asistent2@fit.ba",
                KorisnickoIme = "asistent2",
                LozinkaHash = hashedPassword2,
                JMBG = "0101999173301",
                DatumRodjenja = new DateTime(2003, 1, 1),
                SpolId = muskiSpol.Id,
                GradId = mostar.Id,
                Uloge = Uloge.Asistent,
                Plata = 3000,
            });
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
