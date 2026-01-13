using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yatzy.Migrations
{
    /// <inheritdoc />
    public partial class wdadwdergølkfjnjfkhjafkhuhiskfifuhuhfudhfurvfug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SUM",
                table: "SpillerTabel");

            migrationBuilder.AlterColumn<int>(
                name: "TotalSum",
                table: "SpillerTabel",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TotalSum",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SUM",
                table: "SpillerTabel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
