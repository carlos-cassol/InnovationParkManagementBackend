using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Domain.Entities
{
    public class Client
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(18)]
        public string CpfCnpj { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Contact { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;
        
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        
        // Relationships
        public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    }
} 