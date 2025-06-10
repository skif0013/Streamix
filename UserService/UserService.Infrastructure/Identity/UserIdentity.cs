using System.ComponentModel.DataAnnotations;

namespace UserService.Infrastructure.Identity
{
    public class UserIdentity : IdentityUser<Guid>, IUser
    { 
        [MaxLength(50)]
        public string? LastName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastModified { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ICollection<IdentityUserRole<Guid>>? UserRoles { get; set; }
        public ICollection<Shop>? Shops { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<ProductReview>? ProductReviews { get ; set; }
        public ICollection<FavoriteProduct>? FavoriteProducts { get; set; }
        public Cart? Cart { get; set; }
        public UserInfo? UserInfo { get; set; }
    }
}