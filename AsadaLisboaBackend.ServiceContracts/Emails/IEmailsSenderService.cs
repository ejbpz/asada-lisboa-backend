using AsadaLisboaBackend.Models.DTOs.InformationMessage;

namespace AsadaLisboaBackend.ServiceContracts.Emails
{
    public interface IEmailsSenderService
    {
        public Task SendResetPasswordToken(string name, string email, string token);
        public Task SendVerificationCode(string name, string email, string token);
        public Task SendContactMessage(SendEmailRequestDTO sendEmailRequestDTO);
    }
}
