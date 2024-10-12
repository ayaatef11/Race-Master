using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunGroop.Infrastructure.Settings
{
    public class Tenant
    {
        public string Name { get; set; } = string.Empty;
        public string TId { get; set; } = string.Empty;
        public string? ConnectionString { get; set; } = string.Empty;///either use this or the default configuration 
    }
}
