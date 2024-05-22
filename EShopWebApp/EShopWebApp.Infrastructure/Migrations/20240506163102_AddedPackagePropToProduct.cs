using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShopWebApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPackagePropToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PackageId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_PackageId",
                table: "Products",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Packages_PackageId",
                table: "Products",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Packages_PackageId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PackageId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "Products");
        }
    }
}
