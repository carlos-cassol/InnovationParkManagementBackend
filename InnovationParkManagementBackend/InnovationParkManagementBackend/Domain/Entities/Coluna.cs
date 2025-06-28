using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Domain.Entities
{
    public class Coluna
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Nome { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Descricao { get; set; }
        
        public int Ordem { get; set; }
        
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        
        // Chave estrangeira
        public int AreaTrabalhoId { get; set; }
        public virtual AreaTrabalho AreaTrabalho { get; set; } = null!;
        
        // Relacionamentos
        public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    }
} 