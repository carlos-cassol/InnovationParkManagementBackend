using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Domain.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Nome { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(18)]
        public string CpfCnpj { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Contato { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(500)]
        public string Endereco { get; set; } = string.Empty;
        
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        
        // Relacionamentos
        public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    }
} 