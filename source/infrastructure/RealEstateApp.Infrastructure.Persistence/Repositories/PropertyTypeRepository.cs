using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Persistence;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Repositories;

public class PropertyTypeRepository : GenericRepository<PropertyType>, IPropertyTypeRepository
{
    public PropertyTypeRepository(RealEstateAppContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
    {
        return await _dbSet.AnyAsync(p =>
            p.Name.ToLower() == name.ToLower() && (excludeId == null || p.Id != excludeId));
    }
}