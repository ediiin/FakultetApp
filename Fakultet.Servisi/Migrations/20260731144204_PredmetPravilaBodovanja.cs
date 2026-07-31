using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fakultet.Servisi.Migrations
{
    /// <inheritdoc />
    public partial class PredmetPravilaBodovanja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PravilaBodovanja",
                table: "Predmeti",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PravilaBodovanja",
                table: "Predmeti");
        }
    }
}
