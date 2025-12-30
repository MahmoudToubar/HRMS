using Core.Entities;
using Core.Identity;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class TamweelyHrDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Job> Jobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(EmployeeConfiguration).Assembly
            );
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(DepartmentConfiguration).Assembly
            );
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(JobConfiguration).Assembly
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}
