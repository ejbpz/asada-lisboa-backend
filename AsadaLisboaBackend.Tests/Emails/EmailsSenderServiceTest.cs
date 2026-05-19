using AsadaLisboaBackend.Models.DTOs.InformationMessage;
using AsadaLisboaBackend.Services.Emails;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Utils.OptionsPattern;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Resend;
using System.Net;

namespace AsadaLisboaBackend.Tests.Emails
{
    public class EmailsSenderServiceTest
    {
        private readonly Mock<IResend> _resendMock;
        private readonly Mock<ILogger<EmailsSenderService>> _loggerMock;

        private readonly EmailsSenderService _service;

        public EmailsSenderServiceTest()
        {
            _resendMock = new Mock<IResend>();

            _loggerMock = new Mock<ILogger<EmailsSenderService>>();

            var options = Options.Create(new ContactEmailOptions
            {
                CONTACT_EMAIL = "contact@test.com"
            });

            _service = new EmailsSenderService(
                _resendMock.Object,
                options,
                _loggerMock.Object
            );
        }

        private ResendResponse<Guid> SuccessResponse()
        {
            return new ResendResponse<Guid>(
                Guid.NewGuid(),
                null
            );
        }

        private ResendResponse<Guid> FailResponse()
        {
            return new ResendResponse<Guid>(
                new ResendException(
                    HttpStatusCode.BadRequest,
                    ErrorType.ApplicationError,
                    "Error sending email"
                ),
                null
            );
        }

        [Fact]
        public async Task SendResetPasswordToken_Should_Send_Email()
        {
            // Arrange
            var name = "Eduardo";
            var email = "test@test.com";
            var token = "abc123";

            EmailMessage? sentMessage = null;

            _resendMock
                .Setup(x => x.EmailSendAsync(
                    It.IsAny<EmailMessage>(),
                    It.IsAny<CancellationToken>()))
                .Callback<EmailMessage, CancellationToken>((message, _) =>
                {
                    sentMessage = message;
                })
                .ReturnsAsync(SuccessResponse());

            // Act
            await _service.SendResetPasswordToken(name, email, token);

            // Assert
            sentMessage.Should().NotBeNull();

            sentMessage!.To.Should().Contain(email);

            sentMessage.Subject.Should()
                .Be("Token restaurar contraseña");

            sentMessage.Template.Should().NotBeNull();

            sentMessage.Template!.Variables.Should()
                .ContainKey("USER_NAME");

            sentMessage.Template!.Variables["USER_NAME"]
                .Should().Be(name);

            sentMessage.Template!.Variables["USER_EMAIL"]
                .Should().Be(email);

            sentMessage.Template!.Variables["RESET_LINK"]
                .Should().Be(
                    $"{Constants.CLIENT_HOST}/cuenta/restaurar-contrasena/?token={token}&email={email}");
        }

        [Fact]
        public async Task SendResetPasswordToken_Should_Throw_When_Send_Fails()
        {
            // Arrange
            _resendMock
                .Setup(x => x.EmailSendAsync(
                    It.IsAny<EmailMessage>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(FailResponse());

            // Act
            Func<Task> act = async () =>
                await _service.SendResetPasswordToken(
                    "Eduardo",
                    "test@test.com",
                    "token");

            // Assert
            await act.Should()
                .ThrowAsync<SendEmailException>()
                .WithMessage("Error al enviar el token.");
        }

        [Fact]
        public async Task SendContactMessage_Should_Send_Email()
        {
            // Arrange
            var request = new SendEmailRequestDTO
            {
                FullName = "Eduardo",
                Email = "edu@test.com",
                Subject = "Consulta",
                Message = "Hola mundo"
            };

            EmailMessage? sentMessage = null;

            _resendMock
                .Setup(x => x.EmailSendAsync(
                    It.IsAny<EmailMessage>(),
                    It.IsAny<CancellationToken>()))
                .Callback<EmailMessage, CancellationToken>((message, _) =>
                {
                    sentMessage = message;
                })
                .ReturnsAsync(SuccessResponse());

            // Act
            await _service.SendContactMessage(request);

            // Assert
            sentMessage.Should().NotBeNull();

            sentMessage!.To.Should()
                .Contain("contact@test.com");

            sentMessage.ReplyTo.Should()
                .Contain(x => x.Email == request.Email);

            sentMessage.Subject.Should()
                .Be(request.Subject);

            sentMessage.Template.Should().NotBeNull();
            sentMessage.Template!.Variables.Should().NotBeNull();

            sentMessage.Template!.Variables["CONTACT_MESSAGE"]
                .Should().Be(request.Message);
        }

        [Fact]
        public async Task SendContactMessage_Should_Throw_When_Send_Fails()
        {
            // Arrange
            var request = new SendEmailRequestDTO
            {
                FullName = "Eduardo",
                Email = "edu@test.com",
                Subject = "Consulta",
                Message = "Hola"
            };

            _resendMock
                .Setup(x => x.EmailSendAsync(
                    It.IsAny<EmailMessage>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(FailResponse());

            // Act
            Func<Task> act = async () =>
                await _service.SendContactMessage(request);

            // Assert
            await act.Should()
                .ThrowAsync<SendEmailException>()
                .WithMessage("Error al enviar el mensaje de contacto.");
        }

        [Fact]
        public async Task SendVerificationCode_Should_Send_Email()
        {
            // Arrange
            var name = "Eduardo";
            var email = "test@test.com";
            var token = "verification-token";

            EmailMessage? sentMessage = null;

            _resendMock
                .Setup(x => x.EmailSendAsync(
                    It.IsAny<EmailMessage>(),
                    It.IsAny<CancellationToken>()))
                .Callback<EmailMessage, CancellationToken>((message, _) =>
                {
                    sentMessage = message;
                })
                .ReturnsAsync(SuccessResponse());

            // Act
            await _service.SendVerificationCode(name, email, token);

            // Assert
            sentMessage.Should().NotBeNull();

            sentMessage!.To.Should().Contain(email);

            sentMessage.Subject.Should()
                .Be("Token confirmación correo electrónico");

            sentMessage.Template.Should().NotBeNull();

            sentMessage.Template!.Variables.Should().NotBeNull();

            sentMessage.Template!.Variables["USER_NAME"]
                .Should().Be(name);

            sentMessage.Template!.Variables["USER_EMAIL"]
                .Should().Be(email);

            sentMessage.Template!.Variables["RESET_LINK"]
                .Should().Be(
                    $"{Constants.CLIENT_HOST}/cuenta/confirmar-correo/?token={token}&email={email}");
        }

        [Fact]
        public async Task SendVerificationCode_Should_Throw_When_Send_Fails()
        {
            // Arrange
            _resendMock
                .Setup(x => x.EmailSendAsync(
                    It.IsAny<EmailMessage>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(FailResponse());

            // Act
            Func<Task> act = async () =>
                await _service.SendVerificationCode(
                    "Eduardo",
                    "test@test.com",
                    "token");

            // Assert
            await act.Should()
                .ThrowAsync<SendEmailException>()
                .WithMessage("Error al enviar el mensaje de contacto.");
        }

        [Fact]
        public async Task SendVerificationCode_Should_Use_Correct_TemplateId()
        {
            // Arrange
            EmailMessage? sentMessage = null;

            _resendMock
                .Setup(x => x.EmailSendAsync(
                    It.IsAny<EmailMessage>(),
                    It.IsAny<CancellationToken>()))
                .Callback<EmailMessage, CancellationToken>((message, _) =>
                {
                    sentMessage = message;
                })
                .ReturnsAsync(SuccessResponse());

            // Act
            await _service.SendVerificationCode(
                "Eduardo",
                "test@test.com",
                "token");

            // Assert
            sentMessage!.Template!.TemplateId.Should()
                .Be(new Guid("a807f333-12ae-4b12-9e00-e5e3ecb02f2b"));
        }
    }
}
