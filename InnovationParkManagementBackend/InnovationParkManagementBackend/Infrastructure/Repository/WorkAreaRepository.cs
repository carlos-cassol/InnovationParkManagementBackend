using InnovationParkManagementBackend.Application.Interfaces;
using InnovationParkManagementBackend.Domain.Entities;
using InnovationParkManagementBackend.Infrastructure.Context;
using InnovationParkManagementBackend.Infrastructure.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace InnovationParkManagementBackend.Infrastructure.Repository
{
    public class WorkAreaRepository : Repository<WorkArea>, IWorkAreaRepository
    {
        public WorkAreaRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<WorkArea?> GetByIdAsync(Guid id)
        {
            return await _context.Set<WorkArea>()
                .Include(w => w.Columns.OrderBy(c => c.Order))
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<WorkArea?> GetWithColumnsAsync(Guid id)
        {
            return await _context.Set<WorkArea>()
                .Include(w => w.Columns.OrderBy(c => c.Order))
                    .ThenInclude(c => c.Cards.OrderBy(card => card.Index))
                        .ThenInclude(card => card.Client)
                .Include(w => w.Columns.OrderBy(c => c.Order))
                    .ThenInclude(c => c.Cards.OrderBy(card => card.Index))
                        .ThenInclude(card => card.Files)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<WorkArea>> GetAllWithColumnsAsync()
        {
            return await _context.Set<WorkArea>()
                .Include(w => w.Columns.OrderBy(c => c.Order))
                    .ThenInclude(c => c.Cards.OrderBy(card => card.Index))
                        .ThenInclude(card => card.Client)
                .Include(w => w.Columns.OrderBy(c => c.Order))
                    .ThenInclude(c => c.Cards.OrderBy(card => card.Index))
                        .ThenInclude(card => card.Files)
                .ToListAsync();
        }
    }
} 