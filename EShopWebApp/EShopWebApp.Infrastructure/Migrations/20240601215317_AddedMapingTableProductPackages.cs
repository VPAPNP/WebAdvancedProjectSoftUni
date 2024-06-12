using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShopWebApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedMapingTableProductPackages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Products_ProductId",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_ProductId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Packages");

            migrationBuilder.CreateTable(
                name: "ProductsPackages",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsPackages", x => new { x.ProductId, x.PackageId });
                    table.ForeignKey(
                        name: "FK_ProductsPackages_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductsPackages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsPackages_PackageId",
                table: "ProductsPackages",
                column: "PackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsPackages");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Packages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packages_ProductId",
                table: "Packages",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Products_ProductId",
                table: "Packages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
