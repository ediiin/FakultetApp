using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fakultet.Servisi.Migrations
{
    /// <inheritdoc />
    public partial class InicijalnaKreacija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drzave",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Regija = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Oznaka = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drzave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spolovi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Oznaka = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spolovi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Studiji",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Smjer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zvanje = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studiji", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gradovi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DrzavaId = table.Column<int>(type: "int", nullable: false),
                    Kanton = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gradovi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gradovi_Drzave_DrzavaId",
                        column: x => x.DrzavaId,
                        principalTable: "Drzave",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GodineStudija",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudijId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GodineStudija", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GodineStudija_Studiji_StudijId",
                        column: x => x.StudijId,
                        principalTable: "Studiji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Osobe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpolId = table.Column<int>(type: "int", nullable: false),
                    GradId = table.Column<int>(type: "int", nullable: false),
                    DatumRodjenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JMBG = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KorisnickoIme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lozinka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Uloge = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Osobe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Osobe_Gradovi_GradId",
                        column: x => x.GradId,
                        principalTable: "Gradovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Osobe_Spolovi_SpolId",
                        column: x => x.SpolId,
                        principalTable: "Spolovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Asistenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Plata = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asistenti_Osobe_Id",
                        column: x => x.Id,
                        principalTable: "Osobe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatPoruke",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PosiljalacId = table.Column<int>(type: "int", nullable: false),
                    PrimalacId = table.Column<int>(type: "int", nullable: false),
                    Sadrzaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VrijemeSlanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Procitano = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatPoruke", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatPoruke_Osobe_PosiljalacId",
                        column: x => x.PosiljalacId,
                        principalTable: "Osobe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatPoruke_Osobe_PrimalacId",
                        column: x => x.PrimalacId,
                        principalTable: "Osobe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Profesori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Plata = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Zvanje = table.Column<int>(type: "int", nullable: false),
                    Ocjena = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesori", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profesori_Osobe_Id",
                        column: x => x.Id,
                        principalTable: "Osobe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Studenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Indeks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumUpisa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GodinaStudijaId = table.Column<int>(type: "int", nullable: false),
                    ZavrsioFakultet = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Studenti_GodineStudija_GodinaStudijaId",
                        column: x => x.GodinaStudijaId,
                        principalTable: "GodineStudija",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Studenti_Osobe_Id",
                        column: x => x.Id,
                        principalTable: "Osobe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Predmeti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ECTS = table.Column<int>(type: "int", nullable: false),
                    ProfesorId = table.Column<int>(type: "int", nullable: false),
                    GodinaStudijaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predmeti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Predmeti_GodineStudija_GodinaStudijaId",
                        column: x => x.GodinaStudijaId,
                        principalTable: "GodineStudija",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Predmeti_Profesori_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZahtjeviZaPotvrde",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    StanjePotvrde = table.Column<int>(type: "int", nullable: false),
                    SvrhaPotvrde = table.Column<int>(type: "int", nullable: false),
                    DatumPodnosenja = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZahtjeviZaPotvrde", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZahtjeviZaPotvrde_Studenti_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Studenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsistentiPredmeti",
                columns: table => new
                {
                    AsistentId = table.Column<int>(type: "int", nullable: false),
                    PredmetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsistentiPredmeti", x => new { x.AsistentId, x.PredmetId });
                    table.ForeignKey(
                        name: "FK_AsistentiPredmeti_Asistenti_AsistentId",
                        column: x => x.AsistentId,
                        principalTable: "Asistenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AsistentiPredmeti_Predmeti_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmeti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ispiti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PredmetId = table.Column<int>(type: "int", nullable: false),
                    DatumOdrzavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BrojZadataka = table.Column<int>(type: "int", nullable: false),
                    MaxBrojBodova = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ispiti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ispiti_Predmeti_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmeti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materijali",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkDoVidea = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumPostavljanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PredmetId = table.Column<int>(type: "int", nullable: false),
                    OsobaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materijali", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materijali_Osobe_OsobaId",
                        column: x => x.OsobaId,
                        principalTable: "Osobe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materijali_Predmeti_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmeti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Postovi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naslov = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sadrzaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumObjave = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PredmetId = table.Column<int>(type: "int", nullable: true),
                    OsobaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postovi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Postovi_Osobe_OsobaId",
                        column: x => x.OsobaId,
                        principalTable: "Osobe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Postovi_Predmeti_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmeti",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentiPredmeti",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    PredmetId = table.Column<int>(type: "int", nullable: false),
                    Polozio = table.Column<bool>(type: "bit", nullable: false),
                    Ocjena = table.Column<int>(type: "int", nullable: true),
                    BrojBodova = table.Column<int>(type: "int", nullable: false),
                    Napomena = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentiPredmeti", x => new { x.StudentId, x.PredmetId });
                    table.ForeignKey(
                        name: "FK_StudentiPredmeti_Predmeti_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmeti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentiPredmeti_Studenti_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Studenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentiIspiti",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    IspitId = table.Column<int>(type: "int", nullable: false),
                    BrojIzlazaka = table.Column<int>(type: "int", nullable: false),
                    Komisijski = table.Column<bool>(type: "bit", nullable: false),
                    Dodatni = table.Column<bool>(type: "bit", nullable: false),
                    Cijena = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ocjena = table.Column<int>(type: "int", nullable: true),
                    Polozio = table.Column<bool>(type: "bit", nullable: false),
                    DatumPrijave = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentiIspiti", x => new { x.StudentId, x.IspitId });
                    table.ForeignKey(
                        name: "FK_StudentiIspiti_Ispiti_IspitId",
                        column: x => x.IspitId,
                        principalTable: "Ispiti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentiIspiti_Studenti_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Studenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsistentiPredmeti_PredmetId",
                table: "AsistentiPredmeti",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatPoruke_PosiljalacId",
                table: "ChatPoruke",
                column: "PosiljalacId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatPoruke_PrimalacId",
                table: "ChatPoruke",
                column: "PrimalacId");

            migrationBuilder.CreateIndex(
                name: "IX_GodineStudija_StudijId",
                table: "GodineStudija",
                column: "StudijId");

            migrationBuilder.CreateIndex(
                name: "IX_Gradovi_DrzavaId",
                table: "Gradovi",
                column: "DrzavaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ispiti_PredmetId",
                table: "Ispiti",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Materijali_OsobaId",
                table: "Materijali",
                column: "OsobaId");

            migrationBuilder.CreateIndex(
                name: "IX_Materijali_PredmetId",
                table: "Materijali",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Osobe_GradId",
                table: "Osobe",
                column: "GradId");

            migrationBuilder.CreateIndex(
                name: "IX_Osobe_SpolId",
                table: "Osobe",
                column: "SpolId");

            migrationBuilder.CreateIndex(
                name: "IX_Postovi_OsobaId",
                table: "Postovi",
                column: "OsobaId");

            migrationBuilder.CreateIndex(
                name: "IX_Postovi_PredmetId",
                table: "Postovi",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Predmeti_GodinaStudijaId",
                table: "Predmeti",
                column: "GodinaStudijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Predmeti_ProfesorId",
                table: "Predmeti",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_Studenti_GodinaStudijaId",
                table: "Studenti",
                column: "GodinaStudijaId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentiIspiti_IspitId",
                table: "StudentiIspiti",
                column: "IspitId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentiPredmeti_PredmetId",
                table: "StudentiPredmeti",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_ZahtjeviZaPotvrde_StudentId",
                table: "ZahtjeviZaPotvrde",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsistentiPredmeti");

            migrationBuilder.DropTable(
                name: "ChatPoruke");

            migrationBuilder.DropTable(
                name: "Materijali");

            migrationBuilder.DropTable(
                name: "Postovi");

            migrationBuilder.DropTable(
                name: "StudentiIspiti");

            migrationBuilder.DropTable(
                name: "StudentiPredmeti");

            migrationBuilder.DropTable(
                name: "ZahtjeviZaPotvrde");

            migrationBuilder.DropTable(
                name: "Asistenti");

            migrationBuilder.DropTable(
                name: "Ispiti");

            migrationBuilder.DropTable(
                name: "Studenti");

            migrationBuilder.DropTable(
                name: "Predmeti");

            migrationBuilder.DropTable(
                name: "GodineStudija");

            migrationBuilder.DropTable(
                name: "Profesori");

            migrationBuilder.DropTable(
                name: "Studiji");

            migrationBuilder.DropTable(
                name: "Osobe");

            migrationBuilder.DropTable(
                name: "Gradovi");

            migrationBuilder.DropTable(
                name: "Spolovi");

            migrationBuilder.DropTable(
                name: "Drzave");
        }
    }
}
