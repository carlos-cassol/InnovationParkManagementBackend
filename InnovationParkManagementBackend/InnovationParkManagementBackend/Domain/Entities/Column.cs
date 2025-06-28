using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Domain.Entities
{
    public class Column
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public int Order { get; set; }
        
        public DateTime CreationDate { get; set; } = DateTime.Now;
        
        // Foreign Keys
        public Guid WorkAreaId { get; set; }
        
        // Relationships
        public virtual WorkArea WorkArea { get; set; } = null!;
        public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    }
} 