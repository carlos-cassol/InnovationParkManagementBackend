using AutoMapper;
using InnovationParkManagementBackend.Application.DTO;
using InnovationParkManagementBackend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InnovationParkManagementBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ICardRepository _cardRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IColumnRepository _columnRepository;
        private readonly ICardFileRepository _cardFileRepository;
        private readonly IMapper _mapper;

        public CardController(
            ICardRepository cardRepository,
            IClientRepository clientRepository,
            IColumnRepository columnRepository,
            ICardFileRepository cardFileRepository,
            IMapper mapper)
        {
            _cardRepository = cardRepository;
            _clientRepository = clientRepository;
            _columnRepository = columnRepository;
            _cardFileRepository = cardFileRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardDTO>>> GetAll()
        {
            var cards = await _cardRepository.GetAllAsync();
            var cardDtos = _mapper.Map<IEnumerable<CardDTO>>(cards);
            return Ok(cardDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CardDTO>> GetById(Guid id)
        {
            var card = await _cardRepository.GetWithDetailsAsync(id);
            if (card == null)
                return NotFound();

            var cardDto = _mapper.Map<CardDTO>(card);
            return Ok(cardDto);
        }

        [HttpGet("column/{columnId}")]
        public async Task<ActionResult<IEnumerable<CardDTO>>> GetByColumnId(Guid columnId)
        {
            var cards = await _cardRepository.GetByColumnIdAsync(columnId);
            var cardDtos = _mapper.Map<IEnumerable<CardDTO>>(cards);
            return Ok(cardDtos);
        }

        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<IEnumerable<CardDTO>>> GetByClientId(Guid clientId)
        {
            var cards = await _cardRepository.GetByClientIdAsync(clientId);
            var cardDtos = _mapper.Map<IEnumerable<CardDTO>>(cards);
            return Ok(cardDtos);
        }

        [HttpGet("work-area/{workAreaId}")]
        public async Task<ActionResult<IEnumerable<CardDTO>>> GetByWorkAreaId(Guid workAreaId)
        {
            var cards = await _cardRepository.GetByWorkAreaIdAsync(workAreaId);
            var cardDtos = _mapper.Map<IEnumerable<CardDTO>>(cards);
            return Ok(cardDtos);
        }

        [HttpGet("paid")]
        public async Task<ActionResult<IEnumerable<CardDTO>>> GetPaidCards()
        {
            var cards = await _cardRepository.GetManyAsync(c => c.IsPaid);
            var cardDtos = _mapper.Map<IEnumerable<CardDTO>>(cards);
            return Ok(cardDtos);
        }

        [HttpGet("unpaid")]
        public async Task<ActionResult<IEnumerable<CardDTO>>> GetUnpaidCards()
        {
            var cards = await _cardRepository.GetManyAsync(c => !c.IsPaid);
            var cardDtos = _mapper.Map<IEnumerable<CardDTO>>(cards);
            return Ok(cardDtos);
        }

        [HttpPost]
        public async Task<ActionResult<CardDTO>> Create([FromBody] CreateCardDTO createCardDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verificar se o cliente existe
            var client = await _clientRepository.GetByIdAsync(createCardDto.ClientId);
            if (client == null)
                return BadRequest("Cliente não encontrado");

            // Verificar se a coluna existe
            var column = await _columnRepository.GetByIdAsync(createCardDto.ColumnId);
            if (column == null)
                return BadRequest("Coluna não encontrada");

            // Definir o índice automaticamente (último da coluna)
            var existingCards = await _cardRepository.GetByColumnIdAsync(createCardDto.ColumnId);
            createCardDto.Index = existingCards.Any() ? existingCards.Max(c => c.Index) + 1 : 0;

            var card = _mapper.Map<Domain.Entities.Card>(createCardDto);
            card.ModificationDate = DateTime.Now;
            
            var createdCard = await _cardRepository.CreateAsync(card);
            var cardDto = _mapper.Map<CardDTO>(createdCard);

            return CreatedAtAction(nameof(GetById), new { id = cardDto.Id }, cardDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CardDTO>> Update(Guid id, [FromBody] UpdateCardDTO updateCardDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCard = await _cardRepository.GetByIdAsync(id);
            if (existingCard == null)
                return NotFound();

            // Verificar se o cliente existe
            var client = await _clientRepository.GetByIdAsync(updateCardDto.ClientId);
            if (client == null)
                return BadRequest("Cliente não encontrado");

            // Verificar se a coluna existe
            var column = await _columnRepository.GetByIdAsync(updateCardDto.ColumnId);
            if (column == null)
                return BadRequest("Coluna não encontrada");

            _mapper.Map(updateCardDto, existingCard);
            existingCard.ModificationDate = DateTime.Now;
            
            var updatedCard = await _cardRepository.UpdateAsync(existingCard);
            var cardDto = _mapper.Map<CardDTO>(updatedCard);

            return Ok(cardDto);
        }

        [HttpPatch("{id}/paid")]
        public async Task<ActionResult<CardDTO>> UpdatePaymentStatus(Guid id, [FromBody] bool isPaid)
        {
            var existingCard = await _cardRepository.GetByIdAsync(id);
            if (existingCard == null)
                return NotFound();

            existingCard.IsPaid = isPaid;
            existingCard.ModificationDate = DateTime.Now;

            var updatedCard = await _cardRepository.UpdateAsync(existingCard);
            var cardDto = _mapper.Map<CardDTO>(updatedCard);

            return Ok(cardDto);
        }

        [HttpPatch("{id}/move")]
        public async Task<ActionResult<CardDTO>> MoveCard(Guid id, [FromBody] Guid newColumnId)
        {
            var existingCard = await _cardRepository.GetByIdAsync(id);
            if (existingCard == null)
                return NotFound();

            // Verificar se a nova coluna existe
            var newColumn = await _columnRepository.GetByIdAsync(newColumnId);
            if (newColumn == null)
                return BadRequest("Coluna de destino não encontrada");

            existingCard.ColumnId = newColumnId;
            existingCard.ModificationDate = DateTime.Now;

            var updatedCard = await _cardRepository.UpdateAsync(existingCard);
            var cardDto = _mapper.Map<CardDTO>(updatedCard);

            return Ok(cardDto);
        }

        [HttpPatch("{id}/move-with-index")]
        public async Task<ActionResult<CardDTO>> MoveCardWithIndex(Guid id, [FromBody] MoveCardRequest request)
        {
            var existingCard = await _cardRepository.GetByIdAsync(id);
            if (existingCard == null)
                return NotFound();

            // Verificar se a nova coluna existe
            var newColumn = await _columnRepository.GetByIdAsync(request.NewColumnId);
            if (newColumn == null)
                return BadRequest("Coluna de destino não encontrada");

            // Se mudou de coluna, reordenar os índices
            if (existingCard.ColumnId != request.NewColumnId)
            {
                // Remover da coluna antiga
                var oldColumnCards = await _cardRepository.GetByColumnIdAsync(existingCard.ColumnId);
                var cardsToReorder = oldColumnCards.Where(c => c.Index > existingCard.Index).ToList();
                foreach (var card in cardsToReorder)
                {
                    card.Index--;
                    await _cardRepository.UpdateAsync(card);
                }

                // Adicionar à nova coluna
                var newColumnCards = await _cardRepository.GetByColumnIdAsync(request.NewColumnId);
                var maxIndex = newColumnCards.Any() ? newColumnCards.Max(c => c.Index) : -1;
                existingCard.Index = maxIndex + 1;
            }
            else
            {
                // Mesma coluna, apenas reordenar
                var columnCards = await _cardRepository.GetByColumnIdAsync(request.NewColumnId);
                var currentIndex = existingCard.Index;
                var newIndex = request.NewIndex;

                if (currentIndex < newIndex)
                {
                    // Movendo para baixo
                    var cardsToMove = columnCards.Where(c => c.Index > currentIndex && c.Index <= newIndex).ToList();
                    foreach (var card in cardsToMove)
                    {
                        card.Index--;
                        await _cardRepository.UpdateAsync(card);
                    }
                }
                else if (currentIndex > newIndex)
                {
                    // Movendo para cima
                    var cardsToMove = columnCards.Where(c => c.Index >= newIndex && c.Index < currentIndex).ToList();
                    foreach (var card in cardsToMove)
                    {
                        card.Index++;
                        await _cardRepository.UpdateAsync(card);
                    }
                }

                existingCard.Index = newIndex;
            }

            existingCard.ColumnId = request.NewColumnId;
            existingCard.ModificationDate = DateTime.Now;

            var updatedCard = await _cardRepository.UpdateAsync(existingCard);
            var cardDto = _mapper.Map<CardDTO>(updatedCard);

            return Ok(cardDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var card = await _cardRepository.GetByIdAsync(id);
            if (card == null)
                return NotFound();

            await _cardRepository.DeleteAsync(id);
            return NoContent();
        }
    }

    public class MoveCardRequest
    {
        public Guid NewColumnId { get; set; }
        public int NewIndex { get; set; }
    }
} 