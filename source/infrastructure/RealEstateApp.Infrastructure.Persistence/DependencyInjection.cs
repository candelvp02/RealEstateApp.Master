using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Interfaces.Persistence;
using RealEstateApp.Infrastructure.Persistence.Context;
using RealEstateApp.Infrastructure.Persistence.Repositories;

namespace RealEstateApp.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RealEstateAppContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
        services.AddScoped<ISaleTypeRepository, SaleTypeRepository>();
        services.AddScoped<IImprovementRepository, ImprovementRepository>();
        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IOfferRepository, OfferRepository>();
        services.AddScoped<IFavoritePropertyRepository, FavoritePropertyRepository>();

        return services;
    }
}