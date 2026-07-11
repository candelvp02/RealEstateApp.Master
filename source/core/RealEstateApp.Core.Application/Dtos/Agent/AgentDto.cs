namespace RealEstateApp.Core.Application.Dtos.Agent;

public class AgentDto
{
    public string Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? ProfilePicturePath { get; set; }
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }
    public int PropertiesCount { get; set; }
    public bool IsActive { get; set; }
}