using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Services.Accounts;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Emails;

namespace AsadaLisboaBackend.Tests.Accounts
{
    public class ResetPasswordServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

        private readonly Mock<IEmailsSenderService> _emailsSenderServiceMock;
        private readonly Mock<ILogger<ResetPasswordService>> _loggerMock;

        private readonly ResetPasswordService _service;

        public ResetPasswordServiceTest()
        {
            _fixture = new Fixture();

            _userManagerMock = CreateUserManagerMock();

            _emailsSenderServiceMock = new Mock<IEmailsSenderService>();
            _loggerMock = new Mock<ILogger<ResetPasswordService>>();

            _service = new ResetPasswordService(
                _userManagerMock.Object,
                _emailsSenderServiceMock.Object,
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
        public async Task ForgotPassword_Should_Send_Reset_Token()
        {
            // Arrange
            var email = "test@test.com";

            var user = _fixture.Build<ApplicationUser>()
                .With(x => x.Email, email)
                .With(x => x.EmailConfirmed, true)
                .Create();

            var rawToken = "raw-reset-token";

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync(rawToken);

            // Act
            await _service.ForgotPassword(email);

            // Assert

            _userManagerMock.Verify(
                x => x.GeneratePasswordResetTokenAsync(user),
                Times.Once);

            _emailsSenderServiceMock.Verify(
                x => x.SendResetPasswordToken(
                    user.FirstName,
                    email,
                    It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task ForgotPassword_Should_Throw_When_User_Does_Not_Exist()
        {
            // Arrange
            var email = "test@test.com";

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            Func<Task> act = async () => await _service.ForgotPassword(email);

            // Assert

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("No existe un usuario con este correo electrónico.");

            _emailsSenderServiceMock.Verify(
                x => x.SendResetPasswordToken(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task ForgotPassword_Should_Throw_When_Email_Is_Not_Confirmed()
        {
            // Arrange
            var email = "test@test.com";

            var user = _fixture.Build<ApplicationUser>()
                .With(x => x.Email, email)
                .With(x => x.EmailConfirmed, false)
                .Create();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            // Act
            Func<Task> act = async () => await _service.ForgotPassword(email);

            // Assert

            await act.Should()
                .ThrowAsync<NotFoundException>();

            _emailsSenderServiceMock.Verify(
                x => x.SendResetPasswordToken(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task ForgotPassword_Should_Send_Encoded_Token()
        {
            // Arrange
            var email = "test@test.com";

            var user = _fixture.Build<ApplicationUser>()
                .With(x => x.Email, email)
                .With(x => x.EmailConfirmed, true)
                .Create();

            var rawToken = "my-raw-token";

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync(rawToken);

            string? sentToken = null;

            _emailsSenderServiceMock
                .Setup(x => x.SendResetPasswordToken(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Callback<string, string, string>((_, _, token) =>
                {
                    sentToken = token;
                });

            // Act
            await _service.ForgotPassword(email);

            // Assert

            sentToken.Should().NotBeNull();

            sentToken.Should().NotBe(rawToken);
        }

        [Fact]
        public async Task ResetPassword_Should_Reset_Successfully()
        {
            // Arrange
            var email = "test@test.com";

            var user = _fixture.Build<ApplicationUser>()
                .With(x => x.Email, email)
                .With(x => x.EmailConfirmed, true)
                .Create();

            var token = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes("reset-token"));

            var password = "Password123!";

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.ResetPasswordAsync(
                    user,
                    "reset-token",
                    password))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _service.ResetPassword(email, token, password);

            // Assert

            _userManagerMock.Verify(
                x => x.ResetPasswordAsync(
                    user,
                    "reset-token",
                    password),
                Times.Once);
        }

        [Fact]
        public async Task ResetPassword_Should_Throw_When_User_Does_Not_Exist()
        {
            // Arrange
            var email = "test@test.com";

            var token = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes("token"));

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            Func<Task> act = async () =>
                await _service.ResetPassword(email, token, "Password123!");

            // Assert

            await act.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ResetPassword_Should_Throw_When_Email_Not_Confirmed()
        {
            // Arrange
            var email = "test@test.com";

            var token = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes("token"));

            var user = _fixture.Build<ApplicationUser>()
                .With(x => x.EmailConfirmed, false)
                .Create();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            // Act
            Func<Task> act = async () =>
                await _service.ResetPassword(email, token, "Password123!");

            // Assert

            await act.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ResetPassword_Should_Throw_When_Reset_Fails()
        {
            // Arrange
            var email = "test@test.com";

            var user = _fixture.Build<ApplicationUser>()
                .With(x => x.Email, email)
                .With(x => x.EmailConfirmed, true)
                .Create();

            var token = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes("reset-token"));

            var password = "Password123!";

            var identityErrors = new[]
            {
        new IdentityError
        {
            Code = "PasswordTooShort",
            Description = "Password too short"
        }
    };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.ResetPasswordAsync(
                    user,
                    "reset-token",
                    password))
                .ReturnsAsync(IdentityResult.Failed(identityErrors));

            // Act
            Func<Task> act = async () =>
                await _service.ResetPassword(email, token, password);

            // Assert

            var exception = await act.Should()
                .ThrowAsync<IdentityErrorException>();

            exception.Which.Errors.Should().HaveCount(1);

            exception.Which.Errors
                .First().Code
                .Should().Be("PasswordTooShort");
        }

        [Fact]
        public async Task ResetPassword_Should_Decode_Token_Before_Reset()
        {
            // Arrange
            var email = "test@test.com";

            var user = _fixture.Build<ApplicationUser>()
                .With(x => x.EmailConfirmed, true)
                .Create();

            var rawToken = "my-reset-token";

            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(rawToken));

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.ResetPasswordAsync(
                    user,
                    rawToken,
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _service.ResetPassword(
                email,
                encodedToken,
                "Password123!");

            // Assert

            _userManagerMock.Verify(
                x => x.ResetPasswordAsync(
                    user,
                    rawToken,
                    "Password123!"),
                Times.Once);
        }
    }
}
