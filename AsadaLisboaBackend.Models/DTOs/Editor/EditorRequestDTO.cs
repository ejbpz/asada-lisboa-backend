using Microsoft.AspNetCore.Http;
using AsadaLisboaBackend.Utils.ImageAttribute;

namespace AsadaLisboaBackend.Models.DTOs.Editor
{
    public class EditorRequestDTO
    {
        [MaxFileSize(5, ErrorMessage = "El tamaño máximo de imagen es {0} MB.")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".webp", ".jfif" }, ErrorMessage = "La extensión de la imagen no es válida.")]
        public IFormFile File { get; set; } = null!;
    }
}
