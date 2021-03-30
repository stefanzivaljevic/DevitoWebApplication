using Microsoft.EntityFrameworkCore.Migrations;

namespace DevitoWebsite.Data.Migrations
{
    public partial class SeedSizesToProductsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Products_ProductId",
                table: "Sizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes");

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

            migrationBuilder.RenameTable(
                name: "Sizes",
                newName: "Size");

            migrationBuilder.RenameIndex(
                name: "IX_Sizes_ProductId",
                table: "Size",
                newName: "IX_Size_ProductId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Size",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "Size",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Sizes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Image", "ItemNumber", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "Duks broj jedan broj jedan duks", "/lib/images/duks.png", "000000264", 5500.0, "Duks broj jedan" },
                    { 2, "Duks broj dva broj dva duks", "/lib/images/duks.png", "000000265", 5000.0, "Duks broj dva" },
                    { 3, "Duks broj tri broj tri duks", "/lib/images/duks.png", "000700265", 6000.0, "Duks broj tri" },
                    { 4, "Duks broj cetiri broj cetiri duks", "/lib/images/duks.png", "000708265", 6500.0, "Duks broj cetiri" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Products_ProductId",
                table: "Sizes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
