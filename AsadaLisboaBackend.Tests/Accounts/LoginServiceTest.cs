using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Tests.Fakes;
using AsadaLisboaBackend.Models.DTOs.Jwt;
using AsadaLisboaBackend.Services.Accounts;
using AsadaLisboaBackend.Models.DTOs.Account;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Jwts;

namespace AsadaLisboaBackend.Tests.Accounts
{
    public class LoginServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<FakeSignInManager> _signInManagerMock;

        private readonly Mock<IJwtsService> _jwtsServiceMock;
        private readonly Mock<ILogger<LoginService>> _loggerMock;

        private readonly LoginService _service;

        public LoginServiceTest()
        {
            _fixture = new Fixture();

            _userManagerMock = CreateUserManagerMock();

            _signInManagerMock = CreateSignInManagerMock(_userManagerMock);

            _jwtsServiceMock = new Mock<IJwtsService>();
            _loggerMock = new Mock<ILogger<LoginService>>();

            _service = new LoginService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _jwtsServiceMock.Object,
                _loggerMock.Object
            );
        }

        private Mock<UserManager<ApplicationUser>> CreateUserManagerMock()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();

            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>();

            var userValidators = new List<IUserValidator<ApplicationUser>>();
            var passwordValidators = new List<IPasswordValidator<ApplicationUser>>();

            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errors = new Mock<IdentityErrorDescriber>();
            var services = new Mock<IServiceProvider>();

            var logger = new Mock<ILogger<UserManager<ApplicationUser>>>();

            return new Mock<UserManager<ApplicationUser>>(
                store.Object,
                options.Object,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                keyNormalizer.Object,
                errors.Object,
                services.Object,
                logger.Object
            );
        }

        private Mock<FakeSignInManager> CreateSignInManagerMock(Mock<UserManager<ApplicationUser>> userManagerMock)
        {
            var contextAccessor =
                new Mock<IHttpContextAccessor>();

            var claimsFactory =
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

            var options =
                new Mock<IOptions<IdentityOptions>>();

            options.Setup(x => x.Value)
                .Returns(new IdentityOptions());

            var logger =
                new Mock<ILogger<SignInManager<ApplicationUser>>>();

            var schemes =
                new Mock<IAuthenticationSchemeProvider>();

            return new Mock<FakeSignInManager>(
                userManagerMock.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                options.Object,
                logger.Object,
                schemes.Object
            );
        }

        [Fact]
        public async Task Login_Should_Return_Tokens_When_Credentials_Are_Valid()
        {
            // Arrange
            var request = _fixture.Build<LoginRequestDTO>()
                .With(x => x.Email, "test@test.com")
                .With(x => x.Password, "Password123!")
                .Create();

            var user = _fixture.Build<ApplicationUser>()
                .With(x => x.Email, request.Email)
                .Create();

            var authResponse = _fixture.Build<AuthenticationResponseDTO>()
                .With(x => x.RefreshToken, "refresh-token")
                .Create();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(
                    user,
                    request.Password,
                    true,
                    false))
                .ReturnsAsync(SignInResult.Success);

            _jwtsServiceMock
                .Setup(x => x.GenerateToken(user))
                .ReturnsAsync(authResponse);

            _userManagerMock
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _service.Login(request);

            // Assert

            result.Should().NotBeNull();

            result.Should().BeEquivalentTo(authResponse);

            user.RefreshToken.Should().Be(authResponse.RefreshToken);

            user.RefreshTokenExpiration.Should()
                .Be(authResponse.RefreshTokenExpiration);

            _userManagerMock.Verify(
                x => x.UpdateAsync(user),
                Times.Once);

            _jwtsServiceMock.Verify(
                x => x.GenerateToken(user),
                Times.Once);
        }

        [Fact]
        public async Task Login_Should_Throw_When_User_Does_Not_Exist()
        {
            // Arrange
            var request = _fixture.Create<LoginRequestDTO>();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            Func<Task> act = async () => await _service.Login(request);

            // Assert

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("No existe un usuario con este correo electrónico.");

            _signInManagerMock.Verify(
                x => x.PasswordSignInAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()),
                Times.Never);

            _jwtsServiceMock.Verify(
                x => x.GenerateToken(It.IsAny<ApplicationUser>()),
                Times.Never);
        }

        [Fact]
        public async Task Login_Should_Throw_When_Credentials_Are_Invalid()
        {
            // Arrange
            var request = _fixture.Create<LoginRequestDTO>();

            var user = _fixture.Create<ApplicationUser>();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(
                    user,
                    request.Password,
                    true,
                    false))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            Func<Task> act = async () => await _service.Login(request);

            // Assert

            await act.Should()
                .ThrowAsync<InvalidCredentialsException>()
                .WithMessage("Correo electrónico y/o contraseña incorrectos.");

            _jwtsServiceMock.Verify(
                x => x.GenerateToken(It.IsAny<ApplicationUser>()),
                Times.Never);

            _userManagerMock.Verify(
                x => x.UpdateAsync(It.IsAny<ApplicationUser>()),
                Times.Never);
        }

        [Fact]
        public async Task Login_Should_Update_User_With_RefreshToken()
        {
            // Arrange
            var request = _fixture.Create<LoginRequestDTO>();

            var user = _fixture.Create<ApplicationUser>();

            var authResponse = _fixture.Build<AuthenticationResponseDTO>()
                .With(x => x.RefreshToken, "new-refresh-token")
                .With(x => x.RefreshTokenExpiration, DateTime.UtcNow.AddDays(7))
                .Create();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(
                    user,
                    request.Password,
                    true,
                    false))
                .ReturnsAsync(SignInResult.Success);

            _jwtsServiceMock
                .Setup(x => x.GenerateToken(user))
                .ReturnsAsync(authResponse);

            _userManagerMock
                .Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _service.Login(request);

            // Assert

            _userManagerMock.Verify(
                x => x.UpdateAsync(It.Is<ApplicationUser>(u =>
                    u.RefreshToken == authResponse.RefreshToken &&
                    u.RefreshTokenExpiration == authResponse.RefreshTokenExpiration
                )),
                Times.Once);
        }

        [Fact]
        public async Task Login_Should_Propagate_Exception_When_GenerateToken_Fails()
        {
            // Arrange
            var request = _fixture.Create<LoginRequestDTO>();

            var user = _fixture.Create<ApplicationUser>();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(
                    user,
                    request.Password,
                    true,
                    false))
                .ReturnsAsync(SignInResult.Success);

            _jwtsServiceMock
                .Setup(x => x.GenerateToken(user))
                .ThrowsAsync(new Exception("JWT Error"));

            // Act
            Func<Task> act = async () => await _service.Login(request);

            // Assert

            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("JWT Error");
        }
    }
}
