using System.Security.Claims;
using AsadaLisboaBackend.Models.DTOs.Jwt;
using AsadaLisboaBackend.Models.IdentityModels;

namespace AsadaLisboaBackend.ServiceContracts.Jwts;

public interface IJwtsService
{
    public Task<AuthenticationResponseDto> GenerateToken(ApplicationUser user);
    public ClaimsPrincipal? GetClaimsPrincipal(string token);
    public Task DeleteToken();
    public Task<AuthenticationResponseDto> ValidateRefreshToken(RefreshTokenRequestDto refreshTokenRequestDto);
}