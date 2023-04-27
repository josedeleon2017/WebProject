using StoreAPI.Repository.IRepository;

namespace StoreAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        //private readonly ApplicationDbContext _db;
        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetOne(System.Linq.Expressions.Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}
