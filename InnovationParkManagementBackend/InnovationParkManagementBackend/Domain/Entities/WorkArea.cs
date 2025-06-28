using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Domain.Entities
{
    public class WorkArea
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public DateTime CreationDate { get; set; } = DateTime.Now;
        
        // Relationships
        public virtual ICollection<Column> Columns { get; set; } = new List<Column>();
    }
} 