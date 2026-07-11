using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Persistence;

public interface IFavoritePropertyRepository : IGenericRepository<FavoriteProperty>
{
    Task<FavoriteProperty?> GetByClientAndPropertyAsync(string clientId, int propertyId);
    Task<List<FavoriteProperty>> GetByClientIdAsync(string clientId);
}