namespace UserService.Core.Models;

public interface IUser
{
    public string? UserName { get; set; }
    public UserInfo UserInfo { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}