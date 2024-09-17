using RunGroop.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunGroop.Data.Models.Data
{
   public  interface Entity:IMustHaveTenant
    {
        public int Id { get; set; }
    }
}
