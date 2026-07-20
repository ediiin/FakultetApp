using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fakultet.Servisi.Migrations
{
    /// <inheritdoc />
    public partial class DodanAktivan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Aktivan",
                table: "Osobe",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aktivan",
                table: "Osobe");
        }
    }
}
