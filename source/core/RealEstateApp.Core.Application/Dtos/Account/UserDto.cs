namespace RealEstateApp.Core.Application.Dtos.Account;

public class UserDto
{
    public string Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }
    public string? ProfilePicturePath { get; set; }
    public string? Cedula { get; set; }
    public bool IsActive { get; set; }
    public string Role { get; set; } = default!;
}