using InnovationParkManagementBackend.Domain.Entities;
using InnovationParkManagementBackend.Infrastructure.Repository.GenericRepository;

namespace InnovationParkManagementBackend.Application.Interfaces
{
    public interface ICardRepository : IRepository<Card>
    {
        Task<Card?> GetWithDetailsAsync(Guid id);
        Task<IEnumerable<Card>> GetByColumnIdAsync(Guid columnId);
        Task<IEnumerable<Card>> GetByClientIdAsync(Guid clientId);
        Task<IEnumerable<Card>> GetByWorkAreaIdAsync(Guid workAreaId);
    }
} 