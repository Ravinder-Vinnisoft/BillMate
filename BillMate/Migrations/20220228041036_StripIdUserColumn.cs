using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class StripIdUserColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeId",
                table: "Client");

            migrationBuilder.AddColumn<string>(
                name: "EmailId",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeId",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "StripeId",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "StripeId",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
