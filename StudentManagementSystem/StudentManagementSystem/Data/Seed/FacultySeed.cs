using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data.Seed
{
    public static class FacultySeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
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
    }
}