using VideoService.Application.Model;

namespace VideoService.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Avatar> Avatars { get; set; }
    public  DbSet<Video> Videos { get; set; }
}