using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using AsadaLisboaBackend.Utils.ImageAttribute;

namespace AsadaLisboaBackend.Models.DTOs.Image
{
    public class ImageRequestDTO
    {
        [Required(ErrorMessage = "El titulo es requerido")]  
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerido")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        
        [MaxFileSize(5, ErrorMessage = "El tamaño máximo de imagen es {0} MB.")] // Límite de 5 MB
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "La extensión de la imagen no es válida.")]
        public IFormFile File { get; set; } = null!;


        [Required(ErrorMessage = "Debe selecionar un estado válido")]
        public Guid StatusId { get; set; }

        public List<Guid> CategoryIds { get; set; } = new();
    }
}