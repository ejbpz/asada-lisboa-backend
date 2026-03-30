namespace AsadaLisboaBackend.ServiceContracts.Account
{
    public  interface IVerificationCodeService
    {
        public Task GenerateCode(string email);
        public Task ConfirmEmailAsync(string email, string token);
    }
}
