using System.ComponentModel.DataAnnotations;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Models.DTOs
{
    public class ForgotPasswordRequestDTO
    {
        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [RegularExpression(Constants.EMAIL_REGEX, ErrorMessage = "No corresponde a un formato de correo electrónico.")]
        public string Email { get; set; } = string.Empty;
    }
}
