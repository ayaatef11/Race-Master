using Microsoft.EntityFrameworkCore;
using RunGroop.Data.Models.Data;
using RunGroop.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunGroop.Repository.Repository
{
    public class BaseRepository<T>(DbContext _context) : IBaseRepository<T>
    {

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(T entity)
        {
            _context.Update(entity);
            return Save();
        }

    }
}
