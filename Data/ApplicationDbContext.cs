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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
                .HasOne(u => u.Client)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.ClientId)
                .OnDelete(DeleteBehavior.SetNull);

     
            modelBuilder.Entity<AppUser>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<AppUser>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
