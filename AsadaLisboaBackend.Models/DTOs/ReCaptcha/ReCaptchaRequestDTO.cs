using System.ComponentModel.DataAnnotations;

namespace AsadaLisboaBackend.Models.DTOs.ReCaptcha
{
    public class ReCaptchaRequestDTO
    {
        [Required(ErrorMessage = "El token de ReCAPTCHA es obligatorio.")]
        public string ReCaptchaRequest { get; set; } = string.Empty;
    }
}
