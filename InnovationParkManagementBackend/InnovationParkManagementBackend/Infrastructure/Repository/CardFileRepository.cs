using InnovationParkManagementBackend.Application.Interfaces;
using InnovationParkManagementBackend.Domain.Entities;
using InnovationParkManagementBackend.Infrastructure.Context;
using InnovationParkManagementBackend.Infrastructure.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace InnovationParkManagementBackend.Infrastructure.Repository
{
    public class CardFileRepository : Repository<CardFile>, ICardFileRepository
    {
        public CardFileRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<CardFile?> GetByIdAsync(Guid id)
        {
            return await _context.Set<CardFile>()
                .Include(cf => cf.Card)
                .FirstOrDefaultAsync(cf => cf.Id == id);
        }

        public async Task<IEnumerable<CardFile>> GetByCardIdAsync(Guid cardId)
        {
            return await _context.Set<CardFile>()
                .Where(cf => cf.CardId == cardId)
                .ToListAsync();
        }

        public async Task<CardFile?> GetPaymentReceiptByCardIdAsync(Guid cardId)
        {
            return await _context.Set<CardFile>()
                .FirstOrDefaultAsync(cf => cf.CardId == cardId && cf.IsPaymentReceipt);
        }
    }
} 