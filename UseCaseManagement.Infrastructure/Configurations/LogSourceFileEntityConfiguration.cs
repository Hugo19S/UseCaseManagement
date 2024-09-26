using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Infrastructure.Configurations;

public class LogSourceFileEntityConfiguration : IEntityTypeConfiguration<LogSourceFile>
{
    public void Configure(EntityTypeBuilder<LogSourceFile> builder)
    {
        builder.Property(p => p.LogSourceId).IsRequired();
        builder.Property(p => p.FileName).IsRequired();
        builder.Property(p => p.FileSize).IsRequired();
        builder.Property(p => p.Uri).IsRequired();
        builder.Property(p => p.Type).IsRequired();
    }
}
