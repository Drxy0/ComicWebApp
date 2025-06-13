using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComicWebApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeKeyForPagesColumnPart1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ComicPages",
                table: "ComicPages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComicPages",
                table: "ComicPages",
                columns: new[] { "ChapterId", "PageNumber" });

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ComicPages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ComicPages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComicPages",
                table: "ComicPages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComicPages",
                table: "ComicPages",
                column: "Id");
        }
    }
}
