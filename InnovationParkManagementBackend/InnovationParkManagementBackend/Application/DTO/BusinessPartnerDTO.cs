using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Application.DTO
{
    public class BusinessPartnerDTO
    {
        public string IdCompany { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Cpf { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        [EmailAddress]
        public string? Mail { get; set; }
    }
}
