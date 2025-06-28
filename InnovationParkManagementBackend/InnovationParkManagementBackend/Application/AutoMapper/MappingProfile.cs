using AutoMapper;
using InnovationParkManagementBackend.Application.DTO;
using InnovationParkManagementBackend.Domain.Entities;

namespace InnovationParkManagementBackend.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Client mappings
            CreateMap<Client, ClientDTO>().ReverseMap();
            CreateMap<Client, CreateClientDTO>().ReverseMap();
            CreateMap<Client, UpdateClientDTO>().ReverseMap();

            // WorkArea mappings
            CreateMap<WorkArea, WorkAreaDTO>().ReverseMap();
            CreateMap<WorkArea, CreateWorkAreaDTO>().ReverseMap();
            CreateMap<WorkArea, UpdateWorkAreaDTO>().ReverseMap();

            // Column mappings
            CreateMap<Column, ColumnDTO>().ReverseMap();
            CreateMap<Column, CreateColumnDTO>().ReverseMap();
            CreateMap<Column, UpdateColumnDTO>().ReverseMap();

            // Card mappings
            CreateMap<Card, CardDTO>().ReverseMap();
            CreateMap<Card, CreateCardDTO>().ReverseMap();
            CreateMap<Card, UpdateCardDTO>().ReverseMap();

            // CardFile mappings
            CreateMap<CardFile, CardFileDTO>().ReverseMap();
        }
    }
} 