using UserService.Core.Models;

namespace UserService.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<UserIdentity, RoleIdentity, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<UserInfo> UserInfo { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<UserInfo>()
            .HasQueryFilter(u => u.DeletedAt == null)
            .HasOne<UserIdentity>()
            .WithOne(u => u.UserInfo)
            .HasForeignKey<UserInfo>(ui => ui.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}