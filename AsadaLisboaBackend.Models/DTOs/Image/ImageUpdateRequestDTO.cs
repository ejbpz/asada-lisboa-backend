using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using AsadaLisboaBackend.Utils.ImageAttribute;

namespace AsadaLisboaBackend.Models.DTOs.Image
{
    public class ImageUpdateRequestDTO
    {
        [Required(ErrorMessage = "El titulo es requerido")]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerido")]
        [StringLength(100)]
        public string Description { get; set; } = string.Empty;

        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" })]
        [MaxFileSize(5)] // Límite de 5 MB
        public IFormFile? File { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe selecionar un estado válido")]
        public Guid StatusId { get; set; }

        public List<Guid> CategoryIds { get; set; } = new();
        

    }
}
