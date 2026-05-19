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
    public class VerificationCodeServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

        private readonly Mock<IEmailsSenderService> _emailsSenderServiceMock;
        private readonly Mock<ILogger<VerificationCodeService>> _loggerMock;

        private readonly VerificationCodeService _service;

        public VerificationCodeServiceTest()
        {
            _fixture = new Fixture();

            _userManagerMock = CreateUserManagerMock();

            _emailsSenderServiceMock = new Mock<IEmailsSenderService>();
            _loggerMock = new Mock<ILogger<VerificationCodeService>>();

            _service = new VerificationCodeService(
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
        public async Task GenerateCode_Should_Send_Verification_Email()
        {
            // Arrange
            var email = "test@test.com";

            var user = _fixture.Build<ApplicationUser>()
                .With(x => x.Email, email)
                .Create();

            var token = "verification-token";

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.GenerateEmailConfirmationTokenAsync(user))
                .ReturnsAsync(token);

            // Act
            await _service.GenerateCode(email);

            // Assert
            _emailsSenderServiceMock.Verify(
                x => x.SendVerificationCode(
                    user.FirstName,
                    email,
                    It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task GenerateCode_Should_Throw_When_User_Not_Found()
        {
            // Arrange
            var email = "test@test.com";

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            Func<Task> act = async () =>
                await _service.GenerateCode(email);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Usuario inexistente.");

            _emailsSenderServiceMock.Verify(
                x => x.SendVerificationCode(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task GenerateCode_Should_Send_Encoded_Token()
        {
            // Arrange
            var email = "test@test.com";

            var user = _fixture.Create<ApplicationUser>();

            var rawToken = "raw-token";

            string? sentToken = null;

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.GenerateEmailConfirmationTokenAsync(user))
                .ReturnsAsync(rawToken);

            _emailsSenderServiceMock
                .Setup(x => x.SendVerificationCode(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Callback<string, string, string>((_, _, token) =>
                {
                    sentToken = token;
                });

            // Act
            await _service.GenerateCode(email);

            // Assert
            sentToken.Should().NotBeNull();
            sentToken.Should().NotBe(rawToken);
        }

        [Fact]
        public async Task ConfirmEmailAsync_Should_Confirm_Email()
        {
            // Arrange
            var email = "test@test.com";

            var user = _fixture.Build<ApplicationUser>()
                .With(x => x.EmailConfirmed, false)
                .With(x => x.IsActive, false)
                .Create();

            var rawToken = "confirm-token";

            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(rawToken));

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.ConfirmEmailAsync(user, rawToken))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _service.ConfirmEmailAsync(email, encodedToken);

            // Assert
            user.EmailConfirmed.Should().BeTrue();

            user.IsActive.Should().BeTrue();

            _userManagerMock.Verify(
                x => x.UpdateAsync(user),
                Times.Once);
        }

        [Theory]
        [InlineData(null, "token")]
        [InlineData("test@test.com", null)]
        [InlineData("", "token")]
        [InlineData("test@test.com", "")]
        public async Task ConfirmEmailAsync_Should_Throw_When_Email_Or_Token_Are_Invalid(string? email, string? token)
        {
            // Act
            Func<Task> act = async () =>
                await _service.ConfirmEmailAsync(email, token);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Email y token son requeridos.");
        }

        [Fact]
        public async Task ConfirmEmailAsync_Should_Throw_When_User_Not_Found()
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
                await _service.ConfirmEmailAsync(email, token);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ConfirmEmailAsync_Should_Throw_When_Email_Already_Confirmed()
        {
            // Arrange
            var email = "test@test.com";

            var token = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes("token"));

            var user = _fixture.Build<ApplicationUser>()
                .With(x => x.EmailConfirmed, true)
                .Create();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            // Act
            Func<Task> act = async () =>
                await _service.ConfirmEmailAsync(email, token);

            // Assert
            await act.Should()
                .ThrowAsync<UpdateObjectException>()
                .WithMessage("El email del usuario ya ha sido confirmado.");
        }

        [Fact]
        public async Task ConfirmEmailAsync_Should_Throw_When_Confirmation_Fails()
        {
            // Arrange
            var email = "test@test.com";

            var rawToken = "token";

            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(rawToken));

            var user = _fixture.Build<ApplicationUser>()
                .With(x => x.EmailConfirmed, false)
                .Create();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.ConfirmEmailAsync(user, rawToken))
                .ReturnsAsync(IdentityResult.Failed());

            // Act
            Func<Task> act = async () =>
                await _service.ConfirmEmailAsync(email, encodedToken);

            // Assert
            await act.Should()
                .ThrowAsync<UpdateObjectException>();
        }

        [Fact]
        public async Task ConfirmEmailAsync_Should_Decode_Token()
        {
            // Arrange
            var email = "test@test.com";

            var rawToken = "my-token";

            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(rawToken));

            var user = _fixture.Create<ApplicationUser>();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.ConfirmEmailAsync(user, rawToken))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _service.ConfirmEmailAsync(email, encodedToken);

            // Assert
            _userManagerMock.Verify(
                x => x.ConfirmEmailAsync(user, rawToken),
                Times.Once);
        }
    }
}
