using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class CreateTaskTableFinal1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskName = table.Column<string>(nullable: false),
                    TaskType = table.Column<string>(nullable: false),
                    TaskNature = table.Column<string>(maxLength: 20, nullable: false),
                    AssignedEmployee = table.Column<string>(nullable: false),
                    AssignedClient = table.Column<string>(nullable: false),
                    TaskTag = table.Column<string>(maxLength: 20, nullable: true),
                    TaskStatus = table.Column<string>(maxLength: 20, nullable: true),
                    createdDate = table.Column<DateTime>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    Comments = table.Column<string>(nullable: false),
                    IsSelfCreated = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskName_TaskType_TaskNature",
                table: "Tasks",
                columns: new[] { "TaskName", "TaskType", "TaskNature" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
