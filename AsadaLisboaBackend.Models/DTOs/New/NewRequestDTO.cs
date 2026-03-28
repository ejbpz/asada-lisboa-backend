using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using AsadaLisboaBackend.Utils.ImageAttribute;

namespace AsadaLisboaBackend.Models.DTOs.New
{
    public class NewRequestDTO
    {
        [Required(ErrorMessage = "El título de la noticia es requerido.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El título de la imagen debe ser entre {1} y {0} caracteres.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción de la noticia es requerida.")]
        [StringLength(5000, MinimumLength = 2, ErrorMessage = "La descripción de la imagen debe ser entre {1} y {0} caracteres.")]

        public string Description { get; set; } = string.Empty;


        [MaxFileSize(5, ErrorMessage = "El tamaño máximo de imagen de la noticia es {0} MB.")] // Límite de 5 MB
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "La extensión de la imagen de la noticia no es válida.")]
        public IFormFile? File { get; set; }


        [Required(ErrorMessage = "El estado de la noticia es requerido.")]
        public Guid StatusId { get; set; }

        public List<Guid> CategoryIds { get; set; } = new();
    }
}
