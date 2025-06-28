using System.ComponentModel.DataAnnotations;
using InnovationParkManagementBackend.Domain.Enums;
using System.Text.Json.Serialization;

namespace InnovationParkManagementBackend.Application.DTO
{
    public class CardDTO
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        [MaxLength(200)]
        public string? Responsible { get; set; }
        
        public bool IsPaid { get; set; }
        
        public IncubationPlan IncubationPlan { get; set; }
        
        [MaxLength(200)]
        public string? IncubatorType { get; set; }
        
        [MaxLength(200)]
        public string? IncubationStatus { get; set; }
        
        [MaxLength(200)]
        public string? TechnologyPlatform { get; set; }
        
        [MaxLength(2000)]
        public string? FreeDescription { get; set; }
        
        public int Index { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public DateTime ModificationDate { get; set; }
        
        public Guid ClientId { get; set; }
        public Guid ColumnId { get; set; }
        
        public ClientDTO Client { get; set; } = null!;
        [JsonIgnore]
        public ColumnDTO Column { get; set; } = null!;
        public List<CardFileDTO> Files { get; set; } = new List<CardFileDTO>();
    }

    public class CreateCardDTO
    {
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
        
        [Required]
        public Guid ClientId { get; set; }
        
        [Required]
        public Guid ColumnId { get; set; }
    }

    public class UpdateCardDTO
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        [MaxLength(200)]
        public string? Responsible { get; set; }
        
        public bool IsPaid { get; set; }
        
        public IncubationPlan IncubationPlan { get; set; }
        
        [MaxLength(200)]
        public string? IncubatorType { get; set; }
        
        [MaxLength(200)]
        public string? IncubationStatus { get; set; }
        
        [MaxLength(200)]
        public string? TechnologyPlatform { get; set; }
        
        [MaxLength(2000)]
        public string? FreeDescription { get; set; }
        
        public int Index { get; set; }
        
        [Required]
        public Guid ClientId { get; set; }
        
        [Required]
        public Guid ColumnId { get; set; }
    }
} 