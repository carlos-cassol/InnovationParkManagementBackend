using InnovationParkManagementBackend.Application.Interfaces;
using InnovationParkManagementBackend.Domain.Entities;
using InnovationParkManagementBackend.Infrastructure.Context;
using InnovationParkManagementBackend.Infrastructure.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace InnovationParkManagementBackend.Infrastructure.Repository
{
    public class CardRepository : Repository<Card>, ICardRepository
    {
        public CardRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<Card?> GetByIdAsync(Guid id)
        {
            return await _context.Set<Card>()
                .Include(c => c.Client)
                .Include(c => c.Column)
                .Include(c => c.Files)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Card?> GetWithDetailsAsync(Guid id)
        {
            return await _context.Set<Card>()
                .Include(c => c.Client)
                .Include(c => c.Column)
                    .ThenInclude(col => col.WorkArea)
                .Include(c => c.Files)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Card>> GetByColumnIdAsync(Guid columnId)
        {
            return await _context.Set<Card>()
                .Include(c => c.Client)
                .Include(c => c.Files)
                .Where(c => c.ColumnId == columnId)
                .OrderBy(c => c.Index)
                .ToListAsync();
        }

        public async Task<IEnumerable<Card>> GetByClientIdAsync(Guid clientId)
        {
            return await _context.Set<Card>()
                .Include(c => c.Column)
                .Include(c => c.Files)
                .Where(c => c.ClientId == clientId)
                .OrderBy(c => c.Index)
                .ToListAsync();
        }

        public async Task<IEnumerable<Card>> GetByWorkAreaIdAsync(Guid workAreaId)
        {
            return await _context.Set<Card>()
                .Include(c => c.Client)
                .Include(c => c.Column)
                .Include(c => c.Files)
                .Where(c => c.Column.WorkAreaId == workAreaId)
                .OrderBy(c => c.Column.Order)
                .ThenBy(c => c.Index)
                .ToListAsync();
        }
    }
} 