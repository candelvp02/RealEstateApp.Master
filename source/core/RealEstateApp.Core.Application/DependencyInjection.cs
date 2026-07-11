using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Mappings;
using RealEstateApp.Core.Application.Services;

namespace RealEstateApp.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

        services.AddScoped<IPropertyTypeService, PropertyTypeService>();
        services.AddScoped<ISaleTypeService, SaleTypeService>();
        services.AddScoped<IImprovementService, ImprovementService>();
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IOfferService, OfferService>();
        services.AddScoped<IFavoritePropertyService, FavoritePropertyService>();

        return services;
    }
}