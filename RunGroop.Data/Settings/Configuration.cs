using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunGroop.Infrastructure.Settings
{
    public class Configuration
    {
        public string DBProvider { get; set; }=string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
    }
}
