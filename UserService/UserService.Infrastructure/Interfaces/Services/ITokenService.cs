using System.Security.Claims;

namespace UserService.Infrastructure.Interfaces.Services;

public interface ITokenService
{
    Task<string> CreateTokenAsync(UserIdentity user);
}