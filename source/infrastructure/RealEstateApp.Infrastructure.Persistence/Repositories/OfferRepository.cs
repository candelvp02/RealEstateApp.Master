using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Persistence;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Repositories;

public class OfferRepository : GenericRepository<Offer>, IOfferRepository
{
    public OfferRepository(RealEstateAppContext context) : base(context)
    {
    }

    public async Task<List<Offer>> GetByPropertyAndClientAsync(int propertyId, string clientId)
    {
        return await _dbSet
            .Where(o => o.PropertyId == propertyId && o.ClientId == clientId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Offer>> GetByPropertyAsync(int propertyId)
    {
        return await _dbSet
            .Where(o => o.PropertyId == propertyId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> HasPendingOfferAsync(int propertyId, string clientId)
    {
        return await _dbSet.AnyAsync(o =>
            o.PropertyId == propertyId && o.ClientId == clientId && o.Status == OfferStatus.Pending);
    }

    public async Task<bool> HasAcceptedOfferAsync(int propertyId)
    {
        return await _dbSet.AnyAsync(o => o.PropertyId == propertyId && o.Status == OfferStatus.Accepted);
    }
}