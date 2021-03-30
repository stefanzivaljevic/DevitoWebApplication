using Microsoft.EntityFrameworkCore.Migrations;

namespace DevitoWebsite.Data.Migrations
{
    public partial class AddWishListIdToWishListItemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishListItems_WishList_WishListId",
                table: "WishListItems");

            migrationBuilder.AlterColumn<int>(
                name: "WishListId",
                table: "WishListItems",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WishListItems_WishList_WishListId",
                table: "WishListItems",
                column: "WishListId",
                principalTable: "WishList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishListItems_WishList_WishListId",
                table: "WishListItems");

            migrationBuilder.AlterColumn<int>(
                name: "WishListId",
                table: "WishListItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_WishListItems_WishList_WishListId",
                table: "WishListItems",
                column: "WishListId",
                principalTable: "WishList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
