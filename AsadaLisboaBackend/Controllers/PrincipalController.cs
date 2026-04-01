using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AsadaLisboaBackend.Models.DTOs.Principal;
using AsadaLisboaBackend.ServiceContracts.Principals;

namespace AsadaLisboaBackend.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class PrincipalController : ControllerBase
    {
        private readonly IPrincipalsGetterService _principalsGetterService;

        [HttpGet("")]
        public async Task<ActionResult<PrincipalRequestDTO>> GetPrincipalInformation()
        {
            return Ok(await _principalsGetterService.GetPrincipalInformation());
        }
    }
}
