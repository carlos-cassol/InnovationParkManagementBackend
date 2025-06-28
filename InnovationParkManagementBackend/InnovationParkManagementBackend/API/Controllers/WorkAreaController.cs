using AutoMapper;
using InnovationParkManagementBackend.Application.DTO;
using InnovationParkManagementBackend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InnovationParkManagementBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkAreaController : ControllerBase
    {
        private readonly IWorkAreaRepository _workAreaRepository;
        private readonly IMapper _mapper;

        public WorkAreaController(IWorkAreaRepository workAreaRepository, IMapper mapper)
        {
            _workAreaRepository = workAreaRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkAreaDTO>>> GetAll()
        {
            var workAreas = await _workAreaRepository.GetAllWithColumnsAsync();
            var workAreaDtos = _mapper.Map<IEnumerable<WorkAreaDTO>>(workAreas);
            return Ok(workAreaDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkAreaDTO>> GetById(Guid id)
        {
            var workArea = await _workAreaRepository.GetWithColumnsAsync(id);
            if (workArea == null)
                return NotFound();

            var workAreaDto = _mapper.Map<WorkAreaDTO>(workArea);
            return Ok(workAreaDto);
        }

        [HttpPost]
        public async Task<ActionResult<WorkAreaDTO>> Create([FromBody] CreateWorkAreaDTO createWorkAreaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workArea = _mapper.Map<Domain.Entities.WorkArea>(createWorkAreaDto);
            var createdWorkArea = await _workAreaRepository.CreateAsync(workArea);
            var workAreaDto = _mapper.Map<WorkAreaDTO>(createdWorkArea);

            return CreatedAtAction(nameof(GetById), new { id = workAreaDto.Id }, workAreaDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WorkAreaDTO>> Update(Guid id, [FromBody] UpdateWorkAreaDTO updateWorkAreaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingWorkArea = await _workAreaRepository.GetByIdAsync(id);
            if (existingWorkArea == null)
                return NotFound();

            _mapper.Map(updateWorkAreaDto, existingWorkArea);
            var updatedWorkArea = await _workAreaRepository.UpdateAsync(existingWorkArea);
            var workAreaDto = _mapper.Map<WorkAreaDTO>(updatedWorkArea);

            return Ok(workAreaDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var workArea = await _workAreaRepository.GetByIdAsync(id);
            if (workArea == null)
                return NotFound();

            // Verificar se há colunas associadas
            if (workArea.Columns.Any())
                return BadRequest("Não é possível excluir uma área de trabalho que possui colunas");

            await _workAreaRepository.DeleteAsync(id);
            return NoContent();
        }
    }
} 