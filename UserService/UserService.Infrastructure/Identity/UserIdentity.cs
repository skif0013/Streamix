using System.ComponentModel.DataAnnotations;
using UserService.Core.Models;

namespace UserService.Infrastructure.Identity
{
    public class UserIdentity : IdentityUser<Guid>, IUser
    {
        public UserInfo UserInfo { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastModified { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}