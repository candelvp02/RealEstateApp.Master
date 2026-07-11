using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Infrastructure.Persistence.EntityConfigurations;

public class ImprovementEntityConfiguration : IEntityTypeConfiguration<Improvement>
{
    public void Configure(EntityTypeBuilder<Improvement> builder)
    {
        builder.ToTable("Improvements");
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(500);
        builder.HasIndex(p => p.Name).IsUnique();
    }
}