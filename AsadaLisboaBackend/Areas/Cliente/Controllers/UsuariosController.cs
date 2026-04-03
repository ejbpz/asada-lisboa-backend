using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.Users;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    [ApiController]
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsersGetterService _usersGetterService;

        public UsuariosController(IUsersGetterService usersGetterService)
        {
            _usersGetterService = usersGetterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<UserResponseDTO>>> GetUsers([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _usersGetterService.GetUsers(searchSortRequestDTO));
        }
    }
}
