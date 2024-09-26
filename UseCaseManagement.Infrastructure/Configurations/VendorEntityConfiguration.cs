using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Infrastructure.Configurations;

public class VendorEntityConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.Property(x => x.Name).IsRequired();
        builder.HasIndex(p => p.Name).IsUnique();
    }
}
