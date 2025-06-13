using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComicWebApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeKeyForPagesColumnPart3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ComicPages",
                table: "ComicPages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ComicPages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComicPages",
                table: "ComicPages",
                columns: new[] { "ChapterId", "PageNumber" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ComicPages",
                table: "ComicPages");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ComicPages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComicPages",
                table: "ComicPages",
                column: "Id");
        }
    }
}
