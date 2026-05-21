using AsadaLisboaBackend.Models.DTOs.Principal;

namespace AsadaLisboaBackend.ServiceContracts.Principals
{
    public interface IPrincipalsGetterService
    {
        public Task<PrincipalResponseDTO> GetPrincipalInformation();
        public Task<PrincipalResponseDTO> GetPrincipalAdminInformation();
    }
}
