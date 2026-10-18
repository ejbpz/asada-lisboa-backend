using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using AsadaLisboaBackend.Models.DTOs.Jwt;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Jwts;

namespace AsadaLisboaBackend.Services.Jwts;

public class JwtsService(
    IOptions<JwtOptions> jwtOptions,
    IOptions<RefreshJwtOptions> jwtRefreshOptions,
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor httpContextAccessor,
    ILogger<JwtsService> logger)
    : IJwtsService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly RefreshJwtOptions _refreshJwtOptions = jwtRefreshOptions.Value;

    public async Task<AuthenticationResponseDto> GenerateToken(ApplicationUser user)
    {
        logger.LogInformation("Generando token JWT para el usuario con email: {Email}", user.Email);

        // Expiration dates to JWT and refresh token.
        var expirationToken = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes);
        var expirationRefreshToken = DateTime.UtcNow.AddMinutes(_refreshJwtOptions.ExpirationMinutes);

        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject => Who's the user, user's ID. 
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID => Token unique identifier.
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()), // Issued At => Exact moment of issuing.
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.NameIdentifier, user.Email ?? ""),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.FirstLastName} {user.SecondLastName}"),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            
        // Converts Key (string) to Bytes.
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        // Generate credentials using Key and Algorithm.
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Creates token structure (Headers, Payload and Signature).
        var jwtSecurityToken = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: expirationToken,
            signingCredentials: signingCredentials
        );

        // Token generation serialized.
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

        return new AuthenticationResponseDto()
        {
            Token = token,
            Email = user.Email ?? "",
            ExpirationToken = expirationToken,
            RefreshToken = GenerateRefreshToken(),
            RefreshTokenExpiration = expirationRefreshToken,
            FullName = $"{user.FirstName} {user.FirstLastName} {user.SecondLastName}",
        };
    }

    public ClaimsPrincipal? GetClaimsPrincipal(string token)
    {
        logger.LogInformation("Validando token JWT para obtener ClaimsPrincipal.");

        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidAudience = _jwtOptions.Audience,
            ValidateIssuer = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var principal = jwtSecurityTokenHandler.ValidateToken(
            token,
            tokenValidationParameters,
            out var securityToken
        );

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            logger.LogError("Token JWT inválido o con algoritmo de firma no permitido.");
            throw new SecurityTokenException("Token dado es inválido o expirado.");
        }

        return principal;
    }

    public async Task DeleteToken()
    {
        logger.LogInformation("Eliminando token JWT para el usuario autenticado.");

        Guid.TryParse(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id);

        if(id == Guid.Empty)
        {
            logger.LogError("No se pudo obtener el ID del usuario desde el token JWT.");
            throw new NotFoundException("Error al cerrar sesión.");
        }

        var user = await userManager.FindByIdAsync(id.ToString());

        if (user is null)
        {
            logger.LogError("Usuario con id {UserId} no encontrado para eliminar el token JWT.", id);
            throw new NotFoundException("Error al cerrar sesión.");
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiration = DateTime.MinValue;

        await userManager.UpdateAsync(user);
    }

    public async Task<AuthenticationResponseDto> ValidateRefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        logger.LogInformation("Validando token de refrescamiento para el usuario con email: {Email}", refreshTokenRequestDto.Token);

        if (refreshTokenRequestDto.Token is null || string.IsNullOrEmpty(refreshTokenRequestDto.Token) || string.IsNullOrWhiteSpace(refreshTokenRequestDto.Token))
        {
            logger.LogError("Token de acceso proporcionado es nulo, vacío o solo espacios en blanco.");
            throw new InvalidAccessTokenException("Token de acceso inválido.");
        }

        var token = refreshTokenRequestDto.Token;
        var refreshToken = refreshTokenRequestDto.RefreshToken;

        var principal = GetClaimsPrincipal(token);

        if (principal is null)
        {
            logger.LogError("No se pudo obtener ClaimsPrincipal del token de acceso proporcionado.");
            throw new InvalidAccessTokenException("Token de acceso inválido.");
        }

        var email = principal.FindFirstValue(ClaimTypes.Email);

        if (email is null || string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
        {
            logger.LogError("No se pudo obtener el email del usuario desde el token de acceso proporcionado.");
            throw new InvalidAccessTokenException("Token de acceso inválido.");
        }

        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            logger.LogError("Usuario con email {Email} no encontrado para validar el token de refrescamiento.", email);
            throw new NotFoundException("Usuario inexistente.");
        }

        if (user.RefreshToken != refreshToken || user.RefreshTokenExpiration < DateTime.UtcNow || string.IsNullOrEmpty(user.RefreshToken) || string.IsNullOrWhiteSpace(user.RefreshToken))
        {
            logger.LogError("Token de refrescamiento inválido para el usuario con email {Email}.", email);
            throw new InvalidRefreshTokenException("Token de refrescamiento inválido.");
        }

        var authenticationResponseDto = await GenerateToken(user);

        user.RefreshToken = authenticationResponseDto.RefreshToken;
        user.RefreshTokenExpiration = authenticationResponseDto.RefreshTokenExpiration;

        await userManager.UpdateAsync(user);

        return authenticationResponseDto;
    }

    private string GenerateRefreshToken()
    {
        logger.LogInformation("Generando token de refrescamiento.");

        var bytes = new Byte[64];
        var randomNumberGenerator = RandomNumberGenerator.Create();

        randomNumberGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}