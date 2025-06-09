using AutoMapper;
using InnovationParkManagementBackend.Application.DTO;
using InnovationParkManagementBackend.Domain.Entities;
using InnovationParkManagementBackend.Infrastructure.Repository.UnifOfWork;
using Microsoft.AspNetCore.Mvc;

namespace InnovationParkManagementBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private IUnitOfWork Get_unit()
        {
            return _unit;
        }

        public CompanyController(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        [HttpPost("AddNew")]
        public ActionResult<CompanyDTO> AddNew(CompanyDTO companyDTO)
        {
            var company = _mapper.Map<Company>(companyDTO);
            _unit.CompanyRepository.Update(company);
            _unit.Commit();
            var newCompanyDTO = _mapper.Map<CompanyDTO>(company);
            return Ok(newCompanyDTO);
        }

    }
}
