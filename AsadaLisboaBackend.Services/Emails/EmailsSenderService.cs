using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resend;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.ServiceContracts.Emails;
using AsadaLisboaBackend.Models.DTOs.InformationMessage;

namespace AsadaLisboaBackend.Services.Emails
{
    public class EmailsSenderService : IEmailsSenderService
    {
        private readonly IResend _resend;
        private readonly ILogger<EmailsSenderService> _logger;
        private readonly ContactEmailOptions _contactEmailOptions;

        public EmailsSenderService(IResend resend, IOptions<ContactEmailOptions> options, ILogger<EmailsSenderService> logger)
        {
            _logger = logger;
            _resend = resend;
            _contactEmailOptions = options.Value;
        }

        public async Task SendResetPasswordToken(string name, string email, string token)
        {
            string url = $"{Constants.CLIENT_HOST}/cuenta/restaurar-contrasena/?token={token}&email={email}";

            var variables = new Dictionary<string, object>()
            {
                { "USER_NAME", name },
                { "RESET_LINK", url },
                { "USER_EMAIL", email },
            };

            var response = await _resend.EmailSendAsync(
                new EmailMessage()
                {
                    From = "Acme <onboarding@resend.dev>",
                    To = new[] { email },
                    Subject = "Token restaurar contraseña",
                    Template = new EmailMessageTemplate()
                    {
                        TemplateId = new Guid( "994eedad-8199-45ca-8fcf-57eb3e257e9f" ),
                        Variables = variables,
                    }
                }
            );

            if (!response.Success)
            {
                _logger.LogError("Error al enviar el token de restauración de contraseña a {Email}. Response: {Response}", email, response);
                throw new SendEmailException("Error al enviar el token.");
            }
        }

        public async Task SendContactMessage(SendEmailRequestDTO sendEmailRequestDTO)
        {
            var variables = new Dictionary<string, object>()
            {
                { "USER_NAME", sendEmailRequestDTO.FullName },
                { "CONTACT_MESSAGE", sendEmailRequestDTO.Message },
            };

            var response = await _resend.EmailSendAsync(
                new EmailMessage()
                {
                    From = "Acme <onboarding@resend.dev>",
                    //To = new[] { "asadaurblisboa@gmail.com" },
                    To = new[] { _contactEmailOptions.CONTACT_EMAIL },
                    ReplyTo = sendEmailRequestDTO.Email,
                    Subject = sendEmailRequestDTO.Subject,
                    Template = new EmailMessageTemplate()
                    {
                        TemplateId = new Guid( "704ec8df-305f-4c7b-9b59-d723be566c16" ),
                        Variables = variables,
                    }
                }
            );

            if (!response.Success)
            {
                _logger.LogError("Error al enviar el mensaje de contacto de {Email}. Response: {Response}", sendEmailRequestDTO.Email, response);
                throw new SendEmailException("Error al enviar el mensaje de contacto.");
            }
        }

        public async Task SendVerificationCode(string name, string email, string token)
        {
            string url = $"{Constants.CLIENT_HOST}/cuenta/confirmar-correo/?token={token}&email={email}";

            var variables = new Dictionary<string, object>()
            {
                { "USER_NAME", name },
                { "RESET_LINK", url },
                { "USER_EMAIL", email },
            };

            var response = await _resend.EmailSendAsync(
                new EmailMessage()
                {
                    From = "Acme <onboarding@resend.dev>",
                    To = new[] { email },
                    Subject = "Token confirmación correo electrónico",
                    Template = new EmailMessageTemplate()
                    {
                        TemplateId = new Guid("a807f333-12ae-4b12-9e00-e5e3ecb02f2b"),
                        Variables = variables,
                    }
                }
            );

            if (!response.Success)
            {
                _logger.LogError("Error al enviar el token de confirmación de correo electrónico a {Email}. Response: {Response}", email, response);
                throw new SendEmailException("Error al enviar el mensaje de contacto.");
            }
        }
    }
}
