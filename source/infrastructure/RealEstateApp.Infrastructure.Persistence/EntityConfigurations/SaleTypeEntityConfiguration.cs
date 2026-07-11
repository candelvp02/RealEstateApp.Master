using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Infrastructure.Persistence.EntityConfigurations;

public class SaleTypeEntityConfiguration : IEntityTypeConfiguration<SaleType>
{
    public void Configure(EntityTypeBuilder<SaleType> builder)
    {
        builder.ToTable("SaleTypes");
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(500);
        builder.HasIndex(p => p.Name).IsUnique();
    }
}