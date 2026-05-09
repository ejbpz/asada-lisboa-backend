namespace AsadaLisboaBackend.Models.DTOs.Account
{
    public class VerificationCodeRequestDTO
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
