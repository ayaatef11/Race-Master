
namespace RunGroop.Repository.Interfaces
{
    public  interface IProgramRepository<T>
    {
        public void Add(T entity);

        public void Delete(T entity);

        public  Task<IEnumerable<T>> GetAll();
    }
}
