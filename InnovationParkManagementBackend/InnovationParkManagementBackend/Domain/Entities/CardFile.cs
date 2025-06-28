using System.ComponentModel.DataAnnotations;

namespace InnovationParkManagementBackend.Domain.Entities
{
    public class CardFile
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(500)]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string FileType { get; set; } = string.Empty;
        
        [Required]
        public string FilePath { get; set; } = string.Empty;
        
        public long FileSize { get; set; }
        
        public DateTime UploadDate { get; set; } = DateTime.Now;
        
        public bool IsPaymentReceipt { get; set; } = false;
        
        // Foreign Keys
        public Guid CardId { get; set; }
        
        // Relationships
        public virtual Card Card { get; set; } = null!;
    }
} 