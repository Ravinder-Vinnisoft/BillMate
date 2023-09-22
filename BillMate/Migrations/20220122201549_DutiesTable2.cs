using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class DutiesTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Titles_Titles_TitleId",
                table: "Titles");

            migrationBuilder.DropIndex(
                name: "IX_Titles_TitleId",
                table: "Titles");

            migrationBuilder.DropColumn(
                name: "TitleId",
                table: "Titles");

            migrationBuilder.CreateIndex(
                name: "IX_Duty_TitleId",
                table: "Duty",
                column: "TitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Duty_Titles_TitleId",
                table: "Duty",
                column: "TitleId",
                principalTable: "Titles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Duty_Titles_TitleId",
                table: "Duty");

            migrationBuilder.DropIndex(
                name: "IX_Duty_TitleId",
                table: "Duty");

            migrationBuilder.AddColumn<int>(
                name: "TitleId",
                table: "Titles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Titles_TitleId",
                table: "Titles",
                column: "TitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Titles_Titles_TitleId",
                table: "Titles",
                column: "TitleId",
                principalTable: "Titles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
