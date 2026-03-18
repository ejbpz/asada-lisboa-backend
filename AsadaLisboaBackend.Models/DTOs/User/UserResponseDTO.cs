using System.Linq.Expressions;
using AsadaLisboaBackend.Models.IdentityModels;

namespace AsadaLisboaBackend.Models.DTOs.User
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
