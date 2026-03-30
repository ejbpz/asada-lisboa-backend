namespace AsadaLisboaBackend.ServiceContracts.Accounts
{
    public interface IResetPasswordService
    {
        public Task ForgotPassword(string email);
        public Task ResetPassword(string email, string token, string password);
    }
}
