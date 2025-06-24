using Microsoft.Extensions.DependencyInjection;

namespace UserService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AdddInfrastructureServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            
            return services;
        }
    }
}