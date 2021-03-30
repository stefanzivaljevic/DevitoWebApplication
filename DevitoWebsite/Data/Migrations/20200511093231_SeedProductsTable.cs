using Microsoft.EntityFrameworkCore.Migrations;

namespace DevitoWebsite.Data.Migrations
{
    public partial class SeedProductsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ItemNumber", "Price", "Size", "Title" },
                values: new object[,]
                {
                    { 1, "Duks broj jedan broj jedan duks", 264, 5500.0, "XL", "Duks broj jedan" },
                    { 2, "Duks broj dva broj dva duks", 265, 5000.0, "L", "Duks broj dva" },
                    { 3, "Duks broj tri broj tri duks", 700265, 6000.0, "M", "Duks broj tri" },
                    { 4, "Duks broj cetiri broj cetiri duks", 708265, 6500.0, "S", "Duks broj cetiri" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
