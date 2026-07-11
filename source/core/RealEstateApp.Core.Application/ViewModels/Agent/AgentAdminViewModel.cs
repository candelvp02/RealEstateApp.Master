namespace RealEstateApp.Core.Application.ViewModels.Agent;

public class AgentAdminViewModel
{
    public string Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int PropertiesCount { get; set; }
    public bool IsActive { get; set; }
}