using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BillMate.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address1 = table.Column<string>(maxLength: 100, nullable: false),
                    Address2 = table.Column<string>(maxLength: 100, nullable: false),
                    City = table.Column<string>(maxLength: 50, nullable: false),
                    Province = table.Column<string>(maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(maxLength: 20, nullable: false),
                    Country = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressEmployee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address1 = table.Column<string>(maxLength: 100, nullable: false),
                    Address2 = table.Column<string>(maxLength: 100, nullable: false),
                    City = table.Column<string>(maxLength: 50, nullable: false),
                    Province = table.Column<string>(maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(maxLength: 20, nullable: false),
                    Country = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressEmployee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BillingPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsFeesSchedule = table.Column<string>(maxLength: 3, nullable: false),
                    ApplyPositiveAdjusts = table.Column<string>(maxLength: 3, nullable: false),
                    AcceptsInsurancePayments = table.Column<string>(maxLength: 3, nullable: false),
                    InterestPaymentsHandle = table.Column<string>(maxLength: 200, nullable: true),
                    InactiveInsuranceHandle = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTaskLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskName = table.Column<string>(maxLength: 100, nullable: false),
                    Message = table.Column<string>(nullable: false),
                    AssignedTo = table.Column<string>(nullable: false),
                    createdBy = table.Column<string>(maxLength: 100, nullable: false),
                    createdDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTaskLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleCall = table.Column<string>(maxLength: 3, nullable: false),
                    GeneralSoftware = table.Column<string>(maxLength: 3, nullable: false),
                    PatientAccount = table.Column<string>(maxLength: 3, nullable: false),
                    Insurance = table.Column<string>(maxLength: 3, nullable: false),
                    Preferences = table.Column<string>(maxLength: 3, nullable: false),
                    SoftwareTraining = table.Column<string>(maxLength: 3, nullable: false),
                    NewChat = table.Column<string>(maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationsPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotifyInvoices = table.Column<string>(maxLength: 3, nullable: false),
                    NotifyHelpRequest = table.Column<string>(maxLength: 3, nullable: false),
                    NotifyAnnouncements = table.Column<string>(maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationsPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    AddressId = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 20, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    TimeZone = table.Column<string>(maxLength: 100, nullable: false),
                    JobTitle = table.Column<string>(maxLength: 100, nullable: false),
                    WorkEmailAddress = table.Column<string>(maxLength: 100, nullable: false),
                    GoogleDriveLink = table.Column<string>(maxLength: 100, nullable: false),
                    employementStartDate = table.Column<DateTime>(nullable: false),
                    ReviewsScheduled = table.Column<string>(maxLength: 50, nullable: false),
                    ClientAssigned = table.Column<string>(maxLength: 50, nullable: false),
                    AssignOffices = table.Column<string>(nullable: false),
                    TaskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_AddressEmployee_AddressId",
                        column: x => x.AddressId,
                        principalTable: "AddressEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_EmployeeTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "EmployeeTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PracticeName = table.Column<string>(maxLength: 100, nullable: false),
                    AddressId = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 20, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    PrimaryContact = table.Column<string>(maxLength: 100, nullable: false),
                    ClaimsSoftware = table.Column<string>(maxLength: 100, nullable: false),
                    ReferralSource = table.Column<string>(maxLength: 100, nullable: false),
                    OnboardDate = table.Column<DateTime>(nullable: false),
                    FormOfPayment = table.Column<string>(maxLength: 50, nullable: false),
                    TimeZone = table.Column<string>(maxLength: 100, nullable: false),
                    GoogleDriveLink = table.Column<string>(maxLength: 100, nullable: false),
                    BillingDepartmentNumber = table.Column<string>(maxLength: 50, nullable: false),
                    PracticeLogoFileName = table.Column<string>(maxLength: 100, nullable: false),
                    BillingId = table.Column<int>(nullable: false),
                    NotificationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_AddressDetails_AddressId",
                        column: x => x.AddressId,
                        principalTable: "AddressDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Client_BillingPreferences_BillingId",
                        column: x => x.BillingId,
                        principalTable: "BillingPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Client_NotificationsPreferences_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "NotificationsPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_AddressId",
                table: "Client",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_BillingId",
                table: "Client",
                column: "BillingId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_NotificationId",
                table: "Client",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_AddressId",
                table: "Employee",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_TaskId",
                table: "Employee",
                column: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "EmployeeTaskLogs");

            migrationBuilder.DropTable(
                name: "AddressDetails");

            migrationBuilder.DropTable(
                name: "BillingPreferences");

            migrationBuilder.DropTable(
                name: "NotificationsPreferences");

            migrationBuilder.DropTable(
                name: "AddressEmployee");

            migrationBuilder.DropTable(
                name: "EmployeeTasks");
        }
    }
}
