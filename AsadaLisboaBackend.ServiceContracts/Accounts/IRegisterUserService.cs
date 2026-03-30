using AsadaLisboaBackend.Models.DTOs.Account;

namespace AsadaLisboaBackend.ServiceContracts.Accounts
{
    public  interface IRegisterUserService
    {
        public Task RegisterUser(RegisterRequestDTO registerRequestDTO);
    }
}
