using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VideoService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IValidator<UploadUserAvatarRequestDto>, UploadUserAvatarValidator>();
            
            return services;
        }
    }
}
