namespace RealEstateApp.Core.Application.Dtos.Account;

public class RegisterClientAgentDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string ProfilePicturePath { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}