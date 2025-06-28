using AutoMapper;
using InnovationParkManagementBackend.Application.DTO;
using InnovationParkManagementBackend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InnovationParkManagementBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardFileController : ControllerBase
    {
        private readonly ICardFileRepository _cardFileRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public CardFileController(
            ICardFileRepository cardFileRepository,
            ICardRepository cardRepository,
            IMapper mapper,
            IWebHostEnvironment environment)
        {
            _cardFileRepository = cardFileRepository;
            _cardRepository = cardRepository;
            _mapper = mapper;
            _environment = environment;
        }

        [HttpGet("card/{cardId}")]
        public async Task<ActionResult<IEnumerable<CardFileDTO>>> GetByCardId(Guid cardId)
        {
            var files = await _cardFileRepository.GetByCardIdAsync(cardId);
            var fileDtos = _mapper.Map<IEnumerable<CardFileDTO>>(files);
            return Ok(fileDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CardFileDTO>> GetById(Guid id)
        {
            var file = await _cardFileRepository.GetByIdAsync(id);
            if (file == null)
                return NotFound();

            var fileDto = _mapper.Map<CardFileDTO>(file);
            return Ok(fileDto);
        }

        [HttpGet("payment-receipt/{cardId}")]
        public async Task<ActionResult<CardFileDTO>> GetPaymentReceipt(Guid cardId)
        {
            var paymentReceipt = await _cardFileRepository.GetPaymentReceiptByCardIdAsync(cardId);
            if (paymentReceipt == null)
                return NotFound();

            var fileDto = _mapper.Map<CardFileDTO>(paymentReceipt);
            return Ok(fileDto);
        }

        [HttpPost("upload")]
        public async Task<ActionResult<CardFileDTO>> UploadFile([FromForm] CreateCardFileDTO createCardFileDto)
        {
            Console.WriteLine($"Upload iniciado - CardId: {createCardFileDto.CardId}, IsPaymentReceipt: {createCardFileDto.IsPaymentReceipt}");
            
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState inválido:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"  - {error.ErrorMessage}");
                }
                return BadRequest(ModelState);
            }

            // Verificar se o card existe
            var card = await _cardRepository.GetByIdAsync(createCardFileDto.CardId);
            if (card == null)
            {
                Console.WriteLine($"Card não encontrado: {createCardFileDto.CardId}");
                return BadRequest("Card não encontrado");
            }

            if (createCardFileDto.File == null || createCardFileDto.File.Length == 0)
            {
                Console.WriteLine("Arquivo é nulo ou vazio");
                return BadRequest("Arquivo é obrigatório");
            }

            Console.WriteLine($"Arquivo recebido: {createCardFileDto.File.FileName}, Tamanho: {createCardFileDto.File.Length} bytes");

            // Criar diretório se não existir
            var uploadsPath = Path.Combine(_environment.ContentRootPath, "Uploads");
            Console.WriteLine($"Caminho de upload: {uploadsPath}");
            
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
                Console.WriteLine("Diretório de upload criado");
            }

            // Gerar nome único para o arquivo
            var fileName = $"{Guid.NewGuid()}_{createCardFileDto.File.FileName}";
            var filePath = Path.Combine(uploadsPath, fileName);
            Console.WriteLine($"Caminho completo do arquivo: {filePath}");

            // Salvar arquivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await createCardFileDto.File.CopyToAsync(stream);
            }
            Console.WriteLine("Arquivo salvo no disco");

            // Criar entidade CardFile
            var cardFile = new Domain.Entities.CardFile
            {
                FileName = createCardFileDto.File.FileName,
                FileType = createCardFileDto.File.ContentType,
                FilePath = filePath,
                FileSize = createCardFileDto.File.Length,
                IsPaymentReceipt = createCardFileDto.IsPaymentReceipt,
                CardId = createCardFileDto.CardId
            };

            var createdFile = await _cardFileRepository.CreateAsync(cardFile);
            Console.WriteLine($"CardFile criado no banco: {createdFile.Id}");
            
            var fileDto = _mapper.Map<CardFileDTO>(createdFile);
            Console.WriteLine("Upload concluído com sucesso");

            return CreatedAtAction(nameof(GetById), new { id = fileDto.Id }, fileDto);
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var file = await _cardFileRepository.GetByIdAsync(id);
            if (file == null)
                return NotFound();

            if (!System.IO.File.Exists(file.FilePath))
                return NotFound("Arquivo não encontrado no servidor");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(file.FilePath);
            return File(fileBytes, file.FileType, file.FileName);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var file = await _cardFileRepository.GetByIdAsync(id);
            if (file == null)
                return NotFound();

            // Deletar arquivo físico
            if (System.IO.File.Exists(file.FilePath))
            {
                System.IO.File.Delete(file.FilePath);
            }

            await _cardFileRepository.DeleteAsync(id);
            return NoContent();
        }
    }
} 