using InnovationParkManagementBackend.Domain.Entities;
using InnovationParkManagementBackend.Infrastructure.Context;
using InnovationParkManagementBackend.Infrastructure.Repository.GenericRepository;

namespace InnovationParkManagementBackend.Infrastructure.Repository
{
    public class BusinessPartnerRepository : Repository<BusinessPartner>, IBusinessPartnerRepository
    {
        public BusinessPartnerRepository(AppDbContext? context) : base(context)
        {
        }
    }
}
