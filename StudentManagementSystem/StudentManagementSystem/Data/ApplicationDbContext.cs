using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data.Seed;
using StudentManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace StudentManagementSystem.Data
{
    // Entity Framework Core database context.
    // Responsible for communicating with SQL Server.
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Configures relationships and database rules.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Faculty)
                .WithMany(f => f.Students)
                .HasForeignKey(s => s.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Programme>()
                .HasOne(p => p.Faculty)
                .WithMany(f => f.Programmes)
                .HasForeignKey(p => p.FacultyId);

            FacultySeed.Seed(modelBuilder);

            ProgrammeSeed.Seed(modelBuilder);

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Programme> Programmes { get; set; }
    }
}