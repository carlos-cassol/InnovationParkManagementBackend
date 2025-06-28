using InnovationParkManagementBackend.Application.Interfaces;
using InnovationParkManagementBackend.Domain.Entities;
using InnovationParkManagementBackend.Infrastructure.Context;
using InnovationParkManagementBackend.Infrastructure.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace InnovationParkManagementBackend.Infrastructure.Repository
{
    public class ColumnRepository : Repository<Column>, IColumnRepository
    {
        public ColumnRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<Column?> GetByIdAsync(Guid id)
        {
            return await _context.Set<Column>()
                .Include(c => c.WorkArea)
                .Include(c => c.Cards)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Column?> GetWithCardsAsync(Guid id)
        {
            return await _context.Set<Column>()
                .Include(c => c.WorkArea)
                .Include(c => c.Cards)
                    .ThenInclude(card => card.Client)
                .Include(c => c.Cards)
                    .ThenInclude(card => card.Files)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Column>> GetByWorkAreaIdAsync(Guid workAreaId)
        {
            return await _context.Set<Column>()
                .Include(c => c.Cards)
                .Where(c => c.WorkAreaId == workAreaId)
                .OrderBy(c => c.Order)
                .ToListAsync();
        }

        public async Task<IEnumerable<Column>> GetByWorkAreaIdWithCardsAsync(Guid workAreaId)
        {
            return await _context.Set<Column>()
                .Include(c => c.Cards)
                    .ThenInclude(card => card.Client)
                .Include(c => c.Cards)
                    .ThenInclude(card => card.Files)
                .Where(c => c.WorkAreaId == workAreaId)
                .OrderBy(c => c.Order)
                .ToListAsync();
        }
    }
} 