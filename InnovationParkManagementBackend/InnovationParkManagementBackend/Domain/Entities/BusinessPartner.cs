using InnovationParkManagementBackend.Application.DTO;
using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Domain.Entities
{
    public class BusinessPartner
    {
        
        public Guid IdCompany { get; set; }
        [Required]
        public string? Name { get; set; }
        public AddressDTO?  AddressPartner { get; set;}
        [Required]
        public string? Cpf {  get; set;}
        [Phone]
        public string? PhoneNumber { get; set;}
        [EmailAddress]
        public string? Email { get; set;}
    }
}
