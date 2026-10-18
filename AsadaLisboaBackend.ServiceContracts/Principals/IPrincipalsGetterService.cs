using AsadaLisboaBackend.Models.DTOs.Principal;

namespace AsadaLisboaBackend.ServiceContracts.Principals;

public interface IPrincipalsGetterService
{
    public Task<PrincipalResponseDto> GetPrincipalInformation();
    public Task<PrincipalResponseDto> GetPrincipalAdminInformation();
}