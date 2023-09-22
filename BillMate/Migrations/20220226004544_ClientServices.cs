using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class ClientServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    BillingPeriodStart = table.Column<DateTime>(nullable: false),
                    BillingPeriodEnd = table.Column<DateTime>(nullable: false),
                    CostMetric = table.Column<string>(nullable: true),
                    IsFlatFee = table.Column<bool>(nullable: false),
                    FeePercentage = table.Column<decimal>(nullable: false),
                    FeeFlatValue = table.Column<decimal>(nullable: false),
                    IsFeeSingleValue = table.Column<bool>(nullable: false),
                    FeeSingleValue = table.Column<decimal>(nullable: false),
                    FeeRanges = table.Column<string>(nullable: true),
                    StoredTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Service_Name",
                table: "Service",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Service");
        }
    }
}
