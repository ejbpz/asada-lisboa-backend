using AsadaLisboaBackend.Models.IdentityModels;
using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Users
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Charge { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }

    public static partial class UserExtensions
    {
        public static Expression<Func<ApplicationUser, UserResponseDTO>> MapUserResponseDTO()
        {
            return user => new UserResponseDTO
            {
                Id = user.Id,
                ImageUrl = user.ImageUrl,
                Charge = user.Charge!.Name,
                Name = $"{user.FirstName} {user.FirstLastName} {user.SecondLastName}",
            };
        }
    }
}
