using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class Logo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PracticeLogoFileName",
                table: "Client");

            migrationBuilder.AddColumn<string>(
                name: "PracticeLogo",
                table: "Client",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PracticeLogo",
                table: "Client");

            migrationBuilder.AddColumn<string>(
                name: "PracticeLogoFileName",
                table: "Client",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
