using Microsoft.AspNetCore.Identity;
using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Error;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Users;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Services.Users
{
    public class UsersUpdaterService : IUsersUpdaterService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IChargesGetterRepository _chargesGetterRepository;

        public UsersUpdaterService(UserManager<ApplicationUser> userManager, IChargesGetterRepository chargesGetterRepository)
        {
            _userManager = userManager;
            _chargesGetterRepository = chargesGetterRepository;
        }

        public async Task UpdateUser(Guid id, UserUpdateRequestDTO userUpdateRequestDTO)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
                throw new NotFoundException("Usuario inexistente.");

            var charge = await _chargesGetterRepository.GetCharge(userUpdateRequestDTO.ChargeId);

            user.ChargeId = charge.Id;
            user.ImageUrl = userUpdateRequestDTO.ImageUrl;
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
