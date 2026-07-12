using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data.Seed;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

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