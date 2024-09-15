using Microsoft.EntityFrameworkCore;
using RunGroop.Data.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunGroop.Repository.Interfaces
{
    public  interface IProgramRepository<T>
    {
        public void Add(T entity);

        public void Delete(T entity);

        public  Task<IEnumerable<T>> GetAll();
    }
}
