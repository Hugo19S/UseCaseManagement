using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Infrastructure.Configurations;

public class LogSourceEntityConfiguration : IEntityTypeConfiguration<LogSource>
{
    public void Configure(EntityTypeBuilder<LogSource> builder)
    {
        builder.HasIndex(x => x.Name).IsUnique();
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.Description).IsRequired();
    }
}
