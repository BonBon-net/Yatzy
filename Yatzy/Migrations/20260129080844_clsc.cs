using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yatzy.Migrations
{
    /// <inheritdoc />
    public partial class clsc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScoreBoards",
                columns: table => new
                {
                    ScoreBoardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Enere = table.Column<int>(type: "int", nullable: true),
                    Toere = table.Column<int>(type: "int", nullable: true),
                    Treere = table.Column<int>(type: "int", nullable: true),
                    Firere = table.Column<int>(type: "int", nullable: true),
                    Femmere = table.Column<int>(type: "int", nullable: true),
                    Seksere = table.Column<int>(type: "int", nullable: true),
                    BonusValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bonus = table.Column<int>(type: "int", nullable: false),
                    EtPar = table.Column<int>(type: "int", nullable: true),
                    ToPar = table.Column<int>(type: "int", nullable: true),
                    TreEns = table.Column<int>(type: "int", nullable: true),
                    FireEns = table.Column<int>(type: "int", nullable: true),
                    LilleStraight = table.Column<int>(type: "int", nullable: true),
                    StorStraight = table.Column<int>(type: "int", nullable: true),
                    Hus = table.Column<int>(type: "int", nullable: true),
                    Chance = table.Column<int>(type: "int", nullable: true),
                    Yatzy = table.Column<int>(type: "int", nullable: true),
                    TotalSum = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreBoards", x => x.ScoreBoardId);
                });

            migrationBuilder.CreateTable(
                name: "Spillere",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spillere", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpillerSpil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpillerId = table.Column<int>(type: "int", nullable: false),
                    ScoreBoardId = table.Column<int>(type: "int", nullable: false),
                    SpilId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpillerSpil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpillerSpil_ScoreBoards_ScoreBoardId",
                        column: x => x.ScoreBoardId,
                        principalTable: "ScoreBoards",
                        principalColumn: "ScoreBoardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpillerSpil_Spillere_SpillerId",
                        column: x => x.SpillerId,
                        principalTable: "Spillere",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpilTabel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kasted = table.Column<int>(type: "int", nullable: false),
                    SpillerTurIndex = table.Column<int>(type: "int", nullable: false),
                    HighestScorePlayerId = table.Column<int>(type: "int", nullable: false),
                    IsStarted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpilTabel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpilTabel_SpillerSpil_HighestScorePlayerId",
                        column: x => x.HighestScorePlayerId,
                        principalTable: "SpillerSpil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Terninger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiceValue = table.Column<int>(type: "int", nullable: false),
                    IsHeld = table.Column<bool>(type: "bit", nullable: false),
                    SpilId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terninger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Terninger_SpilTabel_SpilId",
                        column: x => x.SpilId,
                        principalTable: "SpilTabel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpillerSpil_ScoreBoardId",
                table: "SpillerSpil",
                column: "ScoreBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_SpillerSpil_SpilId",
                table: "SpillerSpil",
                column: "SpilId");

            migrationBuilder.CreateIndex(
                name: "IX_SpillerSpil_SpillerId",
                table: "SpillerSpil",
                column: "SpillerId");

            migrationBuilder.CreateIndex(
                name: "IX_SpilTabel_HighestScorePlayerId",
                table: "SpilTabel",
                column: "HighestScorePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Terninger_SpilId",
                table: "Terninger",
                column: "SpilId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpillerSpil_SpilTabel_SpilId",
                table: "SpillerSpil",
                column: "SpilId",
                principalTable: "SpilTabel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpillerSpil_ScoreBoards_ScoreBoardId",
                table: "SpillerSpil");

            migrationBuilder.DropForeignKey(
                name: "FK_SpillerSpil_SpilTabel_SpilId",
                table: "SpillerSpil");

            migrationBuilder.DropTable(
                name: "Terninger");

            migrationBuilder.DropTable(
                name: "ScoreBoards");

            migrationBuilder.DropTable(
                name: "SpilTabel");

            migrationBuilder.DropTable(
                name: "SpillerSpil");

            migrationBuilder.DropTable(
                name: "Spillere");
        }
    }
}
