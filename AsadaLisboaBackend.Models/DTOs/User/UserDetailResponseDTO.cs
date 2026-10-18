using System.Linq.Expressions;
using AsadaLisboaBackend.Models.DTOs.Role;
using AsadaLisboaBackend.Models.IdentityModels;

namespace AsadaLisboaBackend.Models.DTOs.User;

public class UserDetailResponseDto
{
    public Guid Id { get; set; }
    public Guid ChargeId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Charge { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public string FirstLastName { get; set; } = string.Empty;
    public string SecondLastName { get; set; } = string.Empty;
    public ICollection<RoleResponseDto> Roles { get; set; } = new List<RoleResponseDto>();
}

public static partial class UserExtensions
{
    public static Expression<Func<ApplicationUser, UserDetailResponseDto>> MapUserDetailResponseDto()
    {
        return user => new UserDetailResponseDto
        {
            Id = user.Id,
            Email = user.Email!,
            Charge = user.Charge!.Name,
            ChargeId = user.Charge!.Id,
            FirstName = user.FirstName,
            PhoneNumber = user.PhoneNumber,
            FirstLastName = user.FirstLastName,
            SecondLastName= user.SecondLastName,
        };
    }
}