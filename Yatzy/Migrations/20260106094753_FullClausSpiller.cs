using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yatzy.Migrations
{
    /// <inheritdoc />
    public partial class FullClausSpiller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "SpillerTabel");

            migrationBuilder.AddColumn<int>(
                name: "Bonus",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Chance",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Enere",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EtPar",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Femmere",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FireEns",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Firere",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Hus",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LilleStraight",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Seksere",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StorStraight",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToPar",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Toere",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalSum",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TreEns",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Treere",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Yatzy",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bonus",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "Chance",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "Enere",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "EtPar",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "Femmere",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "FireEns",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "Firere",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "Hus",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "LilleStraight",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "Seksere",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "StorStraight",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "ToPar",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "Toere",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "TotalSum",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "TreEns",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "Treere",
                table: "SpillerTabel");

            migrationBuilder.DropColumn(
                name: "Yatzy",
                table: "SpillerTabel");

            migrationBuilder.AddColumn<string>(
                name: "Score",
                table: "SpillerTabel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
