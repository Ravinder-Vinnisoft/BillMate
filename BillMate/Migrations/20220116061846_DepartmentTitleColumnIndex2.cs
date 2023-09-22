using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class DepartmentTitleColumnIndex2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Titles_Department_DepartmentId",
                table: "Titles");

            migrationBuilder.DropIndex(
                name: "IX_Titles_DepartmentId",
                table: "Titles");

            migrationBuilder.AlterColumn<string>(
                name: "TitleName",
                table: "Titles",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Titles",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Titles_DepartmentId_TitleName",
                table: "Titles",
                columns: new[] { "DepartmentId", "TitleName" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Titles_Department_DepartmentId",
                table: "Titles",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Titles_Department_DepartmentId",
                table: "Titles");

            migrationBuilder.DropIndex(
                name: "IX_Titles_DepartmentId_TitleName",
                table: "Titles");

            migrationBuilder.AlterColumn<string>(
                name: "TitleName",
                table: "Titles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Titles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Titles_DepartmentId",
                table: "Titles",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Titles_Department_DepartmentId",
                table: "Titles",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
