using System.ComponentModel.DataAnnotations;

namespace AsadaLisboaBackend.Models.DTOs
{
    public class ResetPasswordRequestDTO
    {
        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        public string Email { get; set; } = string.Empty;
    }
}
