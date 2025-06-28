using InnovationParkManagementBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnovationParkManagementBackend.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) { }

        // Novos DbSets
        public DbSet<Client> Clients { get; set; }
        public DbSet<WorkArea> WorkAreas { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardFile> CardFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar enum como string
            modelBuilder.Entity<Card>()
                .Property(c => c.IncubationPlan)
                .HasConversion<string>();

            // Exemplo de configuração de relacionamento (pode ser expandido conforme necessário)
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Cards)
                .WithOne(card => card.Client)
                .HasForeignKey(card => card.ClientId);

            modelBuilder.Entity<WorkArea>()
                .HasMany(w => w.Columns)
                .WithOne(col => col.WorkArea)
                .HasForeignKey(col => col.WorkAreaId);

            modelBuilder.Entity<Column>()
                .HasMany(col => col.Cards)
                .WithOne(card => card.Column)
                .HasForeignKey(card => card.ColumnId);

            modelBuilder.Entity<Card>()
                .HasMany(card => card.Files)
                .WithOne(file => file.Card)
                .HasForeignKey(file => file.CardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
