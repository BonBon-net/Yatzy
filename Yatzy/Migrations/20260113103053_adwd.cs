using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yatzy.Migrations
{
    /// <inheritdoc />
    public partial class adwd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BonusValue",
                table: "SpillerTabel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BonusValue",
                table: "SpillerTabel");
        }
    }
}
