using AsadaLisboaBackend.Models.DTOs.Account;

namespace AsadaLisboaBackend.ServiceContracts.Account
{
    public interface ILoginService
    {
        public Task Login(LoginRequestDTO loginRequestDTO);
    }
}
