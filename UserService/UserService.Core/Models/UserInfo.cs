using System.ComponentModel.DataAnnotations.Schema;
using UserService.Core.Abstractions;

namespace UserService.Core.Models
{
    public class UserInfo : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public string? ProfileImmageUrl { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> SocialLinkUrl { get; set; }

        [NotMapped]
        public IUser User { get; set; }
    }
}