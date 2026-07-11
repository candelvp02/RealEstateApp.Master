namespace RealEstateApp.Core.Application.Dtos.Account;

public class JwtDto
{
    public bool IsAuthenticated { get; set; }
    public string Token { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public List<string> Roles { get; set; } = new();
    public DateTime ExpiresAt { get; set; }
    public string? Message { get; set; }
}