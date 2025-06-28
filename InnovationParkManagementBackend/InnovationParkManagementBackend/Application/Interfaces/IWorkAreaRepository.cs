using InnovationParkManagementBackend.Domain.Entities;
using InnovationParkManagementBackend.Infrastructure.Repository.GenericRepository;

namespace InnovationParkManagementBackend.Application.Interfaces
{
    public interface IWorkAreaRepository : IRepository<WorkArea>
    {
        Task<WorkArea?> GetWithColumnsAsync(Guid id);
        Task<IEnumerable<WorkArea>> GetAllWithColumnsAsync();
    }
} 