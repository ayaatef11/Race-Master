using Microsoft.EntityFrameworkCore;
using RunGroop.Repository.Interfaces;

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
            _context.Update(entity!);
            return Save();
        }

    }
}
