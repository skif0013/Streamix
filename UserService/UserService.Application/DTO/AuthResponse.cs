using UserService.Core.Models;

namespace UserService.Application.DTO;

public class AuthResponse
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}