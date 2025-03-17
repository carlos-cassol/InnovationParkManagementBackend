using InnovationParkManagementBackend.Infrastructure.Context;

namespace InnovationParkManagementBackend.Infrastructure.Repository.UnifOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IBusinessPartnerRepository _repository;
        public AppDbContext? _context;

        public UnitOfWork(AppDbContext? context)
        {
            _context = context;
        }

        public IBusinessPartnerRepository BusinessPartnerRepository
        {
            get { return _repository = _repository ?? new BusinessPartnerRepository(_context) ;}
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
