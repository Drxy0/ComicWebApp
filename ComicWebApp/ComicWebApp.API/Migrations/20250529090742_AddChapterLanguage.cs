using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComicWebApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddChapterLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "ComicChapters",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "ComicChapters");
        }
    }
}
