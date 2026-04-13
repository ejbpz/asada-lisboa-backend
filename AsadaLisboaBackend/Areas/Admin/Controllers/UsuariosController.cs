using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.Users;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for manage users, only accesible by admin users.
    /// </summary>
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsersGetterService _usersGetterService;
        private readonly IUsersUpdaterService _usersUpdaterService;
        private readonly IUsersDeleterService _usersDeleterService;

        /// <summary>
        /// Constructor for UsuariosController.
        /// </summary>
        /// <param name="usersGetterService">Service for getting users.</param>
        /// <param name="usersUpdaterService">Service for updating users.</param>
        /// <param name="usersDeleterService">Service for deleting users.</param>
        public UsuariosController(IUsersGetterService usersGetterService, IUsersUpdaterService usersUpdaterService, IUsersDeleterService usersDeleterService)
        {
            _usersGetterService = usersGetterService;
            _usersUpdaterService = usersUpdaterService;
            _usersDeleterService = usersDeleterService;
        }

        /// <summary>
        /// Get a paginated list of users based on the provided search and sort criteria.
        /// </summary>
        /// <param name="searchSortRequestDTO">An object containing search filters, sorting options, and pagination.</param>
        /// <returns>An ActionResult containing a PageResponseDTO of UserResponseDTO objects that match the provided criteria.</returns>
        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<UserResponseDTO>>> GetUsers([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _usersGetterService.GetUsers(searchSortRequestDTO));
        }

        /// <summary>
        /// Gets a specific user by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve.</param>
        /// <returns>An ActionResult containing the UserDetailResponseDTO object.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailResponseDTO>> GetUser([FromRoute] Guid id)
        {
            return Ok(await _usersGetterService.GetUser(id));
        }

        /// <summary>
        /// Updates an existing user identified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="userUpdateRequestDTO">An object containing the updated details of the user.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UserUpdateRequestDTO userUpdateRequestDTO)
        {
            await _usersUpdaterService.UpdateUser(id, userUpdateRequestDTO);

            return NoContent();
        }

        /// <summary>
        /// Deletes the specified user by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            await _usersDeleterService.DeleteUser(id);
            return NoContent();
        }
    }
}
