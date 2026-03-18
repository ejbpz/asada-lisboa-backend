using System.ComponentModel.DataAnnotations;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Models.DTOs.User
{
    public class UserUpdateRequestDTO
    {
        public Guid ChargeId { get; set; }

        [Required(ErrorMessage = "La ubicación de la imagen del usuario es requerido.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "La ubicación de la imagen del usuario debe ser entre {1} y {0} caracteres.")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del usuario es requerido.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El nombre del usuario debe ser entre {1} y {0} caracteres.")]
        public string FirstName { get; set; } = string.Empty;

        [RegularExpression(Constants.PHONE_REGEX, ErrorMessage = "No corresponde a un formato de teléfono celular.")]
        public string? PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "El primer apellido del usuario es requerido.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El primer apellido del usuario debe ser entre {1} y {0} caracteres.")]
        public string FirstLastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El segundo apellido del usuario es requerido.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "El segundo apellido del usuario debe ser entre {1} y {0} caracteres.")]
        public string SecondLastName { get; set; } = string.Empty;
    }
}
