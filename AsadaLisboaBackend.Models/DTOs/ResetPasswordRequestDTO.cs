using AsadaLisboaBackend.Utils;
using System.ComponentModel.DataAnnotations;

namespace AsadaLisboaBackend.Models.DTOs
{
    public class ResetPasswordRequestDTO
    {
        [Required(ErrorMessage = "La contraseña es requerida.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmación de la contraseña es requerida.")]
        [Compare("Password", ErrorMessage = "Ambas contraseñas deben coincidir.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "La confirmación de la contraseña debe tener entre {2} y {1} caracteres.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "El token para el cambio de contraseña es requerido.")]
        public string Token { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [RegularExpression(Constants.EMAIL_REGEX, ErrorMessage = "No corresponde a un formato de correo electrónico.")]
        public string Email { get; set; } = string.Empty;
    }
}
