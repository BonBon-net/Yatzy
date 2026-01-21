using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yatzy.Migrations
{
    /// <inheritdoc />
    public partial class upfda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Terning_SpilTabel_SpilId",
                table: "Terning");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Terning",
                table: "Terning");

            migrationBuilder.RenameTable(
                name: "Terning",
                newName: "Terninger");

            migrationBuilder.RenameIndex(
                name: "IX_Terning_SpilId",
                table: "Terninger",
                newName: "IX_Terninger_SpilId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Terninger",
                table: "Terninger",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Terninger_SpilTabel_SpilId",
                table: "Terninger",
                column: "SpilId",
                principalTable: "SpilTabel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Terninger_SpilTabel_SpilId",
                table: "Terninger");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Terninger",
                table: "Terninger");

            migrationBuilder.RenameTable(
                name: "Terninger",
                newName: "Terning");

            migrationBuilder.RenameIndex(
                name: "IX_Terninger_SpilId",
                table: "Terning",
                newName: "IX_Terning_SpilId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Terning",
                table: "Terning",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Terning_SpilTabel_SpilId",
                table: "Terning",
                column: "SpilId",
                principalTable: "SpilTabel",
                principalColumn: "Id");
        }
    }
}
