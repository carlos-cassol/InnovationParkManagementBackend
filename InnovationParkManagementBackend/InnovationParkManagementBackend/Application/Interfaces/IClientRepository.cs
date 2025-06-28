using InnovationParkManagementBackend.Domain.Entities;
using InnovationParkManagementBackend.Infrastructure.Repository.GenericRepository;

namespace InnovationParkManagementBackend.Application.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client?> GetByCpfCnpjAsync(string cpfCnpj);
        Task<IEnumerable<Client>> GetByNameAsync(string name);
    }
} 