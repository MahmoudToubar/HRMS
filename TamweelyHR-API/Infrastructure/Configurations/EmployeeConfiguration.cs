using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.FullName)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Mobile)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(e => e.DateOfBirth)
                   .IsRequired();

            builder.Property(e => e.IsActive)
                   .HasDefaultValue(true);

            builder.HasOne(e => e.Department)
                   .WithMany()
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Job)
                   .WithMany()
                   .HasForeignKey(e => e.JobId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(j => j.Mobile)
                .IsUnique();

            builder.HasIndex(j => j.Email)
                .IsUnique();
        }
    }
}
