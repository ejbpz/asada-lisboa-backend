using AsadaLisboaBackend.Models.DTOs.Role;

namespace AsadaLisboaBackend.RepositoryContracts.Roles;

public interface IRolesGetterRepository
{
    public Task<List<RoleResponseDto>> GetRoles();
}