using InnovationParkManagementBackend.Application.Interfaces;
using InnovationParkManagementBackend.Domain.Entities;
using InnovationParkManagementBackend.Infrastructure.Context;
using InnovationParkManagementBackend.Infrastructure.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace InnovationParkManagementBackend.Infrastructure.Repository
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<Client?> GetByIdAsync(Guid id)
        {
            return await _context.Set<Client>()
                .Include(c => c.Cards)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client?> GetByCpfCnpjAsync(string cpfCnpj)
        {
            return await _context.Set<Client>()
                .Include(c => c.Cards)
                .FirstOrDefaultAsync(c => c.CpfCnpj == cpfCnpj);
        }

        public async Task<IEnumerable<Client>> GetByNameAsync(string name)
        {
            return await _context.Set<Client>()
                .Include(c => c.Cards)
                .Where(c => c.Name.Contains(name))
                .ToListAsync();
        }
    }
} 