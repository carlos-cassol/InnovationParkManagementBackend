using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Application.DTO
{
    public class BusinessPartnerDTO
    {
        public Guid IdCompany { get; set; }
        [Required]
        public string? Name { get; set; }
        public AddressDTO? AddressPartner { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
    }
}
