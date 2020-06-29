using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Exam.Web.Presentation.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    State = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(type: "varchar(256)", nullable: true),
                    Password = table.Column<string>(type: "varchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rounds_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentToGames",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentToGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentToGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentToGames_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RoundId = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(type: "varchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RoundId = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(type: "varchar(256)", nullable: true),
                    StudentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responses_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Responses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CorrectResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    Values = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrectResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorrectResponses_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_RoundId",
                table: "Categories",
                column: "RoundId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CorrectResponses_CategoryId",
                table: "CorrectResponses",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_RoundId",
                table: "Responses",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_StudentId",
                table: "Responses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_GameId",
                table: "Rounds",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentToGames_GameId",
                table: "StudentToGames",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentToGames_StudentId",
                table: "StudentToGames",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorrectResponses");

            migrationBuilder.DropTable(
                name: "Responses");

            migrationBuilder.DropTable(
                name: "StudentToGames");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
