using Microsoft.EntityFrameworkCore.Migrations;

namespace DevitoWebsite.Data.Migrations
{
    public partial class AddSizeTable6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Size",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Size_ProductId",
                table: "Size",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Size_Products_ProductId",
                table: "Size",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Size_Products_ProductId",
                table: "Size");

            migrationBuilder.DropIndex(
                name: "IX_Size_ProductId",
                table: "Size");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Size");
        }
    }
}
