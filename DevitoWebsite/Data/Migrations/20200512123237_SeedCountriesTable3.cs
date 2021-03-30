using Microsoft.EntityFrameworkCore.Migrations;

namespace DevitoWebsite.Data.Migrations
{
    public partial class SeedCountriesTable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 5);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "ThreeCountryCode", "Title", "TwoCountryCode" },
                values: new object[] { 5, "RSD", "Srbija", "RS" });
        }
    }
}
