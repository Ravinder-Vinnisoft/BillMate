using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class UpdateEmployeeTableColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "employementStartDate",
                table: "Employee");

            migrationBuilder.AddColumn<DateTime>(
                name: "EmploymentStartDate",
                table: "Employee",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmploymentStartDate",
                table: "Employee");

            migrationBuilder.AddColumn<DateTime>(
                name: "employementStartDate",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
