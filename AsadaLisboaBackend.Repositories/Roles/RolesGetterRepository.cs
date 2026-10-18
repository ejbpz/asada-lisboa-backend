using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DTOs.Role;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Roles;

namespace AsadaLisboaBackend.Repositories.Roles;

public class RolesGetterRepository(ApplicationDbContext context) : IRolesGetterRepository
{
    public async Task<List<RoleResponseDto>> GetRoles()
    {
        return await context.Roles
            .Select(RoleExtensions.MapRoleResponseDto())
            .ToListAsync();
    }
}