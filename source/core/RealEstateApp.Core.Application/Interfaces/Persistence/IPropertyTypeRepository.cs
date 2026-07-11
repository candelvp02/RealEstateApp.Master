using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Persistence;

public interface IPropertyTypeRepository : IGenericRepository<PropertyType>
{
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
}