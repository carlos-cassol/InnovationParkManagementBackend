using AutoMapper;
using InnovationParkManagementBackend.Application.DTO;
using InnovationParkManagementBackend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InnovationParkManagementBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientController(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetAll()
        {
            var clients = await _clientRepository.GetAllAsync();
            var clientDtos = _mapper.Map<IEnumerable<ClientDTO>>(clients);
            return Ok(clientDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> GetById(Guid id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
                return NotFound();

            var clientDto = _mapper.Map<ClientDTO>(client);
            return Ok(clientDto);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> Search([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Nome é obrigatório para busca");

            var clients = await _clientRepository.GetByNameAsync(name);
            var clientDtos = _mapper.Map<IEnumerable<ClientDTO>>(clients);
            return Ok(clientDtos);
        }

        [HttpGet("cpf-cnpj/{cpfCnpj}")]
        public async Task<ActionResult<ClientDTO>> GetByCpfCnpj(string cpfCnpj)
        {
            var client = await _clientRepository.GetByCpfCnpjAsync(cpfCnpj);
            if (client == null)
                return NotFound();

            var clientDto = _mapper.Map<ClientDTO>(client);
            return Ok(clientDto);
        }

        [HttpPost]
        public async Task<ActionResult<ClientDTO>> Create([FromBody] CreateClientDTO createClientDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verificar se CPF/CNPJ já existe
            var existingClient = await _clientRepository.GetByCpfCnpjAsync(createClientDto.CpfCnpj);
            if (existingClient != null)
                return BadRequest("CPF/CNPJ já cadastrado");

            var client = _mapper.Map<Domain.Entities.Client>(createClientDto);
            var createdClient = await _clientRepository.CreateAsync(client);
            var clientDto = _mapper.Map<ClientDTO>(createdClient);

            return CreatedAtAction(nameof(GetById), new { id = clientDto.Id }, clientDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ClientDTO>> Update(Guid id, [FromBody] UpdateClientDTO updateClientDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingClient = await _clientRepository.GetByIdAsync(id);
            if (existingClient == null)
                return NotFound();

            // Verificar se CPF/CNPJ já existe em outro cliente
            var clientWithSameCpfCnpj = await _clientRepository.GetByCpfCnpjAsync(updateClientDto.CpfCnpj);
            if (clientWithSameCpfCnpj != null && clientWithSameCpfCnpj.Id != id)
                return BadRequest("CPF/CNPJ já cadastrado para outro cliente");

            _mapper.Map(updateClientDto, existingClient);
            var updatedClient = await _clientRepository.UpdateAsync(existingClient);
            var clientDto = _mapper.Map<ClientDTO>(updatedClient);

            return Ok(clientDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
                return NotFound();

            await _clientRepository.DeleteAsync(id);
            return NoContent();
        }
    }
} 