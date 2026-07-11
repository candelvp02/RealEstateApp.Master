namespace RealEstateApp.Core.Application.Interfaces.Services;

public interface IFavoritePropertyService
{
    Task<List<int>> GetFavoritePropertyIdsAsync(string clientId);
    Task<bool> IsFavoriteAsync(string clientId, int propertyId);
    Task AddAsync(string clientId, int propertyId);
    Task RemoveAsync(string clientId, int propertyId);
}