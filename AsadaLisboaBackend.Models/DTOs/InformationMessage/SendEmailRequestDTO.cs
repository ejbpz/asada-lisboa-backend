using System.ComponentModel.DataAnnotations;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Models.DTOs.InformationMessage
{
    public class SendEmailRequestDTO
    {
        [Required(ErrorMessage = "Su nombre es requerido.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "El nombre debe ser entre {1} y {0} caracteres.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [RegularExpression(Constants.EMAIL_REGEX, ErrorMessage = "No corresponde a un formato de correo electrónico.")]
        public string Email { get; set; } = string.Empty;

        [RegularExpression(Constants.PHONE_REGEX, ErrorMessage = "No corresponde a un formato de teléfono celular.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "El asunto del mensaje es requerido.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "El asunto del mensaje debe ser entre {1} y {0} caracteres.")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "El mensaje es requerido.")]
        [StringLength(320, MinimumLength = 20, ErrorMessage = "El mensaje debe ser entre {1} y {0} caracteres.")]
        public string Message { get; set; } = string.Empty;
    }
}
