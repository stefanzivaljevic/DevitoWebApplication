using Microsoft.EntityFrameworkCore.Migrations;

namespace DevitoWebsite.Data.Migrations
{
    public partial class SeedSizesToProductsTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Size_Products_ProductId",
                table: "Size");

            migrationBuilder.DropForeignKey(
                name: "FK_Size_Products_ProductId1",
                table: "Size");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Size",
                table: "Size");

            migrationBuilder.DropIndex(
                name: "IX_Size_ProductId1",
                table: "Size");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "Size");

            migrationBuilder.RenameTable(
                name: "Size",
                newName: "Sizes");

            migrationBuilder.RenameIndex(
                name: "IX_Size_ProductId",
                table: "Sizes",
                newName: "IX_Sizes_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Products_ProductId",
                table: "Sizes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Products_ProductId",
                table: "Sizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes");

            migrationBuilder.RenameTable(
                name: "Sizes",
                newName: "Size");

            migrationBuilder.RenameIndex(
                name: "IX_Sizes_ProductId",
                table: "Size",
                newName: "IX_Size_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "Size",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Size",
                table: "Size",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Size_ProductId1",
                table: "Size",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Size_Products_ProductId",
                table: "Size",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Size_Products_ProductId1",
                table: "Size",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
