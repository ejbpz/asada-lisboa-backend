using AsadaLisboaBackend.Models.DTOs.Users;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Users;
using AsadaLisboaBackend.RepositoryContracts.Users;

namespace AsadaLisboaBackend.Services.Users
{
    public class UsersUpdaterService : IUsersUpdaterService
    {
        private readonly IUsersGetterService _usersGetterService;
        private readonly IUsersUpdaterRepository _usersUpdaterRepository;

        public UsersUpdaterService(IUsersUpdaterRepository usersUpdaterRepository, IUsersGetterService usersGetterService)
        {
            _usersUpdaterRepository = usersUpdaterRepository;
            _usersGetterService = usersGetterService;
        }

        public async Task UpdateUser(Guid id, UserUpdateRequestDTO userUpdateRequestDTO)
        {
            var userExists = await _usersGetterService.UserExists(id);

            if (!userExists)
                throw new ArgumentException("Usuario no existente");

            var user = new ApplicationUser()
            {
                Id = id,
                ImageUrl = userUpdateRequestDTO.ImageUrl,
                ChargeId = userUpdateRequestDTO.ChargeId,
                FirstName = userUpdateRequestDTO.FirstName,
                PhoneNumber = userUpdateRequestDTO.PhoneNumber,
                FirstLastName = userUpdateRequestDTO.FirstLastName,
                SecondLastName = userUpdateRequestDTO.SecondLastName,
            };

            await _usersUpdaterRepository.UpdateUser(user);
        }
    }
}
