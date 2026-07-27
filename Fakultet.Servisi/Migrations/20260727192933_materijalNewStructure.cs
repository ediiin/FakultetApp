using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fakultet.Servisi.Migrations
{
    /// <inheritdoc />
    public partial class materijalNewStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LinkDoVidea",
                table: "Materijali",
                newName: "TipMaterijala");

            migrationBuilder.AddColumn<string>(
                name: "Opis",
                table: "Materijali",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PutanjaFajla",
                table: "Materijali",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebLink",
                table: "Materijali",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Opis",
                table: "Materijali");

            migrationBuilder.DropColumn(
                name: "PutanjaFajla",
                table: "Materijali");

            migrationBuilder.DropColumn(
                name: "WebLink",
                table: "Materijali");

            migrationBuilder.RenameColumn(
                name: "TipMaterijala",
                table: "Materijali",
                newName: "LinkDoVidea");
        }
    }
}
