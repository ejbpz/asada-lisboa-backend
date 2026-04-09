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

namespace AsadaLisboaBackend.Services.Jwts
{
    public class JwtsService : IJwtsService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<JwtsService> _logger;
        private readonly RefreshJwtOptions _refreshJwtOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtsService(IOptions<JwtOptions> jwtOptions, IOptions<RefreshJwtOptions> jwtRefreshOptions, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ILogger<JwtsService> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            _refreshJwtOptions = jwtRefreshOptions.Value;
        }

        public async Task<AuthenticationResponseDTO> GenerateToken(ApplicationUser user)
        {
            _logger.LogInformation("Generando token JWT para el usuario con email: {Email}", user.Email);

            // Expiration dates to JWT and refresh token.
            DateTime expirationToken = DateTime.UtcNow.AddMinutes(_jwtOptions.EXPIRATION_MINUTES);
            DateTime expirationRefreshToken = DateTime.UtcNow.AddMinutes(_refreshJwtOptions.EXPIRATION_MINUTES);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject => Who's the user, user's ID. 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID => Token unique identifier.
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()), // Issued At => Exact moment of issuing.
                new Claim(ClaimTypes.Email, user.Email?.ToString() ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Email?.ToString() ?? ""),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.FirstLastName} {user.SecondLastName}"),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            
            // Converts Key (string) to Bytes.
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.KEY));
            // Generate credentials using Key and Algorithm.
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Creates token structure (Headers, Payload and Signature).
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                _jwtOptions.ISSUER,
                _jwtOptions.AUDIENCE,
                claims,
                expires: expirationToken,
                signingCredentials: signingCredentials
            );

            // Token generation serialized.
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            string token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            return new AuthenticationResponseDTO()
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
            _logger.LogInformation("Validando token JWT para obtener ClaimsPrincipal.");

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _jwtOptions.AUDIENCE,
                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.ISSUER,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.KEY)),
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out SecurityToken securityToken
            );

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogError("Token JWT inválido o con algoritmo de firma no permitido.");
                throw new SecurityTokenException("Token dado es inválido o expirado.");
            }

            return principal;
        }

        public async Task DeleteToken()
        {
            _logger.LogInformation("Eliminando token JWT para el usuario autenticado.");

            Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid id);

            if(id == Guid.Empty)
            {
                _logger.LogError("No se pudo obtener el ID del usuario desde el token JWT.");
                throw new NotFoundException("Error al cerrar sesión.");
            }

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
            {
                _logger.LogError("Usuario con id {UserId} no encontrado para eliminar el token JWT.", id);
                throw new NotFoundException("Error al cerrar sesión.");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiration = DateTime.MinValue;

            await _userManager.UpdateAsync(user);
        }

        public async Task<AuthenticationResponseDTO> ValidateRefreshToken(RefreshTokenRequestDTO refreshTokenRequestDTO)
        {
            _logger.LogInformation("Validando token de refrescamiento para el usuario con email: {Email}", refreshTokenRequestDTO.Token);

            if (refreshTokenRequestDTO.Token is null || string.IsNullOrEmpty(refreshTokenRequestDTO.Token) || string.IsNullOrWhiteSpace(refreshTokenRequestDTO.Token))
            {
                _logger.LogError("Token de acceso proporcionado es nulo, vacío o solo espacios en blanco.");
                throw new InvalidAccessTokenException("Token de acceso inválido.");
            }

            string token = refreshTokenRequestDTO.Token;
            string? refreshToken = refreshTokenRequestDTO.RefreshToken;

            var principal = GetClaimsPrincipal(token);

            if (principal is null)
            {
                _logger.LogError("No se pudo obtener ClaimsPrincipal del token de acceso proporcionado.");
                throw new InvalidAccessTokenException("Token de acceso inválido.");
            }

            string? email = principal.FindFirstValue(ClaimTypes.Email);

            if (email is null || string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
            {
                _logger.LogError("No se pudo obtener el email del usuario desde el token de acceso proporcionado.");
                throw new InvalidAccessTokenException("Token de acceso inválido.");
            }

            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                _logger.LogError("Usuario con email {Email} no encontrado para validar el token de refrescamiento.", email);
                throw new NotFoundException("Usuario inexistente.");
            }

            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiration < DateTime.UtcNow || string.IsNullOrEmpty(user.RefreshToken) || string.IsNullOrWhiteSpace(user.RefreshToken))
            {
                _logger.LogError("Token de refrescamiento inválido para el usuario con email {Email}.", email);
                throw new InvalidRefreshTokenException("Token de refrescamiento inválido.");
            }

            AuthenticationResponseDTO authenticationResponseDTO = await GenerateToken(user);

            user.RefreshToken = authenticationResponseDTO.RefreshToken;
            user.RefreshTokenExpiration = authenticationResponseDTO.RefreshTokenExpiration;

            await _userManager.UpdateAsync(user);

            return authenticationResponseDTO;
        }

        private string GenerateRefreshToken()
        {
            _logger.LogInformation("Generando token de refrescamiento.");

            Byte[] bytes = new Byte[64];
            var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
