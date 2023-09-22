﻿// <auto-generated />
using System;
using BillMate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BillMate.Migrations
{
    [DbContext(typeof(BillMateDBContext))]
    [Migration("20211109102503_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BillMate.Models.AddressDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address1")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Address2")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("AddressDetails");
                });

            modelBuilder.Entity("BillMate.Models.AddressEmployee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address1")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Address2")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("AddressEmployee");
                });

            modelBuilder.Entity("BillMate.Models.BillingPreferences", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AcceptsInsurancePayments")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("ApplyPositiveAdjusts")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("InactiveInsuranceHandle")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("InterestPaymentsHandle")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("IsFeesSchedule")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.HasKey("Id");

                    b.ToTable("BillingPreferences");
                });

            modelBuilder.Entity("BillMate.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("BillingDepartmentNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("BillingId")
                        .HasColumnType("int");

                    b.Property<string>("ClaimsSoftware")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("FormOfPayment")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("GoogleDriveLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("NotificationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OnboardDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("PracticeLogoFileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("PracticeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("PrimaryContact")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("ReferralSource")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("BillingId");

                    b.HasIndex("NotificationId");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("BillMate.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("AssignOffices")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientAssigned")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("GoogleDriveLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("ReviewsScheduled")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("TaskId")
                        .HasColumnType("int");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("WorkEmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime>("employementStartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("TaskId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("BillMate.Models.EmployeeTaskLogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssignedTo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("createdBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime>("createdDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("EmployeeTaskLogs");
                });

            modelBuilder.Entity("BillMate.Models.EmployeeTasks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GeneralSoftware")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("Insurance")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("NewChat")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("PatientAccount")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("Preferences")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("ScheduleCall")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("SoftwareTraining")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.HasKey("Id");

                    b.ToTable("EmployeeTasks");
                });

            modelBuilder.Entity("BillMate.Models.NotificationsPreferences", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NotifyAnnouncements")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("NotifyHelpRequest")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("NotifyInvoices")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.HasKey("Id");

                    b.ToTable("NotificationsPreferences");
                });

            modelBuilder.Entity("BillMate.Models.Client", b =>
                {
                    b.HasOne("BillMate.Models.AddressDetails", "AddressDetails")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillMate.Models.BillingPreferences", "BillingPreferences")
                        .WithMany()
                        .HasForeignKey("BillingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillMate.Models.NotificationsPreferences", "NotificationsPreferences")
                        .WithMany()
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BillMate.Models.Employee", b =>
                {
                    b.HasOne("BillMate.Models.AddressEmployee", "AddressEmployee")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillMate.Models.EmployeeTasks", "EmployeeTasks")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
