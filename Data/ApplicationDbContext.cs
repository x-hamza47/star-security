using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Star_Security.Models;

namespace Star_Security.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<SubService> SubServices { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<ClientAssignment> ClientAssignments { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<VacancyApplication> VacancyApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== AppUser =====
            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Grade)
                .WithMany(g => g.Users)
                .HasForeignKey(u => u.GradeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.HiredFromVacancy)
                .WithMany()
                .HasForeignKey(u => u.HiredFromVacancyId)
                .OnDelete(DeleteBehavior.SetNull);

            // ===== SubService -> Department =====
            modelBuilder.Entity<SubService>()
                .HasOne(ss => ss.Department)
                .WithMany(d => d.SubServices)
                .HasForeignKey(ss => ss.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== Branch -> Region =====
            modelBuilder.Entity<Branch>()
                .HasOne(b => b.Region)
                .WithMany(r => r.Branches)
                .HasForeignKey(b => b.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Vacancy =====
            modelBuilder.Entity<Vacancy>()
                .HasOne(v => v.Department)
                .WithMany(d => d.Vacancies)
                .HasForeignKey(v => v.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vacancy>()
                .HasOne(v => v.SubService)
                .WithMany(ss => ss.Vacancies)
                .HasForeignKey(v => v.SubServiceId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Vacancy>()
                .HasOne(v => v.Grade)
                .WithMany()
                .HasForeignKey(v => v.GradeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== VacancyApplication -> Vacancy =====
            modelBuilder.Entity<VacancyApplication>()
                .HasOne(va => va.Vacancy)
                .WithMany(v => v.Applications)
                .HasForeignKey(va => va.VacancyId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== ClientAssignment =====
            modelBuilder.Entity<ClientAssignment>()
                .HasOne(ca => ca.Client)
                .WithMany(c => c.ClientAssignments)
                .HasForeignKey(ca => ca.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientAssignment>()
                .HasOne(ca => ca.Department)
                .WithMany(d => d.ClientAssignments)
                .HasForeignKey(ca => ca.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientAssignment>()
                .HasOne(ca => ca.Employee)
                .WithMany(e => e.ClientAssignments)
                .HasForeignKey(ca => ca.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            // ===== Timestamps =====
            modelBuilder.Entity<AppUser>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<AppUser>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<VacancyApplication>()
             .Property(v => v.Status)
             .HasConversion<string>()
             .HasMaxLength(20)
             .HasDefaultValue(ApplicationStatus.Pending);

            // Seeders
            // Departments 
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Manned Guarding", Code = "MG", Icon = "security", Description = "Guards, Fire Squad, Dog Squad, Bodyguards" },
                new Department { Id = 2, Name = "Cash Services", Code = "CS", Icon = "payments", Description = "Cash transfer, ATM replenishment, Vaulting & Caretaker" },
                new Department { Id = 3, Name = "Recruitment & Training", Code = "RT", Icon = "people", Description = "Recruit, train, and deploy manpower" },
                new Department { Id = 4, Name = "Electronic Security Systems", Code = "ESS", Icon = "videocam", Description = "Access control, CCTV, fire alarms, intruder alarms, perimeter protection" }
            );

            modelBuilder.Entity<Grade>().HasData(
                new Grade { Id = 1, Name = "Junior", LevelCode = "JNR", Description = "Entry-level grade" },
                new Grade { Id = 2, Name = "Mid-Level", LevelCode = "MID", Description = "Intermediate grade" },
                new Grade { Id = 3, Name = "Senior", LevelCode = "SNR", Description = "Experienced grade" },
                new Grade { Id = 4, Name = "Lead", LevelCode = "LD", Description = "Team lead grade" }
            );

            // SubServices
            modelBuilder.Entity<SubService>().HasData(
                new SubService { Id = 1, DepartmentId = 1, Name = "Guards", Description = "Provide security guards for clients" },
                new SubService { Id = 2, DepartmentId = 1, Name = "Fire Squad", Description = "Specialized fire safety team" },
                new SubService { Id = 3, DepartmentId = 1, Name = "Dog Squad", Description = "Trained security dogs and handlers" },
                new SubService { Id = 4, DepartmentId = 1, Name = "Bodyguards", Description = "Personal protection officers" },
                new SubService { Id = 5, DepartmentId = 2, Name = "Cash Transfer", Description = "Secure cash transfer services" },
                new SubService { Id = 6, DepartmentId = 2, Name = "ATM Replenishment", Description = "ATM cash refill and maintenance" },
                new SubService { Id = 7, DepartmentId = 2, Name = "Vaulting & Processing", Description = "Secure vault and processing operations" },
                new SubService { Id = 8, DepartmentId = 2, Name = "Caretaker Services", Description = "Management of client cash-related operations" },
                new SubService { Id = 9, DepartmentId = 3, Name = "Recruitment", Description = "Hiring suitable manpower" },
                new SubService { Id = 10, DepartmentId = 3, Name = "Training", Description = "Training personnel to client standards" },
                new SubService { Id = 11, DepartmentId = 4, Name = "Access Control Systems", Description = "Installation and maintenance of access systems" },
                new SubService { Id = 12, DepartmentId = 4, Name = "CCTV", Description = "Closed-circuit camera systems" },
                new SubService { Id = 13, DepartmentId = 4, Name = "Fire Alarm Systems", Description = "Fire detection and alarm systems" },
                new SubService { Id = 14, DepartmentId = 4, Name = "Fire Suppression Systems", Description = "Fire suppression installations" },
                new SubService { Id = 15, DepartmentId = 4, Name = "Intruder/Burglar Alarms", Description = "Intruder and burglar alarm systems" },
                new SubService { Id = 16, DepartmentId = 4, Name = "Perimeter Protection Systems", Description = "Fencing and perimeter security" }
            );

            modelBuilder.Entity<Region>().HasData(
                  new Region { Id = 1, Name = "North Region" },
                  new Region { Id = 2, Name = "West Region" },
                  new Region { Id = 3, Name = "East Region" },
                  new Region { Id = 4, Name = "South Region" }
             );

            // Branches
            modelBuilder.Entity<Branch>().HasData(
                // North Region
                new Branch { Id = 1, Name = "Maple Street Branch", Area = "Downtown", ManagerName = "Ali Khan", ContactNumber = "+923001234001", RegionId = 1, Latitude = 33.6844, Longitude = 73.0479, IsActive = true },
                new Branch { Id = 2, Name = "Pine Avenue Branch", Area = "Uptown", ManagerName = "Sara Iqbal", ContactNumber = "+923001234002", RegionId = 1, Latitude = 33.6900, Longitude = 73.0500, IsActive = true },
                new Branch { Id = 3, Name = "Cedar Park Branch", Area = "Midtown", ManagerName = "Ahmed Raza", ContactNumber = "+923001234003", RegionId = 1, Latitude = 33.6950, Longitude = 73.0550, IsActive = true },

                // West Region
                new Branch { Id = 4, Name = "Riverfront Branch", Area = "Industrial Area", ManagerName = "Fatima Noor", ContactNumber = "+923001234004", RegionId = 2, Latitude = 24.8607, Longitude = 67.0011, IsActive = true },
                new Branch { Id = 5, Name = "Sunset Boulevard Branch", Area = "Warehouse District", ManagerName = "Omar Khan", ContactNumber = "+923001234005", RegionId = 2, Latitude = 24.8650, Longitude = 67.0050, IsActive = true },
                new Branch { Id = 6, Name = "Harbor Road Branch", Area = "Dock Area", ManagerName = "Ayesha Malik", ContactNumber = "+923001234006", RegionId = 2, Latitude = 24.8700, Longitude = 67.0100, IsActive = true },

                // East Region
                new Branch { Id = 7, Name = "Tech Park Branch", Area = "Business District", ManagerName = "Bilal Shah", ContactNumber = "+923001234007", RegionId = 3, Latitude = 31.5204, Longitude = 74.3587, IsActive = true },
                new Branch { Id = 8, Name = "Innovation Hub Branch", Area = "Startup Area", ManagerName = "Hina Tariq", ContactNumber = "+923001234008", RegionId = 3, Latitude = 31.5250, Longitude = 74.3600, IsActive = true },
                new Branch { Id = 9, Name = "Silicon Avenue Branch", Area = "Tech Center", ManagerName = "Usman Qadir", ContactNumber = "+923001234009", RegionId = 3, Latitude = 31.5300, Longitude = 74.3650, IsActive = true },

                // South Region
                new Branch { Id = 10, Name = "Market Street Branch", Area = "Commercial Area", ManagerName = "Laila Ahmed", ContactNumber = "+923001234010", RegionId = 4, Latitude = 25.3960, Longitude = 68.3578, IsActive = true },
                new Branch { Id = 11, Name = "Garden View Branch", Area = "Residential Area", ManagerName = "Fahad Khan", ContactNumber = "+923001234011", RegionId = 4, Latitude = 25.4000, Longitude = 68.3600, IsActive = true },
                new Branch { Id = 12, Name = "Central Plaza Branch", Area = "Shopping Area", ManagerName = "Sana Malik", ContactNumber = "+923001234012", RegionId = 4, Latitude = 25.4050, Longitude = 68.3650, IsActive = true }
            );


        }
    }
}
