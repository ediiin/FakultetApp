using Fakultet.Core.Modeli;
using Fakultet.Core.Modeli.Forum;
using Microsoft.EntityFrameworkCore;

namespace Fakultet.Servisi.Bazni
{
    public class FakultetAppDbContext : DbContext
    {
        // 1. DEFINISANJE TABELA U BAZI (DbSet)
        public DbSet<Spol> Spolovi { get; set; }
        public DbSet<Drzava> Drzave { get; set; }
        public DbSet<Grad> Gradovi { get; set; }
        public DbSet<Studij> Studiji { get; set; }
        public DbSet<GodinaStudija> GodineStudija { get; set; }

        // Korisnici (EF će prepoznati nasljeđivanje)
        public DbSet<Osoba> Osobe { get; set; }
        public DbSet<Student> Studenti { get; set; }
        public DbSet<Profesor> Profesori { get; set; }
        public DbSet<Asistent> Asistenti { get; set; }

        // Procesi
        public DbSet<Predmet> Predmeti { get; set; }
        public DbSet<AsistentPredmet> AsistentiPredmeti { get; set; }
        public DbSet<StudentPredmet> StudentiPredmeti { get; set; }
        public DbSet<Ispit> Ispiti { get; set; }
        public DbSet<StudentIspit> StudentiIspiti { get; set; }

        // Forum i ostalo
        public DbSet<Post> Postovi { get; set; }
        public DbSet<Materijal> Materijali { get; set; }
        public DbSet<ZahtjevZaPotvrdu> ZahtjeviZaPotvrde { get; set; }
        public DbSet<ChatPoruka> ChatPoruke { get; set; }

        // 2. KONEKCIJA NA BAZU PODATAKA
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Konfiguracija.ConnectionString);
            }
        }

        // 3. FLUENT API – FINO PODEŠAVANJE VEZA I KLJUČEVA
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- KOMPOZITNI KLJUČEVI (Vezne tabele bez jednog Id-a) ---

            // Vezna tabela: AsistentPredmet
            modelBuilder.Entity<AsistentPredmet>()
                .HasKey(ap => new { ap.AsistentId, ap.PredmetId }); // Spajamo dva ID-a u jedan primarni ključ

            // Ručno gašenje kaskadnog brisanja za AsistentPredmet kako SQL Server ne bi paničio
            modelBuilder.Entity<AsistentPredmet>()
                .HasOne(ap => ap.Predmet)
                .WithMany()
                .HasForeignKey(ap => ap.PredmetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AsistentPredmet>()
                .HasOne(ap => ap.Asistent)
                .WithMany()
                .HasForeignKey(ap => ap.AsistentId)
                .OnDelete(DeleteBehavior.Restrict); 

            // Vezna tabela: StudentPredmet
            modelBuilder.Entity<StudentPredmet>()
                .HasKey(sp => new { sp.StudentId, sp.PredmetId });

            // Vezna tabela: StudentIspit
            modelBuilder.Entity<StudentIspit>()
                .HasKey(si => new { si.StudentId, si.IspitId });


            // --- PODEŠAVANJE NASLJEĐIVANJA (Table-per-Type - TPT) ---
            // Pošto Student, Profesor i Asistent nasljeđuju Osobu, ovim govorimo EF-u 
            // da napravi odvojene tabele u bazi koje su povezane preko Id-a sa tabelom Osoba.
            modelBuilder.Entity<Student>().ToTable("Studenti");
            modelBuilder.Entity<Profesor>().ToTable("Profesori");
            modelBuilder.Entity<Asistent>().ToTable("Asistenti");


            // --- DODATNA PRAVILA (Opcionalno, ali sprječava greške u bazi) ---

            // Chat poruke: Isključujemo kaskadno brisanje da SQL ne bi javljao grešku (Multiple Cascade Paths)
            modelBuilder.Entity<ChatPoruka>()
                .HasOne(m => m.Posiljalac)
                .WithMany()
                .HasForeignKey(m => m.PosiljalacId)
                .OnDelete(DeleteBehavior.Restrict); // Ako se obriše osoba, ne briši automatski chat praskom

            modelBuilder.Entity<ChatPoruka>()
                .HasOne(m => m.Primalac)
                .WithMany()
                .HasForeignKey(m => m.PrimalacId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- SREĐIVANJE KASKADA ZA MATERIJALE ---
            modelBuilder.Entity<Materijal>()
                .HasOne(m => m.Predmet)
                .WithMany()
                .HasForeignKey(m => m.PredmetId)
                .OnDelete(DeleteBehavior.Restrict); // Isključujemo kaskadu sa predmeta

            modelBuilder.Entity<Materijal>()
                .HasOne(m => m.Osoba)
                .WithMany()
                .HasForeignKey(m => m.OsobaId)
                .OnDelete(DeleteBehavior.Restrict); // Isključujemo kaskadu sa osobe


            // --- ZA SVAKI SLUČAJ: SREĐIVANJE OSTALIH VEZNIH TABELA ---
            // Student - Predmet
            modelBuilder.Entity<StudentPredmet>()
                .HasOne(sp => sp.Student)
                .WithMany()
                .HasForeignKey(sp => sp.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentPredmet>()
                .HasOne(sp => sp.Predmet)
                .WithMany()
                .HasForeignKey(sp => sp.PredmetId)
                .OnDelete(DeleteBehavior.Restrict);

            // Student - Ispit
            modelBuilder.Entity<StudentIspit>()
                .HasOne(si => si.Student)
                .WithMany()
                .HasForeignKey(si => si.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentIspit>()
                .HasOne(si => si.Ispit)
                .WithMany()
                .HasForeignKey(si => si.IspitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
