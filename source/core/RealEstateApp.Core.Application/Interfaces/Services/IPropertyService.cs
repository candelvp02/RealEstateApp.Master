using RealEstateApp.Core.Application.Dtos.Property;

namespace RealEstateApp.Core.Application.Interfaces.Services;

public interface IPropertyService
{
    Task<List<PropertyDto>> GetAllAsync();
    Task<List<PropertyDto>> GetAvailableAsync();
    Task<List<PropertyDto>> GetAvailableFilteredAsync(PropertyFilterDto filter);
    Task<PropertyDto?> GetByIdAsync(int id);
    Task<PropertyDto?> GetAvailableByCodeAsync(string code);
    Task<List<PropertyDto>> GetByAgentIdAsync(string agentId);
    Task<List<PropertyDto>> GetAvailableByAgentIdAsync(string agentId);
    Task<PropertyDto> CreateAsync(SavePropertyDto dto);
    Task<PropertyDto> UpdateAsync(int id, SavePropertyDto dto);
    Task DeleteAsync(int id);
    Task MarkAsSoldAsync(int id);
}