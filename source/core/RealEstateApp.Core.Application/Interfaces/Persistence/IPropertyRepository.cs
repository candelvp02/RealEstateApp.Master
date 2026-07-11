using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Persistence;

public interface IPropertyRepository : IGenericRepository<Property>
{
    Task<List<Property>> GetAvailableAsync();
    Task<List<Property>> GetAvailableFilteredAsync(PropertyFilterDto filter);
    Task<Property?> GetAvailableByCodeAsync(string code);
    Task<Property?> GetWithDetailsAsync(int id);
    Task<List<Property>> GetByAgentIdAsync(string agentId);
    Task<List<Property>> GetAvailableByAgentIdAsync(string agentId);
    Task<string> GenerateUniqueCodeAsync();
}