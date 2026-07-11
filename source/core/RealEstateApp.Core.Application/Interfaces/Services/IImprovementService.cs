using RealEstateApp.Core.Application.Dtos.Catalog;

namespace RealEstateApp.Core.Application.Interfaces.Services;

public interface IImprovementService
{
    Task<List<ImprovementDto>> GetAllAsync();
    Task<ImprovementDto?> GetByIdAsync(int id);
    Task<ImprovementDto> CreateAsync(ImprovementDto dto);
    Task<ImprovementDto> UpdateAsync(int id, ImprovementDto dto);
    Task DeleteAsync(int id);
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
}