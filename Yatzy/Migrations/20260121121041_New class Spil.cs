using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yatzy.Migrations
{
    /// <inheritdoc />
    public partial class NewclassSpil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpillerTabel_AllScoreboards_ScoreBoardId",
                table: "SpillerTabel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpillerTabel",
                table: "SpillerTabel");

            migrationBuilder.DropIndex(
                name: "IX_SpillerTabel_ScoreBoardId",
                table: "SpillerTabel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllScoreboards",
                table: "AllScoreboards");

            migrationBuilder.DropColumn(
                name: "ScoreBoardId",
                table: "SpillerTabel");

            migrationBuilder.RenameTable(
                name: "SpillerTabel",
                newName: "Spillere");

            migrationBuilder.RenameTable(
                name: "AllScoreboards",
                newName: "ScoreBoards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Spillere",
                table: "Spillere",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScoreBoards",
                table: "ScoreBoards",
                column: "ScoreBoardId");

            migrationBuilder.CreateTable(
                name: "SpilTabel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kasted = table.Column<int>(type: "int", nullable: false),
                    SpillerTurIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpilTabel", x => x.Id);
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
                        name: "FK_SpillerSpil_SpilTabel_SpilId",
                        column: x => x.SpilId,
                        principalTable: "SpilTabel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpillerSpil_Spillere_SpillerId",
                        column: x => x.SpillerId,
                        principalTable: "Spillere",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Terning",
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
                    table.PrimaryKey("PK_Terning", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Terning_SpilTabel_SpilId",
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
                name: "IX_Terning_SpilId",
                table: "Terning",
                column: "SpilId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpillerSpil");

            migrationBuilder.DropTable(
                name: "Terning");

            migrationBuilder.DropTable(
                name: "SpilTabel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Spillere",
                table: "Spillere");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScoreBoards",
                table: "ScoreBoards");

            migrationBuilder.RenameTable(
                name: "Spillere",
                newName: "SpillerTabel");

            migrationBuilder.RenameTable(
                name: "ScoreBoards",
                newName: "AllScoreboards");

            migrationBuilder.AddColumn<int>(
                name: "ScoreBoardId",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpillerTabel",
                table: "SpillerTabel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllScoreboards",
                table: "AllScoreboards",
                column: "ScoreBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_SpillerTabel_ScoreBoardId",
                table: "SpillerTabel",
                column: "ScoreBoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpillerTabel_AllScoreboards_ScoreBoardId",
                table: "SpillerTabel",
                column: "ScoreBoardId",
                principalTable: "AllScoreboards",
                principalColumn: "ScoreBoardId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
