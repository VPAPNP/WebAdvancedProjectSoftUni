using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShopWebApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeNameProductCartsToProductsCarts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCarts_Carts_CartId",
                table: "ProductCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCarts_Products_ProductId",
                table: "ProductCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCarts",
                table: "ProductCarts");

            migrationBuilder.RenameTable(
                name: "ProductCarts",
                newName: "ProductsCarts");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCarts_CartId",
                table: "ProductsCarts",
                newName: "IX_ProductsCarts_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsCarts",
                table: "ProductsCarts",
                columns: new[] { "ProductId", "CartId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsCarts_Carts_CartId",
                table: "ProductsCarts",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsCarts_Products_ProductId",
                table: "ProductsCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsCarts_Carts_CartId",
                table: "ProductsCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsCarts_Products_ProductId",
                table: "ProductsCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsCarts",
                table: "ProductsCarts");

            migrationBuilder.RenameTable(
                name: "ProductsCarts",
                newName: "ProductCarts");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsCarts_CartId",
                table: "ProductCarts",
                newName: "IX_ProductCarts_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCarts",
                table: "ProductCarts",
                columns: new[] { "ProductId", "CartId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCarts_Carts_CartId",
                table: "ProductCarts",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCarts_Products_ProductId",
                table: "ProductCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
