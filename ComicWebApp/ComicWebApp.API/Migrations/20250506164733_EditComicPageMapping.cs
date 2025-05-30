using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComicWebApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EditComicPageMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComicSeries_Users_UserId",
                table: "ComicSeries");

            migrationBuilder.DropIndex(
                name: "IX_ComicSeries_UserId",
                table: "ComicSeries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ComicSeries");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ComicSeries",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComicSeries_UserId",
                table: "ComicSeries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComicSeries_Users_UserId",
                table: "ComicSeries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
