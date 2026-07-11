using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Infrastructure.Persistence.Context;

public class RealEstateAppContext : DbContext
{
    public RealEstateAppContext(DbContextOptions<RealEstateAppContext> options) : base(options)
    {
    }

    public DbSet<PropertyType> PropertyTypes { get; set; } = default!;
    public DbSet<SaleType> SaleTypes { get; set; } = default!;
    public DbSet<Improvement> Improvements { get; set; } = default!;
    public DbSet<Property> Properties { get; set; } = default!;
    public DbSet<PropertyImage> PropertyImages { get; set; } = default!;
    public DbSet<PropertyImprovement> PropertyImprovements { get; set; } = default!;
    public DbSet<FavoriteProperty> FavoriteProperties { get; set; } = default!;
    public DbSet<Message> Messages { get; set; } = default!;
    public DbSet<Offer> Offers { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RealEstateAppContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}