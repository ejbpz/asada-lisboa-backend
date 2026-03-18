using System.Linq.Expressions;
using AsadaLisboaBackend.Models.IdentityModels;

namespace AsadaLisboaBackend.Models.DTOs.User
{
    public class UserDetailResponseDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Charge { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string FirstLastName { get; set; } = string.Empty;
        public string SecondLastName { get; set; } = string.Empty;
    }

    public static partial class UserExtensions
    {
        public static Expression<Func<ApplicationUser, UserDetailResponseDTO>> MapUserDetailResponseDTO()
        {
            return user => new UserDetailResponseDTO
            {
                Id = user.Id,
                Email = user.Email!,
                ImageUrl = user.ImageUrl,
                Charge = user.Charge!.Name,
                FirstName = user.FirstName,
                PhoneNumber = user.PhoneNumber,
                FirstLastName = user.FirstLastName,
                SecondLastName= user.SecondLastName,
            };
        }
    }
}
