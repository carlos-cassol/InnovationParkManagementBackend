using System.ComponentModel.DataAnnotations;
using InnovationParkManagementBackend.Domain.Enums;

namespace InnovationParkManagementBackend.Domain.Entities
{
    public class Card
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        [MaxLength(200)]
        public string? Responsible { get; set; }
        
        public bool IsPaid { get; set; } = false;
        
        public IncubationPlan IncubationPlan { get; set; }
        
        [MaxLength(200)]
        public string? IncubatorType { get; set; }
        
        [MaxLength(200)]
        public string? IncubationStatus { get; set; }
        
        [MaxLength(200)]
        public string? TechnologyPlatform { get; set; }
        
        [MaxLength(2000)]
        public string? FreeDescription { get; set; }
        
        public int Index { get; set; } = 0;
        
        public DateTime CreationDate { get; set; } = DateTime.Now;
        
        public DateTime ModificationDate { get; set; } = DateTime.Now;
        
        // Foreign Keys
        public Guid ClientId { get; set; }
        public Guid ColumnId { get; set; }
        
        // Relationships
        public virtual Client Client { get; set; } = null!;
        public virtual Column Column { get; set; } = null!;
        public virtual ICollection<CardFile> Files { get; set; } = new List<CardFile>();
    }
} 