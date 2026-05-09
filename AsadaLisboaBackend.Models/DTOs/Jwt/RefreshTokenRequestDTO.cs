namespace AsadaLisboaBackend.Models.DTOs.Jwt
{
    public class RefreshTokenRequestDTO
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
