using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Dtos.Agent;
using RealEstateApp.Core.Application.Interfaces.Persistence;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Identity.Services;

public class AgentService : IAgentService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPropertyRepository _propertyRepository;

    private const string AgentRole = "Agent";

    public AgentService(UserManager<ApplicationUser> userManager, IPropertyRepository propertyRepository)
    {
        _userManager = userManager;
        _propertyRepository = propertyRepository;
    }

    private async Task<AgentDto> MapToDtoAsync(ApplicationUser user)
    {
        var properties = await _propertyRepository.GetByAgentIdAsync(user.Id);

        return new AgentDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePicturePath = user.ProfilePicturePath,
            Email = user.Email ?? string.Empty,
            Phone = user.Phone,
            PropertiesCount = properties.Count,
            IsActive = user.IsActive
        };
    }

    public async Task<List<AgentDto>> GetActiveAsync()
    {
        var agents = (await _userManager.GetUsersInRoleAsync(AgentRole))
            .Where(a => a.IsActive)
            .OrderBy(a => a.FirstName)
            .ThenBy(a => a.LastName);

        var result = new List<AgentDto>();
        foreach (var agent in agents)
        {
            result.Add(await MapToDtoAsync(agent));
        }
        return result;
    }

    public async Task<List<AgentDto>> SearchActiveByNameAsync(string name)
    {
        var agents = (await _userManager.GetUsersInRoleAsync(AgentRole))
            .Where(a => a.IsActive &&
                (a.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                 a.LastName.Contains(name, StringComparison.OrdinalIgnoreCase)))
            .OrderBy(a => a.FirstName)
            .ThenBy(a => a.LastName);

        var result = new List<AgentDto>();
        foreach (var agent in agents)
        {
            result.Add(await MapToDtoAsync(agent));
        }
        return result;
    }

    public async Task<List<AgentDto>> GetAllAsync()
    {
        var agents = (await _userManager.GetUsersInRoleAsync(AgentRole))
            .OrderBy(a => a.FirstName)
            .ThenBy(a => a.LastName);

        var result = new List<AgentDto>();
        foreach (var agent in agents)
        {
            result.Add(await MapToDtoAsync(agent));
        }
        return result;
    }

    public async Task<AgentDto?> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return null;

        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains(AgentRole)) return null;

        return await MapToDtoAsync(user);
    }

    public async Task ChangeStatusAsync(string id, bool isActive)
    {
        var user = await _userManager.FindByIdAsync(id)
            ?? throw new KeyNotFoundException("The requested agent does not exist.");

        user.IsActive = isActive;
        await _userManager.UpdateAsync(user);
    }

    public async Task DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id)
            ?? throw new KeyNotFoundException("The requested agent does not exist.");

        var properties = await _propertyRepository.GetByAgentIdAsync(id);
        foreach (var property in properties)
        {
            await _propertyRepository.DeleteAsync(property.Id);
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(" ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"It was not possible to delete the agent. {errors}");
        }
    }
}