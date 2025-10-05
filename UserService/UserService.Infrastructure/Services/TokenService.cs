using Microsoft.IdentityModel.Tokens;
//using UserService.Infrastructure.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using UserService.Application.DTO;
using UserService.Core.Models;
using UserService.Core.Results;
using UserService.Infrastructure.Interfaces.Services;

namespace UserService.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;

    public TokenService(
        IConfiguration configuration,
        UserManager<UserIdentity> userManager,
        IHttpContextAccessor httpContextAccessor,
        ApplicationDbContext context)
    {
        _configuration = configuration;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public async Task<string> CreateTokenAsync(UserIdentity user)
    {
        int expirationMinutes = int.Parse(_configuration["JwtTokenSettings:ExparingTimeMinute"]!);
        var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);
        var token = CreateJwtToken(
            await CreateClaimsAsync(user),
            CreateSigningCredentials(),
            expiration
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
    
    public async Task<RefreshTokenResponseDto> CreateRefreshTokenAsync(UserIdentity user)
    { 
        var refreshToken = await GenereteRefreshTokenAsync();
        var accessToken = await CreateTokenAsync(user);

        RefreshToken RefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            UserId = user.Id,
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };
        
        _context.RefreshTokens.Add(RefreshToken);
        await _context.SaveChangesAsync();
        
        return new RefreshTokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<Result<RefreshTokenResponseDto>> UploadTokensAsync(string refreshToken)
    {
        var refreshTokenFromDb = _context.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);
        if (refreshTokenFromDb == null)
        {
            return Result<RefreshTokenResponseDto>.Failure("Invalid refresh token");
        }
        refreshTokenFromDb.DeletedAt = DateTime.UtcNow;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == refreshTokenFromDb.UserId);
        
        var NewaccessToken = await CreateTokenAsync(user);
        var NewRefreshToken = await GenereteRefreshTokenAsync();

        RefreshToken RefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = NewRefreshToken,
            UserId = user.Id,
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };
        
        _context.RefreshTokens.Add(RefreshToken);
        await _context.SaveChangesAsync();
        
        return Result<RefreshTokenResponseDto>.Success(new RefreshTokenResponseDto
        {
            AccessToken = NewaccessToken,
            RefreshToken = NewRefreshToken
        });
    }
    
    private async Task<string> GenereteRefreshTokenAsync()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration) =>
        new(
            _configuration["JwtTokenSettings:ValidIssuer"],
            _configuration["JwtTokenSettings:ValidAudience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private async Task<List<Claim>> CreateClaimsAsync(UserIdentity user)
    {
        var jwtSub = _configuration["JwtTokenSettings:JwtRegisteredClaimNamesSub"];

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, jwtSub),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new Claim("userId", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private SigningCredentials CreateSigningCredentials()
    {
        var symmetricSecurityKey = _configuration["JwtTokenSettings:SymmetricSecurityKey"];

        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricSecurityKey)),
            SecurityAlgorithms.HmacSha256
        );
    }
}
