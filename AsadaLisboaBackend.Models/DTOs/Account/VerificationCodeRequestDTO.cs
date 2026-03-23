namespace AsadaLisboaBackend.Models.DTOs.Account
{
    public class VerificationCodeRequestDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
