using UserService.Application.DTO;
using UserService.Core.Results;

namespace UserService.Infrastructure.Interfaces.Services;

public interface ITokenService
{
    Task<RefreshTokenResponseDto> CreateRefreshTokenAsync(UserIdentity RefreshToken);
    Task<Result<RefreshTokenResponseDto>> UploadTokensAsync(string refreshToken);
}