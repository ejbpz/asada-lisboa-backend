using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.Utils.ImageAttribute;

namespace AsadaLisboaBackend.Models.DTOs.Image
{
    public class ImageUpdateRequestDTO
    {
        [Required(ErrorMessage = "El titulo es requerido.")]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerido.")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [MaxFileSize(5, ErrorMessage = "El tamaño máximo de imagen es {0} MB.")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".webp", ".jfif", ".mp4", ".mov", ".mkv" }, ErrorMessage = "La extensión de la imagen no es válida.")]
        public IFormFile? File { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe selecionar un estado válido.")]
        public Guid StatusId { get; set; }

        public List<CategoryRequestDTO> Categories { get; set; } = new();


    }
}
