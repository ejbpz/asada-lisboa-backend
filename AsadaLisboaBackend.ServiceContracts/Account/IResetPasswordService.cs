using Microsoft.AspNetCore.Identity;

namespace AsadaLisboaBackend.ServiceContracts.Account
{
    public interface IResetPasswordService
    {
        public Task<bool> ForgotPassword(string email);
        public Task ResetPassword(string email, string token, string password);
    }
}
