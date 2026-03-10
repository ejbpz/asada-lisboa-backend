namespace AsadaLisboaBackend.ServiceContracts.Account
{
    public interface IResetPasswordService
    {
        public Task<bool> ResetPassword(string email);
    }
}
