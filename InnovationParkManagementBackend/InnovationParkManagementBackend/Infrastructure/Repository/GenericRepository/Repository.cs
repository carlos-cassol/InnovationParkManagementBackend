using InnovationParkManagementBackend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnovationParkManagementBackend.Infrastructure.Repository.GenericRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext? _context;

        public Repository(AppDbContext? context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (_context == null)
            {
                throw new ArgumentNullException();
            }
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public T Created(T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException();
            }
            _context.Set<T>().Add(entity);
            return entity;
        }

        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
    }
}
