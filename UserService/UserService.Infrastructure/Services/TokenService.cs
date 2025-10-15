using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.Application.DTO;
using UserService.Core.Models;
using UserService.Core.Results;
using UserService.Infrastructure.Interfaces.Services;

namespace UserService.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly ApplicationDbContext _context;

    public TokenService(
        IConfiguration configuration,
        UserManager<UserIdentity> userManager,
        ApplicationDbContext context)
    {
        _configuration = configuration;
        _userManager = userManager;
        _context = context;
    }

    private async Task<string> CreateTokenAsync(UserIdentity user)
    {
        int expirationMinutes = 999; //int.Parse(_configuration["JwtTokenSettings:ExparingTimeMinute"]!);
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
        
        var newAccessToken = await CreateTokenAsync(user);
        var newRefreshToken = await GenereteRefreshTokenAsync();

        RefreshToken RefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = newRefreshToken,
            UserId = user.Id,
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };
        
        _context.RefreshTokens.Add(RefreshToken);
        await _context.SaveChangesAsync();
        
        return Result<RefreshTokenResponseDto>.Success(new RefreshTokenResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }
    
    private async Task<string> GenereteRefreshTokenAsync()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration) =>
        new(
            "ExampleIssuer",//_configuration["JwtTokenSettings:ValidIssuer"],
            "ValidAudience",//_configuration["JwtTokenSettings:ValidAudience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private async Task<List<Claim>> CreateClaimsAsync(UserIdentity user)
    {
        var jwtSub = "345h098bb8reberbwr4vvb8945";//_configuration["JwtTokenSettings:JwtRegisteredClaimNamesSub"];

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
        var symmetricSecurityKey = "fvh2456477hth44j6wfds98dq9hp8bqh9ubq9gjig3qr0";//_configuration["JwtTokenSettings:SymmetricSecurityKey"];

        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricSecurityKey)),
            SecurityAlgorithms.HmacSha256
        );
    }
}
