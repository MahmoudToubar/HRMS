using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.Property(j => j.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(j => j.IsActive)
                   .HasDefaultValue(true);

            builder.Property(j => j.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(j => j.Name)
                .IsUnique();
        }
    }
}
