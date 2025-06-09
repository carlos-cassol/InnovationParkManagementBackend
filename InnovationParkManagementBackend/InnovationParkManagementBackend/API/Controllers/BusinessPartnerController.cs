using AutoMapper;
using InnovationParkManagementBackend.Application.DTO;
using InnovationParkManagementBackend.Domain.Entities;
using InnovationParkManagementBackend.Infrastructure.Repository.UnifOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InnovationParkManagementBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessPartnerController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private IUnitOfWork Get_unit()
        {
            return _unit;
        }

        public BusinessPartnerController(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }


        [HttpGet("ListAll")]
        public async Task<ActionResult<IEnumerable<BusinessPartnerDTO>>> GetAll()
        {
            var partners = await _unit.BusinessPartnerRepository.GetAllAsync();

            if (partners == null)
                throw new Exception("Não há sócios cadastrados");

            var listPartners = _mapper.Map<IEnumerable<BusinessPartnerDTO>>(partners);
            return Ok(listPartners);
        }
        [HttpGet("Find/{id}")]
        public async Task<ActionResult<BusinessPartnerDTO>> Get(Guid id)
        {
            var partner = _unit.BusinessPartnerRepository.Get(p => p.Id == id);
            var partnerDTO = _mapper.Map<BusinessPartnerDTO>(partner);
            return Ok(partnerDTO);
        }

        [HttpPost("AddNew")]
        public ActionResult<BusinessPartnerDTO> AddNew(BusinessPartnerDTO businessPartnerDTO)
        {
            var partner = _mapper.Map<BusinessPartner>(businessPartnerDTO);
            _unit.BusinessPartnerRepository.Update(partner);
            _unit.Commit();
            var partnerDTO = _mapper.Map<BusinessPartnerDTO>(partner);
            return Ok(partnerDTO);
        }

    }
}
