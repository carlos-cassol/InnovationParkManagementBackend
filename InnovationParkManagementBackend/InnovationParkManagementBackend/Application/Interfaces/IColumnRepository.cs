using InnovationParkManagementBackend.Domain.Entities;
using InnovationParkManagementBackend.Infrastructure.Repository.GenericRepository;

namespace InnovationParkManagementBackend.Application.Interfaces
{
    public interface IColumnRepository : IRepository<Column>
    {
        Task<Column?> GetWithCardsAsync(Guid id);
        Task<IEnumerable<Column>> GetByWorkAreaIdAsync(Guid workAreaId);
        Task<IEnumerable<Column>> GetByWorkAreaIdWithCardsAsync(Guid workAreaId);
    }
} 