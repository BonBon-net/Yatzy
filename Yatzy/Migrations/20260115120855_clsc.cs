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
                name: "AllScoreboards",
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
                    table.PrimaryKey("PK_AllScoreboards", x => x.ScoreBoardId);
                });

            migrationBuilder.CreateTable(
                name: "SpilTabel",
                columns: table => new
                {
                    SpilId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NuværendeSpillerIndex = table.Column<int>(type: "int", nullable: false),
                    AntalKast = table.Column<int>(type: "int", nullable: false),
                    SavedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpilTabel", x => x.SpilId);
                });

            migrationBuilder.CreateTable(
                name: "SpillerTabel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScoreBoardId = table.Column<int>(type: "int", nullable: false),
                    SpilId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpillerTabel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpillerTabel_AllScoreboards_ScoreBoardId",
                        column: x => x.ScoreBoardId,
                        principalTable: "AllScoreboards",
                        principalColumn: "ScoreBoardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpillerTabel_SpilTabel_SpilId",
                        column: x => x.SpilId,
                        principalTable: "SpilTabel",
                        principalColumn: "SpilId");
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
                        principalColumn: "SpilId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpillerTabel_ScoreBoardId",
                table: "SpillerTabel",
                column: "ScoreBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_SpillerTabel_SpilId",
                table: "SpillerTabel",
                column: "SpilId");

            migrationBuilder.CreateIndex(
                name: "IX_Terning_SpilId",
                table: "Terning",
                column: "SpilId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpillerTabel");

            migrationBuilder.DropTable(
                name: "Terning");

            migrationBuilder.DropTable(
                name: "AllScoreboards");

            migrationBuilder.DropTable(
                name: "SpilTabel");
        }
    }
}
