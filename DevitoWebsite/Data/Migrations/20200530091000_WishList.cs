using Microsoft.EntityFrameworkCore.Migrations;

namespace DevitoWebsite.Data.Migrations
{
    public partial class WishList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_WishList_WishListId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_WishListId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WishListId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "WishListItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: true),
                    WishListId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishListItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WishListItems_WishList_WishListId",
                        column: x => x.WishListId,
                        principalTable: "WishList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WishListItems_ProductId",
                table: "WishListItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WishListItems_WishListId",
                table: "WishListItems",
                column: "WishListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WishListItems");

            migrationBuilder.AddColumn<int>(
                name: "WishListId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_WishListId",
                table: "Products",
                column: "WishListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_WishList_WishListId",
                table: "Products",
                column: "WishListId",
                principalTable: "WishList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
