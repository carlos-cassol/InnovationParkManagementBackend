using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Application.DTO
{
    public class ClientDTO
    {
        public Guid Id { get; set; }
        
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
        
        public DateTime RegistrationDate { get; set; }
    }

    public class CreateClientDTO
    {
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
    }

    public class UpdateClientDTO
    {
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
    }
} 