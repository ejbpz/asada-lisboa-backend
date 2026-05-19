using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Moq;
using FluentAssertions;
using AsadaLisboaBackend.Services.Jwts;
using AsadaLisboaBackend.Models.DTOs.Jwt;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Models.IdentityModels;

namespace AsadaLisboaBackend.Tests.Jwts
{
    public class JwtsServiceTest
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<ILogger<JwtsService>> _loggerMock;

        private readonly JwtsService _service;

        public JwtsServiceTest()
        {
            _userManagerMock = CreateUserManagerMock();

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _loggerMock = new Mock<ILogger<JwtsService>>();

            var jwtOptions = Options.Create(new JwtOptions
            {
                KEY = "THIS_IS_A_SUPER_SECRET_KEY_123456789",
                ISSUER = "test-issuer",
                AUDIENCE = "test-audience",
                EXPIRATION_MINUTES = 60
            });

            var refreshOptions = Options.Create(new RefreshJwtOptions
            {
                EXPIRATION_MINUTES = 120
            });

            _service = new JwtsService(
                jwtOptions,
                refreshOptions,
                _userManagerMock.Object,
                _httpContextAccessorMock.Object,
                _loggerMock.Object
            );
        }

        private Mock<UserManager<ApplicationUser>> CreateUserManagerMock()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();

            return new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!);
        }

        [Fact]
        public async Task GenerateToken_Should_Return_Valid_Token()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                FirstName = "Eduardo",
                FirstLastName = "Brenes",
                SecondLastName = "Pérez"
            };

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Admin" });

            // Act
            var result = await _service.GenerateToken(user);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrWhiteSpace();
            result.RefreshToken.Should().NotBeNullOrWhiteSpace();
            result.Email.Should().Be(user.Email);
            result.FullName.Should()
                .Be("Eduardo Brenes Pérez");
        }

        [Fact]
        public async Task GenerateToken_Should_Contain_Role_Claims()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                FirstName = "Eduardo",
                FirstLastName = "Brenes",
                SecondLastName = "Pérez"
            };

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>
                {
                    "Admin",
                    "User"
                });

            // Act
            var response = await _service.GenerateToken(user);
            var principal = _service.GetClaimsPrincipal(response.Token);

            // Assert
            principal!.Claims.Should()
                .Contain(x =>
                    x.Type == ClaimTypes.Role &&
                    x.Value == "Admin");

            principal.Claims.Should()
                .Contain(x =>
                    x.Type == ClaimTypes.Role &&
                    x.Value == "User");
        }

        [Fact]
        public async Task GetClaimsPrincipal_Should_Return_Principal()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                FirstName = "Eduardo",
                FirstLastName = "Brenes",
                SecondLastName = "Pérez"
            };

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var auth = await _service.GenerateToken(user);

            // Act
            var principal = _service.GetClaimsPrincipal(auth.Token);

            // Assert
            principal.Should().NotBeNull();
            principal!.FindFirstValue(ClaimTypes.Email)
                .Should().Be(user.Email);
        }

        [Fact]
        public async Task DeleteToken_Should_Clear_Refresh_Token()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var user = new ApplicationUser
            {
                Id = userId,
                RefreshToken = "REFRESH_TOKEN"
            };

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var identity = new ClaimsIdentity(claims);

            var principal = new ClaimsPrincipal(identity);

            var context = new DefaultHttpContext
            {
                User = principal
            };

            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(context);

            _userManagerMock
                .Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _service.DeleteToken();

            // Assert
            user.RefreshToken.Should().BeNull();
            user.RefreshTokenExpiration.Should()
                .Be(DateTime.MinValue);

            _userManagerMock.Verify(
                x => x.UpdateAsync(user),
                Times.Once);
        }

        [Fact]
        public async Task DeleteToken_Should_Throw_When_NameIdentifier_Is_Invalid()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, "INVALID_GUID")
            };

            var principal = new ClaimsPrincipal(
                new ClaimsIdentity(claims));

            var context = new DefaultHttpContext
            {
                User = principal
            };

            _httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(context);

            // Act
            Func<Task> act = async () =>
                await _service.DeleteToken();

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ValidateRefreshToken_Should_Throw_When_Token_Is_Null()
        {
            // Arrange
            var request = new RefreshTokenRequestDTO
            {
                Token = null!,
                RefreshToken = "refresh"
            };

            // Act
            Func<Task> act = async () =>
                await _service.ValidateRefreshToken(request);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidAccessTokenException>();
        }

        [Fact]
        public async Task ValidateRefreshToken_Should_Throw_When_User_Not_Found()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                FirstName = "Eduardo",
                FirstLastName = "Brenes",
                SecondLastName = "Pérez"
            };

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var auth = await _service.GenerateToken(user);

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync((ApplicationUser?)null);

            var request = new RefreshTokenRequestDTO
            {
                Token = auth.Token,
                RefreshToken = auth.RefreshToken
            };

            // Act
            Func<Task> act = async () =>
                await _service.ValidateRefreshToken(request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ValidateRefreshToken_Should_Throw_When_RefreshToken_Is_Invalid()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                FirstName = "Eduardo",
                FirstLastName = "Brenes",
                SecondLastName = "Pérez",
                RefreshToken = "VALID_REFRESH",
                RefreshTokenExpiration = DateTime.UtcNow.AddHours(1)
            };

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var auth = await _service.GenerateToken(user);

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            var request = new RefreshTokenRequestDTO
            {
                Token = auth.Token,
                RefreshToken = "INVALID_REFRESH"
            };

            // Act
            Func<Task> act = async () =>
                await _service.ValidateRefreshToken(request);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidRefreshTokenException>();
        }

        [Fact]
        public async Task ValidateRefreshToken_Should_Throw_When_RefreshToken_Expired()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                FirstName = "Eduardo",
                FirstLastName = "Brenes",
                SecondLastName = "Pérez",
                RefreshToken = "VALID_REFRESH",
                RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(-1)
            };

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var auth = await _service.GenerateToken(user);

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            var request = new RefreshTokenRequestDTO
            {
                Token = auth.Token,
                RefreshToken = "VALID_REFRESH"
            };

            // Act
            Func<Task> act = async () =>
                await _service.ValidateRefreshToken(request);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidRefreshTokenException>();
        }

        [Fact]
        public async Task ValidateRefreshToken_Should_Return_New_Tokens()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                FirstName = "Eduardo",
                FirstLastName = "Brenes",
                SecondLastName = "Pérez",
                RefreshTokenExpiration = DateTime.UtcNow.AddHours(1)
            };

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var auth = await _service.GenerateToken(user);

            user.RefreshToken = auth.RefreshToken;

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            var request = new RefreshTokenRequestDTO
            {
                Token = auth.Token,
                RefreshToken = auth.RefreshToken
            };

            // Act
            var result = await _service.ValidateRefreshToken(request);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrWhiteSpace();
            result.RefreshToken.Should().NotBeNullOrWhiteSpace();
            result.RefreshToken.Should()
                .NotBe(auth.RefreshToken);

            _userManagerMock.Verify(
                x => x.UpdateAsync(user),
                Times.Once);
        }
    }
}
