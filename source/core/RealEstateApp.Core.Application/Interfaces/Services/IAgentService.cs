using RealEstateApp.Core.Application.Dtos.Agent;

namespace RealEstateApp.Core.Application.Interfaces.Services;

public interface IAgentService
{
    Task<List<AgentDto>> GetActiveAsync();
    Task<List<AgentDto>> SearchActiveByNameAsync(string name);
    Task<List<AgentDto>> GetAllAsync();
    Task<AgentDto?> GetByIdAsync(string id);
    Task ChangeStatusAsync(string id, bool isActive);
    Task DeleteAsync(string id);
}