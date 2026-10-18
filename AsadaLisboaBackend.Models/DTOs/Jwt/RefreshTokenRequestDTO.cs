namespace AsadaLisboaBackend.Models.DTOs.Jwt;

public class RefreshTokenRequestDto
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
}