using Resend;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.ServiceContracts.Email;

namespace AsadaLisboaBackend.Services.Email
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IResend _resend;

        public EmailSenderService(IResend resend)
        {
            _resend = resend;
        }

        public async Task<bool> SendResetPasswordToken(string name, string email, string token)
        {
            string url = $"{Constants.DOMAIN_HOST}/restaurar-contrasena/?token={token}&email={email}";

            var message = new EmailMessage();

            message.From = "Acme <onboarding@resend.dev>"; // TODO: Register ASADA domain in Resend.
            message.To.Add(email);
            message.Subject = "Token restaurar contraseña.";
            message.HtmlBody = $@"
                <!DOCTYPE html>
                <html>
                    <head>
                    <link rel=""preconnect"" href=""https://fonts.googleapis.com"">
                    <link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin>
                    <link href=""https://fonts.googleapis.com/css2?family=Poppins:wght@400;600&display=swap"" rel=""stylesheet"">
    
                    <style>
                        .regular-poppins {{
                        font-family: ""Poppins"", sans-serif;
                        font-weight: 400;
                        font-style: normal;
                        }}
      
                        .poppins-semibold {{
                        font-family: ""Poppins"", sans-serif;
                        font-weight: 600;
                        font-style: normal;
                        }}
    
                        .container {{
                        background-color: #333;
                        color: white;
                        }}
      
                        .link-container {{
                        width: 100%;
                        display: flex;
                        justify-content: center;
                        }}
      
                        .link {{
                        padding: 10px;
                        background-color: #27B4F5;
                        color: white;
                        text-decoration: none;
                        }}
      
                        .link:hover {{
                        background-color: #0CAAF2;
                        }}
                    </style>
                    </head>
                    <body class=""container regular-poppins"">
                    <div>
                        <h1 class=""poppins-semibold"">Hola <strong>{name}</strong>,</h1>
                        <p>Recientemente ha solicitado restablecer su contraseña para su cuenta de {email}. Presione el botón para restablecerla. <strong>Este enlace para restablecer su contraseña tiene una duración de 24 horas.</strong></p>
                    </div>
                    <div class=""link-container"">
                        <a href='{url}' class=""link"">Restablecer contraseña</a>
                    </div>
                    </body>
                </html>
            ";

            return (await _resend.EmailSendAsync(message)).Success;
        }
    }
}
