using System.ComponentModel.DataAnnotations;

namespace AsadaLisboaBackend.Models.DTOs.Contact
{
    public class ContactRequestDTO
    {
        [Required(ErrorMessage = "El nombre del tipo de contacto es requerido.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre del tipo de contacto debe ser entre {1} y {0} caracteres.")]
        public string ContactType { get; set; } = string.Empty;

        [Required(ErrorMessage = "El valor del tipo de contacto es requerido.")]
        [StringLength(100, ErrorMessage = "El valor del tipo de contacto debe ser entre {1} y {0} caracteres.")]
        public string Value { get; set; } = string.Empty;

        [Required(ErrorMessage = "El orden del tipo de contacto es requerido.")]
        [Range(0, 255, ErrorMessage = "El orden del tipo de contacto debe ser entre {0} y {1} caracteres.")]
        public byte Order { get; set; } = 0;
    }
}
