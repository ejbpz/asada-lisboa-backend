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
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IChargesGetterRepository _chargesGetterRepository;

        public UsersUpdaterService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IChargesGetterRepository chargesGetterRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _chargesGetterRepository = chargesGetterRepository;
        }

        public async Task UpdateUser(Guid id, UserUpdateRequestDTO userUpdateRequestDTO)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
                throw new NotFoundException("Usuario inexistente.");

            var charge = await _chargesGetterRepository.GetCharge(userUpdateRequestDTO.ChargeId);

            if (charge is null)
                throw new NotFoundException("Cargo seleccionado no encontrado.");

            var role = await _roleManager.FindByIdAsync(userUpdateRequestDTO.RoleId.ToString());

            if (role is null)
                throw new NotFoundException("Rol seleccionado no encontrado.");

            user.ChargeId = charge.Id;
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

            var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            if(currentRole is not null)
            {
                if (!role.Name!.Equals(currentRole, StringComparison.InvariantCultureIgnoreCase))
                    await _userManager.RemoveFromRoleAsync(user, currentRole);
            }

            await _userManager.AddToRoleAsync(user, role.Name!);
        }
    }
}
