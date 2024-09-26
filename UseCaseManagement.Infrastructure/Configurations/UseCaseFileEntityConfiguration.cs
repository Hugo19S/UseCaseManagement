using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Infrastructure.Configurations;

public class UseCaseFileEntityConfiguration : IEntityTypeConfiguration<UseCaseFile>
{
    public void Configure(EntityTypeBuilder<UseCaseFile> builder)
    {
        builder.Property(p => p.UseCaseId).IsRequired();
        builder.Property(p => p.FileName).IsRequired();
        builder.Property(p => p.FileSize).IsRequired();
        builder.Property(p => p.Uri).IsRequired();
        builder.Property(p => p.Type).IsRequired();
    }
}
