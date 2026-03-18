using AsadaLisboaBackend.Models.DTOs.Error;
using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Users;
using AsadaLisboaBackend.Services.Exceptions;
using Microsoft.AspNetCore.Identity;

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
                throw new NotFoundException("Usuario inexistente.");

            user.ImageUrl = userUpdateRequestDTO.ImageUrl;
            user.ChargeId = userUpdateRequestDTO.ChargeId;
            user.FirstName = userUpdateRequestDTO.FirstName;
            user.PhoneNumber = userUpdateRequestDTO.PhoneNumber;
            user.FirstLastName = userUpdateRequestDTO.FirstLastName;
            user.SecondLastName = userUpdateRequestDTO.SecondLastName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new IdentityErrorException(
                    "Error al actualizar usuario.", 
                    result.Errors.Select(e => new ErrorDetailResponseDTO(e.Code, e.Description)
                    ).ToList()
                );
        }
    }
}
