﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WiseProject.Models;

namespace WiseProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<User,Role, int>
    {
        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-WiseProject-68b33dd6-b292-484e-a18c-23bdfd2b3f8d;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<WiseProject.Models.UserRole> UserRole { get; set; }
    }
}