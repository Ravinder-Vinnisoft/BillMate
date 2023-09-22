using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class DepartmentTitleColumn2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Titles_Department_DepartmentId",
                table: "Titles");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Titles",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Titles_Department_DepartmentId",
                table: "Titles",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Titles_Department_DepartmentId",
                table: "Titles");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Titles",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Titles_Department_DepartmentId",
                table: "Titles",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
