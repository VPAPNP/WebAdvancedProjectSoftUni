using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShopWebApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePhotoPropPhotoIdToProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Products_PhotoId",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "PhotoId",
                table: "Photos",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_PhotoId",
                table: "Photos",
                newName: "IX_Photos_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Products_ProductId",
                table: "Photos",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Products_ProductId",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Photos",
                newName: "PhotoId");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_ProductId",
                table: "Photos",
                newName: "IX_Photos_PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Products_PhotoId",
                table: "Photos",
                column: "PhotoId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
