using System.Linq.Expressions;
using AsadaLisboaBackend.Models.IdentityModels;

namespace AsadaLisboaBackend.Models.DTOs.User;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Charge { get; set; } = string.Empty;
}

public static partial class UserExtensions
{
    public static Expression<Func<ApplicationUser, UserResponseDto>> MapUserResponseDto()
    {
        return user => new UserResponseDto
        {
            Id = user.Id,
            Charge = user.Charge!.Name,
            Name = $"{user.FirstName} {user.FirstLastName} {user.SecondLastName}",
        };
    }
}