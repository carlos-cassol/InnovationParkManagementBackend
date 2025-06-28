using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Domain.Entities
{
    public class AreaTrabalho
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Nome { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Descricao { get; set; }
        
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        
        // Relacionamentos
        public virtual ICollection<Coluna> Colunas { get; set; } = new List<Coluna>();
    }
} 