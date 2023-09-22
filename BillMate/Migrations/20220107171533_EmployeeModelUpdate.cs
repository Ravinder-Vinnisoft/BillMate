using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class EmployeeModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Employee",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Document",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Employee");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Document",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
