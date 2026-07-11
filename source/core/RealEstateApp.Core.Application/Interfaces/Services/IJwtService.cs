using RealEstateApp.Core.Application.Dtos.Account;

namespace RealEstateApp.Core.Application.Interfaces.Services;

public interface IJwtService
{
    Task<JwtDto> GenerateTokenAsync(string userId, string userName, string email, List<string> roles);
}