using System.ComponentModel.DataAnnotations;

namespace UserService.Infrastructure.Identity
{
    public class UserIdentity : IdentityUser<Guid>
    { 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastModified { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}