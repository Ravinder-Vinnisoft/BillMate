using BillMate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace BillMate.Data
{
    public class BillMateDBContext : DbContext
    {
        //private int _companyId { get; set; }

        public BillMateDBContext()
            : base()
        {
        }

        public BillMateDBContext(DbContextOptions<BillMateDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasIndex(d => d.Name).IsUnique();
            modelBuilder.Entity<Title>().HasIndex(t => new { t.DepartmentId, t.TitleName }).IsUnique(true);
            modelBuilder.Entity<Service>().HasIndex(d => d.Name).IsUnique();

            modelBuilder.Entity<BillMate.Models.Task>()
            .HasIndex(t => new { t.TaskName, t.TaskType, t.TaskNature })
            .IsUnique(true);

            modelBuilder.Entity<Document>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<Employee>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<AddressEmployee>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<User>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<BillMate.Models.Task>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<Department>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<Title>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<Duty>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<Client>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<AddressDetails>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<BillingPreferences>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<NotificationsPreferences>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<Service>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<EmployeeOffices>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<Invoice>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<InvoiceService>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<PaymentRequested>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<PaymentMade>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<PatientPayment>().Property<DateTime>("StoredTime");
            //modelBuilder.Entity<SchedulerConfiguration>().Property<DateTime>("StoredTime");
            modelBuilder.Entity<ClientDetail>().Property<DateTime>("StoredTime");


            modelBuilder.Entity<DentalPatient>().HasNoKey();

            modelBuilder.Entity<EmployeeOffices>().HasKey(table => new
            {
                table.EmployeeId,
                table.ClientId
            });

            modelBuilder.Entity<InvoiceService>().HasKey(table => new
            {
                table.InvoiceId,
                table.ServiceId
            });

            modelBuilder.Entity<Company>().HasIndex(d => d.Name).IsUnique();
            modelBuilder.Entity<Company>().Property<DateTime>("StoredTime");

            //modelBuilder.Entity<PatientPaymentAttempt>()
            //.HasOne<DentalPatient>()  // assuming you have DentalPatient entity
            //.WithMany()
            //.HasForeignKey(a => new { a.ClientId, a.PatientId });

            //modelBuilder.Entity<Client>().HasQueryFilter(b => EF.Property<int>(b, "CompanyId") == _companyId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("TestsContext");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries().Where(e =>
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                entityEntry.Property("StoredTime").CurrentValue = DateTime.Now;
            }

            return base.SaveChanges();
        }

        public override System.Threading.Tasks.Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries().Where(e =>
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                entityEntry.Property("StoredTime").CurrentValue = DateTime.Now;
            }

            return base.SaveChangesAsync();
        }

        public DbSet<Client> Client { get; set; }
        public DbSet<AddressDetails> AddressDetails { get; set; }
        public DbSet<BillingPreferences> BillingPreferences { get; set; }
        public DbSet<NotificationsPreferences> NotificationsPreferences { get; set; }
        public DbSet<EmployeeTaskLogs> EmployeeTaskLogs { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<AddressEmployee> AddressEmployee { get; set; }
        public DbSet<EmployeeTasks> EmployeeTasks { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<BillMate.Models.Task> Tasks { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Duty> Duty { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceService> InvoiceServices { get; set; }


        public DbSet<EmployeeOffices> EmployeeOffices { get; set; }

        public DbSet<DentalInsuranceSummedByCarrier> DentalInsuranceSummedByCarrier { get; set; }
        public DbSet<DentalOutstandingClaims> DentalOutstandingClaims { get; set; }
        public DbSet<DentalOutstandingPreAuth> DentalOutstandingPreAuth { get; set; }
        public DbSet<DentalTotalAdjustments> DentalTotalAdjustments { get; set; }
        public DbSet<DentalTotalClaims> DentalTotalClaims { get; set; }
        public DbSet<DentalWriteOffs> DentalWriteOffs { get; set; }
        public DbSet<DentalTotalProduction> DentalTotalProduction { get; set; }
        public DbSet<DentalTotalCollection> DentalTotalCollections { get; set; }
        public DbSet<PatientPayment> PatientPayment { get; set; }
        public DbSet<DentalPatient> DentalPatient { get; set; }
        public DbSet<PaymentRequested> PaymentRequested { get; set; }
        public DbSet<PaymentMade> PaymentMade { get; set; }
        public DbSet<SchedulerConfiguration> SchedulerConfiguration { get; set; }
        public DbSet<ClientDetail> ClientDetails { get; set; }

        public DbSet<PatientPaymentAttempt> PatientPaymentAttempt { get; set; }
        public DbSet<ShortenedURL> ShortenedURL { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}
