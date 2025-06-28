using InnovationParkManagementBackend.Infrastructure.Context;

namespace InnovationParkManagementBackend.Infrastructure.Repository.UnifOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext? _context;

        public UnitOfWork(AppDbContext? context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context?.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
