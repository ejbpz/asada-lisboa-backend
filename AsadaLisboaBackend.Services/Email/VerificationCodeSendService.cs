using Resend;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.ServiceContracts.Email;

namespace AsadaLisboaBackend.Services.Email
{
    public class VerificationCodeSendService : IVerificationCodeSendService
    {
        private readonly IResend _resend;

        public  VerificationCodeSendService(IResend resend)
        {
            _resend = resend;
        }

        public async Task<bool> SendVerificationCode(string name, string email, string token)
        {
            string url = $"{Constants.DOMAIN_HOST}/api/registrar/confirmar-correo/?token={token}&email={email}";

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

            return response.Success;

        }


    }
}
