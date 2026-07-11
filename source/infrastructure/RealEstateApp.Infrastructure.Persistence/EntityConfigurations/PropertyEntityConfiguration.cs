using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Infrastructure.Persistence.EntityConfigurations;

public class PropertyEntityConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable("Properties");

        builder.Property(p => p.Code).IsRequired().HasMaxLength(6);
        builder.HasIndex(p => p.Code).IsUnique();

        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        builder.Property(p => p.Size).HasColumnType("decimal(18,2)");
        builder.Property(p => p.Description).IsRequired().HasMaxLength(2000);
        builder.Property(p => p.AgentId).IsRequired();
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasOne(p => p.PropertyType)
            .WithMany(pt => pt.Properties)
            .HasForeignKey(p => p.PropertyTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.SaleType)
            .WithMany(st => st.Properties)
            .HasForeignKey(p => p.SaleTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Images)
            .WithOne(i => i.Property)
            .HasForeignKey(i => i.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.PropertyImprovements)
            .WithOne(pi => pi.Property)
            .HasForeignKey(pi => pi.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.FavoriteProperties)
            .WithOne(f => f.Property)
            .HasForeignKey(f => f.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Messages)
            .WithOne(m => m.Property)
            .HasForeignKey(m => m.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Offers)
            .WithOne(o => o.Property)
            .HasForeignKey(o => o.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}