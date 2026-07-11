using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Infrastructure.Persistence.EntityConfigurations;

public class FavoritePropertyEntityConfiguration : IEntityTypeConfiguration<FavoriteProperty>
{
    public void Configure(EntityTypeBuilder<FavoriteProperty> builder)
    {
        builder.ToTable("FavoriteProperties");
        builder.Property(f => f.ClientId).IsRequired();
        builder.HasIndex(f => new { f.ClientId, f.PropertyId }).IsUnique();
    }
}