using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.Users;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    /// <summary>
    /// Controller for users, public access.
    /// </summary>
    [ApiController]
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsersGetterService _usersGetterService;

        /// <summary>
        /// Constructor for UsuariosController.
        /// </summary>
        /// <param name="usersGetterService">Service for getting users.</param>
        public UsuariosController(IUsersGetterService usersGetterService)
        {
            _usersGetterService = usersGetterService;
        }

        /// <summary>
        /// Get a paginated list of users based on the provided search and sort criteria.
        /// </summary>
        /// <param name="searchSortRequestDTO">Object containing search, sorting and pagination.</param>
        /// <returns>ActionResult containing a PageResponseDTO of UserResponseDTO objects that match with SearchSortRequestDTO.</returns>
        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<UserResponseDTO>>> GetUsers([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _usersGetterService.GetPublicUsers(searchSortRequestDTO));
        }
    }
}
