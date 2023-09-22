using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class ForeignKeyReferencesinTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AssignedEmployee",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AssignedClient",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedClient",
                table: "Tasks",
                column: "AssignedClient");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedEmployee",
                table: "Tasks",
                column: "AssignedEmployee");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Client_AssignedClient",
                table: "Tasks",
                column: "AssignedClient",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Employee_AssignedEmployee",
                table: "Tasks",
                column: "AssignedEmployee",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Client_AssignedClient",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Employee_AssignedEmployee",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AssignedClient",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AssignedEmployee",
                table: "Tasks");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedEmployee",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "AssignedClient",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
