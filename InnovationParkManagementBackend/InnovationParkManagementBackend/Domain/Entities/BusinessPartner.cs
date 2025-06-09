using InnovationParkManagementBackend.Application.DTO;
using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Domain.Entities
{
    public class BusinessPartner
    {
        
        public Guid Id { get; set; }
        public Guid IdCompany { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Cpf {  get; set;}
        [Phone]
        public string? PhoneNumber { get; set;}
        [EmailAddress]
        public string? Mail { get; set;}
    }
}
