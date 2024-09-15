using RunGroop.Data.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunGroop.Repository.Interfaces
{
    public  interface IBaseRepository<T>
    {
        public bool Save();
        bool Update(T entity);
    }
}
