using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShopWebApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedSessionNavPropToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_SessionId",
                table: "ShoppingCarts",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_ShoppingCartSessions_SessionId",
                table: "ShoppingCarts",
                column: "SessionId",
                principalTable: "ShoppingCartSessions",
                principalColumn: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_ShoppingCartSessions_SessionId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_SessionId",
                table: "ShoppingCarts");
        }
    }
}
