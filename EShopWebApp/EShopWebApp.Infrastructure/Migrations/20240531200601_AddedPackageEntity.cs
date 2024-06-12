using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShopWebApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPackageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PackageType",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "Packages",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageType",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Packages");
        }
    }
}
