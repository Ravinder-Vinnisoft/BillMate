using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class EmployeeTaskColumnUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_EmployeeTasks_TaskId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_TaskId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeTasks",
                table: "Employee",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeTasks",
                table: "Employee");

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_TaskId",
                table: "Employee",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_EmployeeTasks_TaskId",
                table: "Employee",
                column: "TaskId",
                principalTable: "EmployeeTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
