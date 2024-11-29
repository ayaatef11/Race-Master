
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RunGroop.Data.Interfaces.Services;
using RunGroop.Repository.Interfaces;
using RunGroopWebApp.Services;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.File;

   namespace RunGroop.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddSingleton<IUnitOfWork, IUnitOfWork>();
            Services.AddScoped<ILocationService, LocationService>();
            Services.AddScoped<IPhotoService, PhotoService>();
            return Services;
        }

        public static void ConfigureSerilog(this IHostBuilder host)
        {

            host.UseSerilog((ctx, lc) =>
            {
                lc.WriteTo.Console(); 
                lc.WriteTo.File("serilog.txt");
                lc.WriteTo.File(new JsonFormatter(), "serilog.txt");
            });
        }
    }
}