using RunGroop.Infrastructure.Settings;

namespace RunGroop.Data.Services
{
    public interface ITenantService
    {
        string? GetDatabaseProvider();
        string? GetConnectionString();
        Tenant? GetCurrentTenant();
    }
}
