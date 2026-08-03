using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fakultet.Servisi.Migrations
{
    /// <inheritdoc />
    public partial class DodatniRokUIspit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Dodatni",
                table: "Ispiti",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dodatni",
                table: "Ispiti");
        }
    }
}
