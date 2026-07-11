using Microsoft.EntityFrameworkCore;
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
                .HasForeignKey(s => s.FacultyId);

            modelBuilder.Entity<Faculty>().HasData(

    new Faculty
    {
        FacultyId = 1,
        FacultyName = "Natural and Agricultural Sciences"
    },

    new Faculty
    {
        FacultyId = 2,
        FacultyName = "Health Sciences"
    },

    new Faculty
    {
        FacultyId = 3,
        FacultyName = "Education"
    },

    new Faculty
    {
        FacultyId = 4,
        FacultyName = "Law"
    },

    new Faculty
    {
        FacultyId = 5,
        FacultyName = "Economic and Management Sciences"
    }

);
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
    }
}