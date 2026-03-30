namespace AsadaLisboaBackend.ServiceContracts.Accounts
{
    public  interface IVerificationCodeService
    {
        public Task GenerateCode(string email);
        public Task ConfirmEmailAsync(string email, string token);
    }
}
