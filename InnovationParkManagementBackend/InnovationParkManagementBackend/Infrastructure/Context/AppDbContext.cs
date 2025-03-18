using InnovationParkManagementBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnovationParkManagementBackend.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) { }

        public DbSet<BusinessPartner> BusinessPartner { get; set; }
    }
}
