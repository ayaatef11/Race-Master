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
    public class ProgramRepository<T> (DbContext _context) : IBaseRepository<T>, IProgramRepository<T> where T : class //, Entity
        //entity should be interface to accept it, not a class
        //you must specifiy the type of t to make the set works or it will give you error 
    {
        public void Add(T entity) 
        {
            _context.Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
          
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

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

        /*   public async Task<T?> GetByIdAsync(int id,string? include)
           {
               if (include == null)return await _context.Set<T>().FirstOrDefaultAsync(i => i.Id == id);

               return await _context.Set<T>().Include(include).FirstOrDefaultAsync(i => i.Id == id);
           }*/
    }
}
