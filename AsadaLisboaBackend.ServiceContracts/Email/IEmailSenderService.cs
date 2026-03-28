using AsadaLisboaBackend.Models.DTOs.InformationMessage;

namespace AsadaLisboaBackend.ServiceContracts.Email
{
    public interface IEmailSenderService
    {
        public Task SendResetPasswordToken(string name, string email, string token);
        public Task SendVerificationCode(string name, string email, string token);
        public Task SendContactMessage(SendEmailRequestDTO sendEmailRequestDTO);
    }
}
