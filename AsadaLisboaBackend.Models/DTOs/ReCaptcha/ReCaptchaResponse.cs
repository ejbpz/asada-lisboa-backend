namespace AsadaLisboaBackend.Models.DTOs.ReCaptcha
{
    public class ReCaptchaResponse
    {
        public bool Success { get; set; }
        public List<string> ErrorCodes { get; set; } = new List<string>();
    }
}
