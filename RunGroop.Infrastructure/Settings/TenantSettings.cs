using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunGroop.Infrastructure.Settings
{
    public class TenantSettings
    {
        public Configuration Defaults { get; set; } = default!;
        public List<Tenant> Tenants {  get; set; }= new List<Tenant>();
    }
}
