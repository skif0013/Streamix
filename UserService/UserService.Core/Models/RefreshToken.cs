using System.ComponentModel.DataAnnotations.Schema;
using UserService.Core.Abstractions;

namespace UserService.Core.Models;

public class RefreshToken : Entity<Guid>
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
    
    [NotMapped]
    public IUser User { get; set; }
}