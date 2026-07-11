namespace RealEstateApp.Core.Application.ViewModels.Agent;

public class AgentListItemViewModel
{
    public string Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? ProfilePicturePath { get; set; }
}