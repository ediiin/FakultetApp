using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fakultet.Servisi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateZahtjeviZaPotvrdu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DatumObrade",
                table: "ZahtjeviZaPotvrde",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Napomena",
                table: "ZahtjeviZaPotvrde",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatumObrade",
                table: "ZahtjeviZaPotvrde");

            migrationBuilder.DropColumn(
                name: "Napomena",
                table: "ZahtjeviZaPotvrde");
        }
    }
}
