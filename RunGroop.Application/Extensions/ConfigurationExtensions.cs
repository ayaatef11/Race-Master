using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunGroop.Application.Helpers;

namespace RunGroopWebApp.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddConfigurationServices(this IServiceCollection Services,IConfiguration configuration)
        {
            Services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            return Services;
        }
    }
}
