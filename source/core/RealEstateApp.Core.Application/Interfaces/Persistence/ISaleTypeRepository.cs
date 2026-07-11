using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Persistence;

public interface ISaleTypeRepository : IGenericRepository<SaleType>
{
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
}