using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class ClientReferenceDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Document",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Document_ClientId",
                table: "Document",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Client_ClientId",
                table: "Document",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Client_ClientId",
                table: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Document_ClientId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Document");
        }
    }
}
