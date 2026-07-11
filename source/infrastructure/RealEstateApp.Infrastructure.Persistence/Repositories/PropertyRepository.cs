using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Interfaces.Persistence;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Repositories;

public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
{
    public PropertyRepository(RealEstateAppContext context) : base(context)
    {
    }

    private IQueryable<Property> QueryWithDetails()
    {
        return _dbSet
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Images)
            .Include(p => p.PropertyImprovements)
                .ThenInclude(pi => pi.Improvement);
    }

    public async Task<List<Property>> GetAvailableAsync()
    {
        return await QueryWithDetails()
            .Where(p => p.Status == PropertyStatus.Available)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Property>> GetAvailableFilteredAsync(PropertyFilterDto filter)
    {
        var query = QueryWithDetails().Where(p => p.Status == PropertyStatus.Available);

        if (!string.IsNullOrWhiteSpace(filter.Code))
            query = query.Where(p => p.Code == filter.Code);

        if (filter.PropertyTypeId.HasValue)
            query = query.Where(p => p.PropertyTypeId == filter.PropertyTypeId.Value);

        if (filter.MinPrice.HasValue)
            query = query.Where(p => p.Price >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);

        if (filter.Bedrooms.HasValue)
            query = query.Where(p => p.Bedrooms == filter.Bedrooms.Value);

        if (filter.Bathrooms.HasValue)
            query = query.Where(p => p.Bathrooms == filter.Bathrooms.Value);

        return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
    }

    public async Task<Property?> GetAvailableByCodeAsync(string code)
    {
        return await QueryWithDetails()
            .FirstOrDefaultAsync(p => p.Code == code && p.Status == PropertyStatus.Available);
    }

    public async Task<Property?> GetWithDetailsAsync(int id)
    {
        return await QueryWithDetails().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Property>> GetByAgentIdAsync(string agentId)
    {
        return await QueryWithDetails()
            .Where(p => p.AgentId == agentId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Property>> GetAvailableByAgentIdAsync(string agentId)
    {
        return await QueryWithDetails()
            .Where(p => p.AgentId == agentId && p.Status == PropertyStatus.Available)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<string> GenerateUniqueCodeAsync()
    {
        var random = new Random();
        string code;
        bool exists;

        do
        {
            code = random.Next(100000, 999999).ToString();
            exists = await _dbSet.AnyAsync(p => p.Code == code);
        } while (exists);

        return code;
    }
}