

using RunGroop.Infrastructure.Settings;

namespace RunGroopWebApp.Services.interfaces
{
    public interface ITenantService
    {
        string? GetDatabaseProvider();
        string ? GetConnectionString();
        Tenant? GetCurrentTenant();
    }
}
