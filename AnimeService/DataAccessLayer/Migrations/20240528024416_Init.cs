using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Poster = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PosterPartOne = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PosterPartTwo = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PosterPartThree = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountSeasons = table.Column<int>(type: "int", nullable: true),
                    CountParts = table.Column<int>(type: "int", nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfPublish = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Director = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Actors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    TrailerUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgeRestriction = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quality = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Selections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Selections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Studios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AnimeParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    SeasonNumber = table.Column<int>(type: "int", nullable: false),
                    PartNumber = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUri = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeParts_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesAnimes",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesAnimes", x => new { x.AnimeId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_CategoriesAnimes_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriesAnimes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenresAnimes",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenresAnimes", x => new { x.AnimeId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_GenresAnimes_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenresAnimes_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectionAnimes",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    SelectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectionAnimes", x => new { x.AnimeId, x.SelectionId });
                    table.ForeignKey(
                        name: "FK_SelectionAnimes_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectionAnimes_Selections_SelectionId",
                        column: x => x.SelectionId,
                        principalTable: "Selections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudiosAnimes",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    StudioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudiosAnimes", x => new { x.AnimeId, x.StudioId });
                    table.ForeignKey(
                        name: "FK_StudiosAnimes_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudiosAnimes_Studios_StudioId",
                        column: x => x.StudioId,
                        principalTable: "Studios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeRating",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeRating", x => new { x.UserId, x.AnimeId });
                    table.CheckConstraint("CK_Rating_Rate_Range", "[Rate] >= 1 AND [Rate] <= 10");
                    table.ForeignKey(
                        name: "FK_AnimeRating_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeRating_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AnimeId = table.Column<int>(type: "int", nullable: true),
                    AnimePartId = table.Column<int>(type: "int", nullable: true),
                    ParentCommentId = table.Column<int>(type: "int", nullable: true),
                    CountLikes = table.Column<int>(type: "int", nullable: false),
                    CountDislikes = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AnimeParts_AnimePartId",
                        column: x => x.AnimePartId,
                        principalTable: "AnimeParts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentDislikes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentDislikes", x => new { x.UserId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_CommentDislikes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentDislikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "CommentLikes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentLikes", x => new { x.UserId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_CommentLikes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeParts_AnimeId",
                table: "AnimeParts",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRating_AnimeId",
                table: "AnimeRating",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesAnimes_CategoryId",
                table: "CategoriesAnimes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentDislikes_CommentId",
                table: "CommentDislikes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_CommentId",
                table: "CommentLikes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AnimeId",
                table: "Comments",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AnimePartId",
                table: "Comments",
                column: "AnimePartId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GenresAnimes_GenreId",
                table: "GenresAnimes",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectionAnimes_SelectionId",
                table: "SelectionAnimes",
                column: "SelectionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudiosAnimes_StudioId",
                table: "StudiosAnimes",
                column: "StudioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeRating");

            migrationBuilder.DropTable(
                name: "CategoriesAnimes");

            migrationBuilder.DropTable(
                name: "CommentDislikes");

            migrationBuilder.DropTable(
                name: "CommentLikes");

            migrationBuilder.DropTable(
                name: "GenresAnimes");

            migrationBuilder.DropTable(
                name: "SelectionAnimes");

            migrationBuilder.DropTable(
                name: "StudiosAnimes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Selections");

            migrationBuilder.DropTable(
                name: "Studios");

            migrationBuilder.DropTable(
                name: "AnimeParts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Animes");
        }
    }
}
