using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Application.DTO
{
    public class CardFileDTO
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string FileType { get; set; } = string.Empty;
        
        [Required]
        public string FilePath { get; set; } = string.Empty;
        
        public long FileSize { get; set; }
        
        public DateTime UploadDate { get; set; }
        
        public bool IsPaymentReceipt { get; set; }
        
        public Guid CardId { get; set; }
    }

    public class CreateCardFileDTO
    {
        [Required]
        public IFormFile File { get; set; } = null!;
        
        public bool IsPaymentReceipt { get; set; } = false;
        
        [Required]
        public Guid CardId { get; set; }
    }
} 