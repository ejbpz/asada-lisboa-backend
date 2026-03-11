using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Users;
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

        public UsuariosController(IUsersGetterService usersGetterService, IUsersUpdaterService usersUpdaterService)
        {
            _usersGetterService = usersGetterService;
            _usersUpdaterService = usersUpdaterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<UserResponseDTO>?>> GetUsers([FromQuery] int page = 1)
        {
            return Ok(await _usersGetterService.GetUsers(page));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailResponseDTO>> GetUser([FromRoute] Guid id)
        {
            var user = await _usersGetterService.GetUser(id);

            if (user is null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UserUpdateRequestDTO userUpdateRequestDTO)
        {
            await _usersUpdaterService.UpdateUser(id, userUpdateRequestDTO);

            return NoContent();
        }
    }
}
