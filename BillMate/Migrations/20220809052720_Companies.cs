using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class Companies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Titles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Tasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Service",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "NotificationsPreferences",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Employee",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Duty",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Document",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Department",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "DentalWriteOffs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "DentalTotalCollections",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "DentalTotalClaims",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "DentalTotalAdjustments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "DentalOutstandingPreAuth",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "DentalOutstandingClaims",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "DentalInsuranceSummedByCarrier",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Client",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "BillingPreferences",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "AddressEmployee",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "AddressDetails",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Titles");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "NotificationsPreferences");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Duty");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DentalWriteOffs");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DentalTotalCollections");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DentalTotalClaims");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DentalTotalAdjustments");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DentalOutstandingPreAuth");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DentalOutstandingClaims");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "DentalInsuranceSummedByCarrier");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "BillingPreferences");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AddressEmployee");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AddressDetails");
        }
    }
}
