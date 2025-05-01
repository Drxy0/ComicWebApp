using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComicWebApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GeneralOpinion = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "text", nullable: true),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComicSeries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComicSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComicSeries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComicChapters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<float>(type: "real", nullable: false),
                    SeriesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComicChapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComicChapters_ComicSeries_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "ComicSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComicListEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false),
                    ReviewId = table.Column<Guid>(type: "uuid", nullable: false),
                    ComicSeriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComicListEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComicListEntry_ComicSeries_ComicSeriesId",
                        column: x => x.ComicSeriesId,
                        principalTable: "ComicSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComicListEntry_Review_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Review",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComicListEntry_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComicSeriesAppStats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    ReviewCount = table.Column<int>(type: "integer", nullable: false),
                    NumberOfReaders = table.Column<int>(type: "integer", nullable: false),
                    CompletionRate = table.Column<float>(type: "real", nullable: false),
                    DropRate = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComicSeriesAppStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComicSeriesAppStats_ComicSeries_Id",
                        column: x => x.Id,
                        principalTable: "ComicSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComicSeriesMetadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: true),
                    Artist = table.Column<string>(type: "text", nullable: true),
                    Writer = table.Column<string>(type: "text", nullable: true),
                    Penciler = table.Column<string>(type: "text", nullable: true),
                    Inker = table.Column<string>(type: "text", nullable: true),
                    Colorist = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    OriginalLanguage = table.Column<string>(type: "text", nullable: true),
                    PublicationStatus = table.Column<int>(type: "integer", nullable: false),
                    Genres = table.Column<int[]>(type: "integer[]", nullable: false),
                    Themes = table.Column<int[]>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComicSeriesMetadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComicSeriesMetadata_ComicSeries_Id",
                        column: x => x.Id,
                        principalTable: "ComicSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComicPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PageNumber = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComicPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComicPages_ComicChapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "ComicChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComicChapters_SeriesId",
                table: "ComicChapters",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ComicListEntry_ComicSeriesId",
                table: "ComicListEntry",
                column: "ComicSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ComicListEntry_ReviewId",
                table: "ComicListEntry",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_ComicListEntry_UserId",
                table: "ComicListEntry",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ComicPages_ChapterId",
                table: "ComicPages",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_ComicSeries_UserId",
                table: "ComicSeries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComicListEntry");

            migrationBuilder.DropTable(
                name: "ComicPages");

            migrationBuilder.DropTable(
                name: "ComicSeriesAppStats");

            migrationBuilder.DropTable(
                name: "ComicSeriesMetadata");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "ComicChapters");

            migrationBuilder.DropTable(
                name: "ComicSeries");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
