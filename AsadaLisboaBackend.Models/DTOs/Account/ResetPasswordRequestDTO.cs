using System.ComponentModel.DataAnnotations;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Models.DTOs.Account
{
    public class ResetPasswordRequestDTO
    {
        [Required(ErrorMessage = "La contraseña es requerida.")]
        [RegularExpression(Constants.PASSWORD_REGEX, ErrorMessage = "No corresponde a un formato de contraseña.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmación de la contraseña es requerida.")]
        [Compare(nameof(Password), ErrorMessage = "Ambas contraseñas deben coincidir.")]
        [RegularExpression(Constants.PASSWORD_REGEX, ErrorMessage = "No corresponde a un formato de contraseña.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "La confirmación de la contraseña debe tener entre {2} y {1} caracteres.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "El token para el cambio de contraseña es requerido.")]
        public string Token { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [RegularExpression(Constants.EMAIL_REGEX, ErrorMessage = "No corresponde a un formato de correo electrónico.")]
        public string Email { get; set; } = string.Empty;
    }
}
