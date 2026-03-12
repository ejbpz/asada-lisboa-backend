using Microsoft.AspNetCore.Identity;
using AsadaLisboaBackend.Models.DTOs.Users;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Users;

namespace AsadaLisboaBackend.Services.Users
{
    public class UsersUpdaterService : IUsersUpdaterService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersUpdaterService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task UpdateUser(Guid id, UserUpdateRequestDTO userUpdateRequestDTO)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
                throw new ArgumentNullException("Usuario inexistente.");

            user.ImageUrl = userUpdateRequestDTO.ImageUrl;
            user.ChargeId = userUpdateRequestDTO.ChargeId;
            user.FirstName = userUpdateRequestDTO.FirstName;
            user.PhoneNumber = userUpdateRequestDTO.PhoneNumber;
            user.FirstLastName = userUpdateRequestDTO.FirstLastName;
            user.SecondLastName = userUpdateRequestDTO.SecondLastName;

            var result = await _userManager.UpdateAsync(user);

            // TODO: Add errors.
            if (!result.Succeeded)
                throw new ArgumentNullException("Error al actualizar usuario.");
        }
    }
}
