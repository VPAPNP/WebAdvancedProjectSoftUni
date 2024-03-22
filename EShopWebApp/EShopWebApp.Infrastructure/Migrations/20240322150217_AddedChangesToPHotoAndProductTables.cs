using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShopWebApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedChangesToPHotoAndProductTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Photos_PhotoId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "PhotoId",
                table: "Products",
                newName: "FrontPhotoId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_PhotoId",
                table: "Products",
                newName: "IX_Products_FrontPhotoId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Photos",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<Guid>(
                name: "PhotoId",
                table: "Photos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PhotoId",
                table: "Photos",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Products_PhotoId",
                table: "Photos",
                column: "PhotoId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Photos_FrontPhotoId",
                table: "Products",
                column: "FrontPhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Products_PhotoId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Photos_FrontPhotoId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Photos_PhotoId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "FrontPhotoId",
                table: "Products",
                newName: "PhotoId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_FrontPhotoId",
                table: "Products",
                newName: "IX_Products_PhotoId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Photos",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Photos_PhotoId",
                table: "Products",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
