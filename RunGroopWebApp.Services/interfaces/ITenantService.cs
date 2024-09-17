using RunGroop.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunGroopWebApp.Services.interfaces
{
    public interface ITenantService
    {
        string? GetDatabaseProvider();
        string ? GetConnectionString();
        Tenant? GetCurrentTenant();
    }
}
