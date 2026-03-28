using AsadaLisboaBackend.Utils;
using System.ComponentModel.DataAnnotations;

namespace AsadaLisboaBackend.Models.DTOs.Account
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Nombre es requerido.")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Primer apellido es requerido.")]
        public string FirstLastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Segundo apellido es requerido.")]
        public string SecondLastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El cargo es requerido.")]
        public Guid ChargeId { get; set; }

        [Required(ErrorMessage = "Email es requerido.")]
        [RegularExpression(Constants.EMAIL_REGEX, ErrorMessage = "Formato incorrecto para un email.")]
        public string Email { get; set; } = string.Empty;
        
        [RegularExpression(Constants.PHONE_REGEX, ErrorMessage = "Formato incorrecto para un número telefónico.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contraseña es requerida.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "La contraseña debe incluir mayúsculas, minúsculas y números.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmar Contraseña es requerida.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "La contraseña debe incluir mayúsculas, minúsculas y números.")]
        [Compare("Password", ErrorMessage = "Debe coincidir con la contraseña.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
