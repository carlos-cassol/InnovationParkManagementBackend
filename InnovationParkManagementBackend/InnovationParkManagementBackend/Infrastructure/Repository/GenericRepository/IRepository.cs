using System.Linq.Expressions;

namespace InnovationParkManagementBackend.Infrastructure.Repository.GenericRepository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> Get(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();
        T Created(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
