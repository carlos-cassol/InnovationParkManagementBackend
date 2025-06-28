using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Application.DTO
{
    public class ColumnDTO
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public int Order { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public Guid WorkAreaId { get; set; }
        
        public List<CardDTO> Cards { get; set; } = new List<CardDTO>();
    }

    public class CreateColumnDTO
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public int Order { get; set; }
        
        [Required]
        public Guid WorkAreaId { get; set; }
    }

    public class UpdateColumnDTO
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public int Order { get; set; }
    }
} 