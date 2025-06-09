using AutoMapper;
using InnovationParkManagementBackend.Application.DTO;
using InnovationParkManagementBackend.Domain.Entities;

namespace InnovationParkManagementBackend.Application.AutoMapper
{
    public class ProfileDTO : Profile
    {
        public ProfileDTO()
        {
            CreateMap<BusinessPartner, BusinessPartnerDTO>().ReverseMap();
            CreateMap<BusinessPartnerAddress, BusinessPartnerAddressDTO>().ReverseMap();
            CreateMap<Company, CompanyDTO>().ReverseMap();
        }
    }
}
