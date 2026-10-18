using System.Linq.Expressions;
using AsadaLisboaBackend.Models.IdentityModels;

namespace AsadaLisboaBackend.Models.DTOs.Role;

public class RoleResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public static partial class RoleExtensions
{
    public static Expression<Func<ApplicationRole, RoleResponseDto>> MapRoleResponseDto()
    {
        return role => new RoleResponseDto
        {
            Id = role.Id,
            Name = role.Name ?? "",
        };
    }
}