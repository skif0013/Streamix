using UserService.Core.Models;

namespace UserService.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<UserIdentity, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<UserInfo> UserInfo { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<IdentityUserRole<Guid>>(ur =>
        {
            ur.HasKey(r => new { r.UserId, r.RoleId });
            ur.HasOne<RoleIdentity>().WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId);
        });
        
        builder.Entity<UserInfo>()
            .HasQueryFilter(u => u.DeletedAt == null)
            .HasOne<UserIdentity>()
            .WithOne(u => u.UserInfo)
            .HasForeignKey<UserInfo>(ui => ui.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
       
        /*builder.Entity<RefreshToken>()
            .HasQueryFilter(rt => rt.DeletedAt == null)
            .HasOne<UserIdentity>()
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);*/
        
        builder.Entity<RefreshToken>()
            .HasQueryFilter(rt => rt.DeletedAt == null && rt.ExpiresOnUtc > DateTime.UtcNow)
            .HasOne<UserIdentity>()
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}  