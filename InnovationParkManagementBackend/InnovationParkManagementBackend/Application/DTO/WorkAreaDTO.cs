using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Application.DTO
{
    public class WorkAreaDTO
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public List<ColumnDTO> Columns { get; set; } = new List<ColumnDTO>();
    }

    public class CreateWorkAreaDTO
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
    }

    public class UpdateWorkAreaDTO
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
    }
} 