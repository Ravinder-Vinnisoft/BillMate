using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class InvoiceServicesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_Invoices_InvoiceId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Service_InvoiceId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Service");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Service",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Invoices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InvoiceServices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceServices", x => new { x.InvoiceId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_InvoiceServices_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceServices_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceServices_ServiceId",
                table: "InvoiceServices",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceServices");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Invoices");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                table: "Service",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Service_InvoiceId",
                table: "Service",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Invoices_InvoiceId",
                table: "Service",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
