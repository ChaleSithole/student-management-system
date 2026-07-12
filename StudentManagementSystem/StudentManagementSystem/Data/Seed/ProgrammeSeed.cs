using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data.Seed
{
    public static class ProgrammeSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Programme>().HasData(

                // Faculty 1 - Natural and Agricultural Sciences

                new Programme
                {
                    ProgrammeId = 1,
                    ProgrammeName = "BSc Information Technology",
                    FacultyId = 1
                },

                new Programme
                {
                    ProgrammeId = 2,
                    ProgrammeName = "BSc Computer Science",
                    FacultyId = 1
                },

                new Programme
                {
                    ProgrammeId = 3,
                    ProgrammeName = "BSc Mathematics",
                    FacultyId = 1
                },

                // Faculty 2 - Health Sciences

                new Programme
                {
                    ProgrammeId = 4,
                    ProgrammeName = "Bachelor of Nursing",
                    FacultyId = 2
                },

                new Programme
                {
                    ProgrammeId = 5,
                    ProgrammeName = "Physiotherapy",
                    FacultyId = 2
                },

                // Faculty 3 - Education

                new Programme
                {
                    ProgrammeId = 6,
                    ProgrammeName = "BEd Foundation Phase",
                    FacultyId = 3
                },

                // Faculty 4 - Law

                new Programme
                {
                    ProgrammeId = 7,
                    ProgrammeName = "LLB",
                    FacultyId = 4
                },

                // Faculty 5 - Economic and Management Sciences

                new Programme
                {
                    ProgrammeId = 8,
                    ProgrammeName = "BCom Accounting",
                    FacultyId = 5
                },

                new Programme
                {
                    ProgrammeId = 9,
                    ProgrammeName = "BCom Economics",
                    FacultyId = 5
                }

            );
        }
    }
}