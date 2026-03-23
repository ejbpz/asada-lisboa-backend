using Microsoft.Extensions.Options;
using Resend;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.ServiceContracts.Email;
using AsadaLisboaBackend.Models.DTOs.InformationMessage;

namespace AsadaLisboaBackend.Services.Email
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IResend _resend;
        private readonly ContactEmailOptions _contactEmailOptions;

        public EmailSenderService(IResend resend, IOptions<ContactEmailOptions> options)
        {
            _resend = resend;
            _contactEmailOptions = options.Value;
        }

        public async Task<bool> SendResetPasswordToken(string name, string email, string token)
        {
            string url = $"{Constants.DOMAIN_HOST}/api/cuenta/restaurar-contrasena/?token={token}&email={email}";

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

            return response.Success;
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
                throw new Exception("Error al enviar el mensaje de contacto.");
        }
    }
}
