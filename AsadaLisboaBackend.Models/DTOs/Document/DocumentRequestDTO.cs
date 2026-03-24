using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using AsadaLisboaBackend.Utils.DocumentAtribute;

namespace AsadaLisboaBackend.Models.DTOs.Document
{
    public class DocumentRequestDTO
    {
        [Required(ErrorMessage = "El titulo es requerido")]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(100)]
        public string Description { get; set; } = string.Empty;


        [AllowedExtensions(new string[] { ".pdf", ".docx", ".xlsx" })]
        [MaxFileSize(5)] // Límite de 5 MB
        public IFormFile File { get; set; } = null!;

        public Guid StatusId { get; set; }

        public Guid DocumentTypeId { get; set; }

        public List<Guid> CategoryIds { get; set; } = new();
    }
}
