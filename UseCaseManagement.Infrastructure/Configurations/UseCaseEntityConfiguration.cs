using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Infrastructure.Configurations;

public class UseCaseEntityConfiguration : IEntityTypeConfiguration<UseCase>
{
    public void Configure(EntityTypeBuilder<UseCase> builder)
    {
        builder.HasIndex(p => p.Title).IsUnique();
        builder.Property(p => p.Title).IsRequired();
        builder.Property(p => p.Type).IsRequired();
        builder.Property(p => p.Status).IsRequired();
        builder.Property(p => p.Category).IsRequired();
        builder.Property(p => p.Tag).IsRequired(false);
        builder.Property(p => p.Priority).IsRequired();
        builder.Property(p => p.MitreAttacks).IsRequired();
        builder.Property(p => p.Tenants).IsRequired();
        builder.Property(p => p.CreatedBy).IsRequired();
        builder.Property(p => p.UpdatedBy).IsRequired(false);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired(false);
    }
}
