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

        public UsuariosController(IUsersGetterService usersGetterService)
        {
            _usersGetterService = usersGetterService;
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
    }
}
