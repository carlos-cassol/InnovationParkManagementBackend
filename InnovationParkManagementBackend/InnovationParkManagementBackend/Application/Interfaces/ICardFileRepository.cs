using InnovationParkManagementBackend.Domain.Entities;
using InnovationParkManagementBackend.Infrastructure.Repository.GenericRepository;

namespace InnovationParkManagementBackend.Application.Interfaces
{
    public interface ICardFileRepository : IRepository<CardFile>
    {
        Task<IEnumerable<CardFile>> GetByCardIdAsync(Guid cardId);
        Task<CardFile?> GetPaymentReceiptByCardIdAsync(Guid cardId);
    }
} 