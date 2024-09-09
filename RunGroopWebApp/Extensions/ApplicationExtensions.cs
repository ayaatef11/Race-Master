using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Interfaces.Services;
using RunGroopWebApp.Repository;
using RunGroopWebApp.Services;

namespace RunGroopWebApp.Extensions
{
    public  static class ApplicationExtensions
    {
    public static IServiceCollection AddApplicationServices(this IServiceCollection Services) { 

    Services.AddScoped<IClubRepository, ClubRepository>();
    Services.AddScoped<IRaceRepository, RaceRepository>();
    Services.AddScoped<IDashboardRepository, DashboardRepository>();
    Services.AddScoped<IUserRepository, UserRepository>();
    Services.AddScoped<ILocationService, LocationService>();
    Services.AddScoped<IPhotoService, PhotoService>();
                return Services;
    }
}}
