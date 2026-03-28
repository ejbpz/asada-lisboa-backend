using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.Users;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsersGetterService _usersGetterService;
        private readonly IUsersUpdaterService _usersUpdaterService;
        private readonly IUsersDeleterService _usersDeleterService;

        public UsuariosController(IUsersGetterService usersGetterService, IUsersUpdaterService usersUpdaterService, IUsersDeleterService usersDeleterService)
        {
            _usersGetterService = usersGetterService;
            _usersUpdaterService = usersUpdaterService;
            _usersDeleterService = usersDeleterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<UserResponseDTO>>> GetUsers([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _usersGetterService.GetUsers(searchSortRequestDTO));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailResponseDTO>> GetUser([FromRoute] Guid id)
        {
            return Ok(await _usersGetterService.GetUser(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UserUpdateRequestDTO userUpdateRequestDTO)
        {
            await _usersUpdaterService.UpdateUser(id, userUpdateRequestDTO);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            await _usersDeleterService.DeleteUser(id);
            return NoContent();
        }
    }
}
