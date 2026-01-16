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
                name: "SpillerTabel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScoreBoardId = table.Column<int>(type: "int", nullable: false)
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpillerTabel_ScoreBoardId",
                table: "SpillerTabel",
                column: "ScoreBoardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpillerTabel");

            migrationBuilder.DropTable(
                name: "AllScoreboards");
        }
    }
}
