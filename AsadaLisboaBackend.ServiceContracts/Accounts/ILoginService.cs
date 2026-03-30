using AsadaLisboaBackend.Models.DTOs.Jwt;
using AsadaLisboaBackend.Models.DTOs.Account;

namespace AsadaLisboaBackend.ServiceContracts.Accounts
{
    public interface ILoginService
    {
        public Task<AuthenticationResponseDTO> Login(LoginRequestDTO loginRequestDTO);
    }
}
