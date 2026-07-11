namespace RealEstateApp.Core.Application.Dtos.Account;

public class LoginDto
{
    public string UserNameOrEmail { get; set; } = default!;
    public string Password { get; set; } = default!;
}