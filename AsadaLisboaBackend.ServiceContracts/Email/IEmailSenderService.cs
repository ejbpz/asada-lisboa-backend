namespace AsadaLisboaBackend.ServiceContracts.Email
{
    public interface IEmailSenderService
    {
        public Task<bool> SendResetPasswordToken(string name, string email, string token);
    }
}
