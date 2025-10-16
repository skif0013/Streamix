namespace UserService.Infrastructure.Data.InitialData;

public class RoleInitData
{
    public static async Task InitializeAsync(RoleManager<RoleIdentity> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var adminRole = new RoleIdentity{ Name = "Admin", NormalizedName = "ADMIN" };
            await roleManager.CreateAsync(adminRole);
        }
        
        if (!await roleManager.RoleExistsAsync("Creator"))
        {
            var adminRole = new RoleIdentity{ Name = "Creator", NormalizedName = "CREATOR" };
            await roleManager.CreateAsync(adminRole);
        }
        
        if (!await roleManager.RoleExistsAsync("User"))
        {
            var adminRole = new RoleIdentity{ Name = "User", NormalizedName = "USER" };
            await roleManager.CreateAsync(adminRole);
        }
    }
}