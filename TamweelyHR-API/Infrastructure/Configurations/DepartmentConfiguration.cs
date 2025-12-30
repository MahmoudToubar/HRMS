using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.IsActive)
                   .HasDefaultValue(true);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(d => d.Name)
                .IsUnique();
        }
    }
}
