using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.Utils.ImageAttribute;

namespace AsadaLisboaBackend.Models.DTOs.Document
{
    public class DocumentUpdateRequestDTO
    {
        [Required(ErrorMessage = "El titulo es requerido.")]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida.")]
        [StringLength(100)]
        public string Description { get; set; } = string.Empty;

        [MaxFileSize(15, ErrorMessage = "El tamaño máximo de imagen es {0} MB.")]
        [AllowedExtensions(new string[] { ".pdf", ".docx", ".xlsx", ".csv", ".txt", ".zip" }, ErrorMessage = "La extensión del documento no es válida.")]
        public IFormFile? File { get; set; }

        [Required(ErrorMessage = "Debe selecionar un estado válido.")]
        public Guid StatusId { get; set; }

        public List<CategoryRequestDTO> Categories { get; set; } = new();
    }
}
