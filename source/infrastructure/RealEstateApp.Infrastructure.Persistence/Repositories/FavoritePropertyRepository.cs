using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Persistence;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Repositories;

public class FavoritePropertyRepository : GenericRepository<FavoriteProperty>, IFavoritePropertyRepository
{
    public FavoritePropertyRepository(RealEstateAppContext context) : base(context)
    {
    }

    public async Task<FavoriteProperty?> GetByClientAndPropertyAsync(string clientId, int propertyId)
    {
        return await _dbSet.FirstOrDefaultAsync(f => f.ClientId == clientId && f.PropertyId == propertyId);
    }

    public async Task<List<FavoriteProperty>> GetByClientIdAsync(string clientId)
    {
        return await _dbSet.Where(f => f.ClientId == clientId).ToListAsync();
    }
}