using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RunGroop.Infrastructure.Settings;
using RunGroopWebApp.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunGroopWebApp.Services.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private HttpContext _httpContext;
        private Tenant? _currentTenant ;
        public TenantService(IHttpContextAccessor contextAccessor,IOptions<TenantSettings> tenantSettings)
        {

            _httpContext = contextAccessor.HttpContext;
            _tenantSettings = tenantSettings.Value;

            if (_httpContext is not null)

                if (_httpContext.Request.Headers.TryGetValue(key: "tenant", out var tenantId)) setCurrentTenant(tenantId!);
        }
        public string? GetConnectionString()
        {
            var currentConnectionString = _currentTenant is null
? _tenantSettings.Defaults.ConnectionString
: _currentTenant.ConnectionString;

            return currentConnectionString;
        }

        public Tenant? GetCurrentTenant()
        {
            return _currentTenant;
        }

        public string? GetDatabaseProvider()
        {
            return _tenantSettings.Defaults.DBProvider;
        }
        private void setCurrentTenant(string tenantId)
        {

           _currentTenant = _tenantSettings.Tenants.FirstOrDefault(t => t.TId == tenantId);

            if (_currentTenant is null)

                throw new Exception(message: "Invalid tenant ID");

            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))

                _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
        }
    }
}
