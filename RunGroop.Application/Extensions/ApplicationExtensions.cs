
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

            /*  Services.AddScoped<IClubRepository, ClubRepository>();
              Services.AddScoped<IRaceRepository, RaceRepository>();
              Services.AddScoped<IDashboardRepository, DashboardRepository>();
              Services.AddScoped<IUserRepository, UserRepository>();*/
            Services.AddSingleton<IUnitOfWork, IUnitOfWork>();
            Services.AddScoped<ILocationService, LocationService>();
            Services.AddScoped<IPhotoService, PhotoService>();
            return Services;
        }

        public static void ConfigureSerilog(this IHostBuilder host)
        {

            host.UseSerilog((ctx, lc) =>
            {
                lc.WriteTo.Console();//serilog console 
                lc.WriteTo.File("serilog.txt");//serilog file
                lc.WriteTo.File(new JsonFormatter(), "serilog.txt");
            });
        }
    }
}