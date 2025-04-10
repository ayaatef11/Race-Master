
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RunGroop.Data.Interfaces.Services;
using RunGroop.Infrastructure;
using RunGroop.Repository.Interfaces;
using RunGroop.Repository.Repository;
using RunGroopWebApp.Services;
using RunGroopWebApp.Services.interfaces;
using RunGroopWebApp.Services.Services;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.File;

   namespace RunGroop.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<ILocationService, LocationService>();
            Services.AddScoped<IFileService, FileService>();
            Services.AddScoped<INotificationService, NotificationService>();
            Services.AddSingleton<SignalServer>(); // Register SignalServer as a singleton (or transient/scoped as needed)

            return Services;
        }

        public static void ConfigureSerilog(this IHostBuilder host)
        {

            //host.UseSerilog((ctx, lc) =>
            //{
            //    lc.WriteTo.Console(); 
            //    lc.WriteTo.File("serilog.txt");
            //    lc.WriteTo.File(new JsonFormatter(), "serilog.txt");
            //});
        }
    }
}