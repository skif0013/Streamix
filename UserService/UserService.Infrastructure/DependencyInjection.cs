using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Data.InitialData;

namespace UserService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AdddInfrastructureServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<RoleInitData>();
            return services;
        }
    }
}