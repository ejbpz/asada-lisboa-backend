using System.ComponentModel.DataAnnotations;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Models.DTOs.Account
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [RegularExpression(Constants.EMAIL_REGEX, ErrorMessage = "No corresponde a un formato de correo electrónico.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres.")]
        public string Password { get; set; } = string.Empty;
    }
}
