using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class LinkCompanytoModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Companies_AddressId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_UserId",
                table: "Companies");

            migrationBuilder.CreateIndex(
                name: "IX_Titles_CompanyId",
                table: "Titles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CompanyId",
                table: "Tasks",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_CompanyId",
                table: "Service",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsPreferences_CompanyId",
                table: "NotificationsPreferences",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CompanyId",
                table: "Employee",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Duty_CompanyId",
                table: "Duty",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_CompanyId",
                table: "Document",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_CompanyId",
                table: "Department",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalWriteOffs_CompanyId",
                table: "DentalWriteOffs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalTotalCollections_CompanyId",
                table: "DentalTotalCollections",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalTotalClaims_CompanyId",
                table: "DentalTotalClaims",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalTotalAdjustments_CompanyId",
                table: "DentalTotalAdjustments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalOutstandingPreAuth_CompanyId",
                table: "DentalOutstandingPreAuth",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalOutstandingClaims_CompanyId",
                table: "DentalOutstandingClaims",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalInsuranceSummedByCarrier_CompanyId",
                table: "DentalInsuranceSummedByCarrier",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AddressId",
                table: "Companies",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_UserId",
                table: "Companies",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Client_CompanyId",
                table: "Client",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPreferences_CompanyId",
                table: "BillingPreferences",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressEmployee_CompanyId",
                table: "AddressEmployee",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressEmployee_Companies_CompanyId",
                table: "AddressEmployee",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BillingPreferences_Companies_CompanyId",
                table: "BillingPreferences",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Companies_CompanyId",
                table: "Client",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DentalInsuranceSummedByCarrier_Companies_CompanyId",
                table: "DentalInsuranceSummedByCarrier",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DentalOutstandingClaims_Companies_CompanyId",
                table: "DentalOutstandingClaims",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DentalOutstandingPreAuth_Companies_CompanyId",
                table: "DentalOutstandingPreAuth",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DentalTotalAdjustments_Companies_CompanyId",
                table: "DentalTotalAdjustments",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DentalTotalClaims_Companies_CompanyId",
                table: "DentalTotalClaims",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DentalTotalCollections_Companies_CompanyId",
                table: "DentalTotalCollections",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DentalWriteOffs_Companies_CompanyId",
                table: "DentalWriteOffs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Companies_CompanyId",
                table: "Department",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Companies_CompanyId",
                table: "Document",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Duty_Companies_CompanyId",
                table: "Duty",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Companies_CompanyId",
                table: "Employee",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationsPreferences_Companies_CompanyId",
                table: "NotificationsPreferences",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Companies_CompanyId",
                table: "Service",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Companies_CompanyId",
                table: "Tasks",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Titles_Companies_CompanyId",
                table: "Titles",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressEmployee_Companies_CompanyId",
                table: "AddressEmployee");

            migrationBuilder.DropForeignKey(
                name: "FK_BillingPreferences_Companies_CompanyId",
                table: "BillingPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_Client_Companies_CompanyId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_DentalInsuranceSummedByCarrier_Companies_CompanyId",
                table: "DentalInsuranceSummedByCarrier");

            migrationBuilder.DropForeignKey(
                name: "FK_DentalOutstandingClaims_Companies_CompanyId",
                table: "DentalOutstandingClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_DentalOutstandingPreAuth_Companies_CompanyId",
                table: "DentalOutstandingPreAuth");

            migrationBuilder.DropForeignKey(
                name: "FK_DentalTotalAdjustments_Companies_CompanyId",
                table: "DentalTotalAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_DentalTotalClaims_Companies_CompanyId",
                table: "DentalTotalClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_DentalTotalCollections_Companies_CompanyId",
                table: "DentalTotalCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_DentalWriteOffs_Companies_CompanyId",
                table: "DentalWriteOffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Companies_CompanyId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_Document_Companies_CompanyId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_Duty_Companies_CompanyId",
                table: "Duty");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Companies_CompanyId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationsPreferences_Companies_CompanyId",
                table: "NotificationsPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Companies_CompanyId",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Companies_CompanyId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Titles_Companies_CompanyId",
                table: "Titles");

            migrationBuilder.DropIndex(
                name: "IX_Titles_CompanyId",
                table: "Titles");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_CompanyId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Service_CompanyId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_NotificationsPreferences_CompanyId",
                table: "NotificationsPreferences");

            migrationBuilder.DropIndex(
                name: "IX_Employee_CompanyId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Duty_CompanyId",
                table: "Duty");

            migrationBuilder.DropIndex(
                name: "IX_Document_CompanyId",
                table: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Department_CompanyId",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_DentalWriteOffs_CompanyId",
                table: "DentalWriteOffs");

            migrationBuilder.DropIndex(
                name: "IX_DentalTotalCollections_CompanyId",
                table: "DentalTotalCollections");

            migrationBuilder.DropIndex(
                name: "IX_DentalTotalClaims_CompanyId",
                table: "DentalTotalClaims");

            migrationBuilder.DropIndex(
                name: "IX_DentalTotalAdjustments_CompanyId",
                table: "DentalTotalAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_DentalOutstandingPreAuth_CompanyId",
                table: "DentalOutstandingPreAuth");

            migrationBuilder.DropIndex(
                name: "IX_DentalOutstandingClaims_CompanyId",
                table: "DentalOutstandingClaims");

            migrationBuilder.DropIndex(
                name: "IX_DentalInsuranceSummedByCarrier_CompanyId",
                table: "DentalInsuranceSummedByCarrier");

            migrationBuilder.DropIndex(
                name: "IX_Companies_AddressId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_UserId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Client_CompanyId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_BillingPreferences_CompanyId",
                table: "BillingPreferences");

            migrationBuilder.DropIndex(
                name: "IX_AddressEmployee_CompanyId",
                table: "AddressEmployee");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AddressId",
                table: "Companies",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_UserId",
                table: "Companies",
                column: "UserId");
        }
    }
}
