using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShopWebApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPackageTypeToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PackageType",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageType",
                table: "Products");
        }
    }
}
