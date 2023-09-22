using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class DutiesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TitleId",
                table: "Titles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Duty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(nullable: false),
                    TitleId = table.Column<int>(nullable: false),
                    StoredTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duty", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Titles_Titles_TitleId",
                table: "Titles");

            migrationBuilder.DropTable(
                name: "Duty");

            migrationBuilder.DropIndex(
                name: "IX_Titles_TitleId",
                table: "Titles");

            migrationBuilder.DropColumn(
                name: "TitleId",
                table: "Titles");
        }
    }
}
