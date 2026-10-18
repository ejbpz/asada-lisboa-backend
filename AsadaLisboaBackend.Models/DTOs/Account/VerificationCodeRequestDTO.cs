namespace AsadaLisboaBackend.Models.DTOs.Account;

public class VerificationCodeRequestDto
{
    public string? Email { get; set; }
    public string? Token { get; set; }
}