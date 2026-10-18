using AsadaLisboaBackend.Models.DTOs.Role;

namespace AsadaLisboaBackend.ServiceContracts.Roles;

public interface IRolesGetterService
{
    public Task<List<RoleResponseDto>> GetRoles();
}