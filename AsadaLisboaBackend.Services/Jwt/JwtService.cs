using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using AsadaLisboaBackend.Models.DTOs.Jwt;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Jwt;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Models.IdentityModels;

namespace AsadaLisboaBackend.Services.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly RefreshJwtOptions _refreshJwtOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtService(IOptions<JwtOptions> jwtOptions, IOptions<RefreshJwtOptions> jwtRefreshOptions, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            _refreshJwtOptions = jwtRefreshOptions.Value;
        }

        public AuthenticationResponseDTO GenerateToken(ApplicationUser user)
        {
            // Expiration dates to JWT and refresh token.
            DateTime expirationToken = DateTime.UtcNow.AddMinutes(_jwtOptions.EXPIRATION_MINUTES);
            DateTime expirationRefreshToken = DateTime.UtcNow.AddMinutes(_refreshJwtOptions.EXPIRATION_MINUTES);

            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject => Who's the user, user's ID. 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID => Token unique identifier.
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()), // Issued At => Exact moment of issuing.
                new Claim(ClaimTypes.Email, user.Email?.ToString() ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Email?.ToString() ?? ""),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.FirstLastName} {user.SecondLastName}"),
            };

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
                throw new SecurityTokenException("Token dado es inválido o expirado.");

            return principal;
        }

        public async Task DeleteToken()
        {
            Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid id);

            if(id == Guid.Empty)
                throw new NotFoundException("Error al cerrar sesión.");

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
                throw new NotFoundException("Error al cerrar sesión.");

            user.RefreshToken = null;
            user.RefreshTokenExpiration = DateTime.MinValue;

            await _userManager.UpdateAsync(user);
        }

        public async Task<AuthenticationResponseDTO> ValidateRefreshToken(RefreshTokenRequestDTO refreshTokenRequestDTO)
        {
            if (refreshTokenRequestDTO.Token is null)
                throw new InvalidAccessTokenException("Token de acceso inválido.");

            string token = refreshTokenRequestDTO.Token;
            string? refreshToken = refreshTokenRequestDTO.RefreshToken;

            var principal = GetClaimsPrincipal(token);

            if (principal is null)
                throw new InvalidAccessTokenException("Token de acceso inválido.");

            string? email = principal.FindFirstValue(ClaimTypes.Email);

            if (email is null)
                throw new InvalidAccessTokenException("Token de acceso inválido.");

            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                throw new NotFoundException("Usuario inexistente.");

            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiration < DateTime.UtcNow)
                throw new InvalidRefreshTokenException("Token de refrescamiento inválido.");

            AuthenticationResponseDTO authenticationResponseDTO = GenerateToken(user);

            user.RefreshToken = authenticationResponseDTO.RefreshToken;
            user.RefreshTokenExpiration = authenticationResponseDTO.RefreshTokenExpiration;

            await _userManager.UpdateAsync(user);

            return authenticationResponseDTO;
        }

        private string GenerateRefreshToken()
        {
            Byte[] bytes = new Byte[64];
            var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
