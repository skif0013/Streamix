using System.Security.Claims;
using UserService.Application.DTO;
using UserService.Core.Models;
using UserService.Core.Results;

namespace UserService.Infrastructure.Interfaces.Services;

public interface ITokenService
{
    Task<string> CreateTokenAsync(UserIdentity user);
    Task<RefreshTokenResponseDto> CreateRefreshTokenAsync(UserIdentity RefreshToken);
    Task<Result<RefreshTokenResponseDto>> UploadTokensAsync(string refreshToken);
}