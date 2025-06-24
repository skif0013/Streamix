using UserService.Core.Models;

namespace UserService.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<UserIdentity, RoleIdentity, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<UserInfo> UserInfo { get; set; }
}