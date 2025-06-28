using AutoMapper;
using InnovationParkManagementBackend.Application.DTO;
using InnovationParkManagementBackend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InnovationParkManagementBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColumnController : ControllerBase
    {
        private readonly IColumnRepository _columnRepository;
        private readonly IMapper _mapper;

        public ColumnController(IColumnRepository columnRepository, IMapper mapper)
        {
            _columnRepository = columnRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColumnDTO>>> GetAll()
        {
            var columns = await _columnRepository.GetAllAsync();
            var columnDtos = _mapper.Map<IEnumerable<ColumnDTO>>(columns);
            return Ok(columnDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ColumnDTO>> GetById(Guid id)
        {
            var column = await _columnRepository.GetWithCardsAsync(id);
            if (column == null)
                return NotFound();

            var columnDto = _mapper.Map<ColumnDTO>(column);
            return Ok(columnDto);
        }

        [HttpGet("work-area/{workAreaId}")]
        public async Task<ActionResult<IEnumerable<ColumnDTO>>> GetByWorkAreaId(Guid workAreaId)
        {
            var columns = await _columnRepository.GetByWorkAreaIdWithCardsAsync(workAreaId);
            var columnDtos = _mapper.Map<IEnumerable<ColumnDTO>>(columns);
            return Ok(columnDtos);
        }

        [HttpPost]
        public async Task<ActionResult<ColumnDTO>> Create([FromBody] CreateColumnDTO createColumnDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Definir a ordem automaticamente (última da área de trabalho)
            var existingColumns = await _columnRepository.GetByWorkAreaIdAsync(createColumnDto.WorkAreaId);
            createColumnDto.Order = existingColumns.Any() ? existingColumns.Max(c => c.Order) + 1 : 0;

            var column = _mapper.Map<Domain.Entities.Column>(createColumnDto);
            var createdColumn = await _columnRepository.CreateAsync(column);
            var columnDto = _mapper.Map<ColumnDTO>(createdColumn);

            return CreatedAtAction(nameof(GetById), new { id = columnDto.Id }, columnDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ColumnDTO>> Update(Guid id, [FromBody] UpdateColumnDTO updateColumnDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingColumn = await _columnRepository.GetByIdAsync(id);
            if (existingColumn == null)
                return NotFound();

            _mapper.Map(updateColumnDto, existingColumn);
            var updatedColumn = await _columnRepository.UpdateAsync(existingColumn);
            var columnDto = _mapper.Map<ColumnDTO>(updatedColumn);

            return Ok(columnDto);
        }

        [HttpPatch("{id}/reorder")]
        public async Task<ActionResult<ColumnDTO>> ReorderColumn(Guid id, [FromBody] int newOrder)
        {
            var existingColumn = await _columnRepository.GetByIdAsync(id);
            if (existingColumn == null)
                return NotFound();

            var workAreaColumns = await _columnRepository.GetByWorkAreaIdAsync(existingColumn.WorkAreaId);
            var currentOrder = existingColumn.Order;

            Console.WriteLine($"Reordenando coluna {existingColumn.Name} (ID: {id}) de posição {currentOrder} para {newOrder}");

            // Validar se a nova posição é válida
            if (newOrder < 0 || newOrder >= workAreaColumns.Count())
            {
                return BadRequest($"Nova posição {newOrder} é inválida. Deve estar entre 0 e {workAreaColumns.Count() - 1}");
            }

            // Se a posição não mudou, não fazer nada
            if (currentOrder == newOrder)
            {
                Console.WriteLine("Posição não mudou, retornando coluna atual");
                var columnDto = _mapper.Map<ColumnDTO>(existingColumn);
                return Ok(columnDto);
            }

            // Reordenar todas as colunas
            var orderedColumns = workAreaColumns.OrderBy(c => c.Order).ToList();
            
            // Remover a coluna da posição atual
            orderedColumns.RemoveAt(currentOrder);
            
            // Inserir na nova posição
            orderedColumns.Insert(newOrder, existingColumn);
            
            // Atualizar as ordens de todas as colunas
            for (int i = 0; i < orderedColumns.Count; i++)
            {
                var column = orderedColumns[i];
                if (column.Order != i)
                {
                    Console.WriteLine($"Atualizando coluna {column.Name} de ordem {column.Order} para {i}");
                    column.Order = i;
                    await _columnRepository.UpdateAsync(column);
                }
            }

            Console.WriteLine($"Reordenação concluída. Nova ordem da coluna {existingColumn.Name}: {newOrder}");
            var updatedColumnDto = _mapper.Map<ColumnDTO>(existingColumn);
            return Ok(updatedColumnDto);
        }

        [HttpGet("work-area/{workAreaId}/debug")]
        public async Task<ActionResult> DebugWorkAreaColumns(Guid workAreaId)
        {
            var columns = await _columnRepository.GetByWorkAreaIdAsync(workAreaId);
            var columnInfo = columns.Select(c => new { c.Id, c.Name, c.Order }).OrderBy(c => c.Order).ToList();
            
            Console.WriteLine($"Colunas da área de trabalho {workAreaId}:");
            foreach (var col in columnInfo)
            {
                Console.WriteLine($"  - {col.Name} (ID: {col.Id}, Order: {col.Order})");
            }
            
            return Ok(columnInfo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var column = await _columnRepository.GetByIdAsync(id);
            if (column == null)
                return NotFound();

            // Verificar se há cards associados
            if (column.Cards.Any())
                return BadRequest("Não é possível excluir uma coluna que possui cards");

            await _columnRepository.DeleteAsync(id);
            return NoContent();
        }
    }
} 