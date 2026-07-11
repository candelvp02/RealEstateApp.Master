using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Agent;
using RealEstateApp.Core.Application.ViewModels.Property;

namespace RealEstateApp.WebApp.Controllers;

public class AgentsController : Controller
{
    private readonly IAgentService _agentService;
    private readonly IPropertyService _propertyService;

    public AgentsController(IAgentService agentService, IPropertyService propertyService)
    {
        _agentService = agentService;
        _propertyService = propertyService;
    }

    public async Task<IActionResult> Index(string? name)
    {
        var agents = string.IsNullOrWhiteSpace(name)
            ? await _agentService.GetActiveAsync()
            : await _agentService.SearchActiveByNameAsync(name.Trim());

        if (agents.Count == 0)
        {
            ViewBag.NoResultsMessage = string.IsNullOrWhiteSpace(name)
                ? "There are no active agents registered at this time."
                : "No active agents were found with the entered name.";
        }

        ViewBag.SearchName = name;

        var items = agents.Select(a => new AgentListItemViewModel
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            ProfilePicturePath = a.ProfilePicturePath
        }).ToList();

        return View(items);
    }

    public async Task<IActionResult> Properties(string id)
    {
        var agent = await _agentService.GetByIdAsync(id);
        if (agent is null || !agent.IsActive)
        {
            TempData["ErrorMessage"] = "The requested agent does not exist or is not available.";
            return RedirectToAction(nameof(Index));
        }

        var properties = await _propertyService.GetAvailableByAgentIdAsync(id);

        ViewBag.AgentFullName = $"{agent.FirstName} {agent.LastName}";
        ViewBag.AgentProfilePicturePath = agent.ProfilePicturePath;
        ViewBag.NoPropertiesMessage = properties.Count == 0
            ? "This agent has no available properties at this time."
            : null;

        var items = properties.Select(p => new PropertyListItemViewModel
        {
            Id = p.Id,
            Code = p.Code,
            PropertyTypeName = p.PropertyTypeName,
            SaleTypeName = p.SaleTypeName,
            MainImagePath = p.Images.Select(i => i.Path).FirstOrDefault(),
            Price = p.Price,
            Size = p.Size,
            Bedrooms = p.Bedrooms,
            Bathrooms = p.Bathrooms,
            Status = p.Status
        }).ToList();

        return View(items);
    }
}