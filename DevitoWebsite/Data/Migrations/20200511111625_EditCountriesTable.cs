using Microsoft.EntityFrameworkCore.Migrations;

namespace DevitoWebsite.Data.Migrations
{
    public partial class EditCountriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThreeCountryCode",
                table: "Countries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoCountryCode",
                table: "Countries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThreeCountryCode",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "TwoCountryCode",
                table: "Countries");
        }
    }
}
