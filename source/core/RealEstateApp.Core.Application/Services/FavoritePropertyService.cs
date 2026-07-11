using RealEstateApp.Core.Application.Interfaces.Persistence;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Services;

public class FavoritePropertyService : IFavoritePropertyService
{
    private readonly IFavoritePropertyRepository _repository;

    public FavoritePropertyService(IFavoritePropertyRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<int>> GetFavoritePropertyIdsAsync(string clientId)
    {
        var list = await _repository.GetByClientIdAsync(clientId);
        return list.Select(f => f.PropertyId).ToList();
    }

    public async Task<bool> IsFavoriteAsync(string clientId, int propertyId)
    {
        var favorite = await _repository.GetByClientAndPropertyAsync(clientId, propertyId);
        return favorite is not null;
    }

    public async Task AddAsync(string clientId, int propertyId)
    {
        var existing = await _repository.GetByClientAndPropertyAsync(clientId, propertyId);
        if (existing is not null)
            throw new InvalidOperationException("You already have this property added as a favorite.");

        await _repository.AddAsync(new FavoriteProperty { ClientId = clientId, PropertyId = propertyId });
    }

    public async Task RemoveAsync(string clientId, int propertyId)
    {
        var existing = await _repository.GetByClientAndPropertyAsync(clientId, propertyId);
        if (existing is not null)
        {
            await _repository.DeleteAsync(existing.Id);
        }
    }
}